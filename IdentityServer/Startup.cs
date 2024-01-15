using IdentityServer.Auth;
using IdentityServer.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace IdentityServer;

public class Startup(IWebHostEnvironment environment, IConfiguration configuration)
{
    public IWebHostEnvironment Environment { get; } = environment;
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

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
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtConfigurator.JwtIssuer,
                        ValidAudience = jwtConfigurator.JwtIssuer,
                        ClockSkew = TimeSpan.FromSeconds(3 * 60),
                        IssuerSigningKey = jwtConfigurator.SigningKey
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            ctx.Token = ctx.Request.Cookies[JwtConfigurator.JwtCookieName];
                            return Task.CompletedTask;
                        }
                    };
                });

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

    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }


        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();


        app.UseIdentityServer();

        // uncomment, if you want to add MVC
        //app.UseAuthorization();
        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapDefaultControllerRoute();
        //});
    }
}
