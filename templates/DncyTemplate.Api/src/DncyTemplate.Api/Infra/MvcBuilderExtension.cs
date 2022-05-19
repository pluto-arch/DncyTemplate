namespace DncyTemplate.Api.Infra;

public static class MvcBuilderExtension
{
    public static IMvcBuilder AddCustomJsonSerializer(this IMvcBuilder builder)
    {
        builder.AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        });
        return builder;
    }

    public static IMvcBuilder ConfigCustomApiBehaviorOptions(this IMvcBuilder builder)
    {
        builder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var result = new BadRequestObjectResult(context.ModelState);
                result.ContentTypes.Add(MediaTypeNames.Application.Json);
                result.ContentTypes.Add(MediaTypeNames.Application.Xml);
                return result;
            };
        });
        return builder;
    }
}