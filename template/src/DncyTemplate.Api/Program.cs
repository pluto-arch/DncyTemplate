using DncyTemplate.Api.Infra.ApiDoc;
using DncyTemplate.Api.Infra.Authorization;
using DncyTemplate.Api.Infra.HealthChecks;
using DncyTemplate.Api.Infra.LocalizerSetup;
using DncyTemplate.Api.Infra.LogSetup;
using DncyTemplate.Api.Infra.RateLimits;

#if Tenant
using DncyTemplate.Api.Infra.Tenancy;
#endif
using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using DncyTemplate.Infra.EntityFrameworkCore.Migrations;
using FluentValidation;
using FluentValidation.AspNetCore;



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

#if Aspire
// Add service defaults & Aspire components.
builder.AddServiceDefaults();
#endif

#region 服务注册
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 10240;
});

#region 应用、基础设施、领域层注册
builder.Services.AddHttpClient();
var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(x => !string.IsNullOrEmpty(x.FullName) && x.FullName.Contains("DncyTemplate", StringComparison.OrdinalIgnoreCase));

builder.Services.AddApplicationModule(builder.Configuration, assemblies);
builder.Services.AddInfraModule(builder.Configuration);
builder.Services.AddDomainModule();
builder.Services.AddHostedService<EfCoreMigrationHostService>();
#endregion


#region FluentValidation
builder.Services.AddFluentValidationAutoValidation(configs =>
{
}).AddValidatorsFromAssemblies(assemblies);

#endregion


#region 速率限制

builder.Services.ConfigAppRateLimit(builder.Configuration);

#endregion


#region web component
builder.Services.ConfigureWebInfra();
builder.Services.ConfigureSwagger(builder.Environment);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureHealthCheck(builder.Configuration);
#endregion


#if Tenant
builder.Services.ConfigureTenancy(builder.Configuration);
#endif

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

#region 中间件注册

app.UseAppLocalization();
app.UseResponseCompression();
app.UseForwardedHeaders()
    .UseCertificateForwarding();
app.UseHttpRequestLogging();


app.UseCustomizeExceptionHandle();

app.UseCors(AppConstant.DEFAULT_CORS_NAME);
app.UserResponseHeaderAuthTraceId();

if (app.Environment.IsEnvironment(AppConstant.EnvironmentName.DEV))
{
    app.UseCustomizeSwagger();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();



app.UseAuthentication();
#if Tenant
app.UseMultiTenancy();
#endif

// 用户访问器
app.UseCurrentUserAccessor();

app.UseRouting();
app.UseAuthorization();
app.UseRateLimiter();
app.MapDefaultControllerRoute();
app.MapSystemHealthChecks();

#endregion

app.Run();
Log.Information("应用已启动...");
