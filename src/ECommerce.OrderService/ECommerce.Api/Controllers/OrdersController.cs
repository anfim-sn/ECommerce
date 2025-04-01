using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ECommerce.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    //GET: api/Orders
    [HttpGet()]
    public async Task<IEnumerable<OrderResponse?>> GetOrders()
    {
        return await orderService.GetOrders();
    }

    //GET: api/Orders/search/orderId/{orderId}
    [HttpGet("search/orderId/{orderId}")]
    public async Task<OrderResponse?> GetByOrderId(Guid orderId)
    {
        var filter = new FilterDefinitionBuilder<Order>().Eq(o => o.OrderID, orderId);
        return await orderService.GetOrderByCondition(filter);
    }

    //GET: api/Orders/search/productId/{productId}
    [HttpGet("search/productId/{productId}")]
    public async Task<IEnumerable<OrderResponse?>> GetByProductId(Guid productId)
    {
        var filter = Builders<Order>.Filter.ElemMatch(o => o.OrderItems, 
                Builders<OrderItem>.Filter.Eq(p => p.ProductID, productId));
        
        return await orderService.GetOrdersByCondition(filter);
    }

    //GET: api/Orders/search/orderDate/{orderDate}
    [HttpGet("search/orderDate/{orderDate}")]
    public async Task<IEnumerable<OrderResponse?>> GetByOrderDate(DateTime orderDate)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.OrderDate, orderDate);

        return await orderService.GetOrdersByCondition(filter);
    }

    //GET: api/Orders/search/userId/{userId}
    [HttpGet("search/userId/{userId}")]
    public async Task<IEnumerable<OrderResponse?>> GetByUserId(Guid userId)
    {
        var filter = Builders<Order>.Filter.Eq(o => o.UserID, userId);

        return await orderService.GetOrdersByCondition(filter);
    }
    
    //POST: api/Orders
    [HttpPost]
    public async Task<IActionResult> AddOrder(OrderAddRequest? orderRequest)
    {
        if (orderRequest == null)
            return BadRequest();
        
        var orderResponse = await orderService.AddOrder(orderRequest);

        if (orderResponse == null)
            return Problem("Error while adding order");

        return Created($"api/Orders/search/orderId/{orderResponse.OrderId}", orderResponse);
    }

    //PUT: api/Orders/{orderId}
    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrder(Guid orderId, OrderUpdateRequest? orderRequest)
    {
        if (orderRequest == null)
            return BadRequest();
        
        if (orderId != orderRequest.OrderId)
            return BadRequest("Order Id mismatch");
        
        var orderResponse = await orderService.UpdateOrder(orderRequest);

        if (orderResponse == null)
            return Problem("Error while updating order");

        return Ok(orderResponse);
    }
    
    //DELETE: api/Orders/{orderId}
    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(Guid? orderId)
    {
        if (orderId == Guid.Empty)
            return BadRequest();

        var isDeleted = await orderService.DeleteOrder(orderId.Value);
        
        if (!isDeleted)
            return Problem("Error while deleting order");
        
        return Ok(isDeleted);
    }
}