using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using MongoDB.Driver;

namespace BusinessLogicLayer.Services;

public class OrderService(IOrderRepository orderRepository, IMapper mapper) : IOrderService
{

    public async Task<IEnumerable<OrderResponse>> GetOrders()
    {
        var orders = await orderRepository.GetOrders();
        return mapper.Map<List<OrderResponse>>(orders);
    }
    
    public async Task<IEnumerable<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
    {
        var orders = await orderRepository.GetOrdersByCondition(filter);
        return mapper.Map<List<OrderResponse>>(orders);
    }
    
    public async Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
    {
        var order = await orderRepository.GetOrderByCondition(filter);
        return mapper.Map<OrderResponse>(order);
    }
    
    public async Task<OrderResponse?> AddOrder(OrderAddRequest order)
    {
        var orderEntity = mapper.Map<Order>(order);
        var result = await orderRepository.AddOrder(orderEntity);
        return mapper.Map<OrderResponse>(result);
    }
    
    public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest order)
    {
        var orderEntity = mapper.Map<Order>(order);
        var result = await orderRepository.UpdateOrder(orderEntity);
        return mapper.Map<OrderResponse>(result);
    }
    
    public async Task<bool> DeleteOrder(Guid orderId)
    {
        return await orderRepository.DeleteOrder(orderId);
    }
}