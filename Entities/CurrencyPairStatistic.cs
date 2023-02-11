using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeNote.Entities
{
    public class CurrencyPairStatistic
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("İşlem Çifti")]
        public string CurrencyPair { get; set; }

        [DisplayName("İşlemdeki Bakiye")]
        public decimal InTradeBalance { get; set; }

        [DisplayName("Kar Toplamı")]
        public decimal ProfitsSum { get; set; }

        [DisplayName("Zarar Toplamı")]
        public decimal LossesSum { get; set; }

        [DisplayName("Toplam K/Z $")]
        public decimal TotalPnL { get; set; }

        [DisplayName("Toplam K/Z %")]
        public decimal TotalPnLPercent { get; set; }

        [DisplayName("İşlem Sayısı")]
        public int TotalTradeCount { get; set; }

        [DisplayName("Karlı Trade Sayısı")]
        public int WinCount { get; set; }

        [DisplayName("Zararlı Trade Sayısı")]
        public int LossCount { get; set; }

        [DisplayName("Kazanma Yüzdesi")]
        public decimal TradeWinRate { get; set; }

        [DisplayName("Maker Komisyon")]
        public decimal MakerCommission { get; set; }

        [DisplayName("Taker Komisyon")]
        public decimal TakerCommission { get; set; }

        [DisplayName("Komisyon")]
        public decimal TotalCommission { get; set; }

        [DisplayName("Fonlama Maliyeti")]
        public decimal TotalFundingFee { get; set; }
    }
}
