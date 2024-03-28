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
    public class CategoriListModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CategoriListModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IList<Category> Category { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int curentPage { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int count { get; set; }
        public int totalPages => (int)Math.Ceiling(Decimal.Divide(count, pageSize));

        public IActionResult OnGetAsync()
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

            if (SearchText != null)
            {
                count = _categoryService.GetCategories()
                    .Where(P => P.Name.ToLower().Contains(SearchText.ToLower()) || P.CategoryCode.ToLower().Contains(SearchText.ToLower()))
                    .Count();

                Category = _categoryService.GetCategories()
                    .Where(P => P.Name.ToLower().Contains(SearchText.ToLower()) || P.CategoryCode.ToLower().Contains(SearchText.ToLower()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }
            else
            {
                count = _categoryService.GetCategories().Count();
                Category = _categoryService.GetCategories()
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            }

            return Page();
        }
    }
}
