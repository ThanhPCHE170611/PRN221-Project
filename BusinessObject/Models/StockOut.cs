using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class StockOut
    {
        public StockOut()
        {
            StockOutDetails = new HashSet<StockOutDetail>();
        }

        public int StockOutId { get; set; }
        public int AccountId { get; set; }
        public int PartnerId { get; set; }
        public DateTime DateOut { get; set; }
        public string? Note { get; set; }
        public int Status { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Partner Partner { get; set; } = null!;
        public virtual ICollection<StockOutDetail> StockOutDetails { get; set; }
    }
}
