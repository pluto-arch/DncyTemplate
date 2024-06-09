using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace DncyTemplate.Api.Infra
{

    /// <summary>
    /// 调整控制器名称不以controller结尾
    /// </summary>
    public class CustomControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            return typeInfo.IsClass && typeInfo is { IsAbstract: false, IsPublic: true } &&
                   typeInfo.IsAssignableTo(typeof(EndPointBase)) &&
                   !typeInfo.ContainsGenericParameters;
        }
    }

    /// <summary>
    /// 在路由中替换特定字符串
    /// </summary>
    public class CustomControllerNameConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                // 例如，将控制器名称中的 "Endpoint" 替换为空字符串
                if (controller.ControllerName.EndsWith("Endpoints"))
                {
                    controller.ControllerName = controller.ControllerName.Replace("Endpoints", string.Empty);
                }
            }
        }
    }

    /// <summary>
    /// 路由小写转换
    /// </summary>
    public class LowercaseDashedRouteTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object? value)
        {
            if (value == null)
                return null!;

            return value.ToString()?.ToLower()!;
        }
    }
}
