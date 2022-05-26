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
            .ConfigureAppConfiguration((context, builder) =>
            {
                IHostEnvironment env = context.HostingEnvironment;
                IConfiguration baseConfig = GetConfiguration(env);
                builder.AddConfiguration(baseConfig);
            })
            .UseSerilog(dispose: true)
            .Build();
        return host;
    }


    /// <summary>
    /// ����Ӧ������
    /// </summary>
    /// <param name="env"></param>
    /// <returns></returns>
    private static IConfiguration GetConfiguration(IHostEnvironment env)
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", false, true)
            .AddEnvironmentVariables();
        return builder.Build();
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
            .CreateLogger();
    }
    #endregion

}