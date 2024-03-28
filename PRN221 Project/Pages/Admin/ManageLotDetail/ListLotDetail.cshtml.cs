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

namespace PRN221_Project.Pages.Admin.ManageLotDetail
{
    public class ListLotDetailModel : PageModel
    {
        private readonly ILotService lotService;
        private const int PageSize = 5;
        public ListLotDetailModel()
        {
            lotService = new LotService();
        }


        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }


        public IList<LotDetail> LotDetail { get;set; } = default!;

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
                var count = lotService.GetAllLotDetail()
                    .Where(a => a.Lot.LotCode.ToUpper().Equals(SearchText.ToUpper().Trim()) || a.Product.ProductCode.ToUpper().Equals(SearchText.ToUpper().Trim()))
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .Count();
                LotDetail = lotService.GetAllLotDetail()
                    .Where(a => a.Lot.LotCode.ToUpper().Equals(SearchText.ToUpper().Trim()) || a.Product.ProductCode.ToUpper().Equals(SearchText.ToUpper().Trim()))
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .ToList();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            }
            else
            {
               var count = lotService.GetAllLotDetail().Count();
                LotDetail = lotService.GetAllLotDetail()
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .ToList();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            }
            return Page();
        }
    }
}
