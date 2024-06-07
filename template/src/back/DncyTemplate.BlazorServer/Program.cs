using System.IO.Compression;
using DncyTemplate.Application;
using DncyTemplate.BlazorServer.Components;
using DncyTemplate.BlazorServer.Infra;
using DncyTemplate.BlazorServer.Infra.Authorization;
using DncyTemplate.BlazorServer.Infra.HealthChecks;
using DncyTemplate.BlazorServer.Infra.LocalizerSetup;
using DncyTemplate.BlazorServer.Infra.LogSetup;
using DncyTemplate.BlazorServer.Infra.Tenancy;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using DncyTemplate.Infra.EntityFrameworkCore.Migrations;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.FluentUI.AspNetCore.Components;
using Serilog;


string appName = "DncyTemplate.Blazor";

var logConfig = new ConfigurationBuilder()
    .AddJsonFile("serilogsetting.json", false, true)
    .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(logConfig)
    .Enrich.With<ActivityEnricher>()
    .CreateLogger();
Log.Information("[{appName}]日志配置完毕...", appName);


var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(dispose: true);


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

#if Aspire
// Add service defaults & Aspire components.
builder.AddServiceDefaults();
#endif


#region response Compression
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


builder.Services.AddProblemDetails();
builder.Services.AddHttpClient();
var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(x => !string.IsNullOrEmpty(x.FullName) && x.FullName.Contains("DncyTemplate", StringComparison.OrdinalIgnoreCase));

builder.Services.AddApplicationModule(builder.Configuration, assemblies);
builder.Services.AddInfraModule(builder.Configuration);
builder.Services.AddDomainModule();

builder.Services.AddHostedService<EfCoreMigrationHostService>();


builder.Services.ConfigureAuthorization();
builder.Services.ConfigureHealthCheck(builder.Configuration);
builder.Services.AddAppLocalization();
#if Tenant
builder.Services.ConfigureTenancy(builder.Configuration);
#endif

Log.Information("[{appName}]服务注册完毕...", appName);
var app = builder.Build();
Log.Information("[{appName}]应用构建完毕...", appName);

var endPointUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
if (!string.IsNullOrEmpty(endPointUrl))
{
    Log.Logger.Information("ASPNETCORE_URLS: {endPointUrl}", endPointUrl);
}
Log.Logger.Information("NET环境: {@version}", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);

app.UseResponseCompression();
app.UseForwardedHeaders()
    .UseCertificateForwarding()
    .UseResponseCaching();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();


app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapSystemHealthChecks();

app.Run();
Log.Information("[{appName}]应用启动完毕...", appName);