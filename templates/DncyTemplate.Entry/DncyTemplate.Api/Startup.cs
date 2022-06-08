using Dncy.MultiTenancy;
using Dncy.MultiTenancy.AspNetCore;
using Dncy.MultiTenancy.ConnectionStrings;
using Dncy.MultiTenancy.Store;
using DncyTemplate.Api.Infra.Authorization;
using DncyTemplate.Api.Infra.Tenancy;
using DncyTemplate.Api.Infra.UnitofWork;
using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


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
        services.AddCustomCompression();
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
        services.AddTransient<ITenantIdentityParse, UserTenantIdentityParse>();
        services.AddTransient<MultiTenancyMiddleware>();
        #endregion


        #region 认证授权
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "pluto",
                    ValidAudience = "12312",
                    ClockSkew = TimeSpan.FromMinutes(30),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("715B59F3CDB1CF8BC3E7C8F13794CEA9"))
                };
                options.RequireHttpsMetadata = false;
            }); // 认证
        services.AddAuthorization() // 授权
            .AddDynamicPolicyAuthorize();

        #endregion

        services.AddApplicationModule(Configuration);
        services.AddInfraModule(Configuration);
        services.AddDomainModule();
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {

        app.UseResponseCompression();
        app.UseForwardedHeaders()
            .UseCertificateForwarding();

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
            app.UseHsts();
            app.UseHttpsRedirection();
        }


        app.UseCors(DEFAULT_CORS_NAME);
        app.UseMiddleware<UnitOfWorkMiddleware>();
        app.UseAuthentication();
        app.UseMiddleware<MultiTenancyMiddleware>();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapSystemHealthChecks();
            endpoints.MapControllers();
        });

    }
}

