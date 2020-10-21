using System.Data.Entity;

namespace Nasibe.Model
{
    class NasibeContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOptional(c => c.Catalog)
                .WithMany(p => p.Products);
            modelBuilder.Entity<Product>()
                .Ignore(p => p.ProductRoot)
                .Ignore(p => p.TotalPrice)
                .Ignore(p => p.UnitPriceFormated);
        }
    }
}
