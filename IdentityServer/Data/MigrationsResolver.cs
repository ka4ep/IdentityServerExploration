using IdentityServer.Services;
using IdentityServer4.EntityFramework.DbContexts;
using ISEF = IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer.Data;

public class MigrationsResolver(IConfiguration configuration)
{
    private static readonly string _migrationsAssemblyName = typeof(ApplicationDbContext).Assembly.GetName().Name ??
                                                             typeof(ApplicationDbContext).Assembly.GetName().FullName;
    private const string ConnectionKey = "DefaultConnection";
    public Action<DbContextOptionsBuilder> SqlServerOptions => new(builder =>
    {
        var connectionString = configuration.GetConnectionString(ConnectionKey)
            ?? throw new InvalidOperationException($"appsettings.json ConnectionStrings:{ConnectionKey} could not be read");
        builder.UseSqlServer(connectionString, options =>
        {
            options.MigrationsAssembly(_migrationsAssemblyName);
            options.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(15), errorNumbersToAdd: null);
        });
    });


    private static readonly IEqualityComparer<ISEF.Client> _clientComparer = GenericComparer<ISEF.Client>.CreateHash(x => $"{x?.ClientId}{x?.ClientName}".GetHashCode());
    private static readonly IEqualityComparer<ISEF.IdentityResource> _identityResourceComparer = GenericComparer<ISEF.IdentityResource>.CreateHash(x => $"{x?.Id}{x?.Name}".GetHashCode());
    private static readonly IEqualityComparer<ISEF.ApiScope> _apiScopeComparer = GenericComparer<ISEF.ApiScope>.CreateHash(x => $"{x?.Id}{x?.Name}".GetHashCode());
    private static readonly IEqualityComparer<ISEF.ApiResource> _apiResourceComparer = GenericComparer<ISEF.ApiResource>.CreateHash(x => $"{x?.Id}{x?.Name}".GetHashCode());

    public static async Task<IHost> MigrateDatabaseAsync(IHost host)
    {
        using var scope = host.Services.CreateAsyncScope();
        await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
        await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();
        var helper = scope.ServiceProvider.GetRequiredService<HostAddressService>();
        using var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        try
        {
            await context.Database.MigrateAsync();
            if (!context.Clients.Any())
            {
                await context.Clients.AddRangeAsync(Config.GetClients(helper).Select(x => x.ToEntity()).Except(context.Clients, _clientComparer).Distinct(_clientComparer));
                await context.SaveChangesAsync();
            }
            if (!context.IdentityResources.Any())
            {
                await context.IdentityResources.AddRangeAsync(Config.IdentityResources.Select(x => x.ToEntity()).Except(context.IdentityResources, _identityResourceComparer).Distinct(_identityResourceComparer));
                await context.SaveChangesAsync();
            }
            if (!context.ApiScopes.Any())
            {
                await context.ApiScopes.AddRangeAsync(Config.Scopes.Select(x => x.ToEntity()).Except(context.ApiScopes, _apiScopeComparer).Distinct(_apiScopeComparer));
                await context.SaveChangesAsync();
            }
            if (!context.ApiResources.Any())
            {
                await context.ApiResources.AddRangeAsync(Config.Resources.Select(x => x.ToEntity()).Except(context.ApiResources, _apiResourceComparer).Distinct(_apiResourceComparer));
                await context.SaveChangesAsync();
            }




        }
        catch (Exception ex)
        {
            throw;
        }
        return host;
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

        }
    }


    private static async Task SeedWithSampleUsersAsync(UserManager<ApplicationUser> userManager)
    {
        // NOTE: ToList важен, так как при удалении пользователя меняется список пользователей
        foreach (var user in userManager.Users.ToList())
        {
            var claims = await userManager.GetClaimsAsync(user);
            await userManager.RemoveClaimsAsync(user, claims);
            await userManager.DeleteAsync(user);
        }

        {
            var user = new ApplicationUser
            {
                Id = "a83b72ed-3f99-44b5-aa32-f9d03e7eb1fd",
                UserName = "vicky@nonsense.com",
                Email = "vicky@nonsense.com"
            };
            await RegisterUserIfNotExists(userManager, user, "Pass!2");
            await userManager.AddClaimAsync(user, new Claim("testing", "beta"));
        }

        {
            var user = new ApplicationUser
            {
                Id = "dcaec9ce-91c9-4105-8d4d-eee3365acd82",
                UserName = "cristina@nonsense.com",
                Email = "cristina@nonsense.com",
            };
            await RegisterUserIfNotExists(userManager, user, "Pass!2");
            await userManager.AddClaimAsync(user, new Claim("subscription", "paid"));
        }

        {
            var user = new ApplicationUser
            {
                Id = "b9991f69-b4c1-477d-9432-2f7cf6099e02",
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
                Id = "AD111111-3986-4980-6301-283888811531".ToLowerInvariant(),
                UserName = "admin@nonsense.com",
                Email = "admin@nonsense.com"
            };
            await RegisterUserIfNotExists(userManager, user, "Qwerty1234!");
            await userManager.AddClaimAsync(user, new Claim("role", "admin"));
        }
        {
            var user = new ApplicationUser
            {
                Id = "39D706BE-02FA-43BC-A465-46A289FA984A".ToLowerInvariant(),
                UserName = "viewer",
                Email = "viewer@nonsense.com"
            };
            await RegisterUserIfNotExists(userManager, user, "Qwerty1234!");
            await userManager.AddClaimAsync(user, new Claim("role", "viewer"));
        }
    }

    private static async Task RegisterUserIfNotExists<TUser>(UserManager<TUser> userManager,
        TUser user, string password)
        where TUser : IdentityUser<string>
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


