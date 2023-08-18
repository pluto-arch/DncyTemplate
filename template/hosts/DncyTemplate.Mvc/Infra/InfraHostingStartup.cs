using DncyTemplate.Mvc.Infra.LocalizerSetup;
using DncyTemplate.Mvc.Infra.LogSetup;
using DncyTemplate.Mvc.Models.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Localization;
using System.IO.Compression;

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
            services.AddAppLocalization();
            #endregion

            #region mvc基础
            services.AddControllersWithViews(options =>
                {
                    options.ModelBinderProviders.Insert(0, new SortingBinderProvider());

                    options.Filters.Add<AuditLogActionFilter>();

                    var l = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    options.SetUpDefaultDataAnnotation(l);
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization(options =>
                {
                    options.SetUpDataAnnotationLocalizerProvider();
                })
                .AddControllersAsServices()
                .AddRazorRuntimeCompilation();
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
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardLimit = null;// 限制所处理的标头中的条目数
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; // X-Forwarded-For：保存代理链中关于发起请求的客户端和后续代理的信息。X-Forwarded-Proto：原方案的值 (HTTP/HTTPS)
                options.KnownNetworks.Clear(); // 从中接受转接头的已知网络的地址范围。 使用无类别域际路由选择 (CIDR) 表示法提供 IP 范围。使用CDN时应清空
                options.KnownProxies.Clear(); // 从中接受转接头的已知代理的地址。 使用 KnownProxies 指定精确的 IP 地址匹配。使用CDN时应清空
            });
        });
    }
}