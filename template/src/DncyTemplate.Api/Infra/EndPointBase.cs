using DncyTemplate.Application.Models;

namespace DncyTemplate.Api
{
    /// <summary>
    /// 所有controller 基类
    /// </summary>
    public abstract class EndPointBase : ControllerBase, IResponseWraps
    {
        protected T GetRequiredService<T>() where T : class
        {
            return HttpContext.RequestServices.GetRequiredService<T>();
        }

        protected T GetRequiredKeyedService<T>(string key) where T : class
        {
            return HttpContext.RequestServices.GetRequiredKeyedService<T>(key);
        }
    }
}
