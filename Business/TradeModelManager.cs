using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradeNote.Entities;
using TradeNote.Enums;
using TradeNote.Repositories;

namespace TradeNote.Business
{
    public class TradeModelManager
    {
        private TradeModelXmlRepository _tradeModelXmlRepository;
        public TradeModelManager(TradeModelXmlRepository tradeModelXmlRepository)
        {
            _tradeModelXmlRepository = tradeModelXmlRepository;
        }

        public void CreateEmptyXmlFile(string xmlFilePath)
        {
            _tradeModelXmlRepository.CreateEmptyXmlFile(xmlFilePath);
        }

        public void AddTrade(Trade trade, string xmlFilePath)
        {
            int newTradeId = _tradeModelXmlRepository.GetNewTradeId(xmlFilePath);

            trade.Id = newTradeId;

            _tradeModelXmlRepository.AddTrade(trade, xmlFilePath);
        }
        public void UpdateTrade(Trade updatedTrade, string xmlFilePath)
        {
            _tradeModelXmlRepository.UpdateTrade(updatedTrade, xmlFilePath);
        }

        public List<Trade> GetTradeList(string xmlFilePath)
        {
            var tradeList = _tradeModelXmlRepository.GetTrades(xmlFilePath);
            return tradeList;
        }

        public Trade GetTradeById(int id, string xmlFilePath)
        {
            var trade = _tradeModelXmlRepository.GetTradeById(id, xmlFilePath);
            return trade;
        }

        public void AddTradeDetail(int tradeId, TradeDetail tradeDetail, string xmlFilePath)
        {
            int newTradeDetailId = _tradeModelXmlRepository.GetNewTradeDetailId(tradeId, xmlFilePath);

            tradeDetail.Id = newTradeDetailId;

            _tradeModelXmlRepository.AddTradeDetail(tradeId, tradeDetail, xmlFilePath);
        }

        public void UpdateTradeDetail(int tradeId, TradeDetail updatedTradeDetail, string xmlFilePath)
        {
            _tradeModelXmlRepository.UpdateTradeDetail(tradeId, updatedTradeDetail, xmlFilePath);
        }

        public List<TradeDetail> GetTradeDetailList(int tradeId, string xmlFilePath)
        {
            var tradeDetailList = _tradeModelXmlRepository.GetTradeDetailList(tradeId, xmlFilePath);
            return tradeDetailList;
        }

        public TradeDetail GetTradeDetailById(int tradeId, int tradeDetailId, string xmlFilePath)
        {
            var tradeDetail = _tradeModelXmlRepository.GetTradeDetailById(tradeId, tradeDetailId, xmlFilePath);
            return tradeDetail;
        }

        public GeneralInformation GetGeneralInformation(string xmlFilePath)
        {
            var generalInformation = _tradeModelXmlRepository.GetGeneralInformation(xmlFilePath);

            return generalInformation;
        }

        public void UpdateGeneralInformation(GeneralInformation currentGeneralInformation, string xmlFilePath)
        {
            _tradeModelXmlRepository.UpdateGeneralInformation(currentGeneralInformation, xmlFilePath);
        }

