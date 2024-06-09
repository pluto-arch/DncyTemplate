#nullable enable
using DncyTemplate.Application.AppServices.Product;
using DncyTemplate.Application.Models;
using DncyTemplate.Application.Models.Generics;
using DncyTemplate.Application.Models.Product;
using FastEndpoints;
using FluentValidation;

namespace DncyTemplate.ApiEndpoint.Endpoints.Product
{
    public class List : Endpoint<PagedRequest, ResultDto>, IResponseWraps
    {

        public required IProductAppService ProductAppService { get; set; }


        public override void Configure()
        {
            Get("/products");
            Summary(s => s.Description = "获取产品列表");
        }

        public override async Task<ResultDto> ExecuteAsync(PagedRequest req, CancellationToken ct)
        {
            var res = await ProductAppService.GetListAsync(new ProductPagedRequest
            {
                Sorter = req.Sorter,
                PageNo = req.PageNo,
                PageSize = req.PageSize,
                Keyword = req.Keyword
            }, ct);
            return this.Success(res);
        }
    }

    #region request model
    
    public class PagedRequest : PageRequest
    {
        public string? Keyword { get; set; }

        public override IEnumerable<SortingDescriptor>? Sorter { get; set; }
    }

    public class Validate : Validator<PagedRequest>
    {
        public Validate()
        {
            RuleFor(x => x.Keyword)
                .MinimumLength(5)
                .WithMessage("UserName is too short!");
        }
    }

    #endregion
}
