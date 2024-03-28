using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.Models
{
    public partial class LotDetail
    {
        public int LotDetailId { get; set; }
        public int LotId { get; set; }
        public int ProductId { get; set; }
        public int PartnerId { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be high 0")]
        public int Quantity { get; set; }
        public int Status { get; set; }

        public virtual Lot Lot { get; set; } = null!;
        public virtual Partner Partner { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
