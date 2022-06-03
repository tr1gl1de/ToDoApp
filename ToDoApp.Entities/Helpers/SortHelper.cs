using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace ToDoApp.Entities.Helpers;

public class SortHelper<T> : ISortHelper<T>
{
    public IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString)
    {
        if (!entities.Any())
        {
            return entities;
        }

        if (string.IsNullOrWhiteSpace(orderByQueryString))
        {
            return entities;
        }

        var orderParams = orderByQueryString.Trim().Split(",");
        var propertyInfo = typeof(T).GetProperties(
            BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                continue;
            }

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfo.FirstOrDefault(
                pi => pi.Name.Equals(
                    propertyFromQueryName,
                    StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty == null)
            {
                continue;
            }

            var stringOrder = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name} {stringOrder}");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

        if (string.IsNullOrWhiteSpace(orderQuery))
        {
            return entities;
        }

        // use System.Dynamic.Core
        return entities.OrderBy(orderQuery);
    }
}