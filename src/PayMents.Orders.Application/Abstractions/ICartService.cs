using PayMents.Orders.Application.Models.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Abstractions;

public interface ICartService
{
    Task<CartDto> Create(CartDto cart);
}
