using DncyTemplate.Application.AppServices.Generics;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DncyTemplate.Api.Models.ModelBinding
{
    public class SortingBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(IEnumerable<SortingDescriptor>))
            {
                return new SortingModelBinder();
            }

            return null;
        }
    }

    public class SortingModelBinder : IModelBinder
    {

        private static readonly Dictionary<string, SortingOrder> SortingDirectionMap = new()
        {
            { "asc", SortingOrder.Ascending },
            { "desc", SortingOrder.Descending }
        };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext = bindingContext ?? throw new ArgumentNullException(nameof(bindingContext));

            string modelName = bindingContext.ModelName;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            string value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            var sorter = JsonConvert.DeserializeObject<IDictionary<string, string>>(value);

            if (sorter is not null)
            {
                var effectSorter = sorter.Where(item => SortingDirectionMap.ContainsKey(item.Value));
                var sorting = effectSorter.Select(item => new SortingDescriptor
                {
                    PropertyName = item.Key,
                    SortDirection = SortingDirectionMap[item.Value]
                });

                bindingContext.Result = ModelBindingResult.Success(sorting);
            }

            return Task.CompletedTask;
        }
    }
}