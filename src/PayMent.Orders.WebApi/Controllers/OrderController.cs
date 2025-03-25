using AutoMapper;
using Microsoft.AspNetCore.Http;
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
}
