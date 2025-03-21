namespace BusinessLogicLayer.DTO;

public record OrderResponse(Guid OrderId, Guid UserId, decimal TotalBill, DateTime OrderDate, List<OrderItemResponse> OrderItems)
{
    public OrderResponse():this(default, default, default, default, default) { }
}

public record OrderItemResponse(Guid ProductId, decimal UnitPrice, int Quantity, decimal TotalPrice)
{
    public OrderItemResponse() : this(default, default, default, default){}
}