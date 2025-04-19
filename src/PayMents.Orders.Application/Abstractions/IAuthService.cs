using Microsoft.AspNetCore.Identity;
using PayMents.Orders.Application.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Abstractions;

public interface IAuthService
{
    Task<UserResponse> Register(UserRegisterDto userRegisterModel);
    Task<UserResponse> Login(UserLoginDto userLoginModel);
    Task<IdentityResult> ConfirmEmailAsync(string UserId, string Token);
}