using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMent.Orders.Domain.Extension;

public static class ErrorExtensions
{
    public static string ToText(this Exception exception)
    {
        return $"{exception.Message} {exception.StackTrace} {exception.InnerException?.Message} {exception.InnerException?.StackTrace}";
    }
}
