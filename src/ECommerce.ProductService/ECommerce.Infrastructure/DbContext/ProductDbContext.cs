using System.Data;
using ECommerce.Core.Entities;
using Microsoft.EntityFrameworkCore;


namespace ECommerce.Infrastructure.dbcontext;

public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}