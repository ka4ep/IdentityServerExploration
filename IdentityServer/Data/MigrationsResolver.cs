using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace IdentityServer.Data;

public partial class MigrationsResolver(IConfiguration configuration)
{
    public const string ConnectionKey = "DefaultConnection";
    public const string IdentityServerKey = "IdentityServerAccess";


    private static readonly string _migrationsAssemblyName = typeof(ApplicationDbContext).Assembly.GetName().Name ??
                                                             typeof(ApplicationDbContext).Assembly.GetName().FullName;


    public Action<DbContextOptionsBuilder> SqlServerOptions => new(builder =>
    {
        var connectionString = configuration.GetConnectionString(ConnectionKey) ?? throw new InvalidOperationException($"appsettings.json ConnectionStrings:{ConnectionKey} could not be read");
        builder.UseSqlServer(connectionString, options =>
        {
            options.MigrationsAssembly(_migrationsAssemblyName);
            options.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(15), errorNumbersToAdd: null);
        });
    });



}


