using System.Diagnostics.CodeAnalysis;

namespace DncyTemplate.Application.AppServices.Generics;

public enum SortingOrder { Ascending, Descending }


public class SortingDescriptor
{

    [AllowNull]
    public string PropertyName { get; set; }

    public SortingOrder SortDirection { get; set; }
}