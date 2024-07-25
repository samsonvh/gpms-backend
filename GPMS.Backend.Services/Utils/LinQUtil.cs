using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using GPMS.Backend.Services.PageRequests;
using Microsoft.IdentityModel.Tokens;

namespace GPMS.Backend.Services.Utils
{
    public static class LinQUtil
    {
        public static IQueryable<E> SortByAndPaging<E>
        (this IQueryable<E> query, DefaultPageRequest pageRequest)
        where E : class
        {
            Type entityType = typeof(E);
            if (pageRequest.OrderBy.IsNullOrEmpty())
            {
                pageRequest.OrderBy = "Id";
            }
            PropertyInfo? property = entityType.GetProperty(pageRequest.OrderBy);
            if (property == null)
            {
                property = entityType.GetProperty("Id");
            }
            var parameter = Expression.Parameter(entityType, "entity"); //entity
            var propertyAccess = Expression.Property(parameter, property); //entity.property
            var orderByProperty = Expression.Lambda<Func<E, Guid>>(propertyAccess, parameter); //entity => entity.property
            if (pageRequest.IsAscending)
                query = query.OrderBy(orderByProperty);
            else
                query = query.OrderByDescending(orderByProperty);

            return query;
        }
        public static List<E> PagingEntityList<E>(this List<E> entityList, DefaultPageRequest pageRequest)
        {
            return entityList.Skip((pageRequest.PageIndex - 1) * pageRequest.PageSize)
                            .Take(pageRequest.PageSize)
                            .ToList();
        }
    }
}