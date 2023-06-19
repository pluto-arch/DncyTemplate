namespace DncyTemplate.Api.Infra.UnitOfWork
{
    public static class UnitOfWorkMiddlewareExtension
    {

        /// <summary>
        /// 使用uow中间件
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<UnitOfWorkMiddleware>();
            return services;
        }


        /// <summary>
        /// 使用uow中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
        {
            app.UseMiddleware<UnitOfWorkMiddleware>();
            return app;
        }
    }
}