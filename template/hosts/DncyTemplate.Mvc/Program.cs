using DncyTemplate.Mvc.Infra.LogSetup;
using ILogger = Serilog.ILogger;

namespace DncyTemplate.Mvc;

public class Program
{
    public static readonly string AppName = typeof(Program).Namespace;

    public static void Main(string[] args)
    {
        var logConfig = SerilogConfig();
        Log.Logger = CreateSerilogLogger(logConfig);
        try
        {
            Log.Information("׼������{ApplicationContext}...", AppName);
            var host = BuildWebHost(args);
            Log.Information("{ApplicationContext} ������", AppName);
            host.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "{ApplicationContext} ���ִ���:{Messsage} !", AppName, ex.Message);
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static IHost BuildWebHost(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureLogging(builder =>
            {
                builder.Configure(option =>
                {
                    option.ActivityTrackingOptions = ActivityTrackingOptions.TraceId | ActivityTrackingOptions.SpanId;
                });
            })
            .ConfigureWebHostDefaults(webhost =>
            {
                webhost.UseStartup<Startup>()
                        .UseIISIntegration()
                        .CaptureStartupErrors(false);
            })
            .UseSerilog(dispose: true)
            .Build();
        return host;
    }


    #region serilog
    private static IConfiguration SerilogConfig()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("serilogsetting.json", false, true);
        return builder.Build();
    }
    private static ILogger CreateSerilogLogger(IConfiguration configuration)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.With<ActivityEnricher>()
            .CreateLogger();
    }
    #endregion

}