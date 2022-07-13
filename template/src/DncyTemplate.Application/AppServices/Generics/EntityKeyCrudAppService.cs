using AutoMapper;
using DncyTemplate.Application.Models.Generics;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Repository;

namespace DncyTemplate.Application.AppServices.Generics;

public class EntityKeyCrudAppService<TEntity, TKey, TDto, TGetListRequest, TListItemDto, TUpdateRequest, TCreateRequest>
    : AlternateKeyCrudAppService<TEntity, TKey, TDto, TGetListRequest, TListItemDto, TUpdateRequest, TCreateRequest>
    where TEntity : BaseEntity<TKey>
    where TGetListRequest : PageRequest
{

    protected new readonly IRepository<TEntity, TKey> _repository;


    /// <inheritdoc />
    public EntityKeyCrudAppService(IRepository<TEntity, TKey> repository, IMapper mapper) : base(repository, mapper)
    {
        _repository= repository;
    }

    protected override async Task<TEntity> GetEntityByIdAsync(TKey id) => await _repository.GetAsync(id);

    protected override async Task DeleteByIdAsync(TKey id) => await _repository.DeleteAsync(id, true);
}