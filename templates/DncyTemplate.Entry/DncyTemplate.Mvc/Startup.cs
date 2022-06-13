using Dncy.MultiTenancy;
using Dncy.MultiTenancy.AspNetCore;
using Dncy.MultiTenancy.ConnectionStrings;
using Dncy.MultiTenancy.Store;
using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using DncyTemplate.Mvc.Constants;
using DncyTemplate.Mvc.Infra;
using DncyTemplate.Mvc.Infra.Tenancy;
using DncyTemplate.Mvc.Infra.UnitofWork;
using Microsoft.AspNetCore.Authentication.Cookies;

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
        #region 基础服务
        services.AddCustomCompression();
        services.AddControllersWithViews()
            .AddControllersAsServices()
            .AddDataAnnotationsLocalization()
            .AddRazorRuntimeCompilation();

        #endregion

        #region 健康检查
        services.AddCustomHealthCheck(Configuration);
        #endregion

        #region 缓存
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 10240;
        });
        #endregion

        #region http请求相关配置

        services.AddHttpContextAccessor();
        //services.ConfigHttpForwardedHeadersOptions();
        // 路由小写
        services.AddRouting(options => options.LowercaseUrls = true);
        #endregion

        #region Multi Tenancy
        services.Configure<TenantConfigurationOptions>(Configuration);
        services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
        services.AddTransient<ICurrentTenant, CurrentTenant>();
        services.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();
        services.AddTransient<ITenantStore, DefaultTenantStore>();
        services.AddTransient<ITenantResolver, TenantResolver>();
        services.AddTransient<ITenantIdentityParse, UserTenantIdentityParse>();
        services.AddTransient<MultiTenancyMiddleware>();
        #endregion



        #region 认证授权

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();
        services.AddAuthorization() // 授权
            .AddDynamicPolicyAuthorize();

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
            app.UseExceptionHandle();

            // TODO Notice: UseHsts, UseHttpsRedirection are not necessary if using reverse proxy with ssl, like nginx with ssl proxy
            app.UseHsts();
        }
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