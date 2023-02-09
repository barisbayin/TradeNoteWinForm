﻿using System.ComponentModel;

namespace TradeNote
{
    public class GeneralInformation
    {

        [DisplayName("Borsa")]
        public string Exchange { get; set; }

        [DisplayName("Referans Linki")]
        public string ReferralLink { get; set; }

        [DisplayName("Referral Id")]
        public string ReferralId { get; set; }

        [DisplayName("Başlangıç Bakiyesi")]
        public decimal StartingBalance { get; set; }

        [DisplayName("Güncel Bakiye")]
        public decimal LastBalance { get; set; }

        [DisplayName("İşlemdeki Bakiye")]
        public decimal InTradeBalance { get; set; }

        [DisplayName("Boştaki Bakiye")]
        public decimal AvailableBalance { get; set; }

        [DisplayName("Kar Toplamı")]
        public decimal ProfitsSum { get; set; }

        [DisplayName("Zarar Toplamı")]
        public decimal LossesSum { get; set; }

        [DisplayName("Toplam K/Z $")]
        public decimal TotalPnL { get; set; }

        [DisplayName("Toplam K/Z %")]
        public decimal TotalPnLPercent { get; set; }

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