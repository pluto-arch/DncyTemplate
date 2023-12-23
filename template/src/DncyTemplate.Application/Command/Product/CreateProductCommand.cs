using DncyTemplate.Application.Models.Product;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Application.Command.Product
{
    public class CreateProductCommand : ICommand<ProductDto>
    {
        public string Name { get; set; }

        public string Remark { get; set; }
    }


    public class CreateProductCommandlValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandlValidator(IStringLocalizer<CreateProductCommand> localizer)
        {
            RuleFor(x => x.Name)
                .MinimumLength(3)
                .MaximumLength(20)
                .WithMessage(localizer.GetString("FluentLengthLimit"));
        }
    }
}
