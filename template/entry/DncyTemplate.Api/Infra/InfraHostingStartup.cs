using DncyTemplate.Api.Infra.ExceptionHandlers;
using DncyTemplate.Api.Infra.LocalizerSetup;
using DncyTemplate.Api.Models.ModelBinding;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Localization;
using System.IO.Compression;

[assembly: HostingStartup(typeof(InfraHostingStartup))]
namespace DncyTemplate.Api.Infra;

public class InfraHostingStartup : IHostingStartup
{
    /// <inheritdoc />
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((_, services) =>
        {

            #region 本地化
            services.AddAppLocalization();
            #endregion

            #region mvc builder

            services.AddControllers(options =>
                {
                    options.ModelBinderProviders.Insert(0, new SortingBinderProvider());

                    options.Filters.Add<ActionExecptionFilter>();

                    //options.Filters.Add<AuditLogActionFilter>();

                    // 本地化 默认的模型验证信息
                    var F = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    // 默认的数据验证本地化
                    options.SetUpDefaultDataAnnotation(F);
                })
                .AddDataAnnotationsLocalization(options =>
                {
                    // 数据验证的本地化
                    options.SetUpDataAnnotationLocalizerProvider();
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = ModelBindExceptionHandler.Handler;
                })
                .AddControllersAsServices()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            #endregion

            #region response Compression
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    "text/html; charset=utf-8", "application/xhtml+xml", "application/atom+xml", "image/svg+xml",
                    "text/css", "text/html", "text/json","application/json"
                });
            });
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            }).Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.SmallestSize;
            });
            #endregion

            #region http
            services.AddHttpContextAccessor();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardLimit = null;// 限制所处理的标头中的条目数
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; // X-Forwarded-For：保存代理链中关于发起请求的客户端和后续代理的信息。X-Forwarded-Proto：原方案的值 (HTTP/HTTPS)
                options.KnownNetworks.Clear(); // 从中接受转接头的已知网络的地址范围。 使用无类别域际路由选择 (CIDR) 表示法提供 IP 范围。使用CDN时应清空
                options.KnownProxies.Clear(); // 从中接受转接头的已知代理的地址。 使用 KnownProxies 指定精确的 IP 地址匹配。使用CDN时应清空
            });
            #endregion

            #region api version

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
            });
            services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            #endregion


            #region cors
            services.AddCors(options =>
            {
                options.AddPolicy(AppConstant.DEFAULT_CORS_NAME, builder =>
                {
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            #endregion
        });
    }
}