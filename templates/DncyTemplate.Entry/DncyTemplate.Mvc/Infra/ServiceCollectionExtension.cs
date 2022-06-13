using DncyTemplate.Mvc.Infra.Authorization;
using DncyTemplate.Mvc.Infra.HealthChecks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.IO.Compression;

namespace DncyTemplate.Mvc.Infra;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// 配置http请求头
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigHttpForwardedHeadersOptions(this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardLimit = null;// 限制所处理的标头中的条目数
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; // X-Forwarded-For：保存代理链中关于发起请求的客户端和后续代理的信息。X-Forwarded-Proto：原方案的值 (HTTP/HTTPS)
            options.KnownNetworks.Clear(); // 从中接受转接头的已知网络的地址范围。 使用无类别域际路由选择 (CIDR) 表示法提供 IP 范围。使用CDN时应清空
            options.KnownProxies.Clear(); // 从中接受转接头的已知代理的地址。 使用 KnownProxies 指定精确的 IP 地址匹配。使用CDN时应清空
        });
        return services;
    }

    /// <summary>
    /// 健康检查
    /// </summary>
    /// <returns></returns>
    public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MemoryCheckOptions>(options =>
        {
            options.Threshold = configuration.GetValue<long>("HealthCheck:Memory:Threshold");
        });
        services.AddHealthChecks()
            .AddCheck<MemoryHealthCheck>("memory_check", failureStatus: HealthStatus.Degraded);
        //.AddCheck<DatabaseHealthCheck>("database_check", failureStatus: HealthStatus.Unhealthy,
        //    tags: new string[] { "database", "sqlServer" });
        return services;
    }


    /// <summary>
    /// 添加动态授权策略
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDynamicPolicyAuthorize(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();
        return services;
    }



    /// <summary>
    /// 添加自定义响应压缩
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomCompression(this IServiceCollection services)
    {
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
        return services;
    }

}