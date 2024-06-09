using Microsoft.OpenApi.Models;

namespace DncyTemplate.Api.Infra.ApiDoc
{
    public static class SwaggerHostingStartup
    {
        public static void ConfigureSwagger(this IServiceCollection services, IWebHostEnvironment environment)
        {
            if (!environment.IsEnvironment(Constants.AppConstant.EnvironmentName.DEV))
            {
                return;
            }
            services.ConfigureOptions<ConfigureSwaggerGenOptions>();
            services.AddSwaggerGen();
        }
    }
}