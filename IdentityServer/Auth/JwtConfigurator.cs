using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer.Auth;

public class JwtConfigurator
{
    //public const string JwtCookieName = "JwtCookie";

    public const string JwtSection = "Jwt";

    public SecurityKey SigningKey { get; }

    internal X509Certificate2 RSA { get; }

    internal JwtConfiguratorOptions Options { get; }

    public JwtConfigurator(JwtConfiguratorOptions options)
    {
        Options = options;
        // Use RSA, symmetric does not get properly checked against kid/KeyId
        RSA = new X509Certificate2(
            options.CertificatePath ?? throw new AuthenticationFailureException($"appsettings.json does not contain {JwtSection}:{nameof(options.CertificatePath)} value"),
            options.CertificatePass ?? throw new AuthenticationFailureException($"appsettings.json does not contain {JwtSection}:{nameof(options.CertificatePass)} value")
            );

        SigningKey = new RsaSecurityKey(RSA.GetRSAPrivateKey());

        //SigningKey = new(Encoding.Unicode.GetBytes(JwtKey));// { KeyId = "B24B4A5B2F399C56B5BD98E1ED26C4A3" };
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