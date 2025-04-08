using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WebApi.Repositories.Config;

namespace Repositories.EFCore.Context
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
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
