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
    public class ListStorageModel : PageModel
    {
        private readonly IStorageService _storageService;

        public ListStorageModel(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public IList<StorageArea> StorageArea { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int curentPage { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int count { get; set; }
        public int totalPages => (int)Math.Ceiling(decimal.Divide(count, pageSize));


        public IActionResult OnGetAsync()
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

            if (SearchText != null)
            {
                count = _storageService.GetStorageAreas()
                    .Where(P => P.AreaName.ToLower().Contains(SearchText.ToLower()) || P.AreaCode.ToLower().Contains(SearchText.ToLower()))
                    .Count();

                StorageArea = _storageService.GetStorageAreas()
                    .Where(P => P.AreaName.ToLower().Contains(SearchText.ToLower()) || P.AreaCode.ToLower().Contains(SearchText.ToLower()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            else
            {
                count = _storageService.GetStorageAreas().Count();
                StorageArea = _storageService.GetStorageAreas()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            return Page();
        }
    }
}
