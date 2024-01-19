# Preparation

## NuGet packages

Normally only a few NuGet packages required to get started. If `IdentityServer4` packages are used, you may get warnings about `AutoMapper 12` having trouble to resolve some pre-release dependencies. Original `IdentityServer4` is no more maintained and is left with that umm... bug. First of all, let's swap all `IdentityServer4`.* packages for `Cnblogs.IdentityServer4`.* packages. These are the original sources fork and have dependencies fixed for us. But what about the rest? If we take a look in Visual Studio installed packages, we'd see a hundred of transient dependencies of prehistoric versions. We know, packages get updated, bugs fixed, some are deprecated of even vulnerable. So far, the only way to keep these up to date is to convert these into top-level dependencies. I haven't got a tool yet, but it can be done manually. Just click on each, install the latest version... It a tedious process and may take up to an hour. Some dependencies would not install without others having installed beforehand.

Below is a list that could be simply copied into your new server project to save a whole lot of time:

<details>
  <summary>.NET 8 NuGet dependencies</summary>

    <PackageReference Include="AutoMapper" Version="12.0.1" />
    <PackageReference Include="AutoMapper.Collection" Version="9.0.0" />
    <PackageReference Include="Azure.Core" Version="1.37.0" />
    <PackageReference Include="Azure.Identity" Version="1.10.4" />

    <PackageReference Include="Cnblogs.IdentityServer4" Version="4.2.1" />
    <PackageReference Include="Cnblogs.IdentityServer4.AccessTokenValidation" Version="3.1.0" />
    <PackageReference Include="Cnblogs.IdentityServer4.AspNetIdentity" Version="4.2.1" />
    <PackageReference Include="Cnblogs.IdentityServer4.EntityFramework" Version="4.2.1" />
    <PackageReference Include="Cnblogs.IdentityServer4.EntityFramework.Storage" Version="4.2.1" />
    <PackageReference Include="Cnblogs.IdentityServer4.Storage" Version="4.2.1" />
    <PackageReference Include="Humanizer.Core" Version="2.14.1" />
    <PackageReference Include="IdentityModel" Version="6.2.0" />
    <PackageReference Include="IdentityModel.AspNetCore.OAuth2Introspection" Version="6.2.0" />

    <PackageReference Include="Microsoft.AspNetCore.Authentication.Google" Version="8.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Authentication.OpenIdConnect" Version="8.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Cryptography.Internal" Version="8.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Cryptography.KeyDerivation" Version="8.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.1" />
    <PackageReference Include="Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore" Version="8.0.1" />

    <PackageReference Include="Microsoft.CodeAnalysis.Analyzers" Version="3.3.4">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.CodeAnalysis.Common" Version="4.8.0" />
    <PackageReference Include="Microsoft.CodeAnalysis.CSharp" Version="4.8.0" />
    <PackageReference Include="Microsoft.CodeAnalysis.CSharp.Workspaces" Version="4.8.0" />
    <PackageReference Include="Microsoft.CodeAnalysis.Workspaces.Common" Version="4.8.0" />

    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Abstractions" Version="8.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Analyzers" Version="8.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.1">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Relational" Version="8.0.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.1" />

    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.1.4" />
    <PackageReference Include="Microsoft.Data.SqlClient.SNI.runtime" Version="5.1.1" />
    <PackageReference Include="Microsoft.SqlServer.Server" Version="1.0.0" />

    <PackageReference Include="Microsoft.Extensions.Caching.Abstractions" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Caching.Memory" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="8.0.1" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyModel" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Diagnostics.Abstractions" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.FileProviders.Abstractions" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Hosting.Abstractions" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Identity.Core" Version="8.0.1" />
    <PackageReference Include="Microsoft.Extensions.Identity.Stores" Version="8.0.1" />
    <PackageReference Include="Microsoft.Extensions.Logging" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Logging.Abstractions" Version="8.0.0" />
    <PackageReference Include="Microsoft.Extensions.Options" Version="8.0.1" />
    <PackageReference Include="Microsoft.Extensions.Primitives" Version="8.0.0" />

    <PackageReference Include="Microsoft.Identity.Client" Version="4.58.1" />
    <PackageReference Include="Microsoft.Identity.Client.Extensions.Msal" Version="4.58.1" />
    <PackageReference Include="Microsoft.IdentityModel.Abstractions" Version="7.2.0" />
    <PackageReference Include="Microsoft.IdentityModel.JsonWebTokens" Version="7.2.0" />
    <PackageReference Include="Microsoft.IdentityModel.Logging" Version="7.2.0" />
    <PackageReference Include="Microsoft.IdentityModel.Protocols" Version="7.2.0" />
    <PackageReference Include="Microsoft.IdentityModel.Protocols.OpenIdConnect" Version="7.2.0" />
    <PackageReference Include="Microsoft.IdentityModel.Tokens" Version="7.2.0" />



    <PackageReference Include="Microsoft.CSharp" Version="4.7.0" />
    <PackageReference Include="Microsoft.NETCore.Platforms" Version="7.0.4" />
    <PackageReference Include="Microsoft.Bcl.AsyncInterfaces" Version="8.0.0" />
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="8.0.0" />
    <PackageReference Include="Microsoft.Win32.SystemEvents" Version="8.0.0" />

    <PackageReference Include="Mono.TextTemplating" Version="2.3.1" />

    <PackageReference Include="Newtonsoft.Json" Version="13.0.3" />

    <PackageReference Include="Serilog" Version="3.1.1" />
    <PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
    <PackageReference Include="Serilog.Formatting.Compact" Version="2.0.0" />
    <PackageReference Include="Serilog.Settings.Configuration" Version="8.0.0" />
    <PackageReference Include="Serilog.Extensions.Hosting" Version="8.0.0" />
    <PackageReference Include="Serilog.Extensions.Logging" Version="8.0.0" />
    <PackageReference Include="Serilog.Sinks.Console" Version="5.0.1" />
    <PackageReference Include="Serilog.Sinks.Debug" Version="2.0.0" />
    <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
    <PackageReference Include="Serilog.Sinks.TextWriter" Version="2.1.0" />

    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />

    <PackageReference Include="System.CodeDom" Version="8.0.0" />
    <PackageReference Include="System.Collections.Immutable" Version="8.0.0" />
    <PackageReference Include="System.Composition" Version="8.0.0" />
    <PackageReference Include="System.Composition.AttributedModel" Version="8.0.0" />
    <PackageReference Include="System.Composition.Convention" Version="8.0.0" />
    <PackageReference Include="System.Composition.Hosting" Version="8.0.0" />
    <PackageReference Include="System.Composition.Runtime" Version="8.0.0" />
    <PackageReference Include="System.Composition.TypedParts" Version="8.0.0" />
    <PackageReference Include="System.Configuration.ConfigurationManager" Version="8.0.0" />
    <PackageReference Include="System.Diagnostics.DiagnosticSource" Version="8.0.0" />
    <PackageReference Include="System.Diagnostics.EventLog" Version="8.0.0" />
    <PackageReference Include="System.Drawing.Common" Version="8.0.1" />
    <PackageReference Include="System.Formats.Asn1" Version="8.0.0" />
    <PackageReference Include="System.IdentityModel.Tokens.Jwt" Version="7.2.0" />
    <PackageReference Include="System.IO.FileSystem.AccessControl" Version="5.0.0" />
    <PackageReference Include="System.IO.Pipelines" Version="8.0.0" />
    <PackageReference Include="System.Memory" Version="4.5.5" />
    <PackageReference Include="System.Memory.Data" Version="8.0.0" />
    <PackageReference Include="System.Numerics.Vectors" Version="4.5.0" />
    <PackageReference Include="System.Reflection.Metadata" Version="8.0.0" />
    <PackageReference Include="System.Runtime.Caching" Version="8.0.0" />
    <PackageReference Include="System.Runtime.CompilerServices.Unsafe" Version="6.0.0" />
    <PackageReference Include="System.Security.AccessControl" Version="6.0.0" />
    <PackageReference Include="System.Security.Cryptography.Cng" Version="5.0.0" />
    <PackageReference Include="System.Security.Cryptography.ProtectedData" Version="8.0.0" />
    <PackageReference Include="System.Security.Principal.Windows" Version="5.0.0" />
    <PackageReference Include="System.Text.Encoding.CodePages" Version="8.0.0" />
    <PackageReference Include="System.Text.Encodings.Web" Version="8.0.0" />
    <PackageReference Include="System.Text.Json" Version="8.0.1" />
    <PackageReference Include="System.Threading.Channels" Version="8.0.0" />
    <PackageReference Include="System.Threading.Tasks.Extensions" Version="4.5.4" />

