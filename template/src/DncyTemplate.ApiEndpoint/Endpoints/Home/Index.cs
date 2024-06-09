using DncyTemplate.Application.Models;

namespace DncyTemplate.ApiEndpoint.Endpoints.Home
{
    public class Index : EndpointWithoutRequest,IResponseWraps
    {
        public override void Configure()
        {
            Get("/home");
            AllowAnonymous();
            Options(x=>x.RequireRateLimiting("limit_ip_2_pre_ten_second"));
            Summary(c=>c.Summary="home v0");
        }


        public override async Task HandleAsync(CancellationToken ct)
        {
            if (Random.Shared.Next(1,100)>50)
            {
                throw new InvalidOperationException("1231");
            }
            await SendAsync(this.Success("hello home index"), cancellation: ct);
        }
    }


    public class IndexV1 : EndpointWithoutRequest,IResponseWraps
    {
        public override void Configure()
        {
            Get("/home");
            AllowAnonymous();
            Version(1);
            Summary(c=>c.Summary="home v1");
        }


        public override async Task HandleAsync(CancellationToken ct)
        {
            if (Random.Shared.Next(1,100)>50)
            {
                throw new InvalidOperationException("2222222222222222");
            }
            await SendAsync(this.Success("hello home index v1"), cancellation: ct);
        }
    }
}