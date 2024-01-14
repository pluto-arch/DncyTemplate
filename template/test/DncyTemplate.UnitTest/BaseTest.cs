#if Tenant
using Dotnetydd.MultiTenancy.ConnectionStrings;
using Dotnetydd.MultiTenancy.Store;
using Dotnetydd.MultiTenancy;
#endif
using DncyTemplate.Application;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using Microsoft.Extensions.Configuration;

#pragma warning disable NUnit1032

namespace DncyTemplate.UnitTest
{
    public class BaseTest
    {
        protected IServiceProvider ServiceProvider;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();


            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddLogging();

            #region »º´æ

            services.AddMemoryCache(options => { options.SizeLimit = 10240; });

            #endregion

            services.AddApplicationModule(configuration);
            services.AddInfraModule(configuration);
            services.AddDomainModule();

#if Tenant
            services.Configure<TenantConfigurationOptions>(configuration);
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
            services.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();
            services.AddTransient<ITenantStore, DefaultTenantStore>();
#endif


            ServiceProvider = services.BuildServiceProvider();
        }
    }
}