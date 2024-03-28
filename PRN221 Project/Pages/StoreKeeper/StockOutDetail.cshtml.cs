using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.StoreKeeper
{
    public class StockOutDetailModel : PageModel
    {
        private readonly IStockOutService _stockOutService;

        public StockOutDetailModel(IStockOutService stockOutService)
        {
            _stockOutService = stockOutService;
        }

        public StockOut StockOut { get; set; } = default!;  

        public List<StockOutDetail> StockOutDetails { get; set; } = new List<StockOutDetail>();

        public IActionResult OnGet(int id)
        {
            if (HttpContext.Session.GetString("account") is null)
            {
                return RedirectToPage("/Login");
            }

            var role = HttpContext.Session.GetString("account");

            if (role != "storekeeper")
            {
                return RedirectToPage("/Login");
            }

            if (id <= 0)
            {
                return NotFound();
            }

            StockOut = _stockOutService.GetStockOutById(id);
            if (StockOut == null)
            {
                return NotFound();
            }
            else
            {
                var stockOutDetails = _stockOutService.GetStockOutsDetail().Where(s => s.StockOutId == id).ToList();
                if (stockOutDetails == null)
                {
                    ViewData["Notification"] = "Empty List.";
                } 
                else
                {
                    StockOutDetails = stockOutDetails;
                }
            }
            return Page();
        }
    }
}
