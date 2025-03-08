namespace ECommerce.Core.DTO;

public record ProductRequest(
    Guid ProductId,
    string ProductName,
    string Category,
    int UnitPrice,
    int QuantityInStock
    );