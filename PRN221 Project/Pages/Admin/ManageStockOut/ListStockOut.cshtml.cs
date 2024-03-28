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

namespace PRN221_Project.Pages.Admin.ManageStockOut
{
    public class ListStockOutModel : PageModel
    {
        private readonly IStockOutService stockOutService;
        private const int PageSize = 5;
        public ListStockOutModel()
        {
            stockOutService = new StockOutService();
        }

        public IList<StockOut> StockOut { get;set; } = default!;


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
            var stockOutList = stockOutService.GetStockOuts();
            PageIndex = pageIndex ?? 1;
            if(SearchText == null)
            {
                var count = stockOutService.GetStockOuts().Count();
                StockOut = stockOutService.GetStockOuts()
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .ToList();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            } else
            {
                var count = stockOutService.GetStockOuts()
                    .Where(a => a.Account.AccountCode.ToUpper().Contains(SearchText.Trim().ToUpper()) || a.Partner.Name.ToLower().Contains(SearchText.Trim().ToLower()))
                    .Count();
                StockOut = stockOutService.GetStockOuts()
                    .Where(a => a.Account.AccountCode.ToUpper().Contains(SearchText.Trim().ToUpper()) || a.Partner.Name.ToLower().Contains(SearchText.Trim().ToLower()))
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .ToList();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            }
            return Page();
        }
    }
}
