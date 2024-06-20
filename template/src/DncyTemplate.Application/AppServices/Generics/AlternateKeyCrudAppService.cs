using DncyTemplate.Application.Models;
using DncyTemplate.Application.Models.Generics;
using DncyTemplate.Domain.Collections;
using DncyTemplate.Domain.Exceptions;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Domain.Infra.Repository;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Uow;
using Dotnetydd.Tools.Models;

namespace DncyTemplate.Application.AppServices.Generics
{
    public abstract class AlternateKeyCrudAppService<TEntity, TKey, TDto, TGetListRequest, TListItemDto, TUpdateRequest, TCreateRequest>
    where TEntity : BaseEntity
    where TGetListRequest : PageRequest
    {
        protected readonly IEfRepository<TEntity, TKey> _repository;
        protected readonly IUnitOfWork _uow;
        protected readonly IMapper _mapper;

        protected AlternateKeyCrudAppService(IUnitOfWork<DncyTemplateDbContext> uow, IMapper mapper)
        {
            _repository = uow.Resolve<IEfRepository<TEntity, TKey>>();
            _uow = uow;
            _mapper = mapper;
        }

        public virtual async Task<TDto> CreateAsync(TCreateRequest requestModel, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<TEntity>(requestModel);
            await _repository.InsertAsync(entity, true, cancellationToken);
            await _uow.CompleteAsync(cancellationToken);
            return _mapper.Map<TDto>(entity);
        }

        public async Task<TDto> UpdateAsync(TKey id, TUpdateRequest requestModel, CancellationToken cancellationToken = default)
        {
            TEntity entity = await GetEntityByIdAsync(id, cancellationToken);
            _mapper.Map(requestModel, entity);
            await _repository.UpdateAsync(entity, true, cancellationToken);
            await _uow.CompleteAsync(cancellationToken);
            return _mapper.Map<TDto>(entity);
        }

        public virtual async Task<Return<TDto,ErrorResult>> GetAsync(TKey id, CancellationToken cancellationToken = default)
        {
            var data = await GetEntityByIdAsync(id, cancellationToken);
            if (data is null)
            {
                return new ErrorResult($"There is no such an entity given given key: `{id}`");
            }
            return _mapper.Map<TDto>(data);
        }


        public virtual async Task<IPagedList<TListItemDto>> GetListAsync(TGetListRequest model, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = CreateFilteredQuery(model);
            int totalCount = await _repository.AsyncExecuter.CountAsync(query, cancellationToken);
            query = ApplySorting(query, model);
            query = ApplyPaging(query, model);
            var entities = await _repository.AsyncExecuter.ToListAsync(query, cancellationToken);
            return new PagedList<TListItemDto>(_mapper.Map<List<TListItemDto>>(entities), model.PageNo, model.PageSize, totalCount);
        }


        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default) => await DeleteByIdAsync(id, cancellationToken);



        protected abstract Task<TEntity> GetEntityByIdAsync(TKey id, CancellationToken cancellationToken = default);

        protected virtual IQueryable<TEntity> CreateFilteredQuery(TGetListRequest requestModel) => _repository.QuerySet;

        protected virtual IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, TGetListRequest requestModel)
        {
            if (requestModel is PageRequest pagedRequestModel && pagedRequestModel.Sorter is not null && pagedRequestModel.Sorter.Any())
            {
                var properties = query.GetType().GetGenericArguments().First().GetProperties();

                IOrderedQueryable<TEntity> orderedQueryable = null;

                foreach (SortingDescriptor sortingDescriptor in pagedRequestModel.Sorter)
                {
                    string propertyName = (properties.SingleOrDefault(p => string.Equals(p.Name, sortingDescriptor.PropertyName, StringComparison.OrdinalIgnoreCase))?.Name)
                                          ?? throw new KeyNotFoundException(sortingDescriptor.PropertyName);
                    if (sortingDescriptor.SortDirection == SortingOrder.Ascending)
                    {
                        orderedQueryable = orderedQueryable is null ? query.OrderBy(propertyName) : orderedQueryable.ThenBy(propertyName);
                    }
                    else if (sortingDescriptor.SortDirection == SortingOrder.Descending)
                    {
                        orderedQueryable = orderedQueryable is null ? query.OrderByDescending(propertyName) : orderedQueryable.ThenByDescending(propertyName);
                    }
                }

                return orderedQueryable ?? query;
            }
            return query;
        }

        protected virtual IQueryable<TEntity> ApplyPaging(IQueryable<TEntity> query, TGetListRequest requestModel)
        {
            if (requestModel is PageRequest model)
            {
                return query.Skip((model.PageNo - 1) * model.PageSize).Take(model.PageSize);
            }
            return query;
        }

        protected abstract Task DeleteByIdAsync(TKey id, CancellationToken cancellationToken = default);
    }
}
