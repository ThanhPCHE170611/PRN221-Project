using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Partner
    {
        public Partner()
        {
            LotDetails = new HashSet<LotDetail>();
            Lots = new HashSet<Lot>();
            StockOuts = new HashSet<StockOut>();
        }

        public int PartnerId { get; set; }
        public string PartnerCode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Status { get; set; }

        public virtual ICollection<LotDetail> LotDetails { get; set; }
        public virtual ICollection<Lot> Lots { get; set; }
        public virtual ICollection<StockOut> StockOuts { get; set; }
    }
}
