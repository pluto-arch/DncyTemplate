using System.Reflection;
using Dncy.Permission;
using DncyTemplate.Application.Permission;
using DncyTemplate.Infra.EntityFrameworkCore.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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


            services.AutoInjectDncyTemplate_Application();

            return services;
        }


        /// <summary>
        /// use reflection inject all appservice
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var list = ( from c in assembly?.GetTypes()
                         where !c.IsInterface && c.Name.EndsWith("AppService")
                         select c ).ToList();
            if (!list.Any())
            {
                return services;
            }

            foreach (var item in list)
            {
                var enumerable = from c in item.GetInterfaces()
                                 where c.Name.StartsWith("I") && c.Name.EndsWith("AppService")
                                 select c;
                if (!enumerable.Any())
                {
                    continue;
                }

                foreach (var item2 in enumerable)
                {
                    services.AddTransient(item2, item);
                }
            }

            return services;
        }

    }
}