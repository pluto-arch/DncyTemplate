using Dncy.Permission;
using DncyTemplate.Application.Behaviors;
using DncyTemplate.Application.Permission;


namespace DncyTemplate.Application
{
    public static class DependencyInject
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services, IConfiguration _)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !string.IsNullOrEmpty(x.FullName) && ( !x.FullName.Contains("Microsoft", StringComparison.OrdinalIgnoreCase) || !x.FullName.Contains("System", StringComparison.OrdinalIgnoreCase) ));
            services.AddAutoMapper(assemblies.ToArray());
            services.AddMediatR(assemblies.ToArray());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));


            #region permission
            services.AddScoped<IPermissionChecker, PermissionChecker>();
            // permission definition 
            services.AddSingleton<IPermissionDefinitionManager, DefaultPermissionDefinitionManager>();
            services.AddSingleton<IPermissionDefinitionProvider, PermissionDefinitionProvider>();

            //services.AddTransient<IPermissionGrantStore, EfCorePermissionGrantStore>();
            services.AddTransient<IPermissionManager, CachedPermissionManager>();
            services.AddTransient<IPermissionValueProvider, RolePermissionValueProvider>();
            services.AddTransient<IPermissionValueProvider, UserPermissionValueProvider>();
            #endregion


            services.AutoInjectDncyTemplate_Application();

            return services;
        }

    }
}