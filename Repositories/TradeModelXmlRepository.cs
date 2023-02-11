using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using TradeNote.Entities;
using TradeNote.Enums;
using TradeNote.Helpers;

namespace TradeNote.Repositories
{
    public class TradeModelXmlRepository
    {
        public void CreateEmptyXmlFile(string xmlFileName)
        {
            var tradeNotesDirectory = GeneralHelper.GetTradeNotesDirectory();
            var xmlFilePath = Path.Combine(tradeNotesDirectory, xmlFileName + ".xml");

            if (!File.Exists(tradeNotesDirectory))
            {
                TradeModel tradeModel = new TradeModel
                {
                    GeneralInformation = new GeneralInformation(),
                    CurrencyPairStatistics = new List<CurrencyPairStatistic>(),
                    Trades = new List<Trade>()
                };

                XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

                using (TextWriter writer = new StreamWriter(xmlFilePath))
                {
                    serializer.Serialize(writer, tradeModel);
                }
            }
            else
            {
                MessageBox.Show("Bu isimde dosya mevcut. Lütfen dosya adını değiştiriniz!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void DeleteXmlFileByPath(string xmlFilePath)
        {
            if (File.Exists(xmlFilePath))
            {
                string deletedFolderPath = Path.Combine(Application.StartupPath, "DeletedFiles");

                if (!Directory.Exists(deletedFolderPath))
                {
                    Directory.CreateDirectory(deletedFolderPath);
                }
                string deletedFileName = "Deleted_" + Path.GetFileNameWithoutExtension(xmlFilePath) + "_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm") + ".xml_XX";
                string deletedFilePath = Path.Combine(deletedFolderPath, deletedFileName);
                File.Move(xmlFilePath, deletedFilePath);
            }
        }

        public TradeModel GetTradeModel(string xmlFilePath)
        {
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);
            return tradeModel;
        }

        public List<Trade> GetAllTrades(string xmlFilePath)
        {
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);
            return tradeModel.Trades;
        }

        public void UpdateGeneralInformation(GeneralInformation updatedGeneralInformation, string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            // Update the properties of the GeneralInformation object
            tradeModel.GeneralInformation.Exchange = updatedGeneralInformation.Exchange;
            tradeModel.GeneralInformation.ReferralLink = updatedGeneralInformation.ReferralLink;
            tradeModel.GeneralInformation.ReferralId = updatedGeneralInformation.ReferralId;
            tradeModel.GeneralInformation.StartingBalance = updatedGeneralInformation.StartingBalance;
            tradeModel.GeneralInformation.LastBalance = updatedGeneralInformation.LastBalance;
            tradeModel.GeneralInformation.InTradeBalance = updatedGeneralInformation.InTradeBalance;
            tradeModel.GeneralInformation.AvailableBalance = updatedGeneralInformation.AvailableBalance;
            tradeModel.GeneralInformation.ProfitsSum = updatedGeneralInformation.ProfitsSum;
            tradeModel.GeneralInformation.LossesSum = updatedGeneralInformation.LossesSum;
            tradeModel.GeneralInformation.TotalPnL = updatedGeneralInformation.TotalPnL;
            tradeModel.GeneralInformation.TotalPnLPercent = updatedGeneralInformation.TotalPnLPercent;
            tradeModel.GeneralInformation.WinCount = updatedGeneralInformation.WinCount;
            tradeModel.GeneralInformation.LossCount = updatedGeneralInformation.LossCount;
            tradeModel.GeneralInformation.TradeWinRate = updatedGeneralInformation.TradeWinRate;
            tradeModel.GeneralInformation.MakerCommission = updatedGeneralInformation.MakerCommission;
            tradeModel.GeneralInformation.TakerCommission = updatedGeneralInformation.TakerCommission;
            tradeModel.GeneralInformation.TotalCommission = updatedGeneralInformation.TotalCommission;
            tradeModel.GeneralInformation.TotalFundingFee = updatedGeneralInformation.TotalFundingFee;

            // Serialize the updated TradeModel object back to the XML file
            XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

            using (TextWriter writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, tradeModel);
            }
        }

