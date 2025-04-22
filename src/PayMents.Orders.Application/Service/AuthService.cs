using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PayMent.Orders.Domain.Exception;
using PayMent.Orders.Domain.Items;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Models.Auth;
using PayMents.Orders.Application.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PayMents.Orders.Application.Service;

public class AuthService : IAuthService
{
    private readonly IOptions<AppSettings> _options;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public AuthService(
        IOptions<AppSettings> appSettings,
        UserManager<UserIdentity> userManager
        , IMapper mapper
        , IEmailService emailService)
    {
        _options = appSettings;
        _mapper = mapper;
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<UserResponse> Register(UserRegisterDto userRegisterModel)
    {
        var existingUser = await _userManager.FindByEmailAsync(userRegisterModel.Email);
        if (existingUser != null)
        {
            throw new DublicateEntityException($"Email {userRegisterModel.Email} already exists");
        }

        var user = _mapper.Map<UserIdentity>(userRegisterModel);

        var createUserResult = await _userManager.CreateAsync(user, userRegisterModel.Password);

        if (!createUserResult.Succeeded)
        {
            throw new Exception($"User creation failed: {string.Join(", ", createUserResult.Errors)}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, Role.User);
        if (!roleResult.Succeeded)
        {
            throw new Exception($"Role assignment failed: {string.Join(", ", roleResult.Errors)}");
        }

        var userResponse = _mapper.Map<UserResponse>(user);
        userResponse.Roles = [Role.User];

        BackgroundJob.Enqueue(() => GeneratingConfirmationToken(user));

        return userResponse;
    }

    public async Task<UserResponse> Login(UserLoginDto userLoginModel)
    {
        var user = await _userManager.FindByEmailAsync(userLoginModel.Email)
            ?? throw new EntityNotFoundException("ПОльзователь с данный email  не найден");

        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, userLoginModel.Password);
        if (!checkPasswordResult)
        {
            throw new UnauthorizedAccessException("Неверный пароль");
        }

        if(!user.EmailConfirmed)
        {
            throw new InvalidOperationException("Не подвержден почта");
        }

        var userResponse = _mapper.Map<UserResponse>(user);
        userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
        var token = await GenerateToken(user);
        userResponse.Token = token;

        return userResponse;
    }

    public async Task ResendEmailConfirmationAsync(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail) 
            ?? throw new EntityNotFoundException("Не найдена сущность");
        
        if (user.EmailConfirmed)
        {
            throw new InvalidOperationException("Данная почта уже подверждена"); 
        }

        BackgroundJob.Enqueue(() => GeneratingConfirmationToken(user));
    }

    public async Task<IdentityResult> ConfirmEmailAsync(string UserId, string Token)
    {
        var user = await _userManager.FindByIdAsync(UserId)
            ?? throw new EntityNotFoundException("ПОльзователь с данный email  не найден");

        var result = await _userManager.ConfirmEmailAsync(user, Token);

        return result;
    }


    private async Task<string> GenerateToken(UserIdentity userIdentity)
    {
        var handler = new JwtSecurityTokenHandler();

        var key = Encoding
            .ASCII.GetBytes(_options.Value.TokenPrivateKey!);
        var expires = DateTime
            .UtcNow.AddMinutes(_options.Value.Expires);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = await GenerateClaims(userIdentity),
            Expires = expires,
            SigningCredentials = credentials
        };

        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    private async Task<ClaimsIdentity> GenerateClaims(UserIdentity user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };
        
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return new ClaimsIdentity(claims);
    }

    public async Task GeneratingConfirmationToken(UserIdentity user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var confirmationLink = $"http://localhost:5200/accounts?userId={user.Id}&token={WebUtility.UrlEncode(token)}";

        await _emailService.SendEmailAsync(user.Email, confirmationLink);
    }
}
