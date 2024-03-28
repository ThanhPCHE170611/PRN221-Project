using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Manager.CategoryPage
{
    public class BanCategoryModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IStockOutService _stockOutService;
        private readonly PRN221_ProjectContext _context = new PRN221_ProjectContext();

        public BanCategoryModel(ICategoryService categoryService, IProductService productService,
            IStockOutService stockOutService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _stockOutService = stockOutService;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
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
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = _categoryService.GetCategoryById((int)id);

                if (category == null)
                {
                    return NotFound();
                }
                else
                {
                    Category = category;
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                Category category = _context.Categories.FirstOrDefault(x => x.CategoryId == id);

                if (category != null)
                {
                    List<Product> productList = _context.Products.Where(x => x.CategoryId == id).ToList();
                    if(productList != null & productList.Count() > 0)
                    {
                        foreach(var product in productList)
                        {
                            product.Status = 0;
                        }
                    }
                    category.Status = 0;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ViewData["Notification"] = ex.Message;
                OnGetAsync(id);
                return Page();
            }

            return RedirectToPage("./ListCategory");
        }
    }
}
