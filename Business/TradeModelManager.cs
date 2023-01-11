using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeNote.Entities;
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
    }
}
