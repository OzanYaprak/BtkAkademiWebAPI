using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi.Repositories.Config;

namespace WebApi.Repositories
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        // DbContext üzerinden devir alınmış OnModelCreating metodunu override ediyoruz ( Geçersiz kılıyoruz )
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.ApplyConfiguration(new BookConfig()); // Migration oluşturulurken BookCongif içerisindeki konfigürasyonları uygula
        }

        public DbSet<Book> Books { get; set; }
    }
}
