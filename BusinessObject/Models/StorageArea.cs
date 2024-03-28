using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models
{
    public partial class StorageArea
    {
        public StorageArea()
        {
            Products = new HashSet<Product>();
        }

        public int AreaId { get; set; }
        [RegularExpression(@"^AREA\d{3,}$", ErrorMessage = "Area Code must be in the format AREA followed by at least two digits")]
        public string AreaCode { get; set; } = null!;
        public string AreaName { get; set; } = null!;
        public int Status { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
