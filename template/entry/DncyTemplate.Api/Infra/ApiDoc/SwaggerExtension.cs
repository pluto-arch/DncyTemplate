﻿using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace DncyTemplate.Api.Infra.ApiDoc
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

#if Tenant
            // swagger 请求头上添加tenant 用来解析租户
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = AppConstant.TENANT_KEY,
                In = ParameterLocation.Header,
                Required = false
            });
#endif
        }
    }
}