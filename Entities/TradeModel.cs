using System.Collections.Generic;

namespace TradeNote
{
    public class TradeModel
    {
        public GeneralInformation GeneralInformation { get; set; }
        public List<Trade> Trades { get; set; }

    }
}