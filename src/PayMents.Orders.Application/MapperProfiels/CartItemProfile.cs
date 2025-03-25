using AutoMapper;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.MapperProfiels;

public class CartItemProfile : Profile
{
    public CartItemProfile()
    {
        CreateMap<CartItemDto, Cartitem>();
        CreateMap<Cartitem,CartItemDto>();
    }
}
