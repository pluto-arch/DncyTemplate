using DncyTemplate.Api.Infra;

namespace DncyTemplate.Api;

public class Startup
{
    private const string DefaultCorsName = "default";

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


        #region 组件
        services.AddCustomHealthCheck(Configuration)
            .AddCustomSwagger()
        #endregion


        #region 缓存
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 10240;
        });
        #endregion
        

        #region http请求相关配置
        services.ConfigHttpForwardedHeadersOptions();
        #endregion


        // 路由小写
        services.AddRouting(options => options.LowercaseUrls = true);


    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
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

            // TODO if using reverse proxy with ssl。it's not necessary
            app.UseHttpsRedirection();
            app.UseHsts();
        }


        app.UseCors(DefaultCorsName);
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapSystemHealthChecks();
            endpoints.MapControllers();
        });

    }
}