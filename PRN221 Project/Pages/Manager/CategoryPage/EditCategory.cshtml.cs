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

namespace PRN221_Project.Pages.Manager.CategoryPage
{
    public class EditCategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public EditCategoryModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public IActionResult OnGet(int id)
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

            Category category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }
            Category = category;

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
                Category.Status = 1;
                Category.CategoryCode = Category.CategoryCode.ToUpper();
                _categoryService.UpdateCategory(Category);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.CategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch
            {
                ViewData["Error"] = "Category code already exists!";
                return Page();
            }
            return RedirectToPage("./ListCategory");
        }

        private bool CategoryExists(int id)
        {
            return (_categoryService.GetCategoryById(id)) != null;
        }
    }
}
