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

namespace PRN221_Project.Pages.Manager.PartnerPage
{
    public class EditModel : PageModel
    {
        private readonly IPartnerService _context;

        public EditModel(IPartnerService context)
        {
            _context = context;
        }

        [BindProperty]
        public Partner Partner { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
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

            var partner =  _context.GetPartnerByID(id);
            if (partner == null)
            {
                return NotFound();
            }
            Partner = partner;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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
            try
            {
                Partner.Status = 1;
                Partner.PartnerCode = Partner.PartnerCode.ToUpper();
                _context.UpdatePartner(Partner);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartnerExists(Partner.PartnerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }catch
            {
                ViewData["Error"] = "Partner code already exists!";
                return Page();
            }
            return RedirectToPage("./Index");
        }

        private bool PartnerExists(int id)
        {
          return (_context.GetPartnerByID(id)) != null;
        }
    }
}
