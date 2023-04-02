using Dncy.Tools;
using DncyTemplate.Api.BackgroundServices;
using DncyTemplate.Api.Infra.ApiDoc;
using DncyTemplate.Api.Infra.Tenancy;
using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Threading.RateLimiting;

namespace DncyTemplate.Api;
public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        #region 缓存
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 10240;
        });
        #endregion


        services.AddApplicationModule(Configuration);
        services.AddInfraModule(Configuration);
        services.AddDomainModule();

        #region background service
        services.AddHostedService<PrductBackgroundService>();
        #endregion


        #region 速率限制
        services.AddRateLimiter(options =>
        {
            //options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            //    RateLimitPartition.GetFixedWindowLimiter(
            //        partitionKey: httpContext.Connection.RemoteIpAddress?.ToNumber().ToString(),
            //        factory: partition => new FixedWindowRateLimiterOptions
            //        {
            //            AutoReplenishment = true,
            //            PermitLimit = 50,
            //            QueueLimit = 10,
            //            Window = TimeSpan.FromMinutes(1)
            //        }));

            // action or controller use [EnableRateLimiting("Api")] to enable this policy
            options.AddPolicy("home.RateLimit_action", httpContext =>
                 RateLimitPartition.GetFixedWindowLimiter(httpContext.Connection.RemoteIpAddress?.ToNumber().ToString(),
                     partition => new FixedWindowRateLimiterOptions
                     {
                         AutoReplenishment = true,
                         PermitLimit = 2,
                         Window = TimeSpan.FromSeconds(10)
                     }));

        });
        #endregion
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {

        var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
        var address = serverAddressesFeature?.Addresses;
        Log.Logger.Information("应用程序运行地址: {@Address}. net version:{version}", address, Environment.Version);

        app.UseRequestLocalization();

        app.UseResponseCompression();
        app.UseForwardedHeaders()
            .UseCertificateForwarding();

        app.UseHttpRequestLogging();

        app.UseCors(AppConstant.DEFAULT_CORS_NAME);

        if (env.IsEnvironment(AppConstant.EnvironmentName.DEV))
        {
            //app.UseDeveloperExceptionPage();
            // 初始化种子数据
            app.DataSeederAsync().Wait();
            app.UseExceptionHandle();
            app.UseCustomSwagger();
        }
        else
        {
            app.UseExceptionHandle();
            // TODO Notice: UseHsts, UseHttpsRedirection are not necessary if using reverse proxy with ssl, like nginx with ssl proxy
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseMultiTenancy();
        app.UseRouting();
        app.UseRateLimiter();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapSystemHealthChecks();
            endpoints.MapControllers();
        });

    }
}

