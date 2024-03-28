using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BusinessObject.Models;
using Service;

namespace PRN221_Project.Pages.StoreKeeper
{
    public class LotCreateModel : PageModel
    {
        private readonly ILotService _lotService;
        private readonly IAccountService _accService;
        private readonly IProductService _productService;
        private readonly IPartnerService _partnerService;
        public LotCreateModel()
        {
            _lotService = new LotService();
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
            if (role != "storekeeper")
            {
                return RedirectToPage("/Login");
            }
            ViewData["AccountId"] = _accService.GetAccountByID((int)accountId).Email;
            ViewData["PartnerId"] = new SelectList(_partnerService.GetPartners().Where(x => x.Status != 0), "PartnerId", "Name");
            ViewData["ProductId"] = new SelectList(_productService.GetProducts(), "ProductId", "Name");
            return Page();
        }

        [BindProperty]
        public Lot Lot { get; set; } = default!;
        [BindProperty]
        public LotDetail LotDetail { get; set; } = default!;
        [BindProperty]
        public Product Product { get; set; } = default!;
        [BindProperty]
        public Partner Partner { get; set; } = default!;
        [BindProperty]
        public Account Account { get; set; } = default!;
        [BindProperty]
        public List<LotDetail> LotDetails { get; set; } = new List<LotDetail>();

        [BindProperty]
        public List<Product> Products { get; set; } = new List<Product>();

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            bool anySelection = false;
            if (HttpContext.Session.GetString("account") is null || HttpContext.Session.GetString("account") != "storekeeper")
            {
                return RedirectToPage("/Login");
            }
            var accountId = HttpContext.Session.GetInt32("accountId");
            Lot.Status = 1;
            Lot.AccountId = (int)accountId;
            Lot.PartnerId = Partner.PartnerId;
            Lot.LotCode = Lot.LotCode.ToUpper();
            _lotService.AddLot(Lot);

            HashSet<int> selectedProductIds = new HashSet<int>(); // Keep track of selected product IDs
            Dictionary<int, int> originalQuantities = new Dictionary<int, int>(); // Keep track of original quantities
            for (int i = 0; i < 5; i++)
            {
                string productIdString = Request.Form[$"Products[{i}].ProductId"];
                string quantityString = Request.Form[$"LotDetails[{i}].Quantity"];
                if (!string.IsNullOrEmpty(productIdString) && int.TryParse(productIdString, out int productId) &&
                    !string.IsNullOrEmpty(quantityString) && int.TryParse(quantityString, out int quantity) && quantity > 0)
                {
                    anySelection = true;
                    // Check if the product ID has already been selected
                    if (selectedProductIds.Contains(productId))
                    {
                        ViewData["Error"] = $"Duplicate selection of Product ID: {productId}.";
                        var lotDetailsToDelete = _lotService.GetListLotDetailByLotID(Lot.LotId);

                        foreach (var detailToDelete in lotDetailsToDelete)
                        {
                            _lotService.DeleteLotDetailPermanently(detailToDelete);
                        }
                        _lotService.DeleteLotPermanently(Lot);
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

                    var lotDetail = new LotDetail
                    {
                        LotId = Lot.LotId,
                        ProductId = productId,
                        PartnerId = Partner.PartnerId,
                        Status = 1,
                        Quantity = quantity,
                    };

                    _lotService.AddLotDetail(lotDetail);
                    product.Quantity += quantity;
                    _productService.UpdateProduct(product);
                }
            }
            if (!anySelection)
            {
                ViewData["Error"] = "No valid product selection and input was made {quantity needs to be greater than 0}.";
                _lotService.DeleteLotPermanently(Lot);
                InitializeSelectLists();
                return Page();
            }
            return RedirectToPage("./LotList");
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
