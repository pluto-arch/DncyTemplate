namespace DncyTemplate.Domain.Infra;

#if Tenant
public interface IMultiTenant
{
    string TenantId { get; set; }
}
#endif
