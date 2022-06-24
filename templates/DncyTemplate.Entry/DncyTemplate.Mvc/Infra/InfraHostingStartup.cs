using System.Globalization;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using DncyTemplate.Mvc.Infra.Authorization;
using DncyTemplate.Mvc.Infra.LocalizerSetup;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;

[assembly: HostingStartup(typeof(DncyTemplate.Mvc.Infra.InfraHostingStartup))]
namespace DncyTemplate.Mvc.Infra;

public class InfraHostingStartup : IHostingStartup
{
    /// <inheritdoc />
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((_, services) =>
        {

           
            #region 本地化
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]{new CultureInfo("en-US"), new CultureInfo("zh-CN")};
                options.DefaultRequestCulture = new RequestCulture("zh-CN", "zh-CN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            #endregion

            #region mvc基础
            services.AddMvc(options =>
                {
                    var F = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    options.SetUpDefaultDataAnnotation(F);
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.SetUpDataAnnotationLocalizerProvider();
                })
                .AddControllersAsServices()
                .AddDataAnnotationsLocalization()
                .AddRazorRuntimeCompilation();
            #endregion


            #region auth
            services.AddAuthentication(options =>
            {
                options.AddScheme<TempAuthenticationHandler>(TempAuthenticationHandler.SchemeName, nameof(TempAuthenticationHandler));
                options.DefaultAuthenticateScheme = TempAuthenticationHandler.SchemeName;
                options.DefaultChallengeScheme = TempAuthenticationHandler.SchemeName;
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


            services.AddHttpContextAccessor();
            services.AddRouting(options => options.LowercaseUrls = true);


            #region httpForwardedHeaders
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardLimit = null;// 限制所处理的标头中的条目数
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; // X-Forwarded-For：保存代理链中关于发起请求的客户端和后续代理的信息。X-Forwarded-Proto：原方案的值 (HTTP/HTTPS)
                options.KnownNetworks.Clear(); // 从中接受转接头的已知网络的地址范围。 使用无类别域际路由选择 (CIDR) 表示法提供 IP 范围。使用CDN时应清空
                options.KnownProxies.Clear(); // 从中接受转接头的已知代理的地址。 使用 KnownProxies 指定精确的 IP 地址匹配。使用CDN时应清空
            });
            #endregion
        });
    }
}