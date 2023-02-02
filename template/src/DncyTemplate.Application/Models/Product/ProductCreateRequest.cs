using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Application.Models.Product;

public class ProductCreateRequest
{
    [Required(ErrorMessage = "ValueIsRequired")]
    [StringLength(maximumLength: 10, MinimumLength = 3, ErrorMessage = "LengthLimit")]
    [Display(Name = "ProductName")]
    public string Name { get; set; }

    [Required(ErrorMessage = "ValueIsRequired")]
    [StringLength(maximumLength: 200, MinimumLength = 3)]
    public string Remark { get; set; }
}