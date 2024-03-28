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
    public class UpdateStatusAccountModel : PageModel
    {
        private readonly IAccountService accountService;
        public UpdateStatusAccountModel()
        {
            accountService = new AccountService();
        }

        [BindProperty]
      public Account Account { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var account = accountService.GetAccountByID((int)id);

                if (account == null)
                {
                    return NotFound();
                }
                else
                {
                    Account = account;
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            bool result = accountService.BanAccount((int)id);
            return RedirectToPage("./ListAccount");
        }
    }
}
