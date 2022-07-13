using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Application.Models.Product;

public class ProductCreateRequest
{
    [Required(ErrorMessage = "The {0} field is required.")]
    [StringLength(maximumLength: 10, MinimumLength = 3, ErrorMessage = "The field {0} must be a string with a minimum length of {2} and a maximum length of {1}.")]
    [Display(Name = "ProductName")]
    public string Name { get; set; }

    public string Remark { get; set; }
}