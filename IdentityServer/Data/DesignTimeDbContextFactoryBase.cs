﻿using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System;

namespace IdentityServer.Data;

public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
{
    protected string ConnectionStringName { get; }
    protected String MigrationsAssemblyName { get; }
    public DesignTimeDbContextFactoryBase(string connectionStringName, string migrationsAssemblyName)
    {
        ConnectionStringName = connectionStringName;
        MigrationsAssemblyName = migrationsAssemblyName;
    }

    public TContext CreateDbContext(string[] args)
    {
        return Create(
            Directory.GetCurrentDirectory(),
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            ConnectionStringName, MigrationsAssemblyName);
    }
    protected abstract TContext CreateNewInstance(
        DbContextOptions<TContext> options);

    public TContext CreateWithConnectionStringName(string connectionStringName, string migrationsAssemblyName)
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var basePath = AppContext.BaseDirectory;
        return Create(basePath, environmentName, connectionStringName, migrationsAssemblyName);
    }

    private TContext Create(string basePath, string environmentName, string connectionStringName, string migrationsAssemblyName)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{environmentName}.json", true)
            .AddEnvironmentVariables();

        var config = builder.Build();

        var connstr = config.GetConnectionString(connectionStringName);

        if (string.IsNullOrWhiteSpace(connstr) == true)
            throw new InvalidOperationException("Could not find a connection string named 'default'.");

        return CreateWithConnectionString(connstr, migrationsAssemblyName);
    }

    private TContext CreateWithConnectionString(string connectionString, string migrationsAssemblyName)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentException($"{nameof(connectionString)} is null or empty.", nameof(connectionString));

        var optionsBuilder = new DbContextOptionsBuilder<TContext>();

        Console.WriteLine("{1}: Connection string: {0}", connectionString, GetType().Name);

        optionsBuilder.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.MigrationsAssembly(migrationsAssemblyName));

        DbContextOptions<TContext> options = optionsBuilder.Options;

        return CreateNewInstance(options);
    }
}