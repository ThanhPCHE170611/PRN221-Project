using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using Service;
using Microsoft.Identity.Client.Extensions.Msal;

namespace PRN221_Project.Pages.Manager.StoragePage
{
    public class CreateStorageAreaModel : PageModel
    {
        private readonly IStorageService _storageService;

        public CreateStorageAreaModel(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [BindProperty]
        public StorageArea StorageArea { get; set; } = default!;

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("account") is null)
            {
                return RedirectToPage("/Login");
            }

            var role = HttpContext.Session.GetString("account");

            if (role != "manager")
            {
                return RedirectToPage("/Login");
            }

            ViewData["AreaId"] = new SelectList(_storageService.GetStorageAreas(), "AreaId", "AreaName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            bool storage = _storageService.AddStorageArea(StorageArea);

            if (!storage)
            {
                ViewData["Notification"] = "Create not successfully!";
                return Page();
            }

            return RedirectToPage("./ListStorage");
        }
    }
}