</details>

---

Actually, for a small project you may use in-memory `IdentityServer` and have it's contents populated from hard-coded values or even `appsettings.json` which we will use later.

But the big daddies go for the database...

## Entity Framework Core



### Database models and contexts

It will be very benefitial to create new derived classes for users and roles. This way you may even control the type of `Id` columns. Let's define these and (of course!) **ApplicationDbContext**:

```
public class ApplicationUser : IdentityUser<Guid> { }
public class ApplicationRole : IdentityRole<Guid> { }
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options) { }
```
<sup>Note a .NET8 C#12 primary constructor feature used in ApplicationDbContext class. We won't override anything and may keep it a one-liner. Sweet!</sup>

Having defined **ApplicationDbContext** we need to mention another two required database context classes that come from *IdentityServer4.EntityFramework.DbContexts* namespace: **ConfigurationDbContext** and **PersistedGrantDbContext**. Normally we don't need anything extra using those, but just to get ahead of future problems with database migration it's good to define design-time factories for those. It seems there are no problems migrating these having just started writing application code. But if we decide to go from scratch half-way to production server, we'll get a number of exceptions. One of them is a lack of parameter-less constructor that expects some options to be passed in. So, we may want to implement **IDesignTimeDbContextFactory<TDbContext>** interface which will help to pass **DbContextOptions<TDbContext>** into **DbContext** constructor. During *external* migration we'd have only a liited amount of services available. Since we want to define our database connection string in **appsettings.json** file, we need to configure reading from it. I've defined one factory myself, but to make life easier with **ConfigurationDbContext** and **PersistedGrantDbContext** I've found a base class somewhere on the internet:

<details>
  <summary>DesignTimeDbContextFactoryBase</summary>

```
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
```
</details>




