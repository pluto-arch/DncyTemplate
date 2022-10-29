using System.Net;
using System.Security.Claims;

using Dncy.Tools;

using DncyTemplate.Mvc.Constants;

using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace DncyTemplate.Mvc.Infra.Authorization;


/// <summary>
/// TODO 临时的认证scheme处理程序，使用测试用户和cookie的租户
/// </summary>
public class TempAuthenticationHandler : IAuthenticationHandler
{
    public const string SchemeName = "TempAuthScheme";

    private HttpContext _context;

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="scheme"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        _context = context;
        return Task.CompletedTask;
    }

    public async Task<AuthenticateResult> AuthenticateAsync()
    {
        var userInfo= _context.Request.Cookies.FirstOrDefault(x => x.Key == AppConstant.TEMP_COOKIE_USER_KEY);
        if (!string.IsNullOrEmpty(userInfo.Value))
        {
            await Task.Yield();
            var uers=JsonConvert.DeserializeObject<TempUser>(userInfo.Value);
            if (uers == null)
            {
                return AuthenticateResult.Fail("未登录");
            }
            return DebugUser(uers);
        }
        await Task.Yield();
        return AuthenticateResult.Fail("未登录");
    }

    private static AuthenticateResult DebugUser(TempUser loginInfo)
    {
        var claims = new List<Claim>();
        if (!string.IsNullOrEmpty(loginInfo.TenantId))
        {
            claims.Add(new Claim(ClaimTypes.Name, loginInfo.UserName));
            claims.Add(new Claim(AppConstant.TENANT_KEY, loginInfo.TenantId));
            claims.Add(new Claim("tenant_name", loginInfo.TenantName));
            claims.Add(new Claim("user_id", loginInfo.UserId));
            claims.Add(new Claim("user_name", loginInfo.UserName));
            if (loginInfo.Roles.Length>0)
            {
                foreach (var item in loginInfo.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, item));
                }
            }
        }
        var claimsIdentity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(claimsIdentity);
        return AuthenticateResult.Success(new AuthenticationTicket(principal, SchemeName));
    }

    public Task ChallengeAsync(AuthenticationProperties properties)
    {
        _context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        return Task.CompletedTask;
    }

    public Task ForbidAsync(AuthenticationProperties properties)
    {
        _context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        return Task.CompletedTask;
    }
}


/// <summary>
/// TODO 临时角色
/// </summary>
public class TempRoles
{
    /// <summary>
    /// 系统超级管理员
    /// </summary>
    public const string SYS_SA = "sys_sa";

    /// <summary>
    /// 超级管理员
    /// </summary>
    public const string SA = "sa";

    /// <summary>
    /// 普通成员
    /// </summary>
    public const string MEMBER = "member";
}


/// <summary>
/// TODO 临时用户模型
/// </summary>
public class TempUser
{
    public string TenantId { get; set; }

    public string TenantName { get; set; }

    public string UserId { get; set; }

    public string UserName { get; set; }

    public string[] Roles { get; set; }


    public static List<TempUser> Users => new List<TempUser>
    {
        new TempUser
        {
            TenantId = "",
            TenantName = "",
            UserId = "SYSTEMSA001",
            UserName = "系统-超级管理员",
            Roles = new []{TempRoles.SYS_SA},
        },
        new TempUser
        {
            TenantId = "T20210602000001",
            TenantName = "租户1",
            UserId = "T1001",
            UserName = "租户1-超级管理员",
            Roles = new []{TempRoles.SA},
        },
        new TempUser
        {
            TenantId = "T20210602000001",
            TenantName = "租户一",
            UserId = "T1002",
            UserName = "租户1-成员1",
            Roles = new []{TempRoles.MEMBER},
        },
        new TempUser
        {
            TenantId = "T20210602000002",
            TenantName = "租户2",
            UserId = "T2001",
            UserName = "租户2-超级管理员",
            Roles = new []{TempRoles.SA},
        },
        new TempUser
        {
            TenantId = "T20210602000002",
            TenantName = "租户2",
            UserId = "T2002",
            UserName = "租户2-成员1",
            Roles = new []{TempRoles.MEMBER},
        },
        new TempUser
        {
            TenantId = "T20210602000003",
            TenantName = "租户3",
            UserId = "T3001",
            UserName = "租户3-超级管理员",
            Roles = new []{TempRoles.SA},
        },
        new TempUser
        {
            TenantId = "T20210602000003",
            TenantName = "租户3",
            UserId = "T3002",
            UserName = "租户3-成员1",
            Roles = new []{TempRoles.MEMBER},
        }
    };
}