using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMent.Orders.Domain.Models;

public class BaseItem
{
    public long Id { get; set; }
    public DateTime  CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
