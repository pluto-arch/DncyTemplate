using System.ComponentModel.DataAnnotations;
using DncyTemplate.Api.Resources;
using DncyTemplate.Application.AppServices.Demo;
using DncyTemplate.Application.Models;
using DncyTemplate.Application.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [AutoResolveDependency]
    [ApiController]
    public partial class HomeController : BaseController
    {

        [AutoInject]
        private readonly IStringLocalizer<SharedResource> _localizer;

        [AutoInject]
        private readonly IDemoAppServices _demoAppServices;


        [HttpGet]
        [AllowAnonymous]
        public ResultDto Index()
        {
            ThrowHelper.ThrowInvalidOperationException("eww");
            return this.Success("home index");
        }


        /// <summary>
        /// 401测试接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ResultDto UnAuthDemo()
        {
            return this.Success("home index");
        }

        /// <summary>
        /// 403 测试接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(policy: ProductPermission.Product.Default)]
        public ResultDto ForbidDemo()
        {
            return this.Success("home index");
        }


        /// <summary>
        /// 模型绑定失败 测试接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResultDto BadRequestDemo([MinLength(5, ErrorMessage = "minlength is 4")] string key)
        {
            return this.Success("home index");
        }


        [HttpGet]
        public IActionResult SuccessData()
        {
            if (DateTime.Now.Ticks % 2 == 0)
            {
                return BadRequest(this.ErrorRequest());
            }
            return Ok(this.Success("successed"));
        }


        [HttpGet]
        public IActionResult LocalTest()
        {
            var res = _localizer[SharedResource.CurrentTime, "not working"];

            var str2 = _demoAppServices.GetLocalString();

            return Ok(this.Success(new { res, res2 = str2 }));
        }
    }

}

