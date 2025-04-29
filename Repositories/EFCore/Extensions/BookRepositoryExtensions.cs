using Entities.Models;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

namespace Repositories.EFCore.Extensions
{
    public static class BookRepositoryExtensions
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, uint minPrice, uint maxPrice)
        {
            return books.Where(x => x.Price >= minPrice && x.Price <= maxPrice);
        }

        public static IQueryable<Book> Search(this IQueryable<Book> books, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) { return books; }

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return books.Where(x => x.Title.ToLower().Contains(searchTerm));
        }

        public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                return books.OrderBy(x => x.Id);
            }

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Book>(orderByQueryString);

            if (orderQuery is null) { return books.OrderBy(x => x.Id); }

            return books.OrderBy(orderQuery); // System.Linq.Dynamic.Core package install
        }
    }
}