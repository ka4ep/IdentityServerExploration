using IdentityServer4.EntityFramework.Entities;
using System.Collections.Generic;

namespace IdentityServer.Auth;

public class IdentityServerConfigurationOptions
{
    public List<Client> Clients { get; set; } = [];
    public List<ApiScope> ApiScopes { get; set; } = [];
    public List<ApiResource> ApiResources { get; set; } = [];
    public List<IdentityResource> IdentityResources { get; set; } = [];
}
