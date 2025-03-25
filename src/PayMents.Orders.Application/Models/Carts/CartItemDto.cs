using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Models.Carts;

public class CartItemDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int Quentity { get; set; }
    public decimal Price { get; set; }
}
