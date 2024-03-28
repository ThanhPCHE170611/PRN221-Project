using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using Service;
using Microsoft.Identity.Client.Extensions.Msal;

namespace PRN221_Project.Pages.Manager.CategoryPage
{
    public class CreateCategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public CreateCategoryModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

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

            ViewData["CategoryId"] = new SelectList(_categoryService.GetCategories(), "CategoryId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {


            bool cate = _categoryService.AddCategory(Category);
            if (!cate)
            {
                ViewData["Notification"] = "Create not successfully!";
                return Page();
            }

            return RedirectToPage("./ListCategory");
        }
    }
}