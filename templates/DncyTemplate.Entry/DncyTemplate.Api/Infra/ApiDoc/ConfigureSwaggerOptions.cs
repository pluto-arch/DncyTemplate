using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace DncyTemplate.Api.Infra.ApiDoc;

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{

    private readonly IApiVersionDescriptionProvider provider;


    public ConfigureSwaggerOptions(
        IApiVersionDescriptionProvider provider)
    {
        this.provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            if (!options.SwaggerGeneratorOptions.SwaggerDocs.ContainsKey(description.GroupName))
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }

        }
    }

    public void Configure(string name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private OpenApiInfo CreateVersionInfo(
        ApiVersionDescription description)
    {
        var info = new OpenApiInfo()
        {
            Title = AppConstant.SERVICE_NAME,
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}