        public GeneralInformation GetGeneralInformation(string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            return tradeModel.GeneralInformation;
        }

        public List<CurrencyPairStatistic> GetCurrencyPairStatisticList(string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            // Return the list of Trade objects
            return tradeModel.CurrencyPairStatistics;
        }

        public CurrencyPairStatistic GetCurrencyPairStatisticByCurrencyPair(string currencyPair, string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            // Return the list of Trade objects
            return tradeModel.CurrencyPairStatistics.FirstOrDefault(x => x.CurrencyPair == currencyPair);
        }

        public void AddCurrencyPairStatistic(CurrencyPairStatistic currencyPairStatistic, string xmlFilePath)
        {
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            var alreadyAdded = tradeModel.CurrencyPairStatistics.FirstOrDefault(x => x.CurrencyPair == currencyPairStatistic.CurrencyPair);

            if (alreadyAdded == null)
            {
                tradeModel.CurrencyPairStatistics.Add(currencyPairStatistic);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

            using (TextWriter writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, tradeModel);
            }

        }

        public void UpdateCurrencyPairStatisticByCurrencyPair(CurrencyPairStatistic updatedCurrencyPairStatistic, string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            string currencyPair = updatedCurrencyPairStatistic.CurrencyPair;

            CurrencyPairStatistic currencyPairStatistic = tradeModel.CurrencyPairStatistics.FirstOrDefault(t => t.CurrencyPair == currencyPair);

            currencyPairStatistic.InTradeBalance = updatedCurrencyPairStatistic.InTradeBalance;
            currencyPairStatistic.ProfitsSum = updatedCurrencyPairStatistic.ProfitsSum;
            currencyPairStatistic.LossesSum = updatedCurrencyPairStatistic.ProfitsSum;
            currencyPairStatistic.TotalPnL = updatedCurrencyPairStatistic.InTradeBalance;
            currencyPairStatistic.TotalPnLPercent = updatedCurrencyPairStatistic.TotalPnLPercent;
            currencyPairStatistic.TotalTradeCount = updatedCurrencyPairStatistic.TotalTradeCount;
            currencyPairStatistic.WinCount = updatedCurrencyPairStatistic.WinCount;
            currencyPairStatistic.LossCount = updatedCurrencyPairStatistic.LossCount;
            currencyPairStatistic.TradeWinRate = updatedCurrencyPairStatistic.TradeWinRate;
            currencyPairStatistic.MakerCommission = updatedCurrencyPairStatistic.MakerCommission;
            currencyPairStatistic.TakerCommission = updatedCurrencyPairStatistic.TakerCommission;
            currencyPairStatistic.TotalCommission = updatedCurrencyPairStatistic.TotalCommission;
            currencyPairStatistic.TotalFundingFee = updatedCurrencyPairStatistic.TotalFundingFee;

            // Serialize the updated TradeModel object back to the XML file
            XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

            using (TextWriter writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, tradeModel);
            }
        }

        public void AddTrade(Trade trade, string xmlFilePath)
        {
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            // Add the Trade object to the Trades list of the TradeModel object
            tradeModel.Trades.Add(trade);

            // Serialize the updated TradeModel object back to the XML file
            XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

            using (TextWriter writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, tradeModel);
            }

        }

        public void UpdateTrade(Trade updatedTrade, string xmlFilePath)
        {

            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            // Find the Trade object with the specified Id
            int tradeId = updatedTrade.Id;
            Trade trade = tradeModel.Trades.FirstOrDefault(t => t.Id == tradeId);

            // Check if the Trade object was found
            if (trade != null)
            {
                // Update the properties of the Trade object
                trade.CurrencyPair = updatedTrade.CurrencyPair;
                trade.TradeStartDate = updatedTrade.TradeStartDate;
                trade.TradeEndDate = updatedTrade.TradeEndDate;
                trade.PositionSide = updatedTrade.PositionSide;
                trade.AverageEntryBalance = updatedTrade.AverageEntryBalance;
                trade.AverageEntryLotCount = updatedTrade.AverageEntryLotCount;
                trade.Leverage = updatedTrade.Leverage;
                trade.AverageEntryPrice = updatedTrade.AverageEntryPrice;
                trade.TargetedEntryPrice = updatedTrade.TargetedEntryPrice;
                trade.StopLossPrice = updatedTrade.StopLossPrice;
                trade.TakeProfitPrice = updatedTrade.TakeProfitPrice;
                trade.AveragePositionClosePrice = updatedTrade.AveragePositionClosePrice;
                trade.RiskPercent = updatedTrade.RiskPercent;
                trade.RewardPercent = updatedTrade.RewardPercent;
                trade.RiskRewardRatio = updatedTrade.RiskRewardRatio;
                trade.ExpectedRiskValue = updatedTrade.ExpectedRiskValue;
                trade.ExpectedRewardValue = updatedTrade.ExpectedRewardValue;
                trade.AverageCloseBalance = updatedTrade.AverageCloseBalance;
                trade.AverageCloseLotCount = updatedTrade.AverageCloseLotCount;
                trade.PositionResult = updatedTrade.PositionResult;
                trade.ProfitOrLoss = updatedTrade.ProfitOrLoss;
                trade.ProfitOrLossPercent = updatedTrade.ProfitOrLossPercent;
                trade.CommissionSum = updatedTrade.CommissionSum;
                trade.FundingFeeSum = updatedTrade.FundingFeeSum;
                trade.EndTrade = updatedTrade.EndTrade;
                trade.Note = updatedTrade.Note;
            }

            // Serialize the updated TradeModel object back to the XML file
            XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

            using (TextWriter writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, tradeModel);
            }
        }

        public List<Trade> GetTrades(string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            // Return the list of Trade objects
            return tradeModel.Trades;
        }

        public Trade GetTradeById(int id, string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            Trade trade = tradeModel.Trades.FirstOrDefault(t => t.Id == id);

            return trade;
        }

        public void RemoveTradeById(int id, string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            Trade trade = tradeModel.Trades.FirstOrDefault(t => t.Id == id);

            tradeModel.Trades.Remove(trade);

            XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

            using (TextWriter writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, tradeModel);
            }
        }

        public void RemoveTradeDetailById(int tradeId, int tradeDetailId, string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            var tradeDetail = tradeModel.Trades.FirstOrDefault(t => t.Id == tradeId)?.TradeDetails.FirstOrDefault(td => td.Id == tradeDetailId);

            tradeModel.Trades.FirstOrDefault(t => t.Id == tradeId)?.TradeDetails.Remove(tradeDetail);

            XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

            using (TextWriter writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, tradeModel);
            }
        }


        public List<Trade> GetTradesWithSpecificColumns(string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            // Return the list of Trade objects, including their TradeDetails lists
            return tradeModel.Trades.Select(t => new Trade
            {
                Id = t.Id,
                TradeStartDate = t.TradeStartDate,
                PositionSide = t.PositionSide,
                AverageEntryBalance = t.AverageEntryBalance,
                Leverage = t.Leverage,
                AverageEntryPrice = t.AverageEntryPrice,
                StopLossPrice = t.StopLossPrice,
                TakeProfitPrice = t.TakeProfitPrice,
                AveragePositionClosePrice = t.AveragePositionClosePrice,
                RiskPercent = t.RiskPercent,
                RewardPercent = t.RewardPercent,
                RiskRewardRatio = t.RiskRewardRatio,
                ExpectedRiskValue = t.ExpectedRiskValue,
                ExpectedRewardValue = t.ExpectedRewardValue,
                PositionResult = t.PositionResult,
                ProfitOrLoss = t.ProfitOrLoss,
                ProfitOrLossPercent = t.ProfitOrLossPercent,
                Note = t.Note,
                TradeDetails = t.TradeDetails
            }).ToList();
        }

        public void AddTradeDetail(int tradeId, TradeDetail tradeDetail, string xmlFilePath)
        {
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            // Find the Trade object with the specified Id
            Trade trade = tradeModel.Trades.FirstOrDefault(t => t.Id == tradeId);

            // Check if the Trade object was found
            if (trade != null)
            {
                // Add the TradeDetail object to the TradeDetails list of the Trade object
                trade.TradeDetails.Add(tradeDetail);
            }

            // Serialize the updated TradeModel object back to the XML file
            XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

            using (TextWriter writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, tradeModel);
            }

        }

        public void UpdateTradeDetail(int tradeId, TradeDetail updatedTradeDetail, string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            // Find the Trade object with the specified Id
            Trade trade = tradeModel.Trades.FirstOrDefault(t => t.Id == tradeId);

            // Check if the Trade object was found
            if (trade != null)
            {
                // Find the TradeDetail object with the specified Id
                int tradeDetailId = updatedTradeDetail.Id;
                TradeDetail tradeDetail = trade.TradeDetails.FirstOrDefault(td => td.Id == tradeDetailId);

                // Check if the TradeDetail object was found
                if (tradeDetail != null)
                {
                    // Update the properties of the TradeDetail object
                    tradeDetail.TradeType = updatedTradeDetail.TradeType;
                    tradeDetail.EntryBalance = updatedTradeDetail.EntryBalance;
                    tradeDetail.EntryPrice = updatedTradeDetail.EntryPrice;
                    tradeDetail.EntryLotCount = updatedTradeDetail.EntryLotCount;
                    tradeDetail.OrderType = updatedTradeDetail.OrderType;
                    tradeDetail.TradeDate = updatedTradeDetail.TradeDate;
                }
            }
            // Serialize the updated TradeModel object back to the XML file
            XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

            using (TextWriter writer = new StreamWriter(xmlFilePath))
            {
                serializer.Serialize(writer, tradeModel);
            }
        }
        public TradeDetail GetTradeDetailById(int tradeId, int tradeDetailId, string xmlFilePath)
        {

            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            var tradeDetail = tradeModel.Trades.FirstOrDefault(t => t.Id == tradeId)?.TradeDetails.FirstOrDefault(td => td.Id == tradeDetailId);

            if (tradeDetail == null)
            {
                return null;
            }

            return tradeDetail;
        }
        public List<TradeDetail> GetTradeDetailList(int tradeId, string xmlFilePath)
        {
            // Deserialize the XML file to a TradeModel object
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            // Find the Trade object with the specified Id
            Trade trade = tradeModel.Trades.FirstOrDefault(t => t.Id == tradeId);

            // Check if the Trade object was found
            if (trade == null)
            {
                return null;
            }

            // Return the TradeDetails list of the Trade object
            return trade.TradeDetails;
        }

        public int GetNewTradeId(string xmlFilePath)
        {
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            if (tradeModel.Trades.Count == 0)
            {
                return 1;
            }

            // Find the highest existing Trade Id
            int newTradeId = tradeModel.Trades.Max(t => t.Id) + 1;

            return newTradeId;
        }

        public int GetNewTradeDetailId(int tradeId, string xmlFilePath)
        {
            TradeModel tradeModel = DeserializeTradeModel(xmlFilePath);

            Trade trade = tradeModel.Trades.FirstOrDefault(t => t.Id == tradeId);

            if (trade.TradeDetails.Count == 0)
            {
                return 1;
            }
            // Find the highest existing Trade Id
            int newTradeDetailId = trade.TradeDetails.Max(t => t.Id) + 1;

            return newTradeDetailId;
        }


        private static TradeModel DeserializeTradeModel(string xmlFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TradeModel));

            using (TextReader reader = new StreamReader(xmlFilePath))
            {
                return (TradeModel)serializer.Deserialize(reader);
            }
        }
    }
}

