using System.Threading.RateLimiting;
using DncyTemplate.ApiEndpoint.Constants;
using DncyTemplate.Application.Models;
using Dotnetydd.Tools.Extension;
using Microsoft.Extensions.Primitives;

namespace DncyTemplate.ApiEndpoint.Infra.RateLimits
{
    public static class AppRateLimitExtensions
    {
        public static IServiceCollection ConfigAppRateLimit(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, cancelToken) =>
                {
                    //var l = context.HttpContext.RequestServices.GetService<IStringLocalizer<SharedResources>>();
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.Response.Headers.Append("Retry-After", new StringValues("1")); // TODO 根据具体情况返回
                    context.HttpContext.Response.ContentType = AppConstant.DEFAULT_CONTENT_TYPE;
                    var res = ResultDto.TooManyRequest();
                    res.Message = res.Message;
                    await context.HttpContext.Response.WriteAsJsonAsync(res, cancellationToken: cancelToken);
                };
                options.AddPolicy("limit_ip_2_pre_ten_second", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(httpContext.Connection.RemoteIpAddress?.ToNumber().ToString(),
                        partition => new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 2,
                            Window = TimeSpan.FromSeconds(10)
                        }));

            });

            return services;
        }
    }
}
