using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using GPMS.Backend.Services.PageRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Utils
{
    public static class LinQUtil
    {
        public static IQueryable<E> SortBy<E>
        (this IQueryable<E> query, BaseFilterModel filterModel)
        where E : class
        {
            Type entityType = typeof(E);
            if (filterModel.OrderBy.IsNullOrEmpty())
            {
                filterModel.OrderBy = "Id";
            }
            PropertyInfo? property = entityType.GetProperty(filterModel.OrderBy);
            if (property == null)
            {
                property = entityType.GetProperty("Id");
            }
            var parameter = Expression.Parameter(entityType, "entity"); //entity
            var propertyAccess = Expression.Property(parameter, property); //entity.property
            var orderByProperty = Expression.Lambda<Func<E, Guid>>(propertyAccess, parameter); //entity => entity.property
            if (filterModel.IsAscending)
                query = query.OrderBy(orderByProperty);
            else
                query = query.OrderByDescending(orderByProperty);

            return query;
        }

        public static IQueryable<E> Filter<E, F>(this IQueryable<E> query, F entityFilterModel)
        where E : class
        where F : class
        {
            foreach (PropertyInfo propertyInfo in entityFilterModel.GetType().GetProperties())
            {
                var entityFilterModelFieldValue = propertyInfo.GetValue(entityFilterModel);
                if (!entityFilterModelFieldValue.Equals(null))
                {
                    if (propertyInfo.PropertyType.Name.Equals(typeof(string).Name))
                    {
                        query.Where(entity => entity.GetType().GetProperty(propertyInfo.Name).GetValue(entity).ToString().Contains(entityFilterModelFieldValue.ToString(),StringComparison.OrdinalIgnoreCase));
                    }
                }
            }
            return query;
        }

        public static List<E> PagingEntityList<E>(this List<E> entityList, BaseFilterModel baseFilterModel)
        {
            return entityList.Skip((baseFilterModel.PageIndex - 1) * baseFilterModel.PageSize)
                            .Take(baseFilterModel.PageSize)
                            .ToList();
        }
        public static IQueryable<E> PagingEntityQuery<E>(this IQueryable<E> query, BaseFilterModel baseFilterModel)
        {
            return query.Skip((baseFilterModel.PageIndex - 1) * baseFilterModel.PageSize)
                            .Take(baseFilterModel.PageSize);
        }
    }
}