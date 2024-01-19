using IdentityServer.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Data;

internal static class Seed
{
    internal static async Task SeedWithSampleUsersAsync(UserManager<ApplicationUser> userManager)
    {
        foreach (var user in userManager.Users.ToList())
        {
            var claims = await userManager.GetClaimsAsync(user);
            await userManager.RemoveClaimsAsync(user, claims);
            await userManager.DeleteAsync(user);
        }

        {
            var user = new ApplicationUser
            {
                Id = new Guid("b9991f69-b4c1-477d-9432-2f7cf6099e02"),
                UserName = "dev@nonsense.com",
                Email = "dev@nonsense.com"
            };
            await RegisterUserIfNotExists(userManager, user, "Pass!2");
            await userManager.AddClaimAsync(user, new Claim("subscription", "paid"));
            await userManager.AddClaimAsync(user, new Claim("role", "Dev"));
        }

        {
            var user = new ApplicationUser
            {
                Id = new Guid("AD111111-3986-4980-6301-283888811531"),
                UserName = "admin@nonsense.com",
                Email = "admin@nonsense.com"
            };
            await RegisterUserIfNotExists(userManager, user, "Qwerty1234!");
            await userManager.AddToRoleAsync(user, "admin");
            await userManager.AddToRoleAsync(user, "viewer");
        }
        {
            var user = new ApplicationUser
            {
                Id = new Guid("39D706BE-02FA-43BC-A465-46A289FA984A"),
                UserName = "viewer",
                Email = "viewer@nonsense.com"
            };
            await RegisterUserIfNotExists(userManager, user, "Qwerty1234!");
            await userManager.AddToRoleAsync(user, "viewer");
        }
    }


    public static async Task PrepareDataAsync(IHost host)
    {
        using var scope = host.Services.CreateAsyncScope();
        try
        {
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
            if (env.IsDevelopment())
            {
                await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                await SeedWithSampleUsersAsync(userManager);
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Data preparation failed : {ex.GetErrorMessage()}");
        }
    }

    private static async Task RegisterUserIfNotExists<TUser>(UserManager<TUser> userManager,
        TUser user, string password)
        where TUser : ApplicationUser
    {
        if (await userManager.FindByNameAsync(user.UserName ?? string.Empty) == null)
        {
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await userManager.ConfirmEmailAsync(user, code);
            }
        }
    }


    
}