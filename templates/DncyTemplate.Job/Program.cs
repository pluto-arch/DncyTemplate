namespace DncyTemplate.Job
{
    using System;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    using Serilog;

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
                .UseSerilog(dispose: true);
        }


        /// <summary>
        ///     ��־����
        /// </summary>
        /// <returns></returns>
        private static IConfiguration GetLogConfig()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("serilogsetting.json", false, true);
            return builder.Build();
        }
    }
}