using DncyTemplate.Application.Models.Generics;
using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Application.Models.Product;

public class ProductPagedRequest : PageRequest
{
    [MaxLength(3, ErrorMessage = "MaxLengthValidate")]
    public string Keyword { get; set; }
}