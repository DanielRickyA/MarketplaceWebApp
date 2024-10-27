using MarketWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Produk> Produk { get; set; }
        public DbSet<Keranjang> Keranjang { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
