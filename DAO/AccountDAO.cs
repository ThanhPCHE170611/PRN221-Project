using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance = null;

        public AccountDAO() { }

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccountDAO();
                }
                return instance;
            }
        }

        public List<Account> GetAccounts()
        {
            List<Account> account;
            try
            {
                var context = new PRN221_ProjectContext();
                account = context.Accounts
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return account;
        }

        public Account GetAccountByID(int id)
        {
            Account account = null;
            try
            {
                var db = new PRN221_ProjectContext();
                account = db.Accounts.SingleOrDefault(u => u.AccountId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return account;
        }

        public bool AddAccount(Account account)
        {
            try
            {
                bool existingAccount = GetAccounts()
                    .Any(a => a.AccountCode.ToLower().Equals(account.AccountCode.ToLower()) || a.Email.ToLower().Equals(account.Email.ToLower()));
                
                if (existingAccount != true)
                {
                    account.Status = 1;

                    using (var db = new PRN221_ProjectContext())
                    {
                        db.Accounts.Add(account);
                        db.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in Add Account: {ex.Message}", ex);
            }
        }

        public bool UpdateAccount(Account account)
        {
            try
            {
                using (var db = new PRN221_ProjectContext())
                {
                    var existing = db.Accounts.SingleOrDefault(x => x.AccountId == account.AccountId);
                    if (existing != null)
                    {
                        existing.AccountCode = account.AccountCode;
                        existing.Name = account.Name;
                        existing.Email = account.Email;
                        existing.Phone = account.Phone;
                        existing.Password = account.Password;
                        existing.Status = account.Status;

                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Account not found for updating.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAccount: {ex.Message}", ex);
                return false;
            }
        }

        public bool BanAccount(int id)
        {
            try
            {
                using (var db = new PRN221_ProjectContext())
                {
                    Account u = db.Accounts.SingleOrDefault(a => a.AccountId == id);

                    if (u != null)
                    {
                        u.Status = (u.Status == 0) ? 1 : 0;

                        db.Entry(u).State = EntityState.Modified;
                        db.SaveChanges();
                        Console.WriteLine("Account status updated successfully!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Account does not exist!");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BanAccount: {ex.Message}");
                return false;
            }
        }
    }
}
