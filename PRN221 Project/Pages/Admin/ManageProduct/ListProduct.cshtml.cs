using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace PRN221_Project.Pages.Admin.ManageProduct
{
    public class ListProductModel : PageModel
    {
        private readonly IProductService productService;
        private readonly ICategoryService categoryService;
        public ListProductModel()
        {
            productService = new ProductService();
            categoryService = new CategoryService();
        }

        public IList<Product> Product { get;set; } = default!;
        public IList<Category> Categories { get; set; } = default!;


        [BindProperty(SupportsGet = true)]
        public int curentPage { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int count { get; set; }
        public int totalPages => (int)Math.Ceiling(Decimal.Divide(count, pageSize));

        [BindProperty(SupportsGet = true)]
        public int cateid {  get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        public IActionResult OnGet(int? pageIndex, int?cateid)
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

            Categories = categoryService.GetCategories().ToList();
            
            if(SearchText != null)
            {
                count = productService.GetProducts()
                        .Where(a => a.ProductCode.ToUpper().Contains(SearchText.Trim().ToUpper()))
                        .Count();
                Product = productService
                    .GetProducts().Where(a => a.ProductCode.ToUpper().Contains(SearchText.Trim().ToUpper()))
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            } else
            {
                count = productService.GetProducts().Count();
                Product = productService.GetProducts()
                        .Skip((curentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            if(cateid != null)
            {
                count = productService.GetProducts()
                    .Where(x => x.CategoryId == cateid).Count();
                Product = productService.GetProducts()
                    .Where(x => x.CategoryId == cateid)
                    .Skip((curentPage - 1) * pageSize).Take(pageSize)
                    .ToList();
            } else
            {
                count = productService.GetProducts().Count();
                Product = productService.GetProducts()
                        .Skip((curentPage - 1) * pageSize).Take(pageSize)
                        .ToList();
            }
            return Page();
        }
    }
}
