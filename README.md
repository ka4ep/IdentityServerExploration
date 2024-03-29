# Preparation

## NuGet packages

Normally only a few NuGet packages required to get started. If *IdentityServer4* packages are used, you may get warnings about *AutoMapper 12* having trouble to resolve some pre-release dependencies. Original *IdentityServer4* is no more maintained and is left with that umm... bug. First of all, let's swap all *IdentityServer4*.* packages for **Cnblogs.IdentityServer4**.* packages. These are the original sources fork and have dependencies fixed for us. But what about the rest? If we take a look in Visual Studio installed packages, we'd see a hundred of transient dependencies of prehistoric versions. We know, packages get updated, bugs fixed, some are deprecated of even vulnerable. So far, the only way to keep these up to date is to convert these into top-level dependencies. I haven't got a tool yet, but it can be done manually. Just click on each, install the latest version... It a tedious process and may take up to an hour. Some dependencies would not install without others having installed beforehand.

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

## Entity Framework Core

Actually, for a small project you may use in-memory *IdentityServer* and have it's contents populated from hard-coded values or even *appsettings.json* which will be shown later.

But the big daddies go for the database...

### Database models and contexts

It will be very benefitial to create new derived classes for users and roles. This way you may even control the type of `Id` columns. Let's define these and (of course!) **ApplicationDbContext**:

```cs
public class ApplicationUser : IdentityUser<Guid> { }
public class ApplicationRole : IdentityRole<Guid> { }
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options) { }
```
<sup>Note a .NET8 C#12 primary constructor feature used in ApplicationDbContext class. We won't override anything and may keep it a one-liner. Sweet!</sup>

Having defined *ApplicationDbContext* we need to mention another two required database context classes that come from *IdentityServer4.EntityFramework.DbContexts* namespace: **ConfigurationDbContext** and **PersistedGrantDbContext**. Normally we don't need anything extra using theese, but just to get ahead of future problems with database migration, it's good to define design-time factories for aforementioned database contexts. It seems there are no problems migrating these having just started writing application code. But if we decide to reset everything on a half-way to a live production server, we'll get a number of exceptions. One of them is a lack of parameter-less constructor that expects some options to be passed in. So, we may want to implement **IDesignTimeDbContextFactory<TDbContext>** interface which will help to pass *DbContextOptions<TDbContext>* into *DbContext* constructor. During *external* migration we'd have only a small amount of services available in *IServiceProvider*. Since we want to define our database connection string in *appsettings.json* file, we need to help reading from it. I've defined one factory myself, but to make life easier with *ConfigurationDbContext* and *PersistedGrantDbContext* I've found a base class somewhere on the internet:

<details>
  <summary>DesignTimeDbContextFactoryBase</summary>

```cs
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

<details>
  <summary>This is how factory classes look now</summary>

```cs
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
    public ConfigurationContextDesignTimeFactory() : base(MigrationsResolver.ConnectionKey, typeof(Startup).GetTypeInfo().Assembly.GetName().Name)
    {
    }

    protected override ConfigurationDbContext CreateNewInstance(DbContextOptions<ConfigurationDbContext> options)
    {
        return new ConfigurationDbContext(options, new ConfigurationStoreOptions());
    }
}

public class PersistedGrantContextDesignTimeFactory : DesignTimeDbContextFactoryBase<PersistedGrantDbContext>
{
    public PersistedGrantContextDesignTimeFactory() : base(MigrationsResolver.ConnectionKey, typeof(Startup).GetTypeInfo().Assembly.GetName().Name)
    {
    }

