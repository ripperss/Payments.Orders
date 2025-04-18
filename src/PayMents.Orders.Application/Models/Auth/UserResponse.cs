using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Models.Auth;

public  class UserResponse
{
    public long Id { get; set; }
    public string[] Roles { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Phone {  get; set; }
    public string Token { get; set; }
}
