using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Manager.StoragePage
{
    public class BanStorageModel : PageModel
    {
        private readonly IStorageService _storageService;

        public BanStorageModel(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [BindProperty]
      public StorageArea StorageArea { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                StorageArea = _storageService.GetStorageAreaByID((int)id);

                if (StorageArea == null)
                {
                    return NotFound();
                }
                else
                {
                    StorageArea = StorageArea;
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
            try
            {
                bool result = _storageService.BanStorageAreaStatus((int)id);
            } catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                OnGetAsync(id);
                return Page();
            }
            return RedirectToPage("./ListStorage");
        }
    }
}
