using ILogger = Serilog.ILogger;

namespace DncyTemplate.Api;
public class Program
{
    public static readonly string AppName = typeof(Program).Namespace;

    public static void Main(string[] args)
    {
        var logConfig = SerilogConfig();
        Log.Logger = CreateSerilogLogger(logConfig, AppName);
        try
        {
            Log.Information("准备启动{ApplicationContext}...", AppName);
            var host = BuildWebHost(args);
            Log.Information("{ApplicationContext} 已启动", AppName);
            host.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "{ApplicationContext} 出现错误:{Messsage} !", AppName, ex.Message);
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
    /// 加载应用配置
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
    /// 日志配置
    /// </summary>
    /// <returns></returns>
    private static IConfiguration SerilogConfig()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("serilogsetting.json", false, true);
        return builder.Build();
    }
    /// <summary>
    /// 从配置创建serilog
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
