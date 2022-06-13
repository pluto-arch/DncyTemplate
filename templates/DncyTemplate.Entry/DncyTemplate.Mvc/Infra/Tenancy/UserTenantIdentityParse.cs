using Dncy.MultiTenancy.AspNetCore;
using DncyTemplate.Mvc.Constants;

namespace DncyTemplate.Mvc.Infra.Tenancy;

public class UserTenantIdentityParse : HttpTenantIdentityParseBase
{
    /// <inheritdoc />
    protected override string GetTenantIdOrNameFromHttpContextOrNull(ITenantResolveContext context, HttpContext httpContext)
    {
        var tenantIdentity = httpContext.User?.Claims?.SingleOrDefault(x => x.Type == AppConstant.TENANT_KEY)?.Value;
        return tenantIdentity;
    }

    /// <inheritdoc />
    public override string Name => "HttpContextUserTenantIdentityParse";
}