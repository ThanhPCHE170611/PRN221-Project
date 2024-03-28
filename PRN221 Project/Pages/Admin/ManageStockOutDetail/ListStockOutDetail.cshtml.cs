using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Admin.ManageStockOutDetail
{
    public class ListStockOutDetailModel : PageModel
    {
        private readonly IStockOutService stockOutService;
        private const int PageSize = 5;

        public ListStockOutDetailModel()
        {
            stockOutService = new StockOutService();
        }

        public IList<StockOutDetail> StockOutDetail { get; set; } = default!;


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

            if (StockOutDetail == null)
            {
                var count = stockOutService.GetStockOutsDetail().Count();
                StockOutDetail = stockOutService.GetStockOutsDetail()
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .ToList();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            }
            else
            {
                var count = stockOutService.GetStockOutsDetail()
                    .Where(a => a.Product.ProductCode.Equals(SearchText))
                    .Count();
                StockOutDetail = stockOutService.GetStockOutsDetail()
                    .Where(a => a.Product.ProductCode.Equals(SearchText))
                    .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                    .ToList();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            }

            return Page();
        }
    }
}
