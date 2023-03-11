using DncyTemplate.Domain.Collections;

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
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TDto> GetAsync(TKey id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        Task<IPagedList<TListItemDto>> GetListAsync(TGetListRequest requestModel);

        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(TKey id);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        Task<TDto> CreateAsync(TCreateRequest requestModel);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        Task<TDto> UpdateAsync(TKey id, TUpdateRequest requestModel);
    }
}
