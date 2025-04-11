namespace BusinessLogicLayer.DTO;

public record OrderResponse(Guid OrderId, Guid UserId, string PersonName, string Email, decimal TotalBill, DateTime OrderDate, List<OrderItemResponse>? OrderItems)
{
    public OrderResponse():this(default, default, string.Empty, string.Empty, default, default, default) { }
}

public record OrderItemResponse(Guid ProductId, string ProductName, string Category, decimal UnitPrice, int Quantity, decimal TotalPrice)
{
    public OrderItemResponse() : this(default, string.Empty, string.Empty, default, default, default){}
}