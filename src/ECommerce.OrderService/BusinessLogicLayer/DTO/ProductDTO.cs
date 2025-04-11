namespace BusinessLogicLayer.DTO;

public record ProductDTO(
    Guid ProductId,
    string ProductName,
    string? Category,
    decimal? UnitPrice,
    int? QuantityInStock
)
{
    public ProductDTO() : this(default, string.Empty, default, default, default) {}
};