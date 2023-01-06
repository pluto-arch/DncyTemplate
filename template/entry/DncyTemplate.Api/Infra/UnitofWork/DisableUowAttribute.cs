namespace DncyTemplate.Api.Infra.UnitofWork;

[AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
public class DisableUowAttribute:Attribute
{
}