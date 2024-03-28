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
using Microsoft.Identity.Client.Extensions.Msal;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace PRN221_Project.Pages.Manager.ProductPage
{
    public class EditModel : PageModel
    {
        private readonly IProductService productService;
        private readonly IStorageService storageService;
        private readonly ICategoryService categoryService;

        public EditModel()
        {
            productService = new ProductService();
            storageService = new StorageService();
            categoryService = new CategoryService();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

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
            if (id == null)
            {
                return NotFound();
            }

            var product = productService.GetProductByID(id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            var StorageAreasTypeList = storageService.LoadArea();
            var StorageAreasSelectList = StorageAreasTypeList.Select(area => new SelectListItem
            {
                Value = area.AreaId.ToString(),
                Text = area.AreaName.ToString(),
            }).ToList();
            var CategoriesTypeList = categoryService.LoadCategories();
            var CategoriesSelectList = CategoriesTypeList.Select(category => new SelectListItem
            {
                Value = category.CategoryId.ToString(),
                Text = category.Name
            }).ToList();
            ViewData["AreaId"] = new SelectList(StorageAreasSelectList, "Value", "Text");
            ViewData["CategoryId"] = new SelectList(CategoriesSelectList, "Value", "Text");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
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
                Product.Status = 1;
                Product.ProductCode = Product.ProductCode.ToUpper();
                Product = productService.UpdateProduct(Product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductId))
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
                ViewData["Error"] = "Product code already exists!";
                InitializeSelectLists();
                return Page();
            }
            return RedirectToPage("./Index");
        }

        private bool ProductExists(int? id)
        {
            return productService.GetProductByID(id) != null;
        }
        private void InitializeSelectLists()
        {
            ViewData["AreaId"] = new SelectList(storageService.GetStorageAreas(), "AreaId", "AreaName");
            ViewData["CategoryId"] = new SelectList(categoryService.GetCategories(), "CategoryId", "Name");
        }
    }
}
