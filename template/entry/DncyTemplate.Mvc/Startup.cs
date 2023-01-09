using Dncy.MultiTenancy.AspNetCore;
using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using DncyTemplate.Mvc.Constants;
using DncyTemplate.Mvc.Infra;
using DncyTemplate.Mvc.Infra.UnitofWork;
using Microsoft.AspNetCore.Hosting.Server.Features;

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


        services.AddApplicationModule(Configuration);
        services.AddDomainModule();
        services.AddInfraModule(Configuration);
    }


    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {

        var serverAddressesFeature = app.ServerFeatures.Get<IServerAddressesFeature>();
        var address = serverAddressesFeature.Addresses;
        Log.Logger.Information("运行地址: {@Address}", address);
        Log.Logger.Information("NET框架版本: {@version}", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);

        app.UseRequestLocalization();

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
        app.UseHttpRequestLogging();
        app.UseAuthentication();
        app.UseMiddleware<MultiTenancyMiddleware>();
        app.UseRouting();
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