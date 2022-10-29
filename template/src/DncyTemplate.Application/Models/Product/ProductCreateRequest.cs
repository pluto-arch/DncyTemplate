using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Application.Models.Product;

public class ProductCreateRequest
{
    [Required(ErrorMessage = "ValueIsRequired")]
    [StringLength(maximumLength: 10, MinimumLength = 3, ErrorMessage = "LengthLimit")]
    [Display(Name = "ProductName")]
    public string Name { get; set; }

    public string Remark { get; set; }
}