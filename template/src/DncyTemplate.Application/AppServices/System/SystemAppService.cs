using DncyTemplate.Application.AppServices.Product;
using DncyTemplate.Application.Models.Permission;
using DncyTemplate.Application.Permission;

namespace DncyTemplate.Application.AppServices.System
{
    [Injectable(InjectLifeTime.Scoped, typeof(ISystemAppService))]
    public class SystemAppService: ISystemAppService
    {
        private static List<MenuModel> _cacheMenus = new List<MenuModel>();


        public SystemAppService()
        {
            InitInMemoryCacheMonus();
        }

        private void InitInMemoryCacheMonus()
        {
            _cacheMenus.Add(new MenuModel
            {
                Code = "",
                Name = "看板",
                Url = "/home/index",
                Childrens = null,
                Operates = null
            });

            _cacheMenus.Add(new MenuModel
            {
                Code = ProductPermission.Product.Default,
                Name = "产品列表",
                Url = "/home/Product",
                Childrens = null,
                Operates = new List<MenuOperateModel>
                {
                    new MenuOperateModel
                    {
                        Code = ProductPermission.Product.Create,
                        Name = "新建"
                    },
                    new MenuOperateModel
                    {
                        Code = ProductPermission.Product.Edit,
                        Name = "编辑"
                    },
                    new MenuOperateModel
                    {
                        Code = ProductPermission.Product.Delete,
                        Name = "删除"
                    }
                }
            });

        }


        /// <inheritdoc />
        public async Task<List<MenuModel>> GetSysMenus()
        {
            await Task.Delay(1);
            return _cacheMenus;
        }
    }
}