using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using MongoDB.Driver;

namespace BusinessLogicLayer.Services;

public class OrderService(
    IOrderRepository orderRepository, 
    IMapper mapper, 
    IValidator<OrderAddRequest> orderAddRequestValidator,
    IValidator<OrderItemAddRequest> orderItemAddRequestValidator,
    IValidator<OrderUpdateRequest> orderUpdateRequestValidator,
    IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator
    ) : IOrderService
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
    
    public async Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest)
    {
        if (orderAddRequest == null)
            throw new ArgumentNullException(nameof(orderAddRequest));
        
        var validationResult = await orderAddRequestValidator.ValidateAsync(orderAddRequest);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException(errors);
        }
        
        var orderItemsValidation = orderAddRequest.OrderItems.Select(orderItemAddRequestValidator.Validate);
        if (orderItemsValidation.Any(x => !x.IsValid))
        {
            var errors = string.Join(", ", orderItemsValidation.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            throw new ArgumentException(errors);
        }
        
        //Check UserId in Users microservice here
        
        var orderEntity = mapper.Map<Order>(orderAddRequest);
        
        foreach(var orderItem in orderEntity.OrderItems)
            orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;

        orderEntity.TotalBill = orderEntity.OrderItems.Sum(x => x.TotalPrice);
        
        var result = await orderRepository.AddOrder(orderEntity);
        
        if (result == null)
            return null;
        
        return mapper.Map<OrderResponse>(result);
    }
    
    public async Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdateRequest)
    {
        if (orderUpdateRequest == null)
            throw new ArgumentNullException(nameof(orderUpdateRequest));

        var validationResult = await orderUpdateRequestValidator.ValidateAsync(orderUpdateRequest);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(x => x.ErrorMessage));
            throw new ArgumentException(errors);
        }

        var orderItemsValidation = orderUpdateRequest.OrderItems.Select(orderItemUpdateRequestValidator.Validate);
        if (orderItemsValidation.Any(x => !x.IsValid))
        {
            var errors = string.Join(", ", orderItemsValidation.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            throw new ArgumentException(errors);
        }

        //Check UserId in Users microservice here
        var orderEntity = mapper.Map<Order>(orderUpdateRequest);

        foreach (var orderItem in orderEntity.OrderItems)
            orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;

        orderEntity.TotalBill = orderEntity.OrderItems.Sum(x => x.TotalPrice);
        
        var result = await orderRepository.UpdateOrder(orderEntity);

        if (result == null)
            return null;
        
        return mapper.Map<OrderResponse>(result);
    }
    
    public async Task<bool> DeleteOrder(Guid orderId)
    {
        return await orderRepository.DeleteOrder(orderId);
    }
}