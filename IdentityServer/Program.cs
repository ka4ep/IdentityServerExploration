using IdentityServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;

namespace IdentityServer;

public static class Program
{
    public static readonly LogSink logSink = new(100);
    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Verbose)
            .MinimumLevel.Override("Microsoft.AspNetCore.Authentication.JwtBearer", LogEventLevel.Information)
            .Enrich.FromLogContext()
            // uncomment to write to Azure diagnostics stream
            //.WriteTo.File(
            //    @"D:\home\LogFiles\Application\identityserver.txt",
            //    fileSizeLimitBytes: 1_000_000,
            //    rollOnFileSizeLimit: true,
            //    shared: true,
            //    flushToDiskInterval: TimeSpan.FromSeconds(1))
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
            .WriteTo.TextWriter(logSink.Receiver)
            .CreateLogger();
        

        try
        {
            Log.Information("Starting host...");
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly.");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}