using DncyTemplate.Application.Models.Product;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace DncyTemplate.Application.Command.Product
{
    public class CreateProductCommand : ICommand<ProductDto>
    {
        public CreateProductCommand()
        {
            Transactional = false;
        }


        [Required(ErrorMessage = "ValueIsRequired")]
        [StringLength(maximumLength: 100, MinimumLength = 3, ErrorMessage = "LengthLimit")]
        [Display(Name = "ProductName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ValueIsRequired")]
        [StringLength(maximumLength: 200, MinimumLength = 3, ErrorMessage = "LengthLimit")]
        public string Remark { get; set; }

        /// <inheritdoc />
        public bool Transactional { get; }
    }
    
    
    public class MyModelValidator : AbstractValidator<CreateProductCommand>
    {
        public MyModelValidator()
        {
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("名称不能低于3个字符");
        }
    }
}
