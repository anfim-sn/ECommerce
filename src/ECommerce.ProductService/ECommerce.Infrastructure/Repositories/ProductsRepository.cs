using ECommerce.Core.Entities;
using ECommerce.Core.RepositoryContracts;
using ECommerce.Infrastructure.dbcontext;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories;

internal class ProductsRepository(ProductDbContext DbContext) : IProductRepository
{
    public async Task<List<Product>> GetListAsync()
    {
        return await DbContext.Products.ToListAsync();
    }
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await DbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);
    }
    public async Task<List<Product>> SearchAsync(string searchString)
    {
        return await DbContext.Products.Where(x => x.ProductName.Contains(searchString)).ToListAsync();
    }
    public async Task<int> AddAsync(Product product)
    {
        await DbContext.Products.AddAsync(product);
        return await DbContext.SaveChangesAsync();
    }
    public async Task<int> UpdateAsync(Product product)
    {
        DbContext.Products.Update(product);
        return await DbContext.SaveChangesAsync();
    }
    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var product = await DbContext.Products.FirstOrDefaultAsync(x => x.ProductId == id);
        if (product == null)
            return false;
        
        DbContext.Products.Remove(product);
        var affectedRow = await DbContext.SaveChangesAsync();
        
        return affectedRow != 0;
    }
}