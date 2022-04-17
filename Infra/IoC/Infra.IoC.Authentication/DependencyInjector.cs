using System.Text;
using Domain.Core.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infra.IoC.Authentication;

public static class DependencyInjector
{
    public static IServiceCollection AddServices(IServiceCollection services, IConfiguration configuration)
    {
        //Adding Jwt Bearer Token Auth
        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtSecurity.Issuer,
                    ValidAudience = JwtSecurity.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSecurity.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

        return services;
    }
}