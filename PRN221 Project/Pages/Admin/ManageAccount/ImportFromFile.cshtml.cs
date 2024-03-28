using BusinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using Service;
using OfficeOpenXml;

namespace PRN221_Project.Pages.Admin.ManageAccount
{
    public class ImportFromFileModel : PageModel
    {
        private readonly IAccountService accountService;
        private readonly PRN221_ProjectContext context;
        public ImportFromFileModel()
        {
            this.accountService = new AccountService();
            this.context = new PRN221_ProjectContext();
        }
        [BindProperty]
        public IFormFile ExcelFile { get; set; }
        public IList<Account> AccountList { get; set; } = default!;
        public IActionResult OnGet(string? go)
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
            if(go != null && go.Equals("downloadFile")) {
                try
                {
                    using (ExcelPackage excelPackage = new ExcelPackage())
                    {
                        // Add a worksheet
                        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                        // Fill the cells with data
                        worksheet.Cells["A1"].Value = "AccountCode";
                        worksheet.Cells["B1"].Value = "Email";
                        worksheet.Cells["C1"].Value = "Name";
                        worksheet.Cells["D1"].Value = "Password";
                        worksheet.Cells["E1"].Value = "Role";
                        worksheet.Cells["F1"].Value = "Phone";
                        worksheet.Cells["G1"].Value = "Status";

                        // Create a memory stream to store the Excel file
                        MemoryStream stream = new MemoryStream();

                        // Save the Excel package to the memory stream
                        excelPackage.SaveAs(stream);

                        // Set the position of the stream back to the beginning
                        stream.Position = 0;

                        // Return the Excel file as a FileStreamResult
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AccountFileTemplate.xlsx");
                    }
                } catch (Exception ex)
                {

                    ViewData["error"] = "Cannot generate file to download!";
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            AccountList = new List<Account>(); 
            try
            {
                if(ExcelFile != null || ExcelFile.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await ExcelFile.CopyToAsync(stream);
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            var worksheet = package.Workbook.Worksheets[0];
                            // Số dòng và số cột có dữ liệu trong sheet
                            int rowCount = worksheet.Dimension.Rows;
                            int colCount = worksheet.Dimension.Columns;
                            // Duyệt qua từng dòng trong sheet để đọc dữ liệu
                            for (int row = 2; row <= rowCount; row++)
                            {
                                var accountCode = worksheet.Cells[row, 1].Value;
                                var email = worksheet.Cells[row, 2].Value;
                                var name = worksheet.Cells[row, 3].Value;
                                var password = worksheet.Cells[row, 4].Value;
                                var role = worksheet.Cells[row, 5].Value;
                                var phone = worksheet.Cells[row, 6].Value;
                                var status = worksheet.Cells[row, 7].Value;
                                Account newAccount = new Account
                                {
                                    AccountCode = accountCode.ToString(),
                                    Email = email.ToString(),
                                    Name = name.ToString(),
                                    Password = password.ToString(),
                                    Role = Int32.Parse(role.ToString()),
                                    Phone = phone.ToString(),
                                    Status = Int32.Parse(status.ToString())
                                };
                                AccountList.Add(newAccount);
                            }
                            //Handle valid list
                            bool checkFile = HandleValidateListAndSave(AccountList);
                            if (checkFile)
                            {
                                context.Accounts.AddRange(AccountList);
                                context.SaveChanges();
                                return Page();
                            } else
                            {
                                AccountList.Clear();
                                return Page();
                            }
                        }
                    }
                } else
                {
                    ViewData["error"] = "File is not valid!";
                    return Page();
                }
            } catch (Exception ex)
            {
                AccountList.Clear();
                ViewData["error"] = ex.Message;
                return Page();
            }
        }

        private bool HandleValidateListAndSave(IList<Account> accountList)
        {
            for (int i = 0; i < accountList.Count; i++)
            {
                if (CheckAccount(accountList[i]) == true)
                {
                    ViewData["error"] = "Account is existed, the row wrong is at " + (i+2);
                    return false;
                }
            }return true;
        }

        private bool CheckAccount(Account account)
        {
            return accountService.GetAccounts()
                    .Any(a => a.AccountCode.ToLower().Equals(account.AccountCode.ToLower()) || a.Email.ToLower().Equals(account.Email.ToLower()));
        }
    }
}
