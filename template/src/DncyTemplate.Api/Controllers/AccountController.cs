using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DncyTemplate.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace DncyTemplate.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AccountController : ControllerBase, IResponseWraps
    {
        private static readonly List<dynamic> users =
        [
            new
            {
                Id = 1,
                Mobile = "18530064433",
                UserName = "admin3",
                Password = "admin",
                Role = "admin",
#if Tenant
                TenantId = "T20210602000003"
#endif
            },
            new
            {
                Id = 2,
                Mobile = "18530064432",
                UserName = "admin2",
                Password = "admin",
                Role = "admin",
#if Tenant
                TenantId = "T20210602000002"
#endif
            },
            new
            {
                Id = 3,
                Mobile = "18530064431",
                UserName = "sa",
                Password = "admin",
                Role = "SystemAdmin",
#if Tenant
                TenantId = "T20210602000001"
#endif
            }
        ];

        /// <summary>
        /// 获取jwt token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public ResultDto Token([Required, FromForm(Name = "userName")] string user, [Required, FromForm(Name = "password")] string pwd)
        {
            var u = users.FirstOrDefault(x => x.UserName == user && x.Password == pwd);
            if (u == null)
            {
                return this.ErrorRequest("用户不存在");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, u.UserName),
                new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
                new Claim(ClaimTypes.Role, u.Role),
#if Tenant
                new Claim(AppConstant.TENANT_KEY, u.TenantId)
#endif
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("715B59F3CDB1CF8BC3E7C8F13794CEA9"));
            var token = new JwtSecurityToken(
                issuer: "pluto",
                audience: "123",
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(120),
                claims: claims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return this.Success(new
            {
#if Tenant
                tenant = u.TenantId,
#endif
                token = new JwtSecurityTokenHandler().WriteToken(token)
            }, $"{user} 登录成功");
        }



    }
}