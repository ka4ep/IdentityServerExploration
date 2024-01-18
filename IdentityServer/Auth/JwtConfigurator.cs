using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer.Auth;

public class JwtConfigurator
{
    public const string JwtSection = "Jwt";
    public const string JwtSectionKey = "Key";
    public const string JwtSectionIssuer = "Issuer";
    public const string JwtSectionLifespan = "TokenLifespan";
    public const string JwtCookieName = "JwtCookie";
    public const string JwtSectionCertPath = "CertificatePath";
    public const string JwtSectionCertPass = "CertificatePassword";
    
    public TimeSpan TokenLifespan { get; }

    public string JwtKey { get; }
    public string JwtIssuer { get; }
    public SecurityKey SigningKey { get; }

    internal X509Certificate2 RSA { get; }



    public JwtConfigurator(IConfiguration configuration)
    {
        JwtKey = configuration.GetSection($"{JwtSection}:{JwtSectionKey}").Get<string>() ?? throw new AuthenticationFailureException($"appsettings.json does not contain {JwtSection}:{JwtSectionKey} value");
        JwtIssuer = configuration.GetSection($"{JwtSection}:{JwtSectionIssuer}").Get<string>() ?? throw new AuthenticationFailureException($"appsettings.json does not contain {JwtSection}:{JwtSectionIssuer} value");

        // Use RSA, symmetric does not get properly checked against kid/KeyId
        RSA = new X509Certificate2(
            configuration.GetSection($"{JwtSection}:{JwtSectionCertPath}").Get<string>() ?? throw new AuthenticationFailureException($"appsettings.json does not contain {JwtSection}:{JwtSectionCertPath} value"),
            configuration.GetSection($"{JwtSection}:{JwtSectionCertPass}").Get<string>() ?? throw new AuthenticationFailureException($"appsettings.json does not contain {JwtSection}:{JwtSectionCertPass} value")
            );

        SigningKey = new RsaSecurityKey(RSA.GetRSAPrivateKey());

        //SigningKey = new(Encoding.Unicode.GetBytes(JwtKey));// { KeyId = "B24B4A5B2F399C56B5BD98E1ED26C4A3" };

        var lifespan = configuration.GetSection($"{JwtSection}:{JwtSectionLifespan}").Get<int>();
        if (lifespan <= 0) lifespan = 30 * 60;
        TokenLifespan = TimeSpan.FromSeconds(lifespan);
    }
}




/*
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
*/