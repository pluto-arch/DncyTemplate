﻿using Microsoft.AspNetCore.Localization;
using System.Globalization;
using DncyTemplate.Mvc.Constants;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using static DncyTemplate.Mvc.Constants.AppConstant;
using Microsoft.Extensions.DependencyInjection;

namespace DncyTemplate.Mvc.Infra.LocalizerSetup;

public static class LocalizerExtension
{
    public static IServiceCollection AddAppLocalization(this IServiceCollection services)
    {
        services.AddLocalization(options => { options.ResourcesPath = "Resources"; });
        services.AddRequestLocalization(options =>
        {
            var supportedCultures = new[] { new CultureInfo(Culture.EN_US.key), new CultureInfo(Culture.ZN_CH.key) };
            options.DefaultRequestCulture = new RequestCulture(Culture.ZN_CH.key, Culture.EN_US.key);
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
            options.ApplyCurrentCultureToResponseHeaders = true;
        });
        return services;
    }



    public static MvcOptions SetUpDefaultDataAnnotation(this MvcOptions options, IStringLocalizerFactory localizerFactory)
    {
        var L = localizerFactory?.Create("DefaultDataAnnotation", AppConstant.SERVICE_NAME);
        if (L != null)
        {
            options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => L[DefaultDataAnnotation.ValueIsInvalidAccessor, x]);
            options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => L[DefaultDataAnnotation.ValueMustBeANumberAccessor, x]);
            options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => L[DefaultDataAnnotation.MissingBindRequiredValueAccessor, x]);
            options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => L.GetString(DefaultDataAnnotation.AttemptedValueIsInvalidAccessor, x, y));
            options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => L[DefaultDataAnnotation.MissingKeyOrValueAccessor]);
            options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => L[DefaultDataAnnotation.UnknownValueIsInvalidAccessor, x]);
            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => L[DefaultDataAnnotation.ValueMustNotBeNullAccessor, x]);
            options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => L[DefaultDataAnnotation.NonPropertyAttemptedValueIsInvalidAccessor, x]);
            options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => L[DefaultDataAnnotation.UnknownValueIsInvalidAccessor]);
            options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => L[DefaultDataAnnotation.NonPropertyValueMustBeANumberAccessor]);
            options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => L[DefaultDataAnnotation.MissingRequestBodyRequiredValueAccessor]);
        }
        return options;
    }


    public static MvcDataAnnotationsLocalizationOptions SetUpDataAnnotationLocalizerProvider(this MvcDataAnnotationsLocalizationOptions options)
    {
        options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create("DataAnnotation", AppConstant.SERVICE_NAME);
        return options;
    }
}