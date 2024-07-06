
using Asp.Versioning.Builder;
using DncyTemplate.Application.Models;

namespace DncyTemplate.Api.Endpoints
{
    public sealed class DemoEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            ApiVersionSet apiVersionSet = app.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .HasApiVersion(new ApiVersion(2))
                .ReportApiVersions()
                .Build();

            var group = app.MapGroup("api/v{version:apiVersion}/demos")
                .WithApiVersionSet(apiVersionSet)
                .WithTags("demo endpoints");

            group.MapGet("/", GetDemos);
            group.MapPost("/", CreateDemos);
        }


        private static async Task<ResultDto> GetDemos(int pageNo, int pageSize)
        {
            await Task.Delay(111);
            return ResponseWapper.Success("okokok");
        }


        private static async Task<IResult> CreateDemos(int pageNo, int pageSize)
        {
            await Task.Delay(111);
            return Results.Ok();
        }



    }
}
