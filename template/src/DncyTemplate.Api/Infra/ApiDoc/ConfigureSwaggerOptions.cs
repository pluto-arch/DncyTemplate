using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace DncyTemplate.Api.Infra.ApiDoc
{
    /// <summary>
    /// 自定义 SwaggerGenOptions
    /// </summary>
    public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {

        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.SupportNonNullableReferenceTypes();
            options.UseAllOfToExtendReferenceSchemas();


            #region 安全相关
            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
            #endregion


            #region 配置xml文档
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            #endregion


            #region API Version
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                if (!options.SwaggerGeneratorOptions.SwaggerDocs.ContainsKey(description.GroupName))
                {
                    options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
                }
            }
            #endregion
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = AppConstant.SERVICE_NAME,
                Version = description.ApiVersion.ToString(),
                TermsOfService = new Uri("https://example.com/terms"),
                Description = $" This API of version :{description.ApiVersion.ToString()}."
            };

            return info;
        }
    }
}