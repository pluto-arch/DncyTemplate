using DncyTemplate.Application.Models.Product;
using System.ComponentModel.DataAnnotations;

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
}
