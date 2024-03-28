using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class Lot
    {
        public Lot()
        {
            LotDetails = new HashSet<LotDetail>();
        }

        public int LotId { get; set; }
        public int AccountId { get; set; }
        public int PartnerId { get; set; }
        public string LotCode { get; set; } = null!;
        public DateTime DateIn { get; set; }
        public string? Note { get; set; }
        public int Status { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Partner Partner { get; set; } = null!;
        public virtual ICollection<LotDetail> LotDetails { get; set; }
    }
}
