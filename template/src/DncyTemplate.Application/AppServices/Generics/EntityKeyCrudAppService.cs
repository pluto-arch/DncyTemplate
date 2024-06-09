using DncyTemplate.Application.Models.Generics;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Uow;

namespace DncyTemplate.Application.AppServices.Generics
{
    public class EntityKeyCrudAppService<TEntity, TKey, TDto, TGetListRequest, TListItemDto, TUpdateRequest, TCreateRequest>
    : AlternateKeyCrudAppService<TEntity, TKey, TDto, TGetListRequest, TListItemDto, TUpdateRequest, TCreateRequest>
    where TEntity : BaseEntity<TKey>
    where TGetListRequest : PageRequest
    {

        /// <inheritdoc />
        public EntityKeyCrudAppService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
        {
        }

        protected override async Task<TEntity> GetEntityByIdAsync(TKey id, CancellationToken cancellationToken = default) => await _repository.GetAsync(id, cancellationToken);

        protected override async Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default) => await _repository.DeleteAsync(id, true, cancellationToken);
    }
}
