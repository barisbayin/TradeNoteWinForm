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
        public void DeleteXmlFileByPath(string xmlFilePath)
        {
            _tradeModelXmlRepository.DeleteXmlFileByPath(xmlFilePath);
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

        public GeneralSettings GetGeneralSettings(string xmlFilePath)
        {
            var generalSettings = _tradeModelXmlRepository.GetGeneralSettings(xmlFilePath);

            return generalSettings;
        }

        public void UpdateGeneralSettings(GeneralSettings currentGeneralSettings, string xmlFilePath)
        {
            _tradeModelXmlRepository.UpdateGeneralSettings(currentGeneralSettings, xmlFilePath);
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
            decimal totalCommission;
            decimal totalFundingFee;
            decimal inTradeBalance;
            decimal availableBalance;
            decimal totalTradeCount;

            var currentGeneralInformation = GetGeneralInformation(xmlFilePath);

            var dataList = GetTradeList(xmlFilePath);

            if (dataList.Count != 0)
            {
                wonTradeCount = dataList.Count(x => x.PositionResult == PositionResult.TP);
                lossTradeCount = dataList.Count(x => x.PositionResult == PositionResult.SL);
                totalTradeCount = wonTradeCount + lossTradeCount;

                if (wonTradeCount != 0)
                {
                    winRate = Math.Round(Convert.ToDecimal(wonTradeCount) / Convert.ToDecimal(totalTradeCount) * 100, 2);
                }


                profitSum = Math.Round(dataList.Where(x => x.PositionResult == PositionResult.TP).Sum(x => x.ProfitOrLoss), 2);
                lossSum = Math.Round(dataList.Where(x => x.PositionResult == PositionResult.SL).Sum(x => x.ProfitOrLoss), 2);

                totalCommission = Math.Round(dataList.Where(x => x.EndTrade).Sum(x => x.CommissionSum), 2);
                totalFundingFee = Math.Round(dataList.Where(x => x.EndTrade).Sum(x => x.FundingFeeSum), 2);

                totalProfitOrLoss = profitSum + lossSum - totalCommission - totalFundingFee;

                lastBalance = Math.Round(Convert.ToDecimal(currentGeneralInformation.StartingBalance + totalProfitOrLoss - totalCommission), 2);

                totalPnlPercent = Math.Round(Convert.ToDecimal((lastBalance / currentGeneralInformation.StartingBalance - 1) * 100), 2);

                inTradeBalance = Math.Round(_tradeModelXmlRepository.GetAllTrades(xmlFilePath).Where(x => x.EndTrade == false).Sum(x => x.AverageEntryBalance) - _tradeModelXmlRepository.GetAllTrades(xmlFilePath).Where(x => x.EndTrade == false).Sum(x => x.AverageCloseBalance), 2);

                availableBalance = lastBalance - inTradeBalance;

                currentGeneralInformation.StartingBalance = currentGeneralInformation.StartingBalance;
                currentGeneralInformation.LastBalance = lastBalance;
                currentGeneralInformation.InTradeBalance = inTradeBalance;
                currentGeneralInformation.AvailableBalance = availableBalance;
                currentGeneralInformation.LossCount = lossTradeCount;
                currentGeneralInformation.WinCount = wonTradeCount;
                currentGeneralInformation.ProfitsSum = profitSum;
                currentGeneralInformation.LossesSum = lossSum;
                currentGeneralInformation.TotalPnL = totalProfitOrLoss;
                currentGeneralInformation.TradeWinRate = winRate;
                currentGeneralInformation.TotalPnLPercent = totalPnlPercent;
                currentGeneralInformation.TotalCommission = totalCommission;
                currentGeneralInformation.TotalFundingFee = totalFundingFee;

                UpdateGeneralInformation(currentGeneralInformation, xmlFilePath);
            }
        }

        public void CalculateTrade(int tradeId, string xmlFilePath)
        {
            var generalSettings = _tradeModelXmlRepository.GetGeneralSettings(xmlFilePath);

            var willCalculatedTrade = _tradeModelXmlRepository.GetTradeById(tradeId, xmlFilePath);

            var tradeDetails = willCalculatedTrade.TradeDetails;

            try
            {
                decimal closeTotalCount = 0;
                decimal averageEntryBalance = 0;
                decimal entryTotalCount = 0;
                decimal averageEntryPrice = 0;
                decimal entryBalanceMaker = 0;
                decimal entryBalanceTaker = 0;
                decimal averageCloseBalance = 0;
                decimal closeBalanceMaker = 0;
                decimal closeBalanceTaker = 0;
                decimal averagePositionClosePrice = 0;
                decimal expectedRiskValue = 0;
                decimal expectedRewardValue = 0;
                decimal commissionSum = 0;
                decimal openCommissionSum = 0;
                decimal closeCommissionSum = 0;


                switch (willCalculatedTrade.PositionSide)
                {
                    case PositionSide.Long:

                        entryBalanceMaker = tradeDetails.Where(x => x.TradeType == TradeType.OpenLong && x.OrderType == OrderType.Maker).Sum(x => x.EntryBalance);

                        entryBalanceTaker = tradeDetails.Where(x => x.TradeType == TradeType.OpenLong && x.OrderType == OrderType.Taker).Sum(x => x.EntryBalance);

                        averageEntryBalance = entryBalanceMaker + entryBalanceTaker;

                        if (averageEntryBalance != 0)
                        {
                            entryTotalCount = tradeDetails.Where(x => x.TradeType == TradeType.OpenLong)
                                .Sum(x => x.EntryLotCount);
                        }
                        else
                        {
                            entryTotalCount = 0;
                        }

                        averageEntryPrice = averageEntryBalance != 0 ? Math.Round(averageEntryBalance / entryTotalCount, 8) : 0;

                        if (tradeDetails.Count(x => x.TradeType == TradeType.CloseLong) > 0)
                        {
                            closeBalanceMaker = tradeDetails.Where(x => x.TradeType == TradeType.CloseLong && x.OrderType == OrderType.Maker).Sum(x => x.EntryBalance);
                            closeBalanceTaker = tradeDetails.Where(x => x.TradeType == TradeType.CloseLong && x.OrderType == OrderType.Taker).Sum(x => x.EntryBalance);

                            averageCloseBalance = closeBalanceMaker + closeBalanceTaker;

                            closeTotalCount = tradeDetails.Where(x => x.TradeType == TradeType.CloseLong)
                                .Sum(x => x.EntryLotCount);

                            if (closeTotalCount != 0)
                            {
                                averagePositionClosePrice = Math.Round(averageCloseBalance / closeTotalCount, 8);
                            }

                        }

                        if (averageEntryBalance > 0)
                        {
                            willCalculatedTrade.RiskPercent = Math.Round((willCalculatedTrade.StopLossPrice / averageEntryPrice - 1) * 100, 2);
                            willCalculatedTrade.RewardPercent = Math.Round((willCalculatedTrade.TakeProfitPrice / averageEntryPrice - 1) * 100, 2);
                            willCalculatedTrade.RiskRewardRatio = Math.Abs(Math.Round(willCalculatedTrade.RewardPercent / willCalculatedTrade.RiskPercent, 2));

                        }
                        else
                        {
                            willCalculatedTrade.RiskPercent = Math.Round((willCalculatedTrade.StopLossPrice / willCalculatedTrade.TargetedEntryPrice - 1) * 100, 2);
                            willCalculatedTrade.RewardPercent = Math.Round((willCalculatedTrade.TakeProfitPrice / willCalculatedTrade.TargetedEntryPrice - 1) * 100, 2);
                            willCalculatedTrade.RiskRewardRatio = Math.Abs(Math.Round(willCalculatedTrade.RewardPercent / willCalculatedTrade.RiskPercent, 2));
                        }

                        break;

                    case PositionSide.Short:

                        entryBalanceMaker = tradeDetails.Where(x => x.TradeType == TradeType.OpenShort && x.OrderType == OrderType.Maker).Sum(x => x.EntryBalance);

                        entryBalanceTaker = tradeDetails.Where(x => x.TradeType == TradeType.OpenShort && x.OrderType == OrderType.Taker).Sum(x => x.EntryBalance);

                        averageEntryBalance = entryBalanceMaker + entryBalanceTaker;

                        if (averageEntryBalance != 0)
                        {
                            entryTotalCount = tradeDetails.Where(x => x.TradeType == TradeType.OpenShort)
                                .Sum(x => x.EntryLotCount);
                        }
                        else
                        {
                            entryTotalCount = 0;
                        }

                        averageEntryPrice = averageEntryBalance != 0 ? Math.Round(averageEntryBalance / entryTotalCount, 8) : 0;

                        if (tradeDetails.Count(x => x.TradeType == TradeType.CloseShort) > 0)
                        {
                            closeBalanceMaker = tradeDetails.Where(x => x.TradeType == TradeType.CloseShort && x.OrderType == OrderType.Maker).Sum(x => x.EntryBalance);
                            closeBalanceTaker = tradeDetails.Where(x => x.TradeType == TradeType.CloseShort && x.OrderType == OrderType.Taker).Sum(x => x.EntryBalance);

                            averageCloseBalance = closeBalanceMaker + closeBalanceTaker;

                            closeTotalCount = tradeDetails.Where(x => x.TradeType == TradeType.CloseShort)
                                .Sum(x => x.EntryLotCount);

                            if (closeTotalCount != 0)
                            {
                                averagePositionClosePrice = Math.Round(averageCloseBalance / closeTotalCount, 8);
                            }
                        }

                        if (averageEntryBalance > 0)
                        {
                            willCalculatedTrade.RiskPercent = Math.Round((1 - willCalculatedTrade.StopLossPrice / averageEntryPrice) * 100, 2);
                            willCalculatedTrade.RewardPercent = Math.Round((1 - willCalculatedTrade.TakeProfitPrice / averageEntryPrice) * 100, 2);
                            willCalculatedTrade.RiskRewardRatio = Math.Abs(Math.Round(willCalculatedTrade.RewardPercent / willCalculatedTrade.RiskPercent, 2));
                        }
                        else
                        {
                            willCalculatedTrade.RiskPercent = Math.Round((1 - willCalculatedTrade.StopLossPrice / willCalculatedTrade.TargetedEntryPrice) * 100, 2);
                            willCalculatedTrade.RewardPercent = Math.Round((1 - willCalculatedTrade.TakeProfitPrice / willCalculatedTrade.TargetedEntryPrice) * 100, 2);
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


                openCommissionSum = entryBalanceMaker * willCalculatedTrade.Leverage * generalSettings.MakerCommission / 100 +
                                    entryBalanceTaker * willCalculatedTrade.Leverage * generalSettings.TakerCommission / 100;

                closeCommissionSum = closeBalanceMaker * willCalculatedTrade.Leverage * generalSettings.MakerCommission / 100 + closeBalanceTaker * willCalculatedTrade.Leverage * generalSettings.TakerCommission / 100;

                commissionSum = openCommissionSum + closeCommissionSum;


                willCalculatedTrade.CommissionSum = commissionSum;

                if (willCalculatedTrade.EndTrade)
                {
                    if (Math.Abs(entryTotalCount - closeTotalCount) < 0.0001M)
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
                        willCalculatedTrade.PositionResult = profit < 0 ? PositionResult.SL : PositionResult.TP;
                    }
                }

                _tradeModelXmlRepository.UpdateTrade(willCalculatedTrade, xmlFilePath);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Hata oluştu!", MessageBoxButtons.OK);
            }

        }

        public void RemoveTradeById(int tradeId, string xmlFilePath)
        {
            _tradeModelXmlRepository.RemoveTradeById(tradeId, xmlFilePath);
        }

        public void RemoveTradeDetailById(int tradeId, int tradeDetailId, string xmlFilePath)
        {
            _tradeModelXmlRepository.RemoveTradeDetailById(tradeId, tradeDetailId, xmlFilePath);
        }

        public void AddCurrencyPairStatistic(CurrencyPairStatistic currencyPairStatistic, string xmlFilePath)
        {
            int currencyPairStatisticId = _tradeModelXmlRepository.GetNewCurrencyPairStatisticId(xmlFilePath);

            currencyPairStatistic.Id = currencyPairStatisticId;
            _tradeModelXmlRepository.AddCurrencyPairStatistic(currencyPairStatistic, xmlFilePath);
        }

        public void UpdateCurrencyPairStatisticByCurrencyPair(CurrencyPairStatistic currencyPairStatistic, string xmlFilePath)
        {
            _tradeModelXmlRepository.UpdateCurrencyPairStatisticByCurrencyPair(currencyPairStatistic, xmlFilePath);
        }

        public List<CurrencyPairStatistic> GetCurrencyPairStatisticList(string xmlFilePath)
        {
            var currencyPairStatisticList = _tradeModelXmlRepository.GetCurrencyPairStatisticList(xmlFilePath);
            return currencyPairStatisticList;
        }

        public CurrencyPairStatistic GetCurrencyPairStatisticByCurrencyPair(string currencyPair, string xmlFilePath)
        {
            var currencyPairStatistic = _tradeModelXmlRepository.GetCurrencyPairStatisticByCurrencyPair(currencyPair, xmlFilePath);
            return currencyPairStatistic;
        }

        public void CalculateCurrencyPairStatisticByCurrencyPair(string currencyPair, string xmlFilePath)
        {
            decimal profitSum;
            decimal lossSum;
            decimal totalProfitOrLoss;
            int wonTradeCount;
            int lossTradeCount;
            int totalTradeCount;
            decimal winRate = 0;
            decimal totalPnlPercent;
            decimal totalCommission;
            decimal totalFundingFee;
            decimal inTradeBalance;
            decimal totalClosedBalance;
            decimal totalEntryBalance;

            var currentCurrencyPairStatistic = GetCurrencyPairStatisticByCurrencyPair(currencyPair, xmlFilePath);

            var dataList = GetTradeList(xmlFilePath).Where(x => x.CurrencyPair == currencyPair).ToList();

            if (dataList.Count != 0)
            {
                wonTradeCount = dataList.Count(x => x.PositionResult == PositionResult.TP);
                lossTradeCount = dataList.Count(x => x.PositionResult == PositionResult.SL);
                totalTradeCount = wonTradeCount + lossTradeCount;

                if (totalTradeCount != 0)
                {
                    winRate = Math.Round(Convert.ToDecimal(wonTradeCount) / Convert.ToDecimal(totalTradeCount) * 100, 2);
                }

                profitSum = Math.Round(dataList.Where(x => x.PositionResult == PositionResult.TP).Sum(x => x.ProfitOrLoss), 2);
                lossSum = Math.Round(dataList.Where(x => x.PositionResult == PositionResult.SL).Sum(x => x.ProfitOrLoss), 2);

                totalCommission = Math.Round(dataList.Where(x => x.EndTrade).Sum(x => x.CommissionSum), 2);
                totalFundingFee = Math.Round(dataList.Where(x => x.EndTrade).Sum(x => x.FundingFeeSum), 2);

                totalProfitOrLoss = profitSum + lossSum - totalCommission - totalFundingFee;

                /*
                totalClosedBalance = Math.Round(Convert.ToDecimal(dataList.Where(x => x.EndTrade && x.CurrencyPair == currencyPair).Sum(x => x.AverageCloseBalance) - totalCommission - totalFundingFee), 2);

                totalEntryBalance = Math.Round(Convert.ToDecimal(dataList.Where(x => x.EndTrade && x.CurrencyPair == currencyPair).Sum(x => x.AverageEntryBalance)), 2);

                totalPnlPercent = totalEntryBalance != 0 ? Math.Round(Convert.ToDecimal((totalClosedBalance / totalEntryBalance - 1) * 100), 2) : 0;
                */

                inTradeBalance = Math.Round(Convert.ToDecimal(dataList.Where(x => x.EndTrade == false && x.CurrencyPair == currencyPair).Sum(x => x.AverageEntryBalance)) - Math.Round(Convert.ToDecimal(dataList.Where(x => x.EndTrade == false && x.CurrencyPair == currencyPair).Sum(x => x.AverageCloseBalance))), 2);

                currentCurrencyPairStatistic.InTradeBalance = inTradeBalance;
                currentCurrencyPairStatistic.TotalTradeCount = totalTradeCount;
                currentCurrencyPairStatistic.LossCount = lossTradeCount;
                currentCurrencyPairStatistic.WinCount = wonTradeCount;
                currentCurrencyPairStatistic.ProfitsSum = profitSum;
                currentCurrencyPairStatistic.LossesSum = lossSum;
                currentCurrencyPairStatistic.TotalPnL = totalProfitOrLoss;
                currentCurrencyPairStatistic.TradeWinRate = winRate;
                currentCurrencyPairStatistic.TotalCommission = totalCommission;
                currentCurrencyPairStatistic.TotalFundingFee = totalFundingFee;

                UpdateCurrencyPairStatisticByCurrencyPair(currentCurrencyPairStatistic, xmlFilePath);
            }
        }


    }
}
