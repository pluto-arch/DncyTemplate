using DncyTemplate.Mvc.Constants;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.Globalization;
using static DncyTemplate.Mvc.Constants.AppConstant;

namespace DncyTemplate.Mvc.Infra.LocalizerSetup;

public static class LocalizerExtension
{
    public static IServiceCollection AddAppLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
        services.AddRequestLocalization(options =>
        {
            var supportedCultures = new[] { new CultureInfo(Culture.EN_US.key), new CultureInfo(Culture.ZN_CH.key) };
            options.DefaultRequestCulture = new RequestCulture(Culture.ZN_CH.key, Culture.ZN_CH.key);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.ApplyCurrentCultureToResponseHeaders = true;
        });
        return services;
    }



    public static MvcOptions SetUpDefaultDataAnnotation(this MvcOptions options, IStringLocalizerFactory localizerFactory)
    {
        var l = localizerFactory?.Create("DefaultDataAnnotation", AppConstant.SERVICE_NAME);
        if (l != null)
        {
            options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => l[DefaultDataAnnotationResource.ValueIsInvalidAccessor, x]);
            options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => l[DefaultDataAnnotationResource.ValueMustBeANumberAccessor, x]);
            options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => l[DefaultDataAnnotationResource.MissingBindRequiredValueAccessor, x]);
            options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => l.GetString(DefaultDataAnnotationResource.AttemptedValueIsInvalidAccessor, x, y));
            options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => l[DefaultDataAnnotationResource.MissingKeyOrValueAccessor]);
            options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => l[DefaultDataAnnotationResource.UnknownValueIsInvalidAccessor, x]);
            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => l[DefaultDataAnnotationResource.ValueMustNotBeNullAccessor, x]);
            options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => l[DefaultDataAnnotationResource.NonPropertyAttemptedValueIsInvalidAccessor, x]);
            options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => l[DefaultDataAnnotationResource.UnknownValueIsInvalidAccessor]);
            options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => l[DefaultDataAnnotationResource.NonPropertyValueMustBeANumberAccessor]);
            options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => l[DefaultDataAnnotationResource.MissingRequestBodyRequiredValueAccessor]);
        }
        return options;
    }


    public static MvcDataAnnotationsLocalizationOptions SetUpDataAnnotationLocalizerProvider(this MvcDataAnnotationsLocalizationOptions options)
    {
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create("DataAnnotation", AppConstant.SERVICE_NAME);
        return options;
    }
    
    public static IApplicationBuilder UseAppLocalization(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        app.UseRequestLocalization(options.Value);
        return app;
    }
}