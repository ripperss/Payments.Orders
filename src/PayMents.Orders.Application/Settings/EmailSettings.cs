using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Settings;

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int SmptpPort { get; set; }
}
