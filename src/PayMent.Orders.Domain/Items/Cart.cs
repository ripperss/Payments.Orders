using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMent.Orders.Domain.Models;

public class Cart : BaseItem
{
    public List<Cartitem> CartItems {  get; set; }
}
