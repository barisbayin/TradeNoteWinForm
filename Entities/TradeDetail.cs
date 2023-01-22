using System;
using System.ComponentModel;
using TradeNote.Enums;

namespace TradeNote.Entities
{
    public class TradeDetail
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Trade Id")]
        public int TradeId { get; set; }

        [DisplayName("İşlem Tarihi")]
        public DateTime TradeDate { get; set; }

        [DisplayName("İşlem Türü")]
        public TradeType TradeType { get; set; }

        [DisplayName("Emir Tipi")]
        public OrderType OrderType { get; set; }

        [DisplayName("Giriş Bakiyesi $")]
        public decimal EntryBalance { get; set; }

        [DisplayName("Giriş Fiyatı $")]
        public decimal EntryPrice { get; set; }

        [DisplayName("Adet")]
        public decimal EntryLotCount { get; set; }


    }
}
