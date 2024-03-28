using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Manager
{
    public class StockOutEditModel : PageModel
    {
        private readonly IStockOutService stockOutService;
        private readonly IAccountService accountService;
        private readonly IPartnerService partnerService;

        public StockOutEditModel()
        {
            stockOutService = new StockOutService();
            accountService = new AccountService();
            partnerService = new PartnerService();
        }

        [BindProperty]
        public StockOut StockOut { get; set; } = default!;
        [BindProperty]
        public List<StockOutDetail> StockOutDetails { get; set; } = default!;

        public IActionResult OnGet(int? id)
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
            if (id == null)
            {
                return NotFound();
            }

            var stockout = stockOutService.GetStockOutById((int)id);
            var stockoutId = stockOutService.GetStockOutDetailById((int)id);
            if (stockout == null)
            {
                return NotFound();
            } 
            if (stockoutId == null)
            {
                stockoutId = new List<StockOutDetail>();
            }

            StockOut = stockout;
            StockOutDetails = stockoutId;
            ViewData["AccountId"] = new SelectList(accountService.GetAccounts(), "AccountId", "Email");
            ViewData["PartnerId"] = new SelectList(partnerService.GetPartners(), "PartnerId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                stockOutService.UpdateStockOuts(StockOut);
                for (int i = 0; i < StockOutDetails.Count; i++)
                {
                    StockOutDetails[i].Quantity = Convert.ToInt32(Request.Form[$"StockOutDetails[{i}].Quantity"]);
                    StockOutDetails[i].StockOutDetailId = Convert.ToInt32(Request.Form[$"StockOutDetails[{i}].StockOutDetailId"]);
                    stockOutService.UpdateStockOutsDetail(StockOutDetails[i].StockOutDetailId, StockOutDetails[i].Quantity);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockOutExists(StockOut.StockOutId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./StockOutList");
        }

        private bool StockOutExists(int id)
        {
            return stockOutService.GetStockOutById(id) != null;
        }
    }
}
