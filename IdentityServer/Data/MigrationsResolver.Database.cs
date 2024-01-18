using IdentityServer.Auth;
using IdentityServer.Services;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ISEF = IdentityServer4.EntityFramework.Entities;

namespace IdentityServer.Data;

public partial class MigrationsResolver
{
    /// <summary>
    /// Prepare database structure changes and push configuration from appsettings.json file if available. Otherwise, <see cref="DefaultConfig"/> is used.
    /// </summary>
    /// <param name="host"></param>
    /// <returns></returns>
    public static async Task<IHost> MigrateDatabaseAsync(IHost host)
    {
        try
        {
            Log.Information($"Preparing database migration");
            var environment = host.Services.GetRequiredService<IHostEnvironment>();

            using var scope = host.Services.CreateAsyncScope();
            var hostAddressService = scope.ServiceProvider.GetRequiredService<HostAddressService>();

            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
            var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            Log.Information($"Running {nameof(ApplicationDbContext)} migration");
            await applicationDbContext.Database.MigrateAsync();
            Log.Information($"Running {nameof(PersistedGrantDbContext)} migration");
            await persistedGrantDbContext.Database.MigrateAsync();
            Log.Information($"Running {nameof(ConfigurationDbContext)} migration");
            await configurationDbContext.Database.MigrateAsync();

            Log.Information($"Reading appsettings.json file '{IdentityServerKey}' section configuration");
            var fromFile = false;
            var fileConfiguration = scope.ServiceProvider.GetService<IOptionsMonitor<IdentityServerConfigurationOptions>>();
            if (fromFile = fileConfiguration?.CurrentValue is not null)
                Log.Information($"File configuration is present, values are to be replaced in the database if present.");
            else
                Log.Warning($"File configuration is not present, checking against embedded {nameof(DefaultConfig)}.");


            var clients = (fromFile ? fileConfiguration.CurrentValue.Clients.AsEnumerable() : DefaultConfig.GetClients(hostAddressService).Select(x => x.ToEntity())).ToList();
            var scopes = (fromFile ? fileConfiguration.CurrentValue.ApiScopes.AsEnumerable() : DefaultConfig.Scopes.Select(x => x.ToEntity())).ToList();
            var resources = (fromFile ? fileConfiguration.CurrentValue.ApiResources.AsEnumerable() : DefaultConfig.Resources.Select(x => x.ToEntity())).ToList();
            var identities = (fromFile ? fileConfiguration.CurrentValue.IdentityResources.AsEnumerable() : DefaultConfig.IdentityResources.Select(x => x.ToEntity())).ToList();

            // TODO: Create and use migration options. Let src(empty) src(force) dst(empty) -- if src(empty,force) => clear db tables, if src(empty) => ignore, if src(empty),dst(empty) => ignore, if src,dst(empty) => insert
            // TODO: For now, cleaning and inserting

            if (clients.Count > 0)
            {
                Log.Warning($"(For now...) Replacing {nameof(ConfigurationDbContext)}.{nameof(ConfigurationDbContext.Clients)} with {clients.Count} {nameof(configurationDbContext.Clients)}");
                configurationDbContext.Clients.RemoveRange(await configurationDbContext.Clients.ToListAsync());
                configurationDbContext.Clients.AddRange(clients);
                await configurationDbContext.SaveChangesAsync();
            }
            else Log.Debug($"No {nameof(ConfigurationDbContext)}.{nameof(ConfigurationDbContext.Clients)} to insert/update");

            if (scopes.Count > 0)
            {
                Log.Warning($"(For now...) Replacing {nameof(ConfigurationDbContext)}.{nameof(ConfigurationDbContext.ApiScopes)} with {scopes.Count} {nameof(configurationDbContext.ApiScopes)}");
                configurationDbContext.ApiScopes.RemoveRange(await configurationDbContext.ApiScopes.ToListAsync());
                configurationDbContext.ApiScopes.AddRange(scopes);
                await configurationDbContext.SaveChangesAsync();
            }
            else Log.Debug($"No {nameof(ConfigurationDbContext)}.{nameof(ConfigurationDbContext.ApiScopes)} to insert/update");

            if (resources.Count > 0)
            {
                Log.Warning($"(For now...) Replacing {nameof(ConfigurationDbContext)}.{nameof(ConfigurationDbContext.ApiResources)} with {resources.Count} {nameof(configurationDbContext.ApiResources)}");
                configurationDbContext.ApiResources.RemoveRange(await configurationDbContext.ApiResources.ToListAsync());
                configurationDbContext.ApiResources.AddRange(resources);
                await configurationDbContext.SaveChangesAsync();
            }
            else Log.Debug($"No {nameof(ConfigurationDbContext)}.{nameof(ConfigurationDbContext.ApiResources)} to insert/update");

            if (identities.Count > 0)
            {
                Log.Warning($"(For now...) Replacing {nameof(ConfigurationDbContext)}.{nameof(ConfigurationDbContext.IdentityResources)} with {identities.Count} {nameof(configurationDbContext.IdentityResources)}");
                configurationDbContext.IdentityResources.RemoveRange(await configurationDbContext.IdentityResources.ToListAsync());
                configurationDbContext.IdentityResources.AddRange(identities);
                await configurationDbContext.SaveChangesAsync();
            }
            else Log.Debug($"No {nameof(ConfigurationDbContext)}.{nameof(ConfigurationDbContext.IdentityResources)} to insert/update");

        }
        catch (Exception ex)
        {
            Log.Fatal($"Database migration failed");
            Log.Fatal(ex.ToString());
            throw;
        }
        Log.Information($"Database migration finished");
        return host;
    }

    private static readonly IEqualityComparer<ISEF.Client> _clientComparer = GenericComparer<ISEF.Client>.CreateHash(x => $"{x?.ClientId}{x?.ClientName}".GetHashCode());
    private static readonly IEqualityComparer<ISEF.IdentityResource> _identityResourceComparer = GenericComparer<ISEF.IdentityResource>.CreateHash(x => $"{x?.Id}{x?.Name}".GetHashCode());
    private static readonly IEqualityComparer<ISEF.ApiScope> _apiScopeComparer = GenericComparer<ISEF.ApiScope>.CreateHash(x => $"{x?.Id}{x?.Name}".GetHashCode());
    private static readonly IEqualityComparer<ISEF.ApiResource> _apiResourceComparer = GenericComparer<ISEF.ApiResource>.CreateHash(x => $"{x?.Id}{x?.Name}".GetHashCode());

}
