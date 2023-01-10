
using AutoMapper;
using Dncy.SnowFlake;
using DncyTemplate.Application.AppServices.Generics;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.DomainEvents.Product;
using DncyTemplate.Domain.Repository;
using DncyTemplate.Infra.Utils;
using Microsoft.EntityFrameworkCore;

namespace DncyTemplate.Application.AppServices.Product;

[Injectable(InjectLifeTime.Scoped, typeof(IProductAppService))]
public class ProductAppService
    : EntityKeyCrudAppService<Domain.Aggregates.Product.Product, string, ProductDto, ProductPagedRequest, ProductListItemDto, ProductUpdateRequest, ProductCreateRequest>, IProductAppService
{

    private static SnowFlake idg_1 = new SnowFlake(1);

    /// <inheritdoc />
    public ProductAppService(IRepository<Domain.Aggregates.Product.Product, string> repository, IMapper mapper) : base(repository, mapper)
    {
    }

    /// <inheritdoc cref="AlternateKeyCrudAppService{TEntity,TKey,TDto,TGetListRequest,TListItemDto,TUpdateRequest,TCreateRequest}"/>
    public override async Task<ProductDto> CreateAsync(ProductCreateRequest requestModel)
    {
        var entity = _mapper.Map<Domain.Aggregates.Product.Product>(requestModel);
        entity.Id = SnowFlakeId.Generator.GetUniqueId();
        entity.CreationTime = DateTimeOffset.Now;
        entity.AddDomainEvent(new NewProductCreateDomainEvent(entity));
        entity = await _repository.InsertAsync(entity, true);
        return _mapper.Map<ProductDto>(entity);
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