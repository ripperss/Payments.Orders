using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PayMent.Orders.Domain.Data;
using PayMent.Orders.Domain.Exception;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Models.Orders;


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
        if (createOrderDto.Cart == null)
        {
            throw new ArgumentNullException(nameof(createOrderDto.Cart));
        }
        
        var cart = await _cartService.Create(createOrderDto.Cart);
        createOrderDto.CartId = cart.Id;

        var entity = _mapper.Map<Order>(createOrderDto);

        await _context.Orders.AddAsync(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<OrderDto>(entity);
    }

    public async Task<List<OrderDto>> GetAll()
    {
        var orders = await _context.Orders
        .Include(o => o.Cart)
        .ThenInclude(c => c.CartItems)
        .ToListAsync();
        
        return _mapper.Map<List<OrderDto>>(orders); 
    }

    public async Task<OrderDto> GetById(long orderId)
    {
        var entity = await _context.Orders
        .Include(o => o.Cart)
        .ThenInclude(c => c.CartItems)
        .FirstOrDefaultAsync(o => o.Id == orderId);

        if (entity == null)
        {
            throw new EntityNotFoundException("Order not found");
        }

        return _mapper.Map<OrderDto>(entity);
    }

    public async Task<List<OrderDto>> GetByUser(long customerId)
    {
        var orders = await _context.Orders
        .Include(o => o.Cart)
        .ThenInclude(c => c.CartItems)
        .Where(o => o.CustomerId == customerId)
        .ToListAsync();

        return _mapper.Map<List<OrderDto>>(orders);
    }

    public Task Reject(long orderId)
    {
        throw new NotImplementedException();
    }
}
