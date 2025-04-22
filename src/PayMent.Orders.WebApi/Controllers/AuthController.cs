using Microsoft.AspNetCore.Mvc;
using PayMent.Orders.Domain.Exception;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Models.Auth;

namespace PayMent.Orders.WebApi.Controllers;


[Route("accounts")]
public class AuthController : ApiBaseController
{
    private readonly IAuthService _authService;
    private readonly IRoleInitializerService _roleInitializerService;

    public AuthController(IAuthService authService, IRoleInitializerService roleInitializerService)
    {
        _authService = authService;
        _roleInitializerService = roleInitializerService;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
    {
        try
        {
            await _roleInitializerService.CreateRoleAsync();

            var userResponse = await _authService.Register(userRegisterDto);
            return Created("", userResponse);
        }
        catch (DublicateEntityException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        try
        {
            var userResponse = await _authService.Login(userLoginDto);
            return Ok(userResponse);

        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("confirmEmail")]
    public async Task<IActionResult> ConfirmEmailAsync(string userId, string Token)
    {
        var result = await _authService.ConfirmEmailAsync(userId, Token);

        if(result.Succeeded)
        {
            return Ok(result);
        }

        return Problem(result.Errors.ToString());
    }

    [HttpGet("resendEmail")]
    public async Task<IActionResult> ResendEmailConfirmationAsync(string userEmail)
    {
        await _authService.ResendEmailConfirmationAsync(userEmail);

        return Ok();
    }
}
