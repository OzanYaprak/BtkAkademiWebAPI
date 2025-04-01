using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
    }
}
