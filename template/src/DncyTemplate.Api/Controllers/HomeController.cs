﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.ComTypes;
using DncyTemplate.Application.Models;
using DncyTemplate.Application.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [AutoResolveDependency]
    [ApiController]
    public partial class HomeController : ControllerBase, IResponseWraps
    {
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
        [Authorize(policy:ProductPermission.Product.Default)]
        public ResultDto ForbidDemo()
        {
            return this.Success("home index");
        }


        /// <summary>
        /// 模型绑定失败 测试接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ResultDto BadRequestDemo([MinLength(5,ErrorMessage = "minlength is 4")]string key)
        {
            return this.Success("home index");
        }
    }

}

