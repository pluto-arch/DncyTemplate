using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace DncyTemplate.Api.Infra.ApiDoc;

public static class SwaggerApplicationBuilderExtension
{
    /// <summary>
    /// Swagger
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        var versionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in versionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{AppConstant.SERVICE_NAME} - {description.GroupName}");
            }
        });
        return app;
    }
}