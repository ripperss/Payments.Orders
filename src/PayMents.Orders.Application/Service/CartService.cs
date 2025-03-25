using AutoMapper;
using PayMent.Orders.Domain.Data;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Service;

public class CartService : ICartService
{
    private readonly OrdersDbContext _context;
    private readonly IMapper _mapper;    

    public CartService(OrdersDbContext context
        , IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CartDto> Create(CartDto cart)
    {
        var cartEntity = _mapper.Map<Cart>(cart);

        await _context.AddAsync(cartEntity);
        await _context.SaveChangesAsync(); 

        var cartEntityDto = _mapper.Map<CartDto>(cart);
        return cartEntityDto;
    }
}
