using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayMent.Orders.Domain.Exception;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Models.Account;
using PayMents.Orders.Application.Validators;

namespace PayMent.Orders.WebApi.Controllers;

/// <summary>
/// Контроллер для управления аккаунтами пользователей
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    private readonly IAccountsService _accountsService;
    private readonly IValidator<AccountRequest> _accountRequestValidator;
    private readonly IValidator<AccountResponse> _accountResponseValidator;

    public AccountController(
        IAccountsService accountsService,
        IValidator<AccountRequest> accountRequestValidator,
        IValidator<AccountResponse> accountResponseValidator)
    {
        _accountsService = accountsService;
        _accountRequestValidator = accountRequestValidator;
        _accountResponseValidator = accountResponseValidator;
    }

    /// <summary>
    /// Получение информации о текущем аккаунте
    /// </summary>
    [HttpGet("me/{token}")]
    public async Task<IActionResult> GetMyAccount(string token)
    {
        try
        {
            var account = await _accountsService.GetAccountAsync(token);
            var validationResult = await _accountRequestValidator.ValidateAsync(account);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok(account);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Получение информации об аккаунте по ID
    /// </summary>
    [HttpGet("{accountId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAccount(string accountId)
    {
        try
        {
            var account = await _accountsService.GetAccountAsync(accountId);
            var validationResult = await _accountRequestValidator.ValidateAsync(account);
            
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            return Ok(account);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Получение списка всех аккаунтов
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllAccounts()
    {
        try
        {
            var accounts = await _accountsService.GetAllAccountsAsync();
            foreach (var account in accounts)
            {
                var validationResult = await _accountRequestValidator.ValidateAsync(account);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
            }
            return Ok(accounts);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Обновление информации о текущем аккаунте
    /// </summary>
    [HttpPut("me/{token}")]
    public async Task<IActionResult> UpdateMyAccount(AccountResponse updateAccount, string token)
    {
        try
        {
            var validationResult = await _accountResponseValidator.ValidateAsync(updateAccount);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var account = await _accountsService.UpdateAccountAsync(updateAccount, token);
            return Ok(account);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Обновление информации об аккаунте по ID
    /// </summary>
    [HttpPut("{accountId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAccount(string accountId, AccountResponse updateAccount)
    {
        try
        {
            var validationResult = await _accountResponseValidator.ValidateAsync(updateAccount);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var account = await _accountsService.UpdateAccountAsync(updateAccount, accountId);
            return Ok(account);
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Удаление аккаунта по ID
    /// </summary>
    [HttpDelete("{accountId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAccount(string accountId)
    {
        try
        {
            await _accountsService.DeleteAccountAsync(accountId);
            return NoContent();
        }
        catch (EntityNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
