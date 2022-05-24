using Dncy.Specifications;
using Dncy.Specifications.Extensions;
using EntityFrameworkCore.Specifications.Test.Models;

namespace EntityFrameworkCore.Specifications.Test;

public class IdBetweenOneAndTenSpecification:Specification<User>
{
    public IdBetweenOneAndTenSpecification()
    {
        Query.Where(x => x.Id >= 1&&x.Id<=10);
    }
}