        public void CalculateGeneralInformation(string xmlFilePath)
        {
            decimal profitSum;
            decimal lossSum;
            decimal totalProfitOrLoss;
            int wonTradeCount;
            int lossTradeCount;
            decimal winRate = 0;
            decimal lastBalance;
            decimal totalPnlPercent;

            var currentGeneralInformation = GetGeneralInformation(xmlFilePath);

            var dataList = GetTradeList(xmlFilePath);

            if (dataList.Count != 0)
            {
                wonTradeCount = dataList.Count(x => x.PositionResult == PositionResult.TP);
                lossTradeCount = dataList.Count(x => x.PositionResult == PositionResult.SL);

                if (wonTradeCount != 0)
                {
                    winRate = Math.Round(Convert.ToDecimal(wonTradeCount) / (Convert.ToDecimal(wonTradeCount) + Convert.ToDecimal(lossTradeCount)) * 100, 2);
                }


                profitSum = Math.Round(dataList.Where(x => x.PositionResult == PositionResult.TP).Sum(x => x.ProfitOrLoss), 2);
                lossSum = Math.Round(dataList.Where(x => x.PositionResult == PositionResult.SL).Sum(x => x.ProfitOrLoss), 2);
                totalProfitOrLoss = profitSum + lossSum;
                lastBalance = Math.Round(Convert.ToDecimal(currentGeneralInformation.StartingBalance + totalProfitOrLoss), 2);
                totalPnlPercent = Math.Round(Convert.ToDecimal((lastBalance / currentGeneralInformation.StartingBalance - 1) * 100), 2);



                currentGeneralInformation.StartingBalance = currentGeneralInformation.StartingBalance;
                currentGeneralInformation.LastBalance = lastBalance;
                currentGeneralInformation.LossCount = lossTradeCount;
                currentGeneralInformation.WinCount = wonTradeCount;
                currentGeneralInformation.ProfitsSum = profitSum;
                currentGeneralInformation.LossesSum = lossSum;
                currentGeneralInformation.TotalPnL = totalProfitOrLoss;
                currentGeneralInformation.TradeWinRate = winRate;
                currentGeneralInformation.TotalPnLPercent = totalPnlPercent;

                UpdateGeneralInformation(currentGeneralInformation, xmlFilePath);
            }
        }

