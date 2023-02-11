using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeNote.Entities
{
    public class GeneralSettings
    {

        [DisplayName("Borsa")]
        public string Exchange { get; set; }

        [DisplayName("Referans Linki")]
        public string ReferralLink { get; set; }

        [DisplayName("Referral Id")]
        public string ReferralId { get; set; }
        [DisplayName("Maker Komisyon")]
        public decimal MakerCommission { get; set; }

        [DisplayName("Taker Komisyon")]
        public decimal TakerCommission { get; set; }
    }
}
