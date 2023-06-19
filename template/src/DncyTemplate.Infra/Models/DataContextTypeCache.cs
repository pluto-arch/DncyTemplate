﻿using DncyTemplate.Uow;
using System.Collections.Immutable;

namespace DncyTemplate;

public static class DataContextTypeCache
{


    private static readonly List<Type> dataContexts = new List<Type>();

    public static ImmutableArray<Type> GetApplicationDataContextList() => dataContexts.ToImmutableArray();


    public static void AddDataContext(List<Type> dataContext)
    {
        if (dataContext == null) throw new ArgumentNullException(nameof(dataContext));
        foreach (var item in dataContext)
        {
            if (dataContexts.Contains(item))
            {
                return;
            }

            if (item.IsAssignableTo(typeof(IDataContext)))
            {
                dataContexts.Add(item);
            }
        }
    }
}