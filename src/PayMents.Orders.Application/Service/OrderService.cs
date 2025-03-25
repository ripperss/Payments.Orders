using AutoMapper;
using PayMent.Orders.Domain.Data;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Service;

public class OrderService : IOrderService
{
    private readonly OrdersDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICartService _cartService;

    public OrderService(OrdersDbContext context
        , IMapper mapper
        , ICartService cartService)
    {
        _context = context;
        _mapper = mapper;
        _cartService = cartService;
    }
    
    public async Task<OrderDto> Create(CreateOrderDto createOrderDto)
    {
        var cart = await _cartService.Create(createOrderDto.Cart);
        createOrderDto.CartId = cart.Id;

        var entity = _mapper.Map<Order>(createOrderDto);

        await _context.Orders.AddAsync(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<OrderDto>(entity);
    }

    public Task<List<OrderDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<OrderDto> GetById(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderDto>> GetByUser(int customerId)
    {
        throw new NotImplementedException();
    }

    public Task Reject(int orderId)
    {
        throw new NotImplementedException();
    }
}
