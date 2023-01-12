﻿using DncyTemplate.Application.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Application.Command.Product
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        [Required(ErrorMessage = "ValueIsRequired")]
        [StringLength(maximumLength: 10, MinimumLength = 3, ErrorMessage = "LengthLimit")]
        [Display(Name = "ProductName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "ValueIsRequired")]
        [StringLength(maximumLength: 200, MinimumLength = 3)]
        public string Remark { get; set; }
    }
}