    protected override PersistedGrantDbContext CreateNewInstance(DbContextOptions<PersistedGrantDbContext> options)
    {
        return new PersistedGrantDbContext(options, new OperationalStoreOptions());
    }
}
// MigrationsResolver.ConnectionKey is section string 'DefaultConnection'
```  
</details>

### External database migration

Let's prepare a database migration (changes list, rollback list) and actually update the database to have our tables created along with **Id** columns being of type **Guid** we defined earlier.

```bat
dotnet ef migrations add InitialIdentityServerApplicationDbMigration -c ApplicationDbContext -o Data/Migrations/IdentityServer/ApplicationDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context ConfigurationDbContext
dotnet ef database update --context PersistedGrantDbContext
```
Now we should have 30 *IdentityServer* tables created for us plus *_EFMigrationsHistory*.

### Certificates

Although we may create certificates at any time, let's just get over with it now.

```bat
"C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\MakeCert" -n "CN=localhost" -a sha256 -sv debug.pvk -r debug.cer
"C:\Program Files (x86)\Windows Kits\10\bin\10.0.22621.0\x64\pvk2pfx" -pvk debug.pvk -spc debug.cer -pfx debug.pfx -pi passwordEnteredEarlier
```
* Change Windows version *22621* to any that's in your 'Kits' directory.
* I'm not sure if *CN=localhost* is important here. Probably. I have no problems since I host my server on https://localhost:5001
* Your password will go along the certificate path in configuration.

# Configuration

### Startup.cs

Yes, It's a legacy way to set up things and I got it from *IdentityServer* template. Didn't bother to swap for top-level statements in *Program.cs*.

I've tried to keep the most settings in **appsettings.json** file so that we wouldn't have to ship a new version for any little change. Basically, we tell *IdentityServer* that we want to use *EntityFrameworkCore* with, say *SqlServer* and the same database, and migrations are there too. Note, that we don't use *.AddDeveloperSigningCredential()* even for debugging purposes. Instead, we use our newly created certificate and read it's path and password from *appsettings.json* *Jwt* section. Our helper class *JwtConfigurator* does the same. Class *JwtConfiguratorOptions* holds settings for *TokenValidationParameters* in *AddJwtBearer* method.

Quite *important* scope registered class **ProfileService** - this is our implementation of **IProfileService**. I found this one on the internet too :) It matches claimed roles of an authenticated user with ones registered in the database and passes what's matched into the middleware context. According to that, the server will either grant access to a controller method, or throw a 403 Forbidden status code. That's where kind of magic happens. The *DefaultProfileService* out-of-the-box actually doesn't do anything interesting except for logging. But we automatically add users' roles to an IssuedClaims list, which then is checked against [Authorize] attribute requirements.

Next, there's an **.AddAuthorizationBuilder()** call with a policies setup, that's also really important. But we'll get there [later](#authbuilder).

### Program.cs

The only thing worth mentioning is I override *Serilog* settings for particular namespaces that unclutter my logs of warnings - some stuff that I can't resolve for now (or package developers will fix back one day).

### appsettings.json

Being mentioned so many times, we haven't looked in it yet. Let's do so:

<details>
  <summary>appsettings.json</summary>

```cs
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=DOOM\\DOOM;Database=IdentityDb;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=No"
  },
  "Jwt": {
    "CertificatePath": "epasaule.pfx",
    "CertificatePass": "epasaule",

    "ValidIssuer": "https://localhost:5001",
    "ValidateIssuer": false,
    "ValidateIssuerSigningKey": false,

    "ValidAudience": "https://localhost:5001",
    "ValidateAudience": false,

    "TokenLifespan": 1800,
    "ValidateTokenLifespan": false
  },
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5000"
      },
      "Https": {
        "Url": "https://localhost:5001"
        //,
        //"Certificate": {
        //  "Path": "epasaule.pfx",
        //  "Password": "epasaule.lv"
        //}
      }
    }
  },
  "IdentityServerAccess": {
    "Clients": [
      {
        "ClientId": "Test",
        "ClientName": "Test client",
        "AllowedGrantTypes": [
          {
            "GrantType": "password"
          }
        ],
        "RequireClientSecret": false,
        "RequirePkce": true,
        "AllowOfflineAccess": false,
        "AllowAccessTokensViaBrowser": false,
        "AlwaysSendClientClaims": true,
        "AlwaysIncludeUserClaimsInIdToken": true,
        "AccessTokenLifetime": 3600,
        "IdentityTokenLifetime": 300,
        "AllowedCorsOrigins": [
          {
            "Origin": "http://localhost:5000"
          },
          {
            "Origin": "https://localhost:5001"
          }
        ],
        "RedirectUris": [
          {
            "RedirectUri": "https://localhost:5001/callback.html"
          },
          {
            "RedirectUri": "https://localhost:5001/callback-silent.html"
          }
        ],
        "PostLogoutRedirectUris": [
          {
            "PostLogoutRedirectUri": "https://localhost:5001/index.html"
          }
        ],
        "AllowedScopes": [
          {
            "Scope": "openid"
          },
          {
            "Scope": "roles"
          },
          {
            "Scope": "profile"
          }//,
          //{
          //  "Scope": "api1"
          //}
        ]
      }
    ],
    "IdentityResources": [
      {
        "Name": "openid"
      },
      {
        "Name": "profile"
      },
      {
        "Name": "email"
      },
      {
        "Name": "roles"
      }
    ]//,
    //"ApiScopes": [
    //  {
    //    "Name": "roles2"
    //  }
    //],
    //"ApiResources": [
    //  {
    //    "Name": "api1",
    //    "Scopes": [
    //      {
    //        "Scope": "roles"
    //      }
    //    ],
    //    "UserClaims": [
    //      {
    //        "Type": "role"
    //      }
    //    ]
    //  }
    //]
  }
}
```  
</details>

We have **ConnectionStrings.DefaultConnection** for our database. There is 'original' **Kestrel** section for which we can find some information on the internet and tune however we want. Plus, endpoint address is there too - we'll want to use it in other sections.

Next, **Jwt** section is of our own C# type **JwtConfiguratorOptions**. It defines what certificate we use, what happens with all (some, actually) validations we may require. *CertificatePath* and *CertificatePass* (for password) are crucial for our server. That **IssuerSigningKey**(-s) can be of a *SymmetricSecurityKey* type, but as fas as I've tried, internally it gets validated against *RsaSecurityKey* and fails. Error says something like *kid/KeyId* are not found or incorrect, even if I've explicitly defined one! The only option left is to use *RsaSecurityKey*. No problem with that so far.

Moving on to **IdentityServerAccess** section. It corresponds to our *IdentityServerConfigurationOptions* class with lists of original models from *IdentityServer4.EntityFramework.Entities* namespace. That's why some arrays look that ugly: objects have a few properties, but only the ones I've put are required. Again, we may consider rolling our own classes, or may be just use models from *IdentityServer4.Models* namespace and then convert them using *.ToEntity()* extension methods. Any approach will do.

# Identity Server

Let's discuss and try to understand, what parts of the *IdentityServer* configuration is required for role-based web login/password driven authorization.

### IdentityResources

It's a list of scope keywords that server is happy to accept if authenticated user would call /connect/userinfo and pass his access token (along with scope keywords). Scope `openid` is *mandatory* to match user by id with the one in the database. Keywords
* *sub name family_name given_name middle_name nickname preferred_username profile picture website gender birthdate zoneinfo locale updated_at*
are optional.

### ApiScopes

Doesn't seem to be used in our role-based password flow. I had an error saying ApiScope 'roles' cannot be the same as some other scopes. I can't reproduce it, but just beware...

### ApiResources

Same story here - doesn't seem to be used in role-based password flow.

### Clients

This is one of allowed clients - our web-based one. GrantType - password, does not require client secret (it's opaque in sources via browser dev console), require PKCE. We don't use *offline access* to feedle around with refresh tokens. We don't yet use redirect uri, nor post logout redirect uri. What is interesting - *allowed scopes*. Obviously, we allow *openid*, we want *profile* too.

## What about the users?

Basic user config is actually quite simple. We need an *Id* of type *Guid* (remember [Database models and contexts] section?), *UserName*, maybe *Email*. We register them providing password that will be hashed and salted in the database. We as well add roles to users. One day we'll be able to set users' roles from dashboard UI.

<details>
  <summary>Seed the users just for the tests</summary>
  
```cs
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
```  
</details>

We've got `admin` and `viewer` users.

## Controllers and their methods

Let's pretend for a moment that we'll user roles as *roles*. Just what documentation states. We'll test access using an *ExampleController*:

```cs
[Route("api/[controller]")]
[ApiController]
public class ExampleController : ControllerBase
{
    [Authorize]
    [HttpGet(nameof(Test))]
    public async Task<IActionResult> Test()
    {
        await Response.WriteAsJsonAsync($"{{ \"Test\": \"Successful\" }}");
        return new EmptyResult();
    }

