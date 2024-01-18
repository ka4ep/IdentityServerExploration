using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace IdentityServer.Data;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddJsonFile("appsettings.json");
        var configuration = configBuilder.Build();
        var resolver = new MigrationsResolver(configuration);
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        resolver.SqlServerOptions(builder);
        return new ApplicationDbContext(builder.Options);
    }
}

public class ConfigurationContextDesignTimeFactory : DesignTimeDbContextFactoryBase<ConfigurationDbContext>
{
    public ConfigurationContextDesignTimeFactory()
        : base("DefaultConnection", typeof(Startup).GetTypeInfo().Assembly.GetName().Name)
    {
    }

    protected override ConfigurationDbContext CreateNewInstance(DbContextOptions<ConfigurationDbContext> options)
    {
        return new ConfigurationDbContext(options, new ConfigurationStoreOptions());
    }
}

public class PersistedGrantContextDesignTimeFactory : DesignTimeDbContextFactoryBase<PersistedGrantDbContext>
{
    public PersistedGrantContextDesignTimeFactory()
        : base("DefaultConnection", typeof(Startup).GetTypeInfo().Assembly.GetName().Name)
    {
    }

    protected override PersistedGrantDbContext CreateNewInstance(DbContextOptions<PersistedGrantDbContext> options)
    {
        return new PersistedGrantDbContext(options, new OperationalStoreOptions());
    }
}
