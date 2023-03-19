using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Text.RegularExpressions;
using System.Web;


namespace DncyTemplate.Api.Infra;

public class FirewallFilterAttribute : ActionFilterAttribute
{
    public int Limit { get; set; }

    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var request = context.HttpContext.Request;

        if (context.Filters.Any(m => m.ToString()?.Contains(nameof(AllowAccessFirewallAttribute)) ?? false))
        {
            return;
        }

        var _memoryCache = context.HttpContext.RequestServices.GetService<IMemoryCache>();

        var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
        // var trueIp = request.Headers["CF-Connecting-IP"].ToString(); // 由于X-Forwarded-For标头可能会被客户端篡改进行伪造，可以用CDN提供的一个转发客户端真实IP并且不可篡改的标头进行双重检查

        #region IP地址检测
        // TODO IP黑白名单校验，地域校验等
        #endregion

        #region 敏感词
        var path = HttpUtility.UrlDecode(request.Path + request.QueryString, Encoding.UTF8);
        if (Regex.Match(path ?? "", "彩票|办证|AV女优", RegexOptions.IgnoreCase).Length > 0) // 写死的敏感词，实际项目请从敏感词库中动态读取
        {
            // TODO:记录拦截日志
            context.Result = new BadRequestObjectResult("参数不合法！");
            return;
        }
        #endregion

    }
}