using System.Web;
using DncyTemplate.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Mvc.Controllers
{
    /// <summary>
    /// 错误处理器
    /// </summary>
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly IStringLocalizer<SharedResource> _localizedizer;
        public ErrorController(IStringLocalizer<SharedResource> localizedizer)
        {
            _localizedizer = localizedizer;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("/error/{code:int}/{error?}")]
        public IActionResult Error(int code, string error = null)
        {
            ViewData["ErrorCode"] = $"{code}";
            ViewData["ErrorMessage"] = _localizedizer[SharedResource.ServiceUnavailable];
            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            if (statusCodeReExecuteFeature is not null)
            {
                string originalPathAndQuery = string.Join(
                    statusCodeReExecuteFeature.OriginalPathBase,
                    statusCodeReExecuteFeature.OriginalPath,
                    statusCodeReExecuteFeature.OriginalQueryString);
                ViewData["ErrorMessage"] = $"{_localizedizer[SharedResource.ServiceUnavailable, originalPathAndQuery]}";
            }
            else
            {
                if (!string.IsNullOrEmpty(error))
                {
                    ViewData["ErrorMessage"] = $"{_localizedizer[SharedResource.ServiceUnavailable, HttpUtility.UrlDecode(error)]}";
                }
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id, TraceIdentifier = HttpContext.TraceIdentifier });
        }
    }
}
