namespace IdentityServer.Auth;

public class JwtConfiguratorOptions
{
    public string CertificatePath { get; set; } = string.Empty;
    public string CertificatePass { get; set; } = string.Empty;

    public string ValidIssuer { get; set; } = string.Empty;
    public bool ValidateIssuer { get; set; } = false;
    public bool ValidateIssuerSigningKey { get;set; } = false;

    public string ValidAudience { get; set; } = string.Empty;
    public bool ValidateAudience { get; set; } = false;

    public int TokenLifespan { get; set; } = 1800;
    public bool ValidateTokenLifespan { get; set; } = false;
}
