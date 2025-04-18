using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PayMent.Orders.Domain.Exception;
using PayMent.Orders.Domain.Items;
using PayMent.Orders.Domain.Models;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayMents.Orders.Application.Service;

public class AuthService : IAuthService
{
    private readonly IOptions<AppSettings> _options;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IMapper _mapper;

    public AuthService(
        IOptions<AppSettings> appSettings,
        UserManager<UserIdentity> userManager
        , IMapper mapper)
    {
        _options = appSettings;
        _mapper = mapper;
        _userManager = userManager;
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

        var userResponse = _mapper.Map<UserResponse>(user);
        userResponse.Roles = (await _userManager.GetRolesAsync(user)).ToArray();
        var token = await GenerateToken(user);
        userResponse.Token = token;

        return userResponse;
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
            Subject = GenerateClaims(userIdentity),
            Expires = expires,
            SigningCredentials = credentials
        };

        var token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    private ClaimsIdentity GenerateClaims(UserIdentity user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        return new ClaimsIdentity(claims);
    }
}
