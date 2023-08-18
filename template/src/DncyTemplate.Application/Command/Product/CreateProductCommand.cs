using DncyTemplate.Application.Models.Product;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Application.Command.Product
{
    public class CreateProductCommand : ICommand<ProductDto>
    {
        public CreateProductCommand()
        {
            Transactional = false;
        }

        [Required]
        [Display(Name = "ProductName")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 3,ErrorMessage = "LengthLimit")]
        public string Remark { get; set; }

        /// <inheritdoc />
        public bool Transactional { get; }
    }
    
    
    public class MyModelValidator : AbstractValidator<CreateProductCommand>
    {
        public MyModelValidator(IStringLocalizer<CreateProductCommand> localizer)
        {
            RuleFor(x => x.Name).MinimumLength(3).WithMessage(localizer.GetString("FluentLengthLimit"));
        }
    }
}
