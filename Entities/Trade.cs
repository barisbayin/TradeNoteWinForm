using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeNote.Entities;

namespace TradeNote
{
    public class Trade
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Başlangıç Tarihi")]
        public DateTime TradeStartDate { get; set; }

        [DisplayName("Bitiş Tarihi")]
        public DateTime? TradeEndDate { get; set; }

        [DisplayName("Yön")]
        public PositionSide PositionSide { get; set; }

        [DisplayName("Giriş Bakiyesi")]
        public decimal AverageEntryBalance { get; set; }

        [DisplayName("Kaldıraç")]
        public int Leverage { get; set; }

        [DisplayName("Hedeflenen Giriş Fiyatı")]
        public decimal TargetedEntryPrice { get; set; }

        [DisplayName("Stop Fiyatı")]
        public decimal StopLossPrice { get; set; }

        [DisplayName("TP Fiyatı")]
        public decimal TakeProfitPrice { get; set; }

        [DisplayName("Giriş Fiyatı $")]
        public decimal AverageEntryPrice { get; set; }

        [DisplayName("Poz. Kap. Fiyatı")]
        public decimal AveragePositionClosePrice { get; set; }

        [DisplayName("Risk %")]
        public decimal RiskPercent { get; set; }

        [DisplayName("Kazanç %")]
        public decimal RewardPercent { get; set; }

        [DisplayName("Risk/Kazanç")]
        public decimal RiskRewardRatio { get; set; }

        [DisplayName("Tahmini Risk $")]
        public decimal ExpectedRiskValue { get; set; }

        [DisplayName("Tahmini Kazanç $")]
        public decimal ExpectedRewardValue { get; set; }

        [DisplayName("Poz. Sonuç")]
        public PositionResult PositionResult { get; set; }

        [DisplayName("PnL $")]
        public decimal ProfitOrLoss { get; set; }

        [DisplayName("PnL %")]
        public decimal ProfitOrLossPercent { get; set; }

        [DisplayName("Not")]
        public string Note { get; set; }

        [DisplayName("Posizyon Detayları")]
        public List<TradeDetail> TradeDetails { get; set; }

    }
}
