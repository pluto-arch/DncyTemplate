namespace DncyTemplate.Application.Models.Permission
{
    public class MenuModel
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 下级
        /// </summary>
        public IList<MenuModel> Childrens { get; set; }

        /// <summary>
        /// 操作
        /// </summary>
        public List<MenuOperateModel> Operates { get; set; }
    }
}