using Dncy.MultiTenancy.AspNetCore;
using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using DncyTemplate.Mvc.Constants;
using DncyTemplate.Mvc.Infra;
using DncyTemplate.Mvc.Infra.UnitofWork;

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

        app.UseForwardedHeaders()
            .UseCertificateForwarding();

        app.UseResponseCompression()
            .UseResponseCaching();

        app.UseHttpRequestLogging();

        if (env.IsEnvironment(AppConstant.EnvironmentName.DEV))
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseInternalServerErrorHandle(); // 处理500错误
            app.UseStatusCodePagesWithRedirects("/error/{0}");
            // TODO Notice: UseHsts, UseHttpsRedirection are not necessary if using reverse proxy with ssl, like nginx with ssl proxy
            app.UseHsts();
        }

        app.UseRequestLocalization();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseMiddleware<MultiTenancyMiddleware>();
        app.UseMiddleware<UnitOfWorkMiddleware>();
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