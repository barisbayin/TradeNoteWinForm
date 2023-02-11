using System.Collections.Generic;
using TradeNote.Entities;

namespace TradeNote
{
    public class TradeModel
    {
        public GeneralSettings GeneralSettings { get; set; }
        public GeneralInformation GeneralInformation { get; set; }
        public List<CurrencyPairStatistic> CurrencyPairStatistics { get; set; }
        public List<Trade> Trades { get; set; }

    }
}