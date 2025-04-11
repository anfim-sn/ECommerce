using BusinessLogicLayer.DTO;
using DataAccessLayer.Entities;
using MongoDB.Driver;

namespace BusinessLogicLayer.ServiceContracts;

public interface IOrderService
{
    public Task<List<OrderResponse?>> GetOrders();
    public Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter);
    public Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter);
    public Task<OrderResponse?> AddOrder(OrderAddRequest order);
    public Task<OrderResponse?> UpdateOrder(OrderUpdateRequest order);
    public Task<bool> DeleteOrder(Guid orderId);
}