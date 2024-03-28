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

namespace PRN221_Project.Pages.Admin.ManageLot
{
    public class LotEditModel : PageModel
    {
        private readonly ILotService lotService;
        private readonly IAccountService accountService;
        private readonly IPartnerService partnerService;

        public LotEditModel()
        {
            lotService = new LotService();
            accountService = new AccountService();
            partnerService = new PartnerService();
        }
        [BindProperty]
        public Lot Lot { get; set; } = default!;
        [BindProperty]
        public List<LotDetail> LotDetail { get; set; } = new List<LotDetail>();

        public async Task<IActionResult> OnGetAsync(int? id)
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
            if (id == null)
            {
                return NotFound();
            }

            var lot = lotService.GetLotById((int)id);
            List<LotDetail> lotdetails = lotService.GetListLotDetailById((int)id);
            if (lot == null)
            {
                return NotFound();
            }
            Lot = lot;
            LotDetail = lotdetails;
            ViewData["AccountId"] = new SelectList(accountService.GetAccounts(), "AccountId", "Email");
            ViewData["PartnerId"] = new SelectList(partnerService.GetPartners(), "PartnerId", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                lotService.UpdateLot(Lot);
                for (int i = 0; i < LotDetail.Count; i++)
                {
                    LotDetail[i].LotDetailId = Convert.ToInt32(Request.Form[$"LotDetail[{i}].LotDetailId"]);
                    LotDetail[i].Quantity = Convert.ToInt32(Request.Form[$"LotDetail[{i}].Quantity"]);
                    LotDetail[i].PartnerId = Lot.PartnerId;
                    lotService.UpdateLotDetail(LotDetail[i]);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LotExists(Lot.LotId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToPage("./ListLot");
        }

        private bool LotExists(int id)
        {
            return lotService.GetLotByAccountId((int)id) != null;
        }
    }
}
