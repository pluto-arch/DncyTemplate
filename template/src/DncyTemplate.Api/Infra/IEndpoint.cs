using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DncyTemplate.Api.Infra
{
    public interface IEndpoint
    {
        void MapEndpoint(IEndpointRouteBuilder app);
    }


    public static class AppEndointExtension
    {
        public static void AddAppEndpoints(this IServiceCollection services)
        {
            ServiceDescriptor[] serviceDescriptors = typeof(IEndpoint).Assembly!
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                               type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                .ToArray();

            services.TryAddEnumerable(serviceDescriptors);
        }


        public static void MapApEndpoints(this WebApplication app)
        {
            IEnumerable<IEndpoint> endpoints = app.Services
                .GetRequiredService<IEnumerable<IEndpoint>>();

            foreach (IEndpoint endpoint in endpoints)
            {
                endpoint.MapEndpoint(app);
            }
        }
    }
}
