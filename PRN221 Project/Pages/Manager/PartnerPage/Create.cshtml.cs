using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Manager.PartnerPage
{
    public class CreateModel : PageModel
    {
        private readonly IPartnerService _context;

        public CreateModel(IPartnerService context)
        {
            _context = context;
        }

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
            return Page();
        }

        [BindProperty]
        public Partner Partner { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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
                Partner.PartnerCode = Partner.PartnerCode?.ToUpperInvariant();
                _context.AddPartner(Partner);
            }
            catch
            {
                ViewData["Error"] = "Product code already exists!";
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
