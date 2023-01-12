
using AutoMapper;
using DncyTemplate.Application.AppServices.Generics;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace DncyTemplate.Application.AppServices.Product;

[Injectable(InjectLifeTime.Scoped, typeof(IProductAppService))]
public class ProductAppService
    : EntityKeyCrudAppService<Domain.Aggregates.Product.Product, string, ProductDto, ProductPagedRequest, ProductListItemDto, ProductUpdateRequest, ProductCreateRequest>, IProductAppService
{
    /// <inheritdoc />
    public ProductAppService(IRepository<Domain.Aggregates.Product.Product, string> repository, IMapper mapper) : base(repository, mapper)
    {
    }

    protected override IQueryable<Domain.Aggregates.Product.Product> CreateFilteredQuery(ProductPagedRequest requestModel)
    {
        var q = base.CreateFilteredQuery(requestModel);
        if (!string.IsNullOrEmpty(requestModel.Keyword))
        {
            q = q.Where(x => EF.Functions.Like(x.Name, $"%{requestModel.Keyword}%"));
        }
        return q;
    }
}