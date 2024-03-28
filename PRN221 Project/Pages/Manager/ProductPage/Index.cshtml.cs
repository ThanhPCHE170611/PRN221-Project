using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BusinessObject.Models;
using Service;
using System.Drawing.Printing;

namespace PRN221_Project.Pages.Manager.ProductPage
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IStorageService _storageService;

        public IndexModel()
        {
            _productService = new ProductService();
            _storageService = new StorageService();
        }

        public IList<Product> Product { get; set; } = default!;
        public IList<StorageArea> Areas { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int areaid { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        [BindProperty(SupportsGet = true)]
        public int curentPage { get; set; } = 1;
        public int pageSize { get; set; } = 5;
        public int count { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }

        public IActionResult OnGetAsync(string searchText, int? areaid)
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
            Areas = _storageService.GetStorageAreas().ToList();
            SearchText = searchText;
            var products = _productService.GetProducts();

            if (!string.IsNullOrEmpty(searchText))
            {
                products = products.Where(p => p.ProductCode.ToUpper().Contains(searchText.Trim().ToUpper())
                ).ToList();
                TotalRecords = products.Count();

                Product = products.Skip((curentPage - 1) * pageSize)
                                  .Take(pageSize)
                .ToList();

                TotalPages = (int)System.Math.Ceiling((double)TotalRecords / pageSize);
                curentPage = curentPage;
            } else
            {
                TotalRecords = products.Count();

                Product = products.Skip((curentPage - 1) * pageSize)
                                  .Take(pageSize)
                .ToList();

                TotalPages = (int)System.Math.Ceiling((double)TotalRecords / pageSize);
                curentPage = curentPage;
            }     
            if(areaid != null  && areaid!=0)
            {
                Product = products.Where(p => p.AreaId == areaid).Skip((curentPage - 1) * pageSize)
                                  .Take(pageSize).ToList();
                TotalRecords = products.Where(p => p.AreaId == areaid).ToList().Count();
                TotalPages = (int)System.Math.Ceiling((double)TotalRecords / pageSize);
                curentPage = curentPage;
            } 
            return Page();
        }
    }
}
