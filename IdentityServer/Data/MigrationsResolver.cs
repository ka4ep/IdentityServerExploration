using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace IdentityServer.Data;

public class MigrationsResolver
{


    private static readonly string _migrationsAssemblyName = typeof(ApplicationDbContext).Assembly.GetName().Name ??
                                                   typeof(ApplicationDbContext).Assembly.GetName().FullName;

    private readonly IConfiguration _configuration;

    public MigrationsResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Action<DbContextOptionsBuilder> SqlServerOptions => new(builder => builder.UseSqlServer(options =>
    {
        options.MigrationsAssembly(_migrationsAssemblyName);
        options.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(15), errorNumbersToAdd: null);
    }));


}
