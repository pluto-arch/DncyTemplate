using System.Net;
using System.Security.Claims;

using DncyTemplate.Mvc.Constants;

using Microsoft.AspNetCore.Authentication;

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
        var tenantId = _context.Request.Cookies.FirstOrDefault(x => x.Key == AppConstant.TENANT_KEY);
        await Task.Yield();
        return DebugUser(tenantId.Value);
    }

    private static AuthenticateResult DebugUser(string tenantId)
    {
        var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "测试用户"),
            };
        if (!string.IsNullOrEmpty(tenantId))
        {
            claims.Add(new Claim(AppConstant.TENANT_KEY, tenantId));
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