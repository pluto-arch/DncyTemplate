using DncyTemplate.Application.AppServices.Generics;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Infra.EntityFrameworkCore.DbContexts;
using DncyTemplate.Infra.EntityFrameworkCore.Repository;
using DncyTemplate.Uow;
using Dotnetydd.Tools.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DncyTemplate.Application.AppServices.Product;

[Injectable(InjectLifeTime.Scoped, typeof(IProductAppService))]
public class ProductAppService
    : EntityKeyCrudAppService<Domain.Aggregates.Product.Product, string, ProductDto, ProductPagedRequest, ProductListItemDto, ProductUpdateRequest, ProductCreateRequest>, IProductAppService
{
    /// <inheritdoc />
    public ProductAppService(IUnitOfWork<DncyTemplateDbContext> uow, IMapper mapper) : base(uow, mapper)
    {
    }

    protected override IQueryable<Domain.Aggregates.Product.Product> CreateFilteredQuery(ProductPagedRequest requestModel)
    {
        var q = base.CreateFilteredQuery(requestModel);
        if (!string.IsNullOrEmpty(requestModel.Keyword))
        {
            q = q.Where(x => EF.Functions.Like(x.Name, $"%{requestModel.Keyword}%"));
        }
        return q;
    }


    public async Task<Return<ProductDto, string>> GetByNameAsync(string productName, CancellationToken cancellationToken = default)
    {
        var res = await _repository.FirstOrDefaultAsync(x => x.Name == productName, cancellationToken);
        if (res == null)
        {
            return "entity not found";
        }
        return _mapper.Map<ProductDto>(res);
    }



    public async Task<Return<ProductDto, string>> GetByNameWithDapperAsync(string productName, CancellationToken cancellationToken = default)
    {
        const string sql = $@"SELECT Id,Name,CreationTime,Remark FROM Products WHERE Name=@name";
        var res = await _uow.Context.SingleOrDefaultFromSqlAsync<ProductDto>(sql, new Dapper.DynamicParameters(new { name = productName }), cancellationToken: cancellationToken);
        if (res == null)
        {
            return "entity not found";
        }
        return _mapper.Map<ProductDto>(res);
    }

}