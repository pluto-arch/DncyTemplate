using Dncy.MultiTenancy;
using Dncy.MultiTenancy.AspNetCore;
using Dncy.MultiTenancy.ConnectionStrings;
using Dncy.MultiTenancy.Store;
using DncyTemplate.Api.Infra.UnitofWork;
using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;


namespace DncyTemplate.Api;
public class Startup
{
    private const string DEFAULT_CORS_NAME = "default";

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        #region 基础服务
        services.AddControllers()
            .AddCustomJsonSerializer()
            .AddXmlSerializerFormatters()
            .AddXmlDataContractSerializerFormatters()
            .AddDataAnnotationsLocalization()
            .ConfigCustomApiBehaviorOptions();

        #endregion

        #region 健康检查
        services.AddCustomHealthCheck(Configuration);
        #endregion

        #region 接口文档
        services.AddCustomSwagger();
        #endregion

        #region 缓存
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 10240;
        });
        #endregion

        #region http请求相关配置

        services.AddHttpContextAccessor();
        services.ConfigHttpForwardedHeadersOptions();
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
        ForwardedHeadersOptions options = new ()
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };
        options.KnownNetworks.Clear();
        options.KnownProxies.Clear();
        app.UseForwardedHeaders(options).UseCertificateForwarding();

        app.UseHttpRequestLogging();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCustomSwagger();
        }
        if (env.IsProduction())
        {
            app.UseExceptionHandle();

            // not necessary if using reverse proxy with ssl, like nginx with ssl proxy
            app.UseHttpsRedirection();
            app.UseHsts();
        }


        app.UseCors(DEFAULT_CORS_NAME);
        app.UseMiddleware<UnitOfWorkMiddleware>();
        app.UseMiddleware<MultiTenancyMiddleware>();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapSystemHealthChecks();
            endpoints.MapControllers();
        });

    }
}

