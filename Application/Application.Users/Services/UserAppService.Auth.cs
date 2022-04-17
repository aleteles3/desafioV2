using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.User.ViewModels.Auth;
using Application.Users.ViewModels.Auth;
using Domain.Core.Enums;
using Microsoft.IdentityModel.Tokens;
using UserDomain = Domain.User.Entities.User;

namespace Application.Users.Services;

public partial class UserAppService
{
    public async Task<TokenViewModel> GenerateUserAuthToken(UserAuthViewModel userAuthViewModel)
    {
        var hashPassword = _security.EncryptString(userAuthViewModel.Password, userAuthViewModel.Login);
        var user = (await Repository.GetUsers(x => x.Login == userAuthViewModel.Login && x.Password == hashPassword))
            .FirstOrDefault();

        if (user == null)
        {
            MemoryBus.RaiseValidationError(ErrorCode.DomainValidationError, "Login was not successfull");
            return null;
        }

        return GenerateToken(user);
    }

    private TokenViewModel GenerateToken(UserDomain user)
    {
        //User Claims
        var claims = new[]
        {
            new Claim("UserLogin", user.Login),
            new Claim("UserId", user.Id.ToString())
        };
        
        //Private Key
        var privateKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSecurity.SecretKey));
        
        //Digital Signature
        var signature = new SigningCredentials(privateKey, SecurityAlgorithms.HmacSha256);
        
        //Expiration Date
        var expirationDate = DateTime.UtcNow.AddHours(1);
        
        //Generate Token
        var token = new JwtSecurityToken(
            issuer: JwtSecurity.Issuer,
            audience: JwtSecurity.Audience,
            claims: claims,
            expires: expirationDate,
            signingCredentials: signature
        );

        return new TokenViewModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpirationDate = expirationDate
        };
    }
}