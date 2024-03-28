using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using Repositories;
using Service;

namespace PRN221_Project.Pages.StoreKeeper
{
    public class ProductCreateModel : PageModel
    {
        private readonly IProductService _product;
        private readonly IStorageService _storage;
        private readonly ICategoryService _category;


        public ProductCreateModel(IProductService product, IStorageService storage, ICategoryService category)
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

            if (role != "storekeeper")
            {
                return RedirectToPage("/Login");
            }

            ViewData["AreaId"] = new SelectList(_storage.GetStorageAreas(), "AreaId", "AreaName");
            ViewData["CategoryId"] = new SelectList(_category.GetCategories(), "CategoryId", "Name");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
          

            var product = _product.AddProduct(Product);
            if(product == null)
            {
                ViewData["Error"] = "Product code already exists!";
            }
            return RedirectToPage("./ProductList");
        }
    }
}
