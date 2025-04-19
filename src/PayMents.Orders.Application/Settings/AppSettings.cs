using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Settings;

public class AppSettings
{
    public string? TokenPrivateKey { get; set; }
    public int Expires { get; set; }
}
