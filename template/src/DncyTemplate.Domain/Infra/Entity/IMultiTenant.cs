namespace DncyTemplate.Domain.Infra;

public interface IMultiTenant
{
    string TenantId { get; set; }
}