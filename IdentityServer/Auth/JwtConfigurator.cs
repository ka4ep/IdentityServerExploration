using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Text;

namespace IdentityServer.Auth;

public class JwtConfigurator
{
    public const string JwtSection = "Jwt";
    public const string JwtSectionKey = "Key";
    public const string JwtSectionIssuer = "Issuer";
    public const string JwtSectionLifespan = "TokenLifespan";
    public const string JwtCookieName = "JwtCookie";
    
    public TimeSpan TokenLifespan { get; }

    public string JwtKey { get; }
    public string JwtIssuer { get; }
    public SymmetricSecurityKey SigningKey { get; }

    public string GenerateToken()
    {
        var now = DateTime.UtcNow;
        var exp = now.Add(TokenLifespan);

        var key = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())]),
            NotBefore = now,
            Expires = exp,
            Issuer = JwtIssuer,
            Audience = JwtIssuer,            
            SigningCredentials = key,
        };        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public JwtConfigurator(IConfiguration configuration)
    {
        JwtKey = configuration.GetSection($"{JwtSection}:{JwtSectionKey}").Get<string>() ?? throw new AuthenticationFailureException($"appsettings.json does not contain {JwtSection}:{JwtSectionKey} value");
        JwtIssuer = configuration.GetSection($"{JwtSection}:{JwtSectionIssuer}").Get<string>() ?? throw new AuthenticationFailureException($"appsettings.json does not contain {JwtSection}:{JwtSectionIssuer} value");
        SigningKey = new(Encoding.UTF8.GetBytes(JwtKey));

        var lifespan = configuration.GetSection($"{JwtSection}:{JwtSectionLifespan}").Get<int>();
        if (lifespan <= 0) lifespan = 30 * 60;
        TokenLifespan = TimeSpan.FromSeconds(lifespan);
    }
}
