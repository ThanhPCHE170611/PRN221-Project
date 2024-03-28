using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models
{
    public partial class Account
    {
        public Account()
        {
            Lots = new HashSet<Lot>();
            StockOuts = new HashSet<StockOut>();
        }

        public int AccountId { get; set; }

        [Required(ErrorMessage = "AccountCode is required")]
        [RegularExpression(@"^ACC\d{2,}$", ErrorMessage = "Account Code must be in the format ACC followed by at least two digits")]
        public string AccountCode { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[A-Z][a-z]*((\s[A-Z][a-z]*)*)$", ErrorMessage = "FullName must have the first letter of each word capitalized")]
        [MinLength(5), MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$", ErrorMessage = "Password must be 8 to 15 characters and include at least one lowercase letter, one uppercase letter, one digit, and one special character.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Role is required")]
        public int Role { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone]
        [RegularExpression(@"^\d{10,11}$", ErrorMessage = "Phone number must be between 10 and 11 digits.")]
        public string? Phone { get; set; }

        public int Status { get; set; }

        public virtual ICollection<Lot> Lots { get; set; }
        public virtual ICollection<StockOut> StockOuts { get; set; }
    }
}
