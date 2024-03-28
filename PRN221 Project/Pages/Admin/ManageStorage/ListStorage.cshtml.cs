using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace PRN221_Project.Pages.Admin.ManageStorage
{
    public class ListStorageModel : PageModel
    {
        private readonly IStorageService storageService;
        private const int PageSize = 5;
        public ListStorageModel()
        {
            storageService = new StorageService();
        }

        public IList<StorageArea> StorageArea { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int curentPage { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int count { get; set; }
        public int totalPages => (int)Math.Ceiling(Decimal.Divide(count, pageSize));

        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

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

            if(SearchText == null)
            {
                count = storageService.GetStorageAreas().Count();
                StorageArea = storageService.GetStorageAreas()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            else
            {
                count = storageService.GetStorageAreas()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .Count();
                StorageArea = storageService.GetStorageAreas()
                    .Where(a => a.AreaCode.ToUpper().Contains(SearchText.Trim().ToUpper()) || a.AreaName.ToLower().Contains(SearchText.Trim().ToLower()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }

            return Page();
        }
    }
}