    [Authorize(Roles = "admin")]
    [HttpGet(nameof(AdminOnly))]
    public async Task<IActionResult> AdminOnly()
    {
        await Response.WriteAsJsonAsync($"{{ \"Administrator\": \"Allowed\" }}");
        return new EmptyResult();
    }
    
    [Authorize(Roles = "admin viewer")]
    [HttpGet(nameof(AdminAndInvoiceOnly))]
    public async Task<IActionResult> AdminAndInvoiceOnly()
    {
        await Response.WriteAsJsonAsync($"{{ \"Administrator\": \"Allowed\", \"InvoiceOperator\": \"Allowed\" }}");
        return new EmptyResult();
    }
}
```
We've put required roles within [Authorize()] attribute. Role names are **case-sensitive**!
* Method `Test` is accessible by any authenticated user
* Method `AdminOnly` is accessible by anyone with role `admin`.
* Method `AdminAndInvoiceOnly` is accessible by... **nobody**.

#### What could possibly go wrong?

Let's see. Controller method, as we think, should have [Authorize(Roles = "admin viewer")]. This means, the user must have **both** roles and not *any of*. Event if we split the attribute into a few having one role each - all of them sum up.

#### Resolution?

Now, **the salt of it all**. My numerous attempts to get sole roles working turned into a pumpkin. It's been said somewhere that role-based authorization is not very practical and it's best to go for policies. Let's redefine attributes for those two methods:

```cs
    [Authorize(Policy = "Admin")]
    [HttpGet(nameof(AdminOnly))]
    public async Task<IActionResult> AdminOnly()
    {
        await Response.WriteAsJsonAsync($"{{ \"Administrator\": \"Allowed\" }}");
        return new EmptyResult();
    }
    
    [Authorize(Policy = "Viewer")]
    [HttpGet(nameof(AdminAndInvoiceOnly))]
    public async Task<IActionResult> AdminAndInvoiceOnly()
    {
        await Response.WriteAsJsonAsync($"{{ \"Administrator\": \"Allowed\", \"InvoiceOperator\": \"Allowed\" }}");
        return new EmptyResult();
    }
