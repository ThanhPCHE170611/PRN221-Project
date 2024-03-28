using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Admin.ManageAccount
{
    public class ListAccountModel : PageModel
    {
        private readonly IAccountService accountService;
        private const int PageSize = 5;

        public ListAccountModel()
        {
            accountService = new AccountService();
        }

        public IList<Account> Account { get;set; } = default!;
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        [BindProperty] public string? SearchBy { get; set; }
        [BindProperty] public string? Keyword { get; set; }

        public IActionResult OnGet(int? pageIndex)
        {
            if (HttpContext.Session.GetString("account") is null)
            {
                return RedirectToPage("/Login");
            }

            var role = HttpContext.Session.GetString("account");

            if (role != "admin")
            {
                return RedirectToPage("/Login");
            }
            var accountList = accountService.GetAccounts().Where(a => a.Role != 0);
            PageIndex = pageIndex ?? 1;

            // Paginate the list
            var count = accountList.Count();
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            var items = accountList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

            Account = items;
            return Page();
        }

        public async Task OnPost(int? pageIndex)
        {
            if (Keyword == null)
            {
                OnGet(pageIndex);
            }
            else
            {
                if (SearchBy.Equals("AccountCode"))
                {
                    Account = accountService.GetAccounts().Where(a => a.AccountCode.ToUpper().Contains(Keyword.Trim().ToUpper())).ToList();
                }
                else if (SearchBy.Equals("Email"))
                {
                    Account = accountService.GetAccounts().Where(a => a.Email.ToLower().Contains(Keyword.Trim().ToLower())).ToList();
                }
                PageIndex = 1;
                TotalPages = 1;
            }
        }
    }
}
