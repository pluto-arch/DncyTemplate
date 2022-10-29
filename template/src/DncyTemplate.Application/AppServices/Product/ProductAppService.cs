﻿
using AutoMapper;
using DncyTemplate.Application.AppServices.Generics;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Domain.DomainEvents.Product;
using DncyTemplate.Domain.Repository;

namespace DncyTemplate.Application.AppServices.Product;

[Injectable(InjectLifeTime.Scoped,typeof(IProductAppService))]
public class ProductAppService
    :EntityKeyCrudAppService<Domain.Aggregates.Product.Product, string, ProductDto, ProductPagedRequest, ProductListItemDto, ProductUpdateRequest, ProductCreateRequest>,IProductAppService
{
    /// <inheritdoc />
    public ProductAppService(IRepository<Domain.Aggregates.Product.Product, string> repository, IMapper mapper) : base(repository, mapper)
    {
    }

    /// <inheritdoc cref="AlternateKeyCrudAppService{TEntity,TKey,TDto,TGetListRequest,TListItemDto,TUpdateRequest,TCreateRequest}"/>
    public override async Task<ProductDto> CreateAsync(ProductCreateRequest requestModel)
    {
        var entity = _mapper.Map<Domain.Aggregates.Product.Product>(requestModel);
        var stamp = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
        entity.Id=$"{stamp}{Random.Shared.Next(10000, 100000)}";
        entity = await _repository.InsertAsync(entity, true);
        entity.AddDomainEvent(new NewProductCreateDomainEvent(entity));
        return _mapper.Map<ProductDto>(entity);
    }
}