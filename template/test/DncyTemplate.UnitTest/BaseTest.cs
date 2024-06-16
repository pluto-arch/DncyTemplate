#if Tenant
using Dotnetydd.MultiTenancy.Store;
using Dotnetydd.MultiTenancy;
#endif
using DncyTemplate.Application;
using DncyTemplate.Constants;
using DncyTemplate.Domain;
using DncyTemplate.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Infrastructure;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.EntityFrameworkCore;
using DncyTemplate.Infra.EntityFrameworkCore.ConnectionStringResolve;
using DncyTemplate.Infra.EntityFrameworkCore.Interceptor;
using DncyTemplate.Infra.Global;
using DncyTemplate.Infra.Providers;

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

            #region 缓存

            services.AddMemoryCache(options => { options.SizeLimit = 10240; });

            #endregion

            services.AddApplicationModule(configuration);


            services.AddSingleton<GlobalAccessor.CurrentUserAccessor>();
            services.AddScoped<GlobalAccessor.CurrentUser>();
            services.AddTransient<IDomainEventDispatcher, MediatrDomainEventDispatcher>();

            #region DncyTemplate DbContext

            services.AddKeyedSingleton<IConnectionStringResolve, DefaultConnectionStringResolve>(nameof(DncyTemplateDbContext));
            services.AddEfCoreInfraComponent<DncyTemplateDbContext>((serviceProvider,optionsBuilder) =>
            {
                optionsBuilder.UseInMemoryDatabase("DncyTemplateUnitTest");

                var mediator = serviceProvider.GetService<IDomainEventDispatcher>() ?? NullDomainEventDispatcher.Instance;
                optionsBuilder.AddInterceptors(new DataChangeSaveChangesInterceptor(mediator));

#if Tenant
                //多租户模式下解析租户连接字符串使用
                var connectionStringResolve = serviceProvider.GetKeyedService<IConnectionStringResolve>(nameof(DncyTemplateDbContext));
                optionsBuilder.AddInterceptors(new TenantDbConnectionInterceptor(connectionStringResolve, InfraConstantValue.DEFAULT_CONNECTIONSTRING_NAME));
#endif
            });
            services.AddEfUnitofWorkWithAccessor<DncyTemplateDbContext>();

            #endregion



            services.AddDomainModule();

#if Tenant
            services.Configure<TenantConfigurationOptions>(configuration);
            services.AddSingleton<ICurrentTenantAccessor, CurrentTenantAccessor>();
            services.AddTransient<ICurrentTenant, CurrentTenant>();
            services.AddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();
            services.AddTransient<ITenantStore, DefaultTenantStore>();
#endif


            ServiceProvider = services.BuildServiceProvider();

            using var sc = ServiceProvider.CreateScope();
            var ctx = sc.ServiceProvider.GetRequiredService<DncyTemplateDbContext>();
            ctx.Database.EnsureCreated();
        }
    }
}