using Dncy.Permission;
using DncyTemplate.Application.Permission;
using DncyTemplate.Infra.EntityFrameworkCore.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DncyTemplate.Application
{
    public static class DependencyInject
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services, IConfiguration _)
        {

            #region permission

            services.AddScoped<IPermissionChecker, DefaultPermissionChecker>();
            
            // permission definition 
            services.AddSingleton<IPermissionDefinitionManager, DefaultPermissionDefinitionManager>();
            services.AddSingleton<IPermissionDefinitionProvider, PermissionDefinitionProvider>();


            services.AddTransient<IPermissionGrantStore, EfCorePermissionGrantStore>();
            services.AddTransient<IPermissionManager, CachedPermissionManager>(); 
            services.AddTransient<IPermissionValueProvider, RolePermissionValueProvider>();
            services.AddTransient<IPermissionValueProvider, UserPermissionValueProvider>();
            #endregion


            return services;
        }
    }
}