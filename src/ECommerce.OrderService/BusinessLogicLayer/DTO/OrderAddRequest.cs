namespace BusinessLogicLayer.DTO;

public record OrderAddRequest(Guid UserId, DateTime OrderDate, List<OrderItemAddRequest> OrderItems)
{
    public OrderAddRequest():this(default, default, default) { }
}

public record OrderItemAddRequest(Guid ProductId, decimal UnitPrice, int Quantity)
{
    public OrderItemAddRequest():this(default, default, default) { }
}