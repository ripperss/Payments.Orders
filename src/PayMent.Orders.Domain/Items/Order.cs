using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMent.Orders.Domain.Models;

public class Order : BaseItem
{
    public string Name { get; set; }
    public string OrderName { get; set; }

    public Customer Customer { get; set; }
    public int CustomerId { get; set; }

    public Cart  Cart { get; set; }
    public int CartId { get; set; }
}
