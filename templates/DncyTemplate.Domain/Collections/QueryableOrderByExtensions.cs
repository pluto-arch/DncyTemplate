﻿using System.Collections.Concurrent;

namespace DncyTemplate.Domain.Collections;

public static class QueryableOrderByExtensions
{
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    {
        return OrderingHelper<T>.OrderBy(source, propertyName);
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    {
        return OrderingHelper<T>.OrderByDescending(source, propertyName);
    }

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string propertyName)
    {
        return OrderingHelper<T>.ThenBy(source, propertyName);
    }

    public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string propertyName)
    {
        return OrderingHelper<T>.ThenByDescending(source, propertyName);
    }

    private static class OrderingHelper<TSource>
    {
        private static readonly ConcurrentDictionary<string, LambdaExpression> cached = new();

        public static IOrderedQueryable<TSource> OrderBy(IQueryable<TSource> source, string propertyName)
        {
            return Queryable.OrderBy(source, (dynamic)CreateLambdaExpression(propertyName));
        }

        public static IOrderedQueryable<TSource> OrderByDescending(IQueryable<TSource> source, string propertyName)
        {
            return Queryable.OrderByDescending(source, (dynamic)CreateLambdaExpression(propertyName));
        }

        public static IOrderedQueryable<TSource> ThenBy(IOrderedQueryable<TSource> source, string propertyName)
        {
            return Queryable.ThenBy(source, (dynamic)CreateLambdaExpression(propertyName));
        }

        public static IOrderedQueryable<TSource> ThenByDescending(IOrderedQueryable<TSource> source,
            string propertyName)
        {
            return Queryable.ThenByDescending(source, (dynamic)CreateLambdaExpression(propertyName));
        }

        private static LambdaExpression CreateLambdaExpression(string propertyName)
        {
            if (cached.ContainsKey(propertyName))
            {
                return cached[propertyName];
            }

            ParameterExpression parameter = Expression.Parameter(typeof(TSource));
            MemberExpression body = Expression.Property(parameter, propertyName);
            LambdaExpression keySelector = Expression.Lambda(body, parameter);
            cached[propertyName] = keySelector;

            return keySelector;
        }
    }
}