```
To make these policies work, we have to <a id="authbuilder">define</a> them in the `Startup.cs` file:

```cs
        services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy => policy.RequireRole("admin"))
            .AddPolicy("Viewer", policy =>
            {
                policy.RequireAssertion(ctx => 
                ctx.User.IsInRole("admin") || ctx.User.IsInRole("viewer"));
            });
```
Now both methods' authorization works as expected, hence the ***or*** within assertion. This could be anything we want, including database check.

> An important point-of-view change: we'll use ***Policies*** as *roles* and ***Roles*** as some sort of actions.

Policies allow for us to use flexible *this **or** that*  match, rather than fixed *this **and** that*. So, we may allow admin or viewer, not just someone who is both admin and viewer. An admin role may include viewer, but not otherwise. An authorization will fail this way.

#### Future thoughts

Defining hard-coded policies is not very practical in both *Startup.cs* and the *controllers*. Ideally, just as assumption, we could:
* Utilize *ApiResources* to enlist our controllers and their methods. Possibly, in a dynamic manner on start.
* We wouldn't use public policies in [Authorize] attributes, may be just technical ones or a single one.
* We could even create a custom [DatabaseAuthorization] for that. Yeah, naming is hard.
* Create our custom `ApiPolicies` table with a policy name and a set of permitted roles.
* `AddAuthorizationBuilder` would have a one/few technical policies defined, but the `AuthorizationPolicyBuilder` would check access against the database.
* It's best to have a service written just for that, cause we not only will require an UI editor for that, but also a quick API check if this or that resource is available to current logged in user. UI friendly stuff.

## Oh, the calling client

It could be a .NET console app, like the one I've made. For a `password`-flow it's only required to pass a *ClientId*, `openid profile` *scopes*, *login* and *password* to get an *access token*. Then just call your methods including that token and you'll be just fine.

