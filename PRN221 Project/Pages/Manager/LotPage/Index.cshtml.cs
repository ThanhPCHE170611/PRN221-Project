using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Manager.LotPage
{
    public class IndexModel : PageModel
    {
        private readonly ILotService _context;

        public IndexModel(ILotService context)
        {
            _context = context;
        }
        public IList<Lot> Lot { get; set; } = default!;
        public string SearchText { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public IActionResult OnGet(string searchText, int currentPage = 1)
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

            SearchText = searchText;
            var lots = _context.GetAllLots();

            if (!string.IsNullOrEmpty(searchText))
            {
                lots = lots.Where(l =>
                    l.LotCode.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    l.Partner.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            TotalRecords = lots.Count();

            Lot = lots.Skip((currentPage - 1) * PageSize)
                       .Take(PageSize)
                       .ToList();

            TotalPages = (int)Math.Ceiling((double)TotalRecords / PageSize);
            CurrentPage = currentPage;
            return Page();
        }
    }
}
