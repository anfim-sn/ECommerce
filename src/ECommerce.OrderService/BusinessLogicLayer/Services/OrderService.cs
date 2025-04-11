using AutoMapper;
using BusinessLogicLayer.DTO;
using BusinessLogicLayer.HttpClients;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContracts;
using FluentValidation;
using MongoDB.Driver;

namespace BusinessLogicLayer.Services;

public class OrderService(
    IOrderRepository ordersRepository,
    UsersMicroserviceClient usersMicroserviceClient,
    ProductsMicroserviceClient productMicroserviceClient,
    IMapper mapper, 
    IValidator<OrderAddRequest> orderAddRequestValidator,
    IValidator<OrderItemAddRequest> orderItemAddRequestValidator,
    IValidator<OrderUpdateRequest> orderUpdateRequestValidator,
    IValidator<OrderItemUpdateRequest> orderItemUpdateRequestValidator
    ) : IOrderService
{

    public async Task<List<OrderResponse?>> GetOrders()
    {
        var orders = await ordersRepository.GetOrders();
        var ordersResponse = mapper.Map<IEnumerable<OrderResponse?>>(orders).ToList();
        
        foreach (var orderResponse in ordersResponse)
        {
            if (orderResponse == null)
                continue;

            var user = await usersMicroserviceClient.GetUserByUserId(orderResponse.UserId);
            mapper.Map(user, orderResponse);
            
            foreach (var orderItem in orderResponse.OrderItems)
            {
                var product = await productMicroserviceClient.GetProductByProductId(orderItem.ProductId);
                mapper.Map(product, orderItem);
            }
        }
        
        return ordersResponse;
    }
    
    public async Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter)
    {
        var orders = await ordersRepository.GetOrdersByCondition(filter);
        var ordersResponse = mapper.Map<IEnumerable<OrderResponse?>>(orders).ToList();

        foreach (var orderResponse in ordersResponse)
        {
            if (orderResponse == null)
                continue;

            var user = await usersMicroserviceClient.GetUserByUserId(orderResponse.UserId);
            mapper.Map(user, orderResponse);
            
            foreach (var orderItem in orderResponse.OrderItems)
            {
                var product = await productMicroserviceClient.GetProductByProductId(orderItem.ProductId);
                mapper.Map(product, orderItem);
            }
        }

        return ordersResponse;
    }
    
    public async Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter)
    {
        var order = await ordersRepository.GetOrderByCondition(filter);

        if (order == null)
            return null;
        
        var orderResponse = mapper.Map<OrderResponse>(order);

        var user = await usersMicroserviceClient.GetUserByUserId(orderResponse.UserId);
        mapper.Map(user, orderResponse);
        
        foreach (var orderItem in orderResponse.OrderItems)
        {
            var product = await productMicroserviceClient.GetProductByProductId(orderItem.ProductId);
            mapper.Map(product, orderItem);
        }

        return orderResponse;
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
        
        //Check ProductID in Products microservice here
        var products = new List<ProductDTO>();
        foreach (var orderItem in orderAddRequest.OrderItems)
        {
            var product = await productMicroserviceClient.GetProductByProductId(orderItem.ProductId);
            if (product == null)
                throw new ArgumentException($"Product {orderItem.ProductId} not found");

            products.Add(product);
        }

        //Check UserID in Users microservice here
        var user = await usersMicroserviceClient.GetUserByUserId(orderAddRequest.UserId);
        if (user == null)
            throw new ArgumentException("User not found");
        
        var orderEntity = mapper.Map<Order>(orderAddRequest);
        
        foreach(var orderItem in orderEntity.OrderItems)
            orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;

        orderEntity.TotalBill = orderEntity.OrderItems.Sum(x => x.TotalPrice);
        
        var order = await ordersRepository.AddOrder(orderEntity);
        
        if (order == null)
            return null;

        var orderResponse = mapper.Map<OrderResponse>(order);

        mapper.Map(user, orderResponse);
        
        foreach (var orderItem in orderResponse.OrderItems)
        {
            var product = products.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
            mapper.Map(product, orderItem);
        }

        return orderResponse;
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

        //Check UserID in Users microservice here
        var user = await usersMicroserviceClient.GetUserByUserId(orderUpdateRequest.UserId);
        if (user == null)
            throw new ArgumentException("User not found");

        //Check ProductID in Products microservice here
        var products = new List<ProductDTO>();
        foreach (var orderItem in orderUpdateRequest.OrderItems)
        {
            var product = await productMicroserviceClient.GetProductByProductId(orderItem.ProductId);
            if (product == null)
                throw new ArgumentException($"Product {orderItem.ProductId} not found");

            products.Add(product);
        }
        
        var orderEntity = mapper.Map<Order>(orderUpdateRequest);

        foreach (var orderItem in orderEntity.OrderItems)
            orderItem.TotalPrice = orderItem.Quantity * orderItem.UnitPrice;

        orderEntity.TotalBill = orderEntity.OrderItems.Sum(x => x.TotalPrice);
        
        var order = await ordersRepository.UpdateOrder(orderEntity);

        if (order == null)
            return null;

        var orderResponse = mapper.Map<OrderResponse>(order);

        mapper.Map(user, orderResponse);
        
        foreach (var orderItem in orderResponse.OrderItems)
        {
            var product = products.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
            mapper.Map(product, orderItem);
        }

        return orderResponse;
    }
    
    public async Task<bool> DeleteOrder(Guid orderId)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.OrderID, orderId);
        
        var order = await ordersRepository.GetOrderByCondition(filter);
        
        if (order == null)
            return false;
        
        return await ordersRepository.DeleteOrder(orderId);
    }
}