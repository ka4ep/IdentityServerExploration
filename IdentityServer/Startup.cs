using IdentityServer.Auth;
using IdentityServer.Data;
using IdentityServer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer;

public class Startup(IWebHostEnvironment environment, IConfiguration configuration)
{
    public IWebHostEnvironment Environment { get; } = environment;
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();


        var migrationsResolver = new MigrationsResolver(Configuration);
        var jwtConfigurator = new JwtConfigurator(Configuration);

        services.AddDbContext<ApplicationDbContext>(migrationsResolver.SqlServerOptions);
        services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options => options.ConfigureDbContext = migrationsResolver.SqlServerOptions)
                .AddOperationalStore(options => options.ConfigureDbContext = migrationsResolver.SqlServerOptions)
#if DEBUG
                .AddDeveloperSigningCredential()
#else
                //.AddSigningCredential
#endif
                ;

        services.AddTransient<ICookieManager, ChunkingCookieManager>();

        services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
#if DEBUG
                    IdentityModelEventSource.ShowPII = true;
                    IdentityModelEventSource.LogCompleteSecurityArtifact = true;
#endif
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidIssuer = jwtConfigurator.JwtIssuer,
                        
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = jwtConfigurator.SigningKey,

                        ValidateAudience = false,
                        ValidAudience = jwtConfigurator.JwtIssuer,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(3 * 60),
                    };
/*
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            Log.Information($"Requst: [{ctx.Request.Method}] {ctx.Request.Path}");
                            ctx.Token = ctx.Request.Cookies[JwtConfigurator.JwtCookieName];
                            return Task.CompletedTask;
                        },                        
                    };
*/
                    
                });

        services.AddSingleton<HostAddressService>();

        /*
        var builder = services.AddIdentityServer(options =>
        {
            // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
            options.EmitStaticAudienceClaim = true;
        })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients);

        // not recommended for production - you need to store your key material somewhere secure
        builder.AddDeveloperSigningCredential();
        */
    }

    public void Configure(IApplicationBuilder app, IHost host)
    {
        var hostAddress = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
        app.ApplicationServices.GetRequiredService<HostAddressService>().HostAddress = hostAddress;

        if (string.IsNullOrWhiteSpace(hostAddress)) Log.Warning($"{nameof(HostAddressService)} could not get an address from {nameof(IServerAddressesFeature)}");
        else Log.Information($"Host address is {hostAddress}");

        MigrationsResolver.MigrateDatabaseAsync(host).GetAwaiter().GetResult();

        if (Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }


        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();


        app.UseAuthentication();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}
