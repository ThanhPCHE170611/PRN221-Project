using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Manager
{
    public class StockOutListModel : PageModel
    {
        private readonly IStockOutService _stockOutService;

        public StockOutListModel(IStockOutService stockOutService)
        {
            _stockOutService = stockOutService;
        }

        public IList<StockOut> StockOut { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int curentPage { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int count { get; set; }
        public int totalPages => (int)Math.Ceiling(Decimal.Divide(count, pageSize));

        public IActionResult OnGet()
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

            count = _stockOutService.GetStockOuts().Count();
            StockOut = _stockOutService.GetStockOuts()
                .Skip((curentPage - 1) * pageSize).Take(pageSize)
                .ToList();

            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                count = _stockOutService.GetStockOuts()
                    .Where(P => P.Partner.Name.ToLower().Contains(SearchText.ToLower()) || P.Account.Name.ToLower().Contains(SearchText.ToLower()))
                    .Count();

                StockOut = _stockOutService.GetStockOuts()
                    .Where(P => P.Partner.Name.ToLower().Contains(SearchText.ToLower()) || P.Account.Name.ToLower().Contains(SearchText.ToLower()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            else
            {
                count = _stockOutService.GetStockOuts().Count();
                StockOut = _stockOutService.GetStockOuts()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            return Page();
        }
    }
}
