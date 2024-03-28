using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Admin.ManageAccount
{
    public class AccountCreateModel : PageModel
    {
        private readonly IAccountService accountService;

        public AccountCreateModel()
        {
            accountService = new AccountService();
        }

        public IActionResult OnGet()
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
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            bool result = accountService.AddAccount(Account);
            if(result == false) {
                ViewData["notification"] = "Account is existed";
                return Page();
            }
            return RedirectToPage("./ListAccount");
        }
    }
}
