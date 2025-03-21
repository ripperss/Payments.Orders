using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMent.Orders.Domain.Models;

public class Cartitem : BaseModel
{
    public string Name { get; set; }
    public int Quentity { get; set; }
    public decimal Price { get; set; }

    public Cart  Cart { get; set; } 
    public int Cartid { get; set; }
}