        public Trade CalculateTrade(int tradeId, string xmlFilePath)
        {
            var willCalculatedTrade = _tradeModelXmlRepository.GetTradeById(tradeId, xmlFilePath);

            var tradeDetails = willCalculatedTrade.TradeDetails;

            try
            {
                decimal closeTotalCount = 0;
                decimal averageEntryBalance = 0;
                decimal entryTotalCount = 0;
                decimal averageEntryPrice = 0;
                decimal averageCloseBalance = 0;
                decimal averagePositionClosePrice = 0;
                decimal expectedRiskValue = 0;
                decimal expectedRewardValue = 0;


                switch (willCalculatedTrade.PositionSide)
                {
                    case PositionSide.Long:

                        averageEntryBalance = tradeDetails.Where(x => x.TradeType == TradeType.OpenLong).Sum(x => x.EntryBalance);

                        entryTotalCount = tradeDetails.Where(x => x.TradeType == TradeType.OpenLong)
                            .Sum(x => x.EntryLotCount);

                        averageEntryPrice = Math.Round(averageEntryBalance / entryTotalCount, 8);

                        if (tradeDetails.Count(x => x.TradeType == TradeType.CloseLong) > 0)
                        {
                            averageCloseBalance = tradeDetails.Where(x => x.TradeType == TradeType.CloseLong).Sum(x => x.EntryBalance);

                            closeTotalCount = tradeDetails.Where(x => x.TradeType == TradeType.CloseLong)
                                .Sum(x => x.EntryLotCount);

                            averagePositionClosePrice = Math.Round(averageCloseBalance / closeTotalCount, 8);
                        }

                        if (averageEntryBalance > 0)
                        {
                            willCalculatedTrade.RiskPercent = Math.Round((willCalculatedTrade.StopLossPrice / averageEntryPrice - 1) * 100, 2);
                            willCalculatedTrade.RewardPercent = Math.Round((willCalculatedTrade.TakeProfitPrice / averageEntryPrice - 1) * 100, 2);
                            willCalculatedTrade.RiskRewardRatio = Math.Abs(Math.Round(willCalculatedTrade.RewardPercent / willCalculatedTrade.RiskPercent, 2));

                        }


                        break;

                    case PositionSide.Short:

                        averageEntryBalance = tradeDetails.Where(x => x.TradeType == TradeType.OpenShort).Sum(x => x.EntryBalance);

                        entryTotalCount = tradeDetails.Where(x => x.TradeType == TradeType.OpenShort)
                            .Sum(x => x.EntryLotCount);

                        averageEntryPrice = Math.Round(averageEntryBalance / entryTotalCount, 8);

                        if (tradeDetails.Count(x => x.TradeType == TradeType.CloseShort) > 0)
                        {
                            averageCloseBalance = tradeDetails.Where(x => x.TradeType == TradeType.CloseShort)
                                .Sum(x => x.EntryBalance);

                            closeTotalCount = tradeDetails.Where(x => x.TradeType == TradeType.CloseShort)
                                .Sum(x => x.EntryLotCount);

                            averagePositionClosePrice = Math.Round(averageCloseBalance / closeTotalCount, 8);
                        }

                        if (averageEntryBalance > 0)
                        {
                            willCalculatedTrade.RiskPercent = Math.Round((1 - willCalculatedTrade.StopLossPrice / averageEntryPrice) * 100, 2);
                            willCalculatedTrade.RewardPercent = Math.Round((1 - willCalculatedTrade.TakeProfitPrice / averageEntryPrice) * 100, 2);
                            willCalculatedTrade.RiskRewardRatio = Math.Abs(Math.Round(willCalculatedTrade.RewardPercent / willCalculatedTrade.RiskPercent, 2));
                        }

                        break;
                }

                if (averageEntryBalance > 0)
                {
                    expectedRiskValue = Math.Round(averageEntryBalance * willCalculatedTrade.Leverage *
                        willCalculatedTrade.RiskPercent / 100, 2);

                    willCalculatedTrade.ExpectedRiskValue = expectedRiskValue;

                    expectedRewardValue = Math.Round(averageEntryBalance * willCalculatedTrade.Leverage *
                        willCalculatedTrade.RewardPercent / 100, 2);

                    willCalculatedTrade.ExpectedRewardValue = expectedRewardValue;
                }

                willCalculatedTrade.AverageEntryBalance = averageEntryBalance;
                willCalculatedTrade.AverageEntryPrice = averageEntryPrice;
                willCalculatedTrade.AveragePositionClosePrice = averagePositionClosePrice;
                willCalculatedTrade.AverageCloseBalance = averageCloseBalance;
                willCalculatedTrade.AverageCloseLotCount = closeTotalCount;
                willCalculatedTrade.AverageEntryLotCount = entryTotalCount;

                if (willCalculatedTrade.EndTrade)
                {
                    if (Math.Abs(entryTotalCount - closeTotalCount) < 0.0005M)
                    {
                        decimal profit = 0;
                        if (willCalculatedTrade.PositionSide == PositionSide.Long)
                        {
                            profit = (averageCloseBalance - averageEntryBalance) * willCalculatedTrade.Leverage;
                            willCalculatedTrade.ProfitOrLoss = profit;
                            willCalculatedTrade.ProfitOrLossPercent = Math.Round((averageCloseBalance / averageEntryBalance - 1) * 100 * willCalculatedTrade.Leverage, 2);
                        }
                        if (willCalculatedTrade.PositionSide == PositionSide.Short)
                        {
                            profit = (averageEntryBalance - averageCloseBalance) * willCalculatedTrade.Leverage;
                            willCalculatedTrade.ProfitOrLoss = profit;
                            willCalculatedTrade.ProfitOrLossPercent = Math.Round((1 - averageCloseBalance / averageEntryBalance) * 100 * willCalculatedTrade.Leverage, 2);
                        }

                        willCalculatedTrade.TradeEndDate = DateTime.Now;
                        if (profit < 0)
                        {
                            willCalculatedTrade.PositionResult = PositionResult.SL;
                        }
                        else
                        {
                            willCalculatedTrade.PositionResult = PositionResult.TP;
                        }
                    }
                }

                return willCalculatedTrade;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Hata oluştu!", MessageBoxButtons.OK);
            }

            return willCalculatedTrade;
        }
    }
}
