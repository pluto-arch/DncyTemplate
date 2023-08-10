using Dncy.MultiTenancy.AspNetCore;

namespace DncyTemplate.Api.Infra.Tenancy
{
    public static class TenancyApplicationBuilderExtension
    {
        /// <summary>
        /// 异常处理中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app)
        {
            app.UseMiddleware<MultiTenancyMiddleware>();
            return app;
        }
    }
}