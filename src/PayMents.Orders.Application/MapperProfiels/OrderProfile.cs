using AutoMapper;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.MapperProfile;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderDto, OrderDto>();
        CreateMap<OrderDto, CreateOrderDto>();

        CreateMap<CreateOrderDto, Order>();
        CreateMap<Order, CreateOrderDto>();
    }
}
