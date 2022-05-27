using Dncy.MultiTenancy;
using Dncy.MultiTenancy.AspNetCore;
using Dncy.MultiTenancy.ConnectionStrings;
using Dncy.MultiTenancy.Store;
using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using DncyTemplate.Mvc.Constants;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;

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
        services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        }).Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Optimal;
        }).AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
            {
                "text/html; charset=utf-8", "application/xhtml+xml", "application/atom+xml", "image/svg+xml",
                "text/css", "text/html", "text/json"
            });
        });
        services.AddControllersWithViews()
            .AddDataAnnotationsLocalization()
            .AddRazorRuntimeCompilation();

        #endregion

        #region 健康检查
        //services.AddCustomHealthCheck(Configuration);
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
        services.AddTransient<ITenantConstruct, HeaderTenantConstruct>(_ => new HeaderTenantConstruct(headerDic =>
          {
              if (headerDic.ContainsKey(AppConstant.TENANT_KEY))
              {
                  return headerDic[AppConstant.TENANT_KEY];
              }

              return null;
          }));
        services.AddTransient<MultiTenancyMiddleware>();
        #endregion

        services.AddApplicationModule(Configuration);
        services.AddDomainModule();
        services.AddInfraModule(Configuration);
    }


    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        ForwardedHeadersOptions options = new()
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
        app.UseForwardedHeaders(options).UseCertificateForwarding();

        app.UseResponseCompression()
            .UseResponseCaching();

        //app.UseHttpRequestLogging();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        if (env.IsProduction())
        {
            //app.UseExceptionHandle();

            // not necessary if using reverse proxy with ssl, like nginx with ssl proxy
            app.UseHttpsRedirection();
            app.UseHsts();
        }

        app.UseStaticFiles();
        //app.UseMiddleware<UnitOfWorkMiddleware>();
        //app.UseMiddleware<MultiTenancyMiddleware>();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            //endpoints.MapSystemHealthChecks();
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
        });

    }
}