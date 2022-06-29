using DncyTemplate.Mvc.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Web;

namespace DncyTemplate.Mvc.Controllers
{
    /// <summary>
    /// 错误处理器
    /// </summary>
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
            ViewData["ErrorMessage"] = _localizedizer[SharedResource.ErrorController_Error_DefaultMessage];
            var statusCodeReExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            if (statusCodeReExecuteFeature is not null)
            {
                string originalPathAndQuery = string.Join(
                    statusCodeReExecuteFeature.OriginalPathBase,
                    statusCodeReExecuteFeature.OriginalPath,
                    statusCodeReExecuteFeature.OriginalQueryString);
                ViewData["ErrorMessage"] = $"{_localizedizer[SharedResource.ErrorController_Error_DefaultMessageWithPath, originalPathAndQuery]}";
            }
            else
            {
                if (!string.IsNullOrEmpty(error))
                {
                    ViewData["ErrorMessage"] = $"{_localizedizer[SharedResource.ErrorController_Error_DefaultMessageWithMessage, HttpUtility.UrlDecode(error)]}";
                }
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id, TraceIdentifier = HttpContext.TraceIdentifier });
        }
    }
}
