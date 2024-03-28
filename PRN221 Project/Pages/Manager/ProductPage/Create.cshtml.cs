using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Manager.ProductPage
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _product;
        private readonly IStorageService _storage;
        private readonly ICategoryService _category;


        public CreateModel(IProductService product, IStorageService storage, ICategoryService category)
        {
            _category = category;
            _storage = storage;
            _product = product;
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

            InitializeSelectLists();
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;


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
                Product.ProductCode = Product.ProductCode.ToUpper();
                _product.AddProduct(Product);
            }
            catch
            {
                ViewData["Error"] = "Product code already exists!";
                InitializeSelectLists();
                return Page();
            }
            return RedirectToPage("./Index");
        }
        private void InitializeSelectLists()
        {
            ViewData["AreaId"] = new SelectList(_storage.GetStorageAreas(), "AreaId", "AreaName");
            ViewData["CategoryId"] = new SelectList(_category.GetCategories(), "CategoryId", "Name");
        }
    }
}
