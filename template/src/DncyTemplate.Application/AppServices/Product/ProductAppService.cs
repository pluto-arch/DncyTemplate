
using AutoMapper;
using DncyTemplate.Application.AppServices.Generics;
using DncyTemplate.Application.Models.Product;
using Microsoft.EntityFrameworkCore;

namespace DncyTemplate.Application.AppServices.Product;

[Injectable(InjectLifeTime.Scoped, typeof(IProductAppService))]
public class ProductAppServiceIProductAppService: IProductAppService
{
}