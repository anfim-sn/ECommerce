namespace ECommerce.Core.DTO;

public record ProductResponse(
    Guid ProductId,
    string ProductName,
    string Category,
    int UnitPrice,
    int QuantityInStock,
    bool IsSuccess
    );