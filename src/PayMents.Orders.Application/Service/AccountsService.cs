using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PayMent.Orders.Domain.Exception;
using PayMent.Orders.Domain.Items;
using PayMents.Orders.Application.Abstractions;
using PayMents.Orders.Application.Models.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace PayMents.Orders.Application.Service;

public class AccountsService : IAccountsService
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IMapper _mapper;
    private readonly IAuthService _authService;

    public AccountsService(
          UserManager<UserIdentity> userManager
        , IMapper mapper,
          IAuthService authService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _authService = authService;
    }

    public async Task DeleteAccountAsync(string accountId) { 
        var account = await _userManager.FindByIdAsync(accountId)
            ?? throw new EntityNotFoundException("пользователь с даным Id не найден");

        await _userManager.DeleteAsync(account);
    }

    public async Task<AccountRequest> GetAccountAsync(string accountId)
    {
        var account = await _userManager.FindByIdAsync(accountId)
            ?? throw new EntityNotFoundException("пользователь с даным Id не найден");

        return _mapper.Map<AccountRequest>(account);
    }

    public async Task<List<AccountRequest>> GetAllAccountsAsync()
    {
        var accounts = await _userManager.Users.ToListAsync();
        return _mapper.Map<List<AccountRequest>>(accounts);
    }

    public string GetUserId(string token)
    {
        var tokenHnadler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHnadler.ReadJwtToken(token);

        string id = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).ToString();

        return id;
    }

    public async Task<AccountRequest> UpdateAccountAsync(AccountResponse updateAccount, string accountId)
    {
        var account = await _userManager.FindByIdAsync(accountId)
            ?? throw new EntityNotFoundException("пользователь с даным Id не найден");

        _mapper.Map(updateAccount, account);
        await _userManager.UpdateAsync(account);

        return _mapper.Map<AccountRequest>(account);
    }   
}
