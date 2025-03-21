using DataAccessLayer.Entities;
using MongoDB.Driver;

namespace DataAccessLayer.RepositoryContracts;

public interface IOrderRepository
{
    public Task<IEnumerable<Order>> GetOrders();
    public Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter);
    public Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter);
    public Task<Order?> AddOrder(Order order);
    public Task<Order?> UpdateOrder(Order order);
    public Task<bool> DeleteOrder(Guid orderId);
}