using DncyTemplate.Application.Constants;
using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Application.Models.Generics;

public class PageRequest
{
    public virtual IEnumerable<SortingDescriptor> Sorter { get; set; }

    [Range(minimum: 1, maximum: Int32.MaxValue, ErrorMessage = "PageIndexVerifyMessage")]
    public virtual int PageNo { get; set; } = 1;

    [Range(minimum: 1, maximum: 255, ErrorMessage = "PageSizeVerifyMessage")]
    public virtual int PageSize { get; set; } = AppServiceConstants.DefaultPageSize;
}