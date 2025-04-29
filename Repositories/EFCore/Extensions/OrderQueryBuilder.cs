using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class OrderQueryBuilder
    {
        public static String CreateOrderQuery<T>(String orderByQueryString)
        {
            var orderQueryBuilder = new StringBuilder();
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param)) { continue; }

                var propertyFromQueryName = param.Split(' ')[0];

                var objectProperty = propertyInfos.FirstOrDefault(x => x.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null) { continue; }

                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction},");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            return orderQuery;
        }
    }
}