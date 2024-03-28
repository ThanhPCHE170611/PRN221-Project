using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;
using Microsoft.Identity.Client.Extensions.Msal;

namespace PRN221_Project.Pages.Manager.StoragePage
{
    public class EditStorageAreaModel : PageModel
    {
        private readonly IStorageService _storageService;

        public EditStorageAreaModel(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [BindProperty]
        public StorageArea StorageArea { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
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
            if (id == null)
            {
                return NotFound();
            }

            var storageArea = _storageService.GetStorageAreaByID(id);
            if (storageArea == null)
            {
                return NotFound();
            }
            StorageArea = storageArea;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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
            try
            {
                StorageArea.Status = 1;
                StorageArea.AreaCode = StorageArea.AreaCode.ToUpper();
                _storageService.UpdateStorageArea(StorageArea);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StorageAreaExists(StorageArea.AreaId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch
            {
                ViewData["Error"] = "Category code already exists!";
                return Page();
            }
            return RedirectToPage("./ListStorage");
        }

        private bool StorageAreaExists(int id)
        {
            return (_storageService.GetStorageAreaByID(id)) != null;
        }
    }
}
