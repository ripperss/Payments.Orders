using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Models.Orders;

namespace PayMent.Orders.WebApi.Controllers;

[Route("api/orders")]
[ApiController]
public class OrderController : ApiBaseController
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IOrderService orderService, ILogger<OrderController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto request)
    {
        _logger.LogInformation("method api/orders, Создан заказ");
        var result = await _orderService.Create(request);

        return Ok(result);
    }

    [HttpGet]
    [Route("{orderId:long}")]
    public async Task<IActionResult> GetByIdAsync(long orderId)
    {
        _logger.LogInformation($"method api/orders{orderId}, получен заказ");

        var result = await _orderService.GetById(orderId);

        return Ok(result);  
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        _logger.LogInformation($"method api/orders, полученЫ заказы ");
        
        var result = await _orderService.GetAll();

        _logger.LogInformation($"method api/orders, полученЫ заказы количество {result.Count}");

        return Ok(result); 
    }

    [HttpGet]
    [Route("{customerId:long}")]
    public async Task<IActionResult> GetByUserAsync(long customerId)
    {
        _logger.LogInformation($"method api/orders{customerId}, получен заказ");

        var result = await _orderService.GetByUser(customerId);

        return Ok(result);  
    }

    [HttpPost("reject")]
    public async Task<IActionResult> Reject()
    {
        return Ok();
    }
    
}


