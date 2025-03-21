using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using MongoDB.Driver;

namespace DataAccessLayer.Repositories;

public class OrderRepository(IMongoDatabase mongoDatabase) : IOrderRepository
{
    private static readonly string collectionName = "orders";
    private readonly IMongoCollection<Order> _orders = mongoDatabase.GetCollection<Order>(collectionName);
    
    public async Task<IEnumerable<Order>> GetOrders()
    {
        var result = await _orders.FindAsync(Builders<Order>.Filter.Empty);

        return await result.ToListAsync();
    }
    
    public async Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter)
    {
        var result = await _orders.FindAsync(filter);

        return await result.ToListAsync();

    }
    public async Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter)
    {
        var result = await _orders.FindAsync(filter);

        return await result.FirstOrDefaultAsync();
    }
    
    public async Task<Order?> AddOrder(Order order)
    {
        order.OrderId = Guid.NewGuid();
        
        await _orders.InsertOneAsync(order);
        
        return order;
    }
    
    public async Task<Order?> UpdateOrder(Order order)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.OrderId, order.OrderId);

        var existingOrder = (await _orders.FindAsync(filter)).FirstOrDefaultAsync();

        if (existingOrder is null)
            return null;
        
        await _orders.ReplaceOneAsync(o => o.OrderId == order.OrderId, order);

        return order;
    }
    
    public async Task<bool> DeleteOrder(Guid orderId)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.OrderId, orderId);

        var existingOrder = (await _orders.FindAsync(filter)).FirstOrDefaultAsync();
        
        if (existingOrder is null)
            return false;
        
        var result = await _orders.DeleteOneAsync(filter);
        
        return result.DeletedCount > 0;
    }
}