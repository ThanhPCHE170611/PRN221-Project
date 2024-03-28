 using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Service;

namespace PRN221_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public void OnGet()
        {

        }

        [BindProperty]
        public Account Account { get; set; } = default!;

        public IActionResult OnPost(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewData["error"] = "You need to input both username and password to login";
                return Page();
            }

            var account = _accountService.GetAccounts()
                .FirstOrDefault(u => u.Email.Equals(email)
                && u.Password.Equals(password));

            if (account == null)
            {
                ViewData["error"] = "Wrong username or password";
                return Page();
            }
            if (account.Status == 0)
            {
                ViewData["error"] = "You are banned";
                return Page();
            }
            else if (account.Role == 0 && account.Status == 1)
            {
                HttpContext.Session.SetString("account", "admin");
                HttpContext.Session.SetInt32("accountId", account.AccountId);
                HttpContext.Session.SetString("accountName", account.Name);
                return RedirectToPage("/Admin/Dashboard");
            }
            else if (account.Role == 2 && account.Status == 1)
            {
                HttpContext.Session.SetString("account", "manager");
                HttpContext.Session.SetInt32("accountId", account.AccountId);
                HttpContext.Session.SetString("accountName", account.Name);
                return RedirectToPage("/Manager/ProductPage/Index");
            }
            else if (account.Role == 1 && account.Status == 1)
            {
                HttpContext.Session.SetString("account", "storekeeper");
                HttpContext.Session.SetInt32("accountId", account.AccountId);
                HttpContext.Session.SetString("accountName", account.Name);
                return RedirectToPage("/StoreKeeper/ProductList");
            }
                return Page();
        }
    }
}
