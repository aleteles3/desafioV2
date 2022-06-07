using Domain.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Domain.Core.UserToken;

public class UserToken : IUserToken
{
    private readonly IHttpContextAccessor _httpContext;

    public UserToken(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }
    
    public string GetUserLogin()
    {
        return GetTokenClaims("UserLogin");
    }

    public Guid? GetUserId()
    {
        var claimValue = GetTokenClaims("UserId");
        var parsed = Guid.TryParse(claimValue, out var userId);

        return parsed ? userId : null;
    }

    private string GetTokenClaims(string claimType)
    {
        var claims = _httpContext?.HttpContext?.User?.Claims.ToList();

        return claims?.FirstOrDefault(x => x.Type == claimType)?.Value;
    }
}