using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models
{
    public partial class StockOutDetail
    {
        public int StockOutDetailId { get; set; }
        public int ProductId { get; set; }
        public int StockOutId { get; set; }
        [Required]
        [Range(1, int.MaxValue,ErrorMessage ="Quantity must be high 0")]
        public int Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual StockOut StockOut { get; set; } = null!;
    }
}
