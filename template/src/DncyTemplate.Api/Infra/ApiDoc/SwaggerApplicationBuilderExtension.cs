using Asp.Versioning.ApiExplorer;

namespace DncyTemplate.Api.Infra.ApiDoc
{
    public static class SwaggerApplicationBuilderExtension
    {
        private const string requestAuthorization = "(req)=>{const token = localStorage.getItem('token') ;if(token){req.headers.authorization =`Bearer ${token}`;alert(123)}return req}";

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
                options.IndexStream = () => typeof(Program).Assembly.GetManifestResourceStream("DncyTemplate.Api.Infra.ApiDoc.index.html");


                foreach (var description in versionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"{AppConstant.SERVICE_NAME} - {description.GroupName}");
                }
            });
            return app;
        }
    }
}