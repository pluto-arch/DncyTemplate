using Dncy.Permission;
using DncyTemplate.Application.Behaviors;
using DncyTemplate.Application.Permission;
using DncyTemplate.Infra.EntityFrameworkCore.Repository;
using FastExpressionCompiler;
using Mapster;
using MapsterMapper;

namespace DncyTemplate.Application
{
    public static class DependencyInject
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services,IConfiguration configuration, IEnumerable<Assembly> assemblies=null)
        {
            assemblies ??= AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !string.IsNullOrEmpty(x.FullName) && x.FullName.Contains("DncyTemplate", StringComparison.OrdinalIgnoreCase));
            services.AddMapster(assemblies.ToArray());

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(TransactionBehavior<,>).Assembly));

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));


            #region permission
            services.AddScoped<IPermissionChecker, PermissionChecker>();
            // permission definition 
            services.AddSingleton<IPermissionDefinitionManager, DefaultPermissionDefinitionManager>();
            services.AddSingleton<IPermissionDefinitionProvider, PermissionDefinitionProvider>();

            services.AddScoped<IPermissionGrantStore, EfCorePermissionGrantStore>();
            services.AddScoped<IPermissionManager, CachedPermissionManager>();
            services.AddScoped<IPermissionValueProvider, RolePermissionValueProvider>();
            services.AddScoped<IPermissionValueProvider, UserPermissionValueProvider>();
            #endregion


            services.AutoInjectDncyTemplate_Application();

            return services;
        }


        public static void AddMapster(this IServiceCollection services,params Assembly[] assemblies)
        {
            var config=TypeAdapterConfig.GlobalSettings;
            TypeAdapterConfig.GlobalSettings.Compiler = exp => exp.CompileFast();
            config.Scan(assemblies);
            services.AddSingleton(config);
            services.AddSingleton<IMapper, ServiceMapper>();
        }
    }
}