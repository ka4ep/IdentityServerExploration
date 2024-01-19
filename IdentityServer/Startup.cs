using IdentityServer.Auth;
using IdentityServer.Controllers;
using IdentityServer.Data;
using IdentityServer.Helpers;
using IdentityServer.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

        
        services.AddOptions();
        services.AddOptionsWithValidateOnStart<IdentityServerConfigurationOptions>().BindConfiguration(MigrationsResolver.IdentityServerKey);
        services.AddOptionsWithValidateOnStart<JwtConfiguratorOptions>().BindConfiguration(JwtConfigurator.JwtSection);

        var migrationsResolver = new MigrationsResolver(Configuration);
        var jwtConfigurator = new JwtConfigurator(Configuration.GetSection(JwtConfigurator.JwtSection).Get<JwtConfiguratorOptions>());
        services.AddSingleton(jwtConfigurator);


        services.AddDbContext<ApplicationDbContext>(migrationsResolver.SqlServerOptions);
        
        services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        services.AddIdentityServer()                
                .AddAspNetIdentity<ApplicationUser>()
                .AddConfigurationStore(options => options.ConfigureDbContext = migrationsResolver.SqlServerOptions)
                .AddOperationalStore(options => options.ConfigureDbContext = migrationsResolver.SqlServerOptions)
                //.AddDeveloperSigningCredential()
                .AddSigningCredential(new X509Certificate2(
                    jwtConfigurator.Options.CertificatePath,
                    jwtConfigurator.Options.CertificatePass
                    ))
                ;
        

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
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
                        ValidIssuer = jwtConfigurator.Options.ValidIssuer,
                        ValidateIssuer = jwtConfigurator.Options.ValidateIssuer,
                        

                        ValidAudience = jwtConfigurator.Options.ValidAudience,
                        ValidateAudience = jwtConfigurator.Options.ValidateAudience,

                        IssuerSigningKey = jwtConfigurator.SigningKey,
                        IssuerSigningKeys = [jwtConfigurator.SigningKey],
                        ValidateIssuerSigningKey = jwtConfigurator.Options.ValidateIssuerSigningKey,

                        ValidateLifetime = jwtConfigurator.Options.ValidateTokenLifespan,                        
                        ClockSkew = TimeSpan.FromSeconds(3 * 60),
#if DEBUG
                        LogValidationExceptions = true,
                        LogTokenId = true,
#endif                        
                    };
#if DEBUG
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            if (ctx.Request.Path.HasValue)
                            {
                                if (ctx.Request.Path.Value == $"/{nameof(HomeController.FetchLogs)}")
                                    return Task.CompletedTask;
                                Log.Information($"Request: [{ctx.Request.Method}] {ctx.Request.Path}");
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = ctx =>
                        {
                            Log.Warning($"Request: [{ ctx.Request.Method}] {ctx.Request.Path} failed: {ctx.Exception.GetErrorMessage()}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = ctx =>
                        {
                            Log.Information($"Is authenticated : {ctx.Principal.Identity?.IsAuthenticated ?? false}");
                            ctx.Principal.Claims.Select(c => $"{c.Type} {c.Value}").ToList().ForEach(Log.Debug);
                            return Task.CompletedTask;
                        },
                        OnForbidden = ctx =>
                        {
                            Log.Warning($"Authorization failed [{(int)ctx.Response.StatusCode}] {ctx.Principal?.Identity.Name}");
                            return Task.CompletedTask;
                        },
                    };
#endif

                });
        services.AddAuthorizationBuilder()
            .AddPolicy("Admin", policy => policy.RequireRole("admin"))
            .AddPolicy("Viewer", policy =>
            {
                policy.RequireAssertion(ctx => 
                ctx.User.IsInRole("admin") || ctx.User.IsInRole("viewer"));
            });

        services.AddScoped<IProfileService, ProfileService>();
        services.AddTransient<ICookieManager, ChunkingCookieManager>();
        services.AddSingleton<HostAddressService>();

    }

    public void Configure(IApplicationBuilder app, IHost host)
    {
        var hostAddress = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.FirstOrDefault();
        app.ApplicationServices.GetRequiredService<HostAddressService>().HostAddress = hostAddress;

        if (string.IsNullOrWhiteSpace(hostAddress)) Log.Warning($"{nameof(HostAddressService)} could not get an address from {nameof(IServerAddressesFeature)}");
        else Log.Information($"Host address is {hostAddress}");

        MigrationsResolver.MigrateDatabaseAsync(host).GetAwaiter().GetResult();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        if (Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
            Seed.PrepareDataAsync(host).GetAwaiter().GetResult();
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
