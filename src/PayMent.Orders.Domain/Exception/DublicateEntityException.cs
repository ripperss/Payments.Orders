using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMent.Orders.Domain.Exception;

public class DublicateEntityException : System.Exception
{
    public DublicateEntityException(string message) : base(message)
    {

    }
}
