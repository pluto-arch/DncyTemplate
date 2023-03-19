using Serilog.Enrichers.Sensitive;
using ILogger = Serilog.ILogger;

namespace DncyTemplate.Mvc;

public class Program
{
    public static readonly string AppName = typeof(Program).Namespace;

    public static void Main(string[] args)
    {
        var logConfig = SerilogConfig();
        Log.Logger = CreateSerilogLogger(logConfig, AppName);
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
    /// <summary>
    /// ��־����
    /// </summary>
    /// <returns></returns>
    private static IConfiguration SerilogConfig()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("serilogsetting.json", false, true);
        return builder.Build();
    }
    /// <summary>
    /// �����ô���serilog
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="applicationName"></param>
    /// <returns></returns>
    private static ILogger CreateSerilogLogger(IConfiguration configuration, string applicationName)
    {
        return new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithProperty("AppName", applicationName)
            .Enrich.WithSensitiveDataMasking(o =>
            {
                // TODO config Sensitive filter rules
                // https://github.com/serilog-contrib/Serilog.Enrichers.Sensitive
            })
            .CreateLogger();
    }
    #endregion

}