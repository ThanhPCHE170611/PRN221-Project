using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;

namespace PRN221_Project.Pages.Admin
{
    public class DashboardModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly ILotService _lotService;
        private readonly IStockOutService _stockOutService;
        private readonly ICategoryService _categoryService;
        private readonly IPartnerService _partnerService;
        private readonly IProductService _productService;
        private readonly IStorageService _storageService;

        public DashboardModel(IProductService productService, 
            IPartnerService partnerService, 
            ILotService lotService, 
            IStockOutService stockOutService, 
            ICategoryService categoryService, 
            IAccountService accountService,
            IStorageService storageService)
        {
            _productService = productService;
            _lotService = lotService;
            _stockOutService = stockOutService;
            _categoryService = categoryService;
            _accountService = accountService;
            _partnerService = partnerService;
            _storageService = storageService;
        }

        public int ProductCount { get; private set; }
        public int LotCount { get; private set; }
        public int AccountCount { get; private set; }
        public int PartnerCount { get; private set; }
        public int CategoryCount { get; private set; }
        public int StockOutCount { get; private set; }
        public int StorageCount {  get; private set; }
        public int Account1 {  get; private set; }
        public int Account0 { get; private set; }

        public IActionResult OnGet()
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

            ProductCount = _productService.GetProducts().Count();
            LotCount = _lotService.GetAllLots().Count();
            AccountCount = _accountService.GetAccounts().Count();
            PartnerCount = _partnerService.GetPartners().Count();
            CategoryCount = _categoryService.GetCategories().Count();
            StockOutCount = _stockOutService.GetStockOuts().Count();
            StorageCount = _storageService.GetStorageAreas().Count();

            Account1 = _accountService.GetAccounts().Where(s => s.Status == 1).ToList().Count();
            Account0 = _accountService.GetAccounts().Where(s => s.Status == 0).ToList().Count();

            return Page();
        }
    }
}
