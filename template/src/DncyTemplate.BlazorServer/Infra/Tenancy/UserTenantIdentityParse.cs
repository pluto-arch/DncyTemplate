#if  Tenant
using Dotnetydd.MultiTenancy.AspNetCore;
using Dotnetydd.MultiTenancy.AspNetCore.TenantIdentityParse;

namespace DncyTemplate.BlazorServer;

public class UserTenantIdentityParse : HttpTenantIdentityParseBase
{
    /// <inheritdoc />
    protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
    {
        var tenantIdentity = httpContext.User?.Claims?.SingleOrDefault(x => x.Type == "")?.Value;
        return tenantIdentity;
    }

    /// <inheritdoc />
    public override string Name => "HttpContextUserTenantIdentityParse";
}
#endif