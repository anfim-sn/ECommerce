using ECommerce.Core.DTO;
using ECommerce.Core.Entities;
using ECommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")] //api/products
[ApiController]
public class ProductsController(IProductsService productsService) : ControllerBase
{
    [HttpGet] //GET api/products
    public async Task<IActionResult> GetProducts()
    {
        var products = await productsService.GetListAsync();
        return Ok(products);
    }
    
    [HttpGet("search/productid/{productId}")] //GET api/products/search/productid/{productId}
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await productsService.GetByIdAsync(id);
        
        if (product == null)
            return NotFound();
        
        return Ok(product);
    }
    
    [HttpGet("search/{searchString}")] //GET api/products/search/{productId}
    public async Task<IActionResult> SearchProduct(string searchString)
    {
        var products = await productsService.SearchAsync(searchString);
            
        return Ok(products);
    }
    
    [HttpPost] //POST api/products
    public async Task<IActionResult> AddProduct(ProductRequest product)
    {
        var productResponse = await productsService.AddAsync(product);
        
        if (!productResponse.IsSuccess)
            return BadRequest();
        
        return Ok(productResponse);
    }
    
    [HttpPut] //PUT api/products
    public async Task<IActionResult> UpdateProduct(ProductRequest product)
    {
        var productResponse = await productsService.UpdateAsync(product);
        if (!productResponse.IsSuccess)
            return NotFound();
        
        return Ok();
    }
    
    [HttpDelete("{productId}")] //DELETE api/products/{productId}
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await productsService.DeleteByIdAsync(id);
        
        if (!result)
            return NotFound();
        
        return Ok();
    }
}