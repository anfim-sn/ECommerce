namespace ECommerce.Core.DTO;

public record ProductResponse(
    Guid ProductId,
    string ProductName,
    string? Category,
    decimal? UnitPrice,
    int? QuantityInStock,
    bool IsSuccess
)
{
    public ProductResponse() : this(default, string.Empty, default, default, default, default) {}
};