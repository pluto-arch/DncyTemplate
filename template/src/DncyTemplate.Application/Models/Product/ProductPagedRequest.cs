using DncyTemplate.Application.Models.Generics;
using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Application.Models.Product;

public class ProductPagedRequest : PageRequest
{
    [MaxLength(3)]
    public string Keyword { get; set; }
}