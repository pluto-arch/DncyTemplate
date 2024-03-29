﻿using Microsoft.OpenApi.Models;

namespace DncyTemplate.Api.Infra.ApiDoc
{
    public static class SwaggerHostingStartup
    {
        /// <inheritdoc />
        public static void ConfigureSwagger(this IServiceCollection services, IWebHostEnvironment environment)
        {
            if (!environment.IsEnvironment(Constants.AppConstant.EnvironmentName.DEV))
            {
                return;
            }
            services.ConfigureOptions<ConfigureSwaggerOptions>();
            services.AddSwaggerGen(c =>
            {
                c.SupportNonNullableReferenceTypes();

                c.UseAllOfToExtendReferenceSchemas();

                c.AddSecurityDefinition("Bearer", //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "Bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer", //The name of the previously defined security scheme.
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}