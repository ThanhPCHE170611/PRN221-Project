using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.Manager
{
    public class StockOutCreateModel : PageModel
    {
        private readonly IStockOutService _stockoutService;
        private readonly IAccountService _accService;
        private readonly IProductService _productService;
        private readonly IPartnerService _partnerService;
        public StockOutCreateModel()
        {
            _stockoutService = new StockOutService();
            _productService = new ProductService();
            _partnerService = new PartnerService();
            _accService = new AccountService();
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("account") is null)
            {
                return RedirectToPage("/Login");
            }

            var role = HttpContext.Session.GetString("account");
            var accountId = HttpContext.Session.GetInt32("accountId");
            if (role != "manager")
            {
                return RedirectToPage("/Login");
            }
            ViewData["AccountId"] = _accService.GetAccountByID((int)accountId).Email;
            ViewData["PartnerId"] = new SelectList(_partnerService.GetPartners().Where(x => x.Status != 0), "PartnerId", "Name");
            ViewData["ProductId"] = new SelectList(_productService.GetProducts(), "ProductId", "Name");
            return Page();
        }

        [BindProperty]
        public StockOut StockOut { get; set; } = default!;
        [BindProperty]
        public StockOutDetail StockOutDetail { get; set; } = default!;
        [BindProperty]
        public Product Product { get; set; } = default!;
        [BindProperty]
        public Partner Partner { get; set; } = default!;
        [BindProperty]
        public Account Account { get; set; } = default!;
        [BindProperty]
        public List<StockOutDetail> StockDetails { get; set; } = new List<StockOutDetail>();

        [BindProperty]
        public List<Product> Products { get; set; } = new List<Product>();


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            bool anySelection = false;
            if (HttpContext.Session.GetString("account") is null || HttpContext.Session.GetString("account") != "manager")
            {
                return RedirectToPage("/Login");
            }
            var accountId = HttpContext.Session.GetInt32("accountId");
            StockOut.AccountId = (int)accountId;
            StockOut.PartnerId = Partner.PartnerId;
            _stockoutService.AddStockOut(StockOut);

            HashSet<int> selectedProductIds = new HashSet<int>(); // Keep track of selected product IDs
            Dictionary<int, int> originalQuantities = new Dictionary<int, int>(); // Keep track of original quantities
            for (int i = 0; i < 5; i++)
            {
                string productIdString = Request.Form[$"Products[{i}].ProductId"];
                string quantityString = Request.Form[$"StockOutDetails[{i}].Quantity"];
                if (!string.IsNullOrEmpty(productIdString) && int.TryParse(productIdString, out int productId) &&
                    !string.IsNullOrEmpty(quantityString) && int.TryParse(quantityString, out int quantity) && quantity > 0)
                {
                    anySelection = true;
                    var productCheck = _productService.GetProductByID(productId);

                    // Check if the quantity is greater than the available quantity in the product
                    if (quantity > productCheck.Quantity)
                    {
                        ViewData["Error"] = $"Quantity for Product ID: {productId} exceeds available quantity:{productCheck.Quantity}.";
                        // Delete the newly created stock (if any)
                        _stockoutService.DeleteStockOutPermanently(StockOut);
                        InitializeSelectLists();
                        return Page();
                    }

                    // Check if the product ID has already been selected
                    if (selectedProductIds.Contains(productId))
                    {
                        ViewData["Error"] = $"Duplicate selection of Product ID: {productId}.";
                        var stockoutDetailsToDelete = _stockoutService.GetStockOutDetailById(StockOut.StockOutId);

                        foreach (var detailToDelete in stockoutDetailsToDelete)
                        {
                            _stockoutService.DeleteStockOutDetailsPermanently(detailToDelete);
                        }
                        _stockoutService.DeleteStockOutPermanently(StockOut);
                        // Revert product quantities to the original state
                        foreach (var kvp in originalQuantities)
                        {
                            var productToUpdate = _productService.GetProductByID(kvp.Key);
                            productToUpdate.Quantity = kvp.Value;
                            _productService.UpdateProduct(productToUpdate);
                        }

                        InitializeSelectLists();
                        return Page();
                    }

                    selectedProductIds.Add(productId);
                    var product = _productService.GetProductByID(productId);
                    originalQuantities[productId] = product.Quantity; // Save the original quantity before the update

                    var stockDetail = new StockOutDetail
                    {
                        StockOutId = StockOut.StockOutId,
                        ProductId = productId,
                        Quantity = quantity,
                    };

                    _stockoutService.AddOneStockOutDetail(stockDetail);
                    product.Quantity = product.Quantity-quantity;
                    _productService.UpdateProduct(product);
                }
            }
            if (!anySelection)
            {
                ViewData["Error"] = "No valid product selection and input was made {quantity needs to be greater than 0}.";
                // Delete the newly created stock (if any)
                _stockoutService.DeleteStockOutPermanently(StockOut);
                InitializeSelectLists();
                return Page();
            }
            return RedirectToPage("./StockOutList");
        }
        private void InitializeSelectLists()
        {
            var accountId = HttpContext.Session.GetInt32("accountId");
            ViewData["AccountId"] = _accService.GetAccountByID((int)accountId).Email;
            ViewData["PartnerId"] = new SelectList(_partnerService.GetPartners().Where(x => x.Status != 0), "PartnerId", "Name");
            ViewData["ProductId"] = new SelectList(_productService.GetProducts(), "ProductId", "Name");
        }
    }
}
