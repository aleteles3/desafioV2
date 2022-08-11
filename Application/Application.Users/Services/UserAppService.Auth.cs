using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Users.ViewModels.Auth;
using Domain.Core.Enums;
using Domain.NoSql.UserToken;
using Microsoft.IdentityModel.Tokens;
using UserDomain = Domain.User.Entities.User;

namespace Application.Users.Services;

public partial class UserAppService
{
    public async Task<TokenViewModel> GenerateUserAuthToken(UserAuthViewModel userAuthViewModel)
    {
        var hashPassword = _security.EncryptString(userAuthViewModel.Password, userAuthViewModel.Login);
        var user = (await Repository.GetUsers(x => x.Login == userAuthViewModel.Login && x.Password == hashPassword))
            ?.FirstOrDefault();

        if (user == null)
        {
            MemoryBus.RaiseValidationError(ErrorCode.DomainValidationError, "Login was not successfull");
            return null;
        }

        var token = GenerateTokenViewModel(user);

        var userRefreshToken = new UserRefreshToken
        {
            UserId = user.Id,
            RefreshToken = token.RefreshToken,
            RefreshTokenExpirationDate = token.RefreshTokenExpirationDate
        };

        await WriteUserRefreshTokenInMongoDb(userRefreshToken);

        return token;
    }

    public async Task<TokenViewModel> RefreshToken(RefreshTokenViewModel refreshTokenViewModel)
    {
        var claimsPrincipal = GetClaimsPrincipalFromExpiredToken(refreshTokenViewModel.Token);

        var parsed = Guid.TryParse(claimsPrincipal?.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value, out var userId);

        if (!parsed)
        {
            MemoryBus.RaiseValidationError(ErrorCode.DomainValidationError, "Invalid Token or Refresh Token");
            return null;
        }

        var existingUserRefreshToken = await _userTokenRepository.GetUserToken(userId);

        if (existingUserRefreshToken?.RefreshTokenExpirationDate < DateTime.UtcNow ||
            existingUserRefreshToken.RefreshToken != refreshTokenViewModel.RefreshToken)
        {
            MemoryBus.RaiseValidationError(ErrorCode.DomainValidationError, "Invalid Token or Refresh Token");
            return null;
        }

        var newAccessToken = GenerateJwtToken(claimsPrincipal.Claims);
        var newRefreshToken = GenerateRefreshToken();

        var userRefreshToken = new UserRefreshToken
        {
            UserId = userId,
            RefreshToken = newRefreshToken,
            RefreshTokenExpirationDate = DateTime.UtcNow.AddHours(1)
        };

        await WriteUserRefreshTokenInMongoDb(userRefreshToken);

        return new TokenViewModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            TokenExpirationDate = newAccessToken.ValidTo,
            RefreshToken = newRefreshToken,
            RefreshTokenExpirationDate = userRefreshToken.RefreshTokenExpirationDate
        };
    }

    private ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = JwtSecurity.Audience,
            ValidIssuer = JwtSecurity.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSecurity.SecretKey)),
            ValidateLifetime = false
        };

        var claimsPrincipal =
            new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is JwtSecurityToken jwtsecurityToken && jwtsecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)) 
            return claimsPrincipal;
        
        return null;
    }

    private async Task WriteUserRefreshTokenInMongoDb(UserRefreshToken userRefreshToken)
    {
        await _userTokenRepository.DeleteUserToken(userRefreshToken.UserId);

        await _userTokenRepository.InsertUserToken(userRefreshToken);
    }

    private TokenViewModel GenerateTokenViewModel(UserDomain user)
    {
        //User Claims
        var claims = new[]
        {
            new Claim("UserLogin", user.Login),
            new Claim("UserId", user.Id.ToString())
        };

        //Create JwtToken
        var token = GenerateJwtToken(claims);
        
        //Refresh Token Expiration Date
        var refreshTokenExpirationDate = DateTime.UtcNow.AddHours(1);
        
        //Generate Refresh Token
        var refreshToken = GenerateRefreshToken();
        
        return new TokenViewModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            TokenExpirationDate = token.ValidTo,
            RefreshToken = refreshToken,
            RefreshTokenExpirationDate = refreshTokenExpirationDate
        };
    }

    private JwtSecurityToken GenerateJwtToken(IEnumerable<Claim> claims)
    {
        //Private Key
        var privateKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSecurity.SecretKey));
        
        //Digital Signature
        var signature = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);
        
        //Expiration Date
        var expirationDate = DateTime.UtcNow.AddMinutes(5);
        
        //Generate Token
        var token = new JwtSecurityToken(
            issuer: JwtSecurity.Issuer,
            audience: JwtSecurity.Audience,
            claims: claims,
            expires: expirationDate,
            signingCredentials: signature
        );

        return token;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}