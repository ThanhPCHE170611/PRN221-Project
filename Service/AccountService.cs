using BusinessObject.Models;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;

        public AccountService()
        {
            _accountRepository = new AccountRepository();
        }

        bool IAccountService.AddAccount(Account account) => _accountRepository.AddAccount(account);

        bool IAccountService.BanAccount(int id) => _accountRepository.BanAccount(id);

        Account IAccountService.GetAccountByID(int id) => _accountRepository.GetAccountByID(id);

        List<Account> IAccountService.GetAccounts() => _accountRepository.GetAccounts();

        bool IAccountService.UpdateAccount(Account account) => _accountRepository.UpdateAccount(account);
    }
}
