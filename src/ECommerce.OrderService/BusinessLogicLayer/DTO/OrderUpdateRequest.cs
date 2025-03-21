namespace BusinessLogicLayer.DTO;

public record OrderUpdateRequest(Guid OrderId, Guid UserId, DateTime OrderDate, List<OrderItemUpdateRequest> OrderItems)
{
    public OrderUpdateRequest():this(default, default, default, default) { }
};

public record OrderItemUpdateRequest(Guid ProductId, decimal UnitPrice, int Quantity)
{
    public OrderItemUpdateRequest():this(default, default, default) { }
};