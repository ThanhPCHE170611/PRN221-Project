using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Manager.PartnerPage
{
    public class IndexModel : PageModel
    {
        private readonly IPartnerService _context;

        public IndexModel()
        {
            _context = new PartnerService();
        }

        public IList<Partner> Partner { get;set; } = default!;
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
            var partners = _context.GetPartners();

            if (!string.IsNullOrEmpty(searchText))
            {
                partners = partners.Where(p =>
                    p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    p.PartnerCode.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            TotalRecords = partners.Count();

            Partner = partners.Skip((currentPage - 1) * PageSize)
                              .Take(PageSize)
                              .ToList();

            TotalPages = (int)System.Math.Ceiling((double)TotalRecords / PageSize);
            CurrentPage = currentPage;
            return Page();
        }
    }
}
