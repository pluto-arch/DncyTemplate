using System.Security.Authentication.ExtendedProtection;
using Dncy.MultiTenancy.ConnectionStrings;
using Dncy.MultiTenancy.Store;
using Dncy.MultiTenancy;
using Dncy.MultiTenancy.AspNetCore;
using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace DncyTemplate.Test
{
    public class Init
    {
        internal readonly IServiceProvider ServiceProvider;
        internal readonly IConfiguration Configuration;
        public Init()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDomainModule();
            services.AddApplicationModule(Configuration);
            services.AddInfraModule(Configuration);


            services.Configure<TenantConfigurationOptions>(Configuration);
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
            services.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();
            services.AddTransient<ITenantStore, DefaultTenantStore>();


            ServiceProvider = services.BuildServiceProvider();
        }
    }
}