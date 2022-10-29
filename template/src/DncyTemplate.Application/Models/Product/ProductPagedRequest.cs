﻿using System.ComponentModel.DataAnnotations;
using DncyTemplate.Application.Models.Generics;

namespace DncyTemplate.Application.Models.Product;

public class ProductPagedRequest: PageRequest
{
    [MaxLength(3)]
    public string Keyword { get; set; }
}