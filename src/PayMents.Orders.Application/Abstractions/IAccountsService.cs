using PayMents.Orders.Application.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayMents.Orders.Application.Abstractions
{
    public interface IAccountsService
    {
        Task<AccountRequest> GetAccountAsync(string accountId);
        Task<List<AccountRequest>> GetAllAccountsAsync();
        Task<AccountRequest> UpdateAccountAsync(AccountResponse updateAccount, string accountId);
        Task DeleteAccountAsync(string accountId);
        string GetUserId(string token);
    }
}
