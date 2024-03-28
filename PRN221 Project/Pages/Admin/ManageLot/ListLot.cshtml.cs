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

namespace PRN221_Project.Pages.Admin.ManageLot
{
    public class ListLotModel : PageModel
    {
        private readonly ILotService lotService;
        private const int PageSize = 5;

        public ListLotModel()
        {
            lotService = new LotService();
        }

        public IList<Lot> Lot { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }


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

            PageIndex = pageIndex ?? 1;
            if (SearchText != null)
            {
                var count = lotService.GetAllLots().Count();
                Lot = lotService.GetAllLots()
                    .Where(a => a.LotCode.ToUpper().Contains(SearchText.Trim().ToUpper()))
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .ToList();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            } else
            {
                var count = lotService.GetAllLots().Count();
                Lot = lotService.GetAllLots()
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .ToList();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            }

            return Page();
        }
    }
}
