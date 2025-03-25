using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Models.Carts;

public class CartDto
{
    public long Id { get; set; }    
    public List<CartItemDto> CartItems { get; set; } = null!;
}
