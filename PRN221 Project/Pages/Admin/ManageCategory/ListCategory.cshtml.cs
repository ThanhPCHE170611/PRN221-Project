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
using System.Drawing.Printing;

namespace PRN221_Project.Pages.Admin.ManageCategory
{
    public class ListCategoryModel : PageModel
    {
        private readonly ICategoryService categoryService;
        private const int PageSize = 5;
        public ListCategoryModel()
        {
            categoryService = new CategoryService();
        }

        public IList<Category> Category { get;set; } = default!;


        public int PageIndex { get; set; }
        public int TotalPages { get; set; }


        [BindProperty(SupportsGet = true)]
        public string SearchText { get; set; }

        public IActionResult OnGet(int? pageIndex)
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
            var categoryList = categoryService.GetCategories();
            PageIndex = pageIndex ?? 1;
                var count = categoryList.Count();
                TotalPages = (int)Math.Ceiling(count / (double)PageSize);
                var items = categoryList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                Category = items;
            return Page();
        }
        public async Task OnPost(int? pageIndex)
        {
            if (SearchText == null)
            {
                OnGet(pageIndex);
            }
            else
            {
                Category = categoryService.GetCategories()
                  .Where(a => a.CategoryCode.ToUpper().Contains(SearchText.Trim().ToUpper()) || a.Name.ToLower().Contains(SearchText.Trim().ToLower()))
                  .Skip((PageIndex - 1) * PageSize).Take(PageSize)
                  .ToList();
                PageIndex = 1;
                TotalPages = 1;
            }
        }
    }
}
