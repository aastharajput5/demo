using Microsoft.EntityFrameworkCore;
using demo.Models;

namespace demo.Data
{
    public class ProductDemoContext : DbContext
    {
        public ProductDemoContext(DbContextOptions<ProductDemoContext> options)
            : base(options)
        {
        }

        public DbSet<demo.Models.Product> Product { get; set; } = default!;
    }
}