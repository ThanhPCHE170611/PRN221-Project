using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAccountRepository
    {
        List<Account> GetAccounts();
        Account GetAccountByID(int id);
        bool AddAccount(Account account);
        bool UpdateAccount(Account account);
        bool BanAccount(int id);
    }
}
