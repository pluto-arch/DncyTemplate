﻿using AutoMapper;
using DncyTemplate.Application.Models.Generics;
using DncyTemplate.Domain.Infra;
using DncyTemplate.Infra.EntityFrameworkCore;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;

namespace DncyTemplate.Application.AppServices.Generics
{
    public class EntityKeyCrudAppService<TEntity, TKey, TDto, TGetListRequest, TListItemDto, TUpdateRequest, TCreateRequest>
    : AlternateKeyCrudAppService<TEntity, TKey, TDto, TGetListRequest, TListItemDto, TUpdateRequest, TCreateRequest>
    where TEntity : BaseEntity<TKey>
    where TGetListRequest : PageRequest
    {

        /// <inheritdoc />
        public EntityKeyCrudAppService(EfUnitOfWork<DncyTemplateDbContext> uow, IMapper mapper) : base(uow, mapper)
        {
        }

        protected override async Task<TEntity> GetEntityByIdAsync(TKey id) => await _repository.GetAsync(id);

        protected override async Task DeleteByIdAsync(TKey id) => await _repository.DeleteAsync(id, true);
    }
}
