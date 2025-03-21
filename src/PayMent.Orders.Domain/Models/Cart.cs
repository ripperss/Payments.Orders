using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMent.Orders.Domain.Models;

public class Cart : BaseModel
{
    public List<Cartitem> CartItems {  get; set; }
}
