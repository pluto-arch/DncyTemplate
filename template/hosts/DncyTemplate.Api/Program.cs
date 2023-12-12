﻿using System.Threading.RateLimiting;
using DncyTemplate.Api;
using DncyTemplate.Api.BackgroundServices;
using DncyTemplate.Api.Infra.ApiDoc;
using DncyTemplate.Api.Infra.Authorization;
using DncyTemplate.Api.Infra.HealthChecks;
using DncyTemplate.Api.Infra.LocalizerSetup;
using DncyTemplate.Api.Infra.LogSetup;
#if Tenant
using DncyTemplate.Api.Infra.Tenancy;
#endif
using DncyTemplate.Application;
using DncyTemplate.Application.Models;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using Dotnetydd.Tools.Core.Extension;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Primitives;


string AppName = typeof(Program).Namespace;

var logConfig = new ConfigurationBuilder()
            .AddJsonFile("serilogsetting.json", false, true)
            .Build();
Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(logConfig)
            .Enrich.With<ActivityEnricher>()
            .CreateLogger();
Log.Information("[{AppName}]日志配置完毕...", AppName);

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(dispose: true);
// Add service defaults & Aspire components.
builder.AddServiceDefaults();
// Add services to the container.
builder.Services.AddProblemDetails();


#region 服务注册
// 内存缓存
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 10240;
});

// 应用、基础设施、领域层注册
builder.Services.AddHttpClient();
var assemblies = AppDomain.CurrentDomain.GetAssemblies()
               .Where(x => !string.IsNullOrEmpty(x.FullName) && x.FullName.Contains("DncyTemplate", StringComparison.OrdinalIgnoreCase));

builder.Services.AddApplicationModule(builder.Configuration, assemblies);
builder.Services.AddInfraModule(builder.Configuration);
builder.Services.AddDomainModule();

// 后台服务
builder.Services.AddHostedService<PrductBackgroundService>();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation(configs =>
{
}).AddValidatorsFromAssemblies(assemblies);

// 速率限制
builder.Services.AddRateLimiter(options =>
{
    options.OnRejected = async (context, cancelToken) =>
    {
        var l = context.HttpContext.RequestServices.GetService<IStringLocalizer<SharedResource>>();
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.Headers.Append("Retry-After", new StringValues("1")); // TODO 根据具体情况返回
        context.HttpContext.Response.ContentType = AppConstant.DEFAULT_CONTENT_TYPE;
        var res = ResultDto.TooManyRequest();
        res.Message = l[res.Message];
        await context.HttpContext.Response.WriteAsJsonAsync(res, cancellationToken: cancelToken);
    };
    options.AddPolicy("home.RateLimit_action", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(httpContext.Connection.RemoteIpAddress?.ToNumber().ToString(),
            partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 2,
                Window = TimeSpan.FromSeconds(10)
            }));

});

builder.Services.ConfigureWebInfra();
builder.Services.ConfigureSwagger(builder.Environment);
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureHealthCheck(builder.Configuration);

#if Tenant
builder.Services.ConfigureTenancy(builder.Configuration);
#endif

#endregion
Log.Information("[{AppName}]服务注册完毕...", AppName);

var app = builder.Build();

Log.Information("[{AppName}]构建WebApplication成功...", AppName);

#region 中间件注册
var serverAddressesFeature = app.Services.GetService<IServerAddressesFeature>();
var address = serverAddressesFeature?.Addresses;
Log.Logger.Information("运行地址: {@Address}", address);
Log.Logger.Information("NET框架版本: {@version}", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);


app.UseAppLocalization();
app.UseResponseCompression();
app.UseForwardedHeaders()
    .UseCertificateForwarding();
app.UseHttpRequestLogging();
app.UseCors(AppConstant.DEFAULT_CORS_NAME);
if (app.Environment.IsEnvironment(AppConstant.EnvironmentName.DEV))
{
    //app.UseDeveloperExceptionPage();
    // 初始化种子数据
    app.DataSeederAsync().Wait();
    app.UseExceptionHandle();
    app.UseCustomSwagger();
}
else
{
    app.UseExceptionHandle();
    // TODO Notice: UseHsts, UseHttpsRedirection are not necessary if using reverse proxy with ssl, like nginx with ssl proxy
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
app.UseRateLimiter();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapSystemHealthChecks();

#endregion


app.Run();
Log.Information("[{AppName}]应用已启动...", AppName);
