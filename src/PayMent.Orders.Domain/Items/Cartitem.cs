using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMent.Orders.Domain.Models;

public class Cartitem : BaseItem
{
    public string Name { get; set; }
    public long Quentity { get; set; }
    public decimal Price { get; set; }

    public Cart  Cart { get; set; } 
    public long Cartid { get; set; }
}
