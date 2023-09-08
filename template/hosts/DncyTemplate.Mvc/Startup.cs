#if Tenant
using Dncy.MultiTenancy.AspNetCore;
#endif
using System.Threading.RateLimiting;
using Dncy.Tools;
using DncyTemplate.Application;
using DncyTemplate.Application.Models;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using DncyTemplate.Mvc.BackgroundServices;
using DncyTemplate.Mvc.Constants;
using DncyTemplate.Mvc.Infra;
using DncyTemplate.Mvc.Infra.LocalizerSetup;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Primitives;

namespace DncyTemplate.Mvc;

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


        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !string.IsNullOrEmpty(x.FullName) && x.FullName.Contains("DncyTemplate", StringComparison.OrdinalIgnoreCase));
        
        services.AddApplicationModule(Configuration,assemblies);
        services.AddDomainModule();
        services.AddInfraModule(Configuration);
        
        #region background service
        services.AddHostedService<PrductBackgroundService>();
        #endregion

        
        #region FluentValidation
        services.AddFluentValidationAutoValidation(configs =>
        {
        }).AddValidatorsFromAssemblies(assemblies);
        #endregion

        #region 速率限制

        services.AddRateLimiter(options =>
        {
            options.OnRejected = async (context, cancelToken) =>
            {
                var l = context.HttpContext.RequestServices.GetService<IStringLocalizer<SharedResource>>();
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.Headers.Add("Retry-After", new StringValues("1")); // TODO 根据具体情况返回
                context.HttpContext.Response.ContentType = "text/html; charset=utf-8";
                var res = ResultDto.TooManyRequest();
                res.Message = l[res.Message];
                
                var alertStr=$@"
                             <!DOCTYPE html>
                             <html>
                            <head>
                                <title>Alert</title>
                            </head>
                            <body>
                                <script type='text/javascript'>
                                    alert('{res.Message}');
                                </script>
                            </body>
                            </html>
                             ";
                await context.HttpContext.Response.WriteAsync(alertStr, cancellationToken: cancelToken);
            };
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
        var address = serverAddressesFeature.Addresses;
        Log.Logger.Information("运行地址: {@Address}", address);
        Log.Logger.Information("NET框架版本: {@version}", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);

        app.UseAppLocalization();

        app.UseResponseCompression();
        app.UseForwardedHeaders()
            .UseCertificateForwarding();

        app.UseResponseCompression()
            .UseResponseCaching();

        if (!env.IsEnvironment(AppConstant.EnvironmentName.DEV))
        {
            // 初始化种子数据
            app.DataSeederAsync().Wait();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error/500");
            app.UseStatusCodePagesWithReExecute("/error/{0}");
            // TODO Notice: UseHsts, UseHttpsRedirection are not necessary if using reverse proxy with ssl, like nginx with ssl proxy
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseUnitOfWorkAccessor();
        app.UseHttpRequestLogging();
        app.UseAuthentication();
#if Tenant
        app.UseMiddleware<MultiTenancyMiddleware>();
#endif
        app.UseCurrentUserAccessor();
        app.UseRouting();
        app.UseRateLimiter();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            //endpoints.MapSystemHealthChecks();
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
        });
    }
}