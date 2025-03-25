using AutoMapper;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.MapperProfiels;

public class CartProfile : Profile
{
    public CartProfile()
    {
        CreateMap<CartDto, Cart>();
        CreateMap<Cart, CartDto>();
    }
}
