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

}


