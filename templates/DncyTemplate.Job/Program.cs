namespace DncyTemplate.Job
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using System;
    using System.IO;

    public class Program
    {
        public static readonly string AppName = typeof(Program).Namespace;

        public static void Main(string[] args)
        {
            var baseConfig = GetLogConfig();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(baseConfig)
                .Enrich.WithProperty("ApplicationName", AppName)
                .CreateLogger(); ;
            try
            {
                Log.Information("����{ApplicationContext}...", AppName);
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{ApplicationContext} ���ִ���:{Messsage} !", AppName, ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseIISIntegration()
                        .CaptureStartupErrors(false);
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    IHostEnvironment env = context.HostingEnvironment;
                    IConfiguration baseConfig = GetConfiguration(env);
                    builder.AddConfiguration(baseConfig);
                })
                .UseSerilog(dispose: true);
        }


        /// <summary>
        ///     ��־����
        /// </summary>
        /// <returns></returns>
        private static IConfiguration GetLogConfig()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddJsonFile("serilogsetting.json", false, true);
            return builder.Build();
        }

        /// <summary>
        ///     ��������
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
    }
}