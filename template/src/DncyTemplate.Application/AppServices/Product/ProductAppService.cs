using DncyTemplate.Application.AppServices.Generics;
using DncyTemplate.Application.Models.Product;
using DncyTemplate.Infra.EntityFrameworkCore.Repository;
using DncyTemplate.Uow;
using Dotnetydd.Tools.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DncyTemplate.Application.AppServices.Product;

[Injectable(InjectLifeTime.Scoped, typeof(IProductAppService))]
public class ProductAppService
    : EntityKeyCrudAppService<Domain.Aggregates.Product.Product, string, ProductDto, ProductPagedRequest, ProductListItemDto, ProductUpdateRequest, ProductCreateRequest>, IProductAppService
{
    /// <inheritdoc />
    public ProductAppService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper)
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


    public async Task<Return<ProductDto, string>> GetByName(string productName)
    {
        var res = await _repository.FirstOrDefaultAsync(x => x.Name == productName);
        if (res == null)
        {
            return "entity not found";
        }
        return _mapper.Map<ProductDto>(res);
    }



    public async Task<Return<ProductDto, string>> GetByNameWithDapper(string productName)
    {
        const string sql = $@"SELECT Id,Name,CreationTime,Remark FROM Products WHERE Name=@name";
        var res = await _uow.Context.SingleOrDefaultFromSqlAsync<ProductDto>(sql, new Dapper.DynamicParameters(new { name = productName }));
        if (res == null)
        {
            return "entity not found";
        }
        return _mapper.Map<ProductDto>(res);
    }

}