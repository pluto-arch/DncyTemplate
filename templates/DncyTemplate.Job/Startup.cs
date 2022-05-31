using Dncy.MultiTenancy.AspNetCore;
using Dncy.MultiTenancy.ConnectionStrings;
using Dncy.MultiTenancy.Store;
using Dncy.MultiTenancy;

using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using DncyTemplate.Job.HostedService;
using DncyTemplate.Job.Infra;
using DncyTemplate.Job.Infra.Listenings;
using DncyTemplate.Job.Infra.Stores;
using DncyTemplate.Job.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Spi;
using System.Reflection;

namespace DncyTemplate.Job;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }


    public void ConfigureServices(IServiceCollection services)
    {
        #region 基础服务
        services.AddMvc()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlSerializerFormatters()
            .AddXmlDataContractSerializerFormatters()
            .AddDataAnnotationsLocalization()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new BadRequestObjectResult(context.ModelState);
                    result.ContentTypes.Add(MediaTypeNames.Application.Json);
                    result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                    return result;
                };
            });
        services.AddHttpClient();
        #endregion

        #region 健康检查
        #endregion

        #region 缓存
        services.AddMemoryCache(options =>
        {
            options.SizeLimit = 10240;
        });
        #endregion

        #region http请求相关配置
        services.AddHttpContextAccessor();
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
        });
        services.AddRouting(options => options.LowercaseUrls = true);
        #endregion


        #region Multi Tenancy
        services.Configure<TenantConfigurationOptions>(Configuration);
        services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
        services.AddTransient<ICurrentTenant, CurrentTenant>();
        services.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();
        services.AddTransient<ITenantStore, DefaultTenantStore>();
        services.AddTransient<ITenantResolver, TenantResolver>();
        services.AddTransient<MultiTenancyMiddleware>();
        #endregion


        services.AddApplicationModule(Configuration);
        services.AddInfraModule(Configuration);
        services.AddDomainModule();


        #region QZ
        services.AddSingleton<IJobListener, CustomJobListener>();
        services.AddSingleton<ITriggerListener, CustomTriggerListener>();
        services.AddTransient<IJobInfoStore, JsonFileJobStore>();
        services.AddTransient<IJobLogStore, InMemoryJobLog>();
        services.AddTransient<QuartzJobRunner>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddTransient<IJobStore, RAMJobStore>();
        services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        services.AddSingleton<IJobFactory, SingletonJobFactory>();
        services.AddHostedService<QuartzHostedService>();
        AddJobs(services);
        #endregion
    }

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        var store = app.ApplicationServices.GetService<IJobInfoStore>();
        InitJobsFromConfiguration(store);

        app.UseStaticFiles();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }

    /// <summary>
    /// 从配置文件中初始化静态job
    /// </summary>
    /// <param name="store"></param>
    private void InitJobsFromConfiguration(IJobInfoStore store)
    {
        var jobs = Configuration.GetSection("JobSettings").Get<List<JobSetting>>();
        if (jobs != null)
        {
            foreach (JobSetting job in jobs)
            {
                if (!job.IsOpen)
                {
                    continue;
                }

                store?.AddAsync(new JobInfoModel
                {
                    Id = Guid.NewGuid().ToString("N"),
                    TaskType = EnumTaskType.StaticExecute,
                    TaskName = job.Name,
                    DisplayName = job.DisplayName,
                    GroupName = job.GroupName,
                    Interval = job.Cron,
                    Describe = job.Description,
                    Status = EnumJobStates.Normal
                });
            }
        }
    }


    /// <summary>
    /// 注入静态的job对象
    /// </summary>
    /// <param name="services"></param>
    private static void AddJobs(IServiceCollection services)
    {
        Dictionary<string, Type> jobDefined = new();
        var assembly = Assembly.GetExecutingAssembly();
        var baceType = typeof(IBackgroundJob);
        var implTypes = assembly.GetTypes().Where(c => c != baceType && baceType.IsAssignableFrom(c))
            .ToList();
        if (!implTypes.Any())
            return;

        foreach (Type impltype in implTypes)
        {
            jobDefined.Add(impltype.Name, impltype);
            services.AddTransient(impltype);
        }

        services.AddSingleton(new JobDefined { JobDictionary = jobDefined });
    }
}