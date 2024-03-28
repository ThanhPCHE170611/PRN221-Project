using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;
using System.Drawing.Printing;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace PRN221_Project.Pages.Admin.ManagePartner
{
    public class ListPartnerModel : PageModel
    {
        private readonly IPartnerService partnerService;
        private const int PageSize = 5;
        public ListPartnerModel()
        {
            partnerService = new PartnerService();
        }

        public IList<Partner> Partner { get;set; } = default!;

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
            if(SearchText != null)
            {
               var count = partnerService.GetPartners()
                    .Where(a => a.PartnerCode.ToUpper().Equals(SearchText.ToUpper().Trim()) || a.Name.ToLower().Contains(SearchText.ToLower().Trim()))
                    .Count();
                Partner = partnerService.GetPartners()
                    .Where(a => a.PartnerCode.ToUpper().Equals(SearchText.ToUpper().Trim()) || a.Name.ToLower().Contains(SearchText.ToLower().Trim()))
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .ToList();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            } else
            {
                var count = partnerService.GetPartners().Count();
                Partner = partnerService.GetPartners()
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .ToList();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            }
            return Page();
        }
    }
}
