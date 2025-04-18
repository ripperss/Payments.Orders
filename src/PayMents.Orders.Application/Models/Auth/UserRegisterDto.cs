using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Models.Auth
{
    public record UserRegisterDto(string UserName, string Email, string Phone, string Password);
}
