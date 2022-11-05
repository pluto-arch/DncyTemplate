using DncyTemplate.Application.Models.Permission;

namespace DncyTemplate.Application.AppServices.System
{
    public interface ISystemAppService
    {
        /// <summary>
        /// 获取系统菜单
        /// </summary>
        /// <returns></returns>
        Task<List<MenuModel>> GetSysMenus();
    }
}