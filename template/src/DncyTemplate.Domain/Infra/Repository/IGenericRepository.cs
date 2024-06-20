using DncyTemplate.Domain.Collections;

namespace DncyTemplate.Domain.Infra.Repository
{

    public interface IGenericRepository<T>
    {
        /// <summary>
        /// 获取全部
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// 根绝主键获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<TKey>(TKey key);

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> InsertAsync(T entity);


        /// <summary>
        /// 新增多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> InsertAsync(List<T> entities);


        /// <summary>
        /// 更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(T entity);


        /// <summary>
        /// 更新多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(List<T> entities);


        /// <summary>
        /// 删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(T entity);


        /// <summary>
        /// 删除多个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(List<T> entities);


        /// <summary>
        /// 根据id删除
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        Task<int> DeleteByIdAsync<TKey>(TKey key);
    }
}