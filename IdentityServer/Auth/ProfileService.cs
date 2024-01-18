using IdentityModel;
using IdentityServer.Data;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Auth;

public sealed class ProfileService(
    IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) 
    : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await userManager.FindByIdAsync(sub);
        var userClaims = await userClaimsPrincipalFactory.CreateAsync(user);

        var claims = userClaims.Claims.ToList();
        claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

        if (userManager.SupportsUserRole)
        {
            var roles = await userManager.GetRolesAsync(user);
            foreach (var roleName in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, roleName));
                if (roleManager.SupportsRoleClaims)
                {
                    var role = await roleManager.FindByNameAsync(roleName);
                    if (role is not null)
                        claims.AddRange(await roleManager.GetClaimsAsync(role));
                }
            }
        }

        context.IssuedClaims = claims;
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await userManager.FindByIdAsync(sub);
        context.IsActive = user is not null;
    }
}
