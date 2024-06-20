using DncyTemplate.Domain.Collections;
using DncyTemplate.Domain.Infra;
using Dotnetydd.Tools.Models;

namespace DncyTemplate.Application.AppServices.Generics
{
    /// <summary>
    /// 基础crud 服务
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TGetListRequest"></typeparam>
    /// <typeparam name="TListItemDto"></typeparam>
    /// <typeparam name="TUpdateRequest"></typeparam>
    /// <typeparam name="TCreateRequest"></typeparam>
    public interface ICrudAppService<in TKey, TDto, in TGetListRequest, TListItemDto, in TUpdateRequest, in TCreateRequest>
    {
        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        Task<Return<TDto,ErrorResult>> GetAsync(TKey id, CancellationToken cancellationToken=default);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<IPagedList<TListItemDto>> GetListAsync(TGetListRequest requestModel,CancellationToken cancellationToken=default);

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync(TKey id,CancellationToken cancellationToken = default);

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        Task<TDto> CreateAsync(TCreateRequest requestModel, CancellationToken cancellationToken=default);

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        Task<TDto> UpdateAsync(TKey id, TUpdateRequest requestModel,CancellationToken cancellationToken=default);
    }
}
