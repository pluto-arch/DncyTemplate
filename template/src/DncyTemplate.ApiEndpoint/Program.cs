using System.IO.Compression;
using DncyTemplate.ApiEndpoint.Infra;
using DncyTemplate.ApiEndpoint.Infra.Authorization;
using DncyTemplate.ApiEndpoint.Infra.LogSetup;
using DncyTemplate.ApiEndpoint.Infra.RateLimits;
using DncyTemplate.ApiEndpoint.Infra.Tenancy;
using DncyTemplate.Application;
using DncyTemplate.Application.Models;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using Microsoft.AspNetCore.ResponseCompression;



var logConfig = new ConfigurationBuilder()
    .AddJsonFile("serilogsetting.json", false, true)
    .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(logConfig)
    .Enrich.With<ActivityEnricher>()
    .CreateLogger();
Log.Information("日志配置完毕...");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(dispose: true);

builder.Services.AddFastEndpoints(o =>
{
    o.SourceGeneratorDiscoveredTypes.AddRange(DncyTemplate.ApiEndpoint.DiscoveredTypes.All);
});

#region nswag api doc
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.DocumentName = "Resease";
        s.Title = "DncyTemplate.ApiEndpoints";
        s.Version = "v0";
    };
});
builder.Services.SwaggerDocument(o =>
{
    o.MaxEndpointVersion = 1;
    o.EndpointFilter = e => e.Version.Current==1;
    o.DocumentSettings = s =>
    {
        s.DocumentName = "Release 1.0";
        s.Title = "DncyTemplate.ApiEndpoints";
        s.Version = "v1.0";
    };
});
#endregion


#region rate limit

builder.Services.ConfigAppRateLimit(builder.Configuration);

#endregion

#region cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("allow_all", builder =>
    {
        builder.SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
#endregion

#region services
var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(x => !string.IsNullOrEmpty(x.FullName) && x.FullName.Contains("DncyTemplate", StringComparison.OrdinalIgnoreCase));

builder.Services.AddApplicationModule(builder.Configuration, assemblies);
builder.Services.AddInfraModule(builder.Configuration);
builder.Services.AddDomainModule();
#if Tenant
builder.Services.ConfigureTenancy(builder.Configuration);
#endif

#endregion


#region 响应压缩
builder.Services.AddResponseCompression(options =>
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
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
}).Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});
#endregion


#region 认证授权
builder.Services.ConfigureAuthorization();
#endregion


Log.Information("服务注册完毕...");
var app = builder.Build();
Log.Information("构建WebApplication成功...");

var endPointUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
if (!string.IsNullOrEmpty(endPointUrl))
{
    Log.Logger.Information("ASPNETCORE_URLS: {endPointUrl}", endPointUrl);
}
Log.Logger.Information("NET框架版本: {@version}", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);



app.UseResponseCompression();
app.UseForwardedHeaders()
    .UseCertificateForwarding();
app.UseHttpRequestLogging();


app.UseCors("allow_all");
app.UserResponseHeaderAuthTraceId();
app.UseCustomizeExceptionHandle();


app.UseAuthentication();

#if Tenant
app.UseMultiTenancy();
#endif

app.UseFastEndpoints(c =>
    {
        c.Versioning.Prefix = "v";
        c.Versioning.PrependToRoute = true;
        c.Endpoints.RoutePrefix = "api";

        c.Errors.ResponseBuilder = (failures, _, _) =>
        {
            var errors = failures.GroupBy(f => f.PropertyName)
                .ToDictionary(
                    keySelector: e => e.Key,
                    elementSelector: e => e.Select(m => m.ErrorMessage).ToArray());
            return ResultDto<dynamic>.ErrorRequest(data: errors);
        };

    })
    .UseSwaggerGen();


app.UseAuthorization();

app.UseRateLimiter();

app.Run();
Log.Information("应用已启动...");