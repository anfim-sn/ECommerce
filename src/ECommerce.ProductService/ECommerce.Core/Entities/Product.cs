namespace ECommerce.Core.Entities;

public class Product
{
    public Guid ProductId { get; set; }
    
    public string ProductName { get; set; }
    
    public string? Category { get; set; }
    
    public decimal? UnitPrice { get; set; }
    
    public int? QuantityInStock { get; set; }
}