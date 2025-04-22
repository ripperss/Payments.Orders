using PayMent.Orders.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMent.Orders.Domain.Items;

public class MerchantEntity : BaseItem
{
    public string Name { get; set; }
    public string Phone {  get; set; }
    public string WebSite { get; set; } 
}
