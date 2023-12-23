using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace DncyTemplate.Api.Infra.LocalizerSetup
{
    public static class LocalizerExtension
    {

        public static IServiceCollection AddAppLocalization(this IServiceCollection services)
        {
            services.AddLocalization(o =>
            {
                o.ResourcesPath = "Resources";
            });
            services.AddRequestLocalization(options =>
            {
                var supportedCultures = new[] { new CultureInfo(AppConstant.Culture.EN_US.key), new CultureInfo(AppConstant.Culture.ZN_CH.key) };
                options.DefaultRequestCulture = new RequestCulture(AppConstant.Culture.ZN_CH.key, AppConstant.Culture.ZN_CH.key);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.ApplyCurrentCultureToResponseHeaders = true;
                options.FallBackToParentCultures = true;
                options.FallBackToParentUICultures = true;
            });
            return services;
        }


        public static IApplicationBuilder UseAppLocalization(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
            return app;
        }


        /// <summary>
        /// 默认的数据注解本地化
        /// </summary>
        /// <param name="options"></param>
        /// <param name="localizerFactory"></param>
        /// <returns></returns>
        public static MvcOptions SetUpDefaultDataAnnotation(this MvcOptions options, IStringLocalizerFactory localizerFactory)
        {
            var l = localizerFactory?.Create(typeof(DefaultDataAnnotation));
            if (l != null)
            {
                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(x => l[DefaultDataAnnotation.ValueIsInvalidAccessor, x]);
                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(x => l[DefaultDataAnnotation.ValueMustBeANumberAccessor, x]);
                options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(x => l[DefaultDataAnnotation.MissingBindRequiredValueAccessor, x]);
                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) => l.GetString(DefaultDataAnnotation.AttemptedValueIsInvalidAccessor, x, y));
                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => l[DefaultDataAnnotation.MissingKeyOrValueAccessor]);
                options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(x => l[DefaultDataAnnotation.UnknownValueIsInvalidAccessor, x]);
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(x => l[DefaultDataAnnotation.ValueMustNotBeNullAccessor, x]);
                options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor(x => l[DefaultDataAnnotation.NonPropertyAttemptedValueIsInvalidAccessor, x]);
                options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => l[DefaultDataAnnotation.UnknownValueIsInvalidAccessor]);
                options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => l[DefaultDataAnnotation.NonPropertyValueMustBeANumberAccessor]);
                options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => l[DefaultDataAnnotation.MissingRequestBodyRequiredValueAccessor]);
            }
            return options;
        }


        /// <summary>
        /// 自定义的数据注解本地化
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MvcDataAnnotationsLocalizationOptions SetUpDataAnnotationLocalizerProvider(this MvcDataAnnotationsLocalizationOptions options)
        {
            // 将所有模型的本地化都放在一个资源文件中，如果使用默认的多个资源文件的，这里去掉即可
            options.DataAnnotationLocalizerProvider = (_, factory) => factory.Create(typeof(DataAnnotation));
            return options;
        }
    }
}