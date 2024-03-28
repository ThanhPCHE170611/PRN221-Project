using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models
{
    public partial class Product
    {
        public Product()
        {
            LotDetails = new HashSet<LotDetail>();
            StockOutDetails = new HashSet<StockOutDetail>();
        }

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int AreaId { get; set; }
        public string ProductCode { get; set; } = null!;
        [Required]
        public string? Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be greater than or equal to 0")]
        public int Quantity { get; set; }
        public int Status { get; set; }

        public virtual StorageArea Area { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<LotDetail> LotDetails { get; set; }
        public virtual ICollection<StockOutDetail> StockOutDetails { get; set; }
    }
}
