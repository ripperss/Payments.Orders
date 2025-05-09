﻿using PayMent.Orders.Domain.Items;
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
    public int OrderNumber { get; set; }

    public Customer Customer { get; set; }
    public long CustomerId { get; set; }

    public Cart  Cart { get; set; }
    public long CartId { get; set; }

    public MerchantEntity MerchantEntity { get; set; }
    public long MerchantId { get; set; }
}
