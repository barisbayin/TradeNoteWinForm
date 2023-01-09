using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using TradeNote.Helpers;

namespace TradeNote
{
    public class TradeModelRepository
    {

        public void CreateEmptyXmlFile(string xmlFileName)
        {
            var tradeNotesDirectory = GeneralHelper.GetTradeNotesDirectory();
            var xmlFilePath = Path.Combine(tradeNotesDirectory, xmlFileName + ".xml");

            if (!File.Exists(tradeNotesDirectory))
            {
                var xml = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XElement("TradeModel"));
                xml.Save(xmlFilePath);

                var generalInformation = new GeneralInformation()
                {
                    StartingBalance = 0,
                    LastBalance = 0,
                    ProfitsSum = 0,
                    LossesSum = 0,
                    TotalPnL = 0,
                    TotalPnLPercent = 0,
                    WinCount = 0,
                    LossCount = 0,
                    TradeWinRate = 0
                };

                var xmlDoc = XDocument.Load(xmlFilePath);

                // TradeModel öğesini XML dosyasına ekle
                var element = new XElement("GeneralInformation",
                    new XElement("StaringBalance", generalInformation.StartingBalance),
                    new XElement("LastBalance", generalInformation.LastBalance),
                    new XElement("ProfitsSum", generalInformation.ProfitsSum),
                    new XElement("LossesSum", generalInformation.LossesSum),
                    new XElement("TotalPnL", generalInformation.TotalPnL),
                    new XElement("TotalPnLPercent", generalInformation.TotalPnLPercent),
                    new XElement("WinCount", generalInformation.WinCount),
                    new XElement("LossCount", generalInformation.LossCount),
                    new XElement("TradeWinRate", generalInformation.TradeWinRate)
                );

                xmlDoc.Root?.Add(element);
                xmlDoc.Save(xmlFilePath);

                MessageBox.Show(xmlFileName + " listesi oluşturuldu!", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Bu isimde dosya mevcut. Lütfen dosya adını değiştiriniz!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        public void AddTrade(Trade trade, string xmlFilePath)
        {
            // XML dosyasını okuma
            XDocument doc = XDocument.Load(xmlFilePath);

            // Son Id değerini bulma
            int lastId = doc.Root.Elements("Trade")
                .Select(x => int.Parse(x.Element("Id").Value))
                .OrderByDescending(x => x)
                .FirstOrDefault();

            // Id değerini ayarla
            int newId = 1;

            if (lastId > 0)
            {
                // Id değerini bir artırma
                newId = lastId + 1;
            }

            // Yeni bir <Trade> elementi oluşturma
            XElement element = new XElement("Trade",
                new XElement("Id", newId),
                new XElement("TradeDate", trade.TradeStartDate),
                new XElement("PositionSide", trade.PositionSide),
                new XElement("EntryBalance", trade.AverageEntryBalance),
                new XElement("Leverage", trade.Leverage),
                new XElement("EntryPrice", trade.AverageEntryPrice),
                new XElement("StopLossPrice", trade.StopLossPrice),
                new XElement("TakeProfitPrice", trade.TakeProfitPrice),
                new XElement("PositionClosePrice", trade.AveragePositionClosePrice),
                new XElement("RiskPercent", trade.RiskPercent),
                new XElement("RewardPercent", trade.RewardPercent),
                new XElement("RiskRewardRatio", trade.RiskRewardRatio),
                new XElement("ExpectedRiskValue", trade.ExpectedRiskValue),
                new XElement("ExpectedRewardValue", trade.ExpectedRewardValue),
                new XElement("PositionResult", trade.PositionResult),
                new XElement("ProfitOrLoss", trade.ProfitOrLoss),
                new XElement("ProfitOrLossPercent", trade.ProfitOrLossPercent),
                new XElement("Note", trade.Note)
            );

            // Yeni elementi dosyaya ekleme
            doc.Root.Add(element);

            // Değişiklikleri kaydetme
            doc.Save(xmlFilePath);
        }

        public void UpdateTrade(Trade trade, string xmlFilePath)
        {
            // XML dosyasını okuma
            XDocument doc = XDocument.Load(xmlFilePath);

            // İlgili <trade> elementini bulma
            XElement element = doc.Root.Elements("Trade").FirstOrDefault(x => x.Element("Id").Value == trade.Id.ToString());
            if (element == null)
            {
                return;
            }

            // <trade> elementini güncelleme
            element.SetElementValue("Id", trade.Id);
            element.SetElementValue("TradeDate", trade.TradeStartDate);
            element.SetElementValue("PositionSide", trade.PositionSide);
            element.SetElementValue("EntryBalance", trade.AverageEntryBalance);
            element.SetElementValue("Leverage", trade.Leverage);
            element.SetElementValue("EntryPrice", trade.AverageEntryPrice);
            element.SetElementValue("StopLossPrice", trade.StopLossPrice);
            element.SetElementValue("TakeProfitPrice", trade.TakeProfitPrice);
            element.SetElementValue("PositionClosePrice", trade.AveragePositionClosePrice);
            element.SetElementValue("RiskPercent", trade.RiskPercent);
            element.SetElementValue("RewardPercent", trade.RewardPercent);
            element.SetElementValue("RiskRewardRatio", trade.RiskRewardRatio);
            element.SetElementValue("ExpectedRiskValue", trade.ExpectedRiskValue);
            element.SetElementValue("ExpectedRewardValue", trade.ExpectedRewardValue);
            element.SetElementValue("PositionResult", trade.PositionResult);
            element.SetElementValue("ProfitOrLoss", trade.ProfitOrLoss);
            element.SetElementValue("ProfitOrLossPercent", trade.ProfitOrLossPercent);
            element.SetElementValue("Note", trade.Note);

            // Değişiklikleri kaydetme
            doc.Save(xmlFilePath);
        }

        public void UpdateGeneralInformation(GeneralInformation generalInformation, string xmlFilePath)
        {
            try
            {
                var xmlDoc = XDocument.Load(xmlFilePath);

                var generalInformationElement = xmlDoc.Root
                    .Elements("GeneralInformation")
                    .FirstOrDefault();

                if (generalInformationElement != null)
                {
                    generalInformationElement.SetElementValue("StartingBalance", generalInformation.StartingBalance);
                    generalInformationElement.SetElementValue("LastBalance", generalInformation.LastBalance);
                    generalInformationElement.SetElementValue("ProfitsSum", generalInformation.ProfitsSum);
                    generalInformationElement.SetElementValue("LossesSum", generalInformation.LossesSum);
                    generalInformationElement.SetElementValue("TotalPnL", generalInformation.TotalPnL);
                    generalInformationElement.SetElementValue("TotalPnLPercent", generalInformation.TotalPnLPercent);
                    generalInformationElement.SetElementValue("WinCount", generalInformation.WinCount);
                    generalInformationElement.SetElementValue("LossCount", generalInformation.LossCount);
                    generalInformationElement.SetElementValue("TradeWinRate", generalInformation.TradeWinRate);
                }

                xmlDoc.Save(xmlFilePath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public GeneralInformation ReadGeneralInformation(string xmlFilePath)
        {
            // XML dosyasını oku
            var xmlDoc = XDocument.Load(xmlFilePath);

            var generalInformation = new GeneralInformation();
            // Tüm TradeModel öğelerini oku
            var generalInformationElement = xmlDoc.Root.Elements("GeneralInformation").FirstOrDefault();
            
                generalInformation = new GeneralInformation
                {
                    StartingBalance = (decimal)generalInformationElement.Element("StartingBalance"),
                    LastBalance = (decimal)generalInformationElement.Element("LastBalance"),
                    ProfitsSum = (decimal)generalInformationElement.Element("ProfitsSum"),
                    LossesSum = (decimal)generalInformationElement.Element("LossesSum"),
                    TotalPnL = (decimal)generalInformationElement.Element("TotalPnL"),
                    TotalPnLPercent = (decimal)generalInformationElement.Element("TotalPnLPercent"),
                    WinCount = (int)generalInformationElement.Element("WinCount"),
                    LossCount = (int)generalInformationElement.Element("LossCount"),
                    TradeWinRate = (decimal)generalInformationElement.Element("TradeWinRate")
                };
            
            return generalInformation;
        }

        public List<Trade> ReadTrades(string xmlFilePath)
        {
            // XML dosyasını okuma
            XDocument doc = XDocument.Load(xmlFilePath);

            // <trade> elementlerini seçme ve Trade nesnelerine dönüştürme
            return doc.Root.Elements("Trade").Select(x => new Trade
            {
                Id = int.Parse(x.Element("Id")?.Value),
                TradeStartDate = DateTime.Parse(x.Element("TradeDate").Value),
                PositionSide = (PositionSide)Enum.Parse(typeof(PositionSide), x.Element("PositionSide").Value),
                AverageEntryBalance = decimal.Parse(x.Element("EntryBalance").Value, CultureInfo.InvariantCulture),
                Leverage = int.Parse(x.Element("Leverage").Value),
                AverageEntryPrice = decimal.Parse(x.Element("EntryPrice").Value, CultureInfo.InvariantCulture),
                StopLossPrice = decimal.Parse(x.Element("StopLossPrice").Value, CultureInfo.InvariantCulture),
                TakeProfitPrice = decimal.Parse(x.Element("TakeProfitPrice").Value, CultureInfo.InvariantCulture),
                AveragePositionClosePrice = decimal.Parse(x.Element("PositionClosePrice").Value, CultureInfo.InvariantCulture),
                RiskPercent = decimal.Parse(x.Element("RiskPercent").Value, CultureInfo.InvariantCulture),
                RewardPercent = decimal.Parse(x.Element("RewardPercent").Value, CultureInfo.InvariantCulture),
                RiskRewardRatio = decimal.Parse(x.Element("RiskRewardRatio").Value, CultureInfo.InvariantCulture),
                ExpectedRiskValue = decimal.Parse(x.Element("ExpectedRiskValue").Value, CultureInfo.InvariantCulture),
                ExpectedRewardValue = decimal.Parse(x.Element("ExpectedRewardValue").Value, CultureInfo.InvariantCulture),
                PositionResult = (PositionResult)Enum.Parse(typeof(PositionResult), x.Element("PositionResult").Value),
                ProfitOrLoss = decimal.Parse(x.Element("ProfitOrLoss").Value, CultureInfo.InvariantCulture),
                ProfitOrLossPercent = decimal.Parse(x.Element("ProfitOrLossPercent").Value, CultureInfo.InvariantCulture),
                Note = x.Element("Note").Value
            }).ToList();
        }

        public List<TradeModel> ReadTradeModel(string xmlFilePath)
        {
            var tradeModels = new List<TradeModel>();

            // XML dosyasını oku
            var xmlDoc = XDocument.Load(xmlFilePath);

            // Tüm TradeModel öğelerini oku
            var elements = xmlDoc.Root.Elements("TradeModel");
            foreach (var element in elements)
            {
                var generalInformationElement = element.Element("GeneralInformation");
                var tradesElements = element.Element("Trades").Elements("Trade");

                var generalInformation = new GeneralInformation
                {
                    StartingBalance = (decimal)generalInformationElement.Element("StaringBalance"),
                    LastBalance = (decimal)generalInformationElement.Element("LastBalance"),
                    ProfitsSum = (decimal)generalInformationElement.Element("ProfitsSum"),
                    LossesSum = (decimal)generalInformationElement.Element("LossesSum"),
                    TotalPnL = (decimal)generalInformationElement.Element("TotalPnL"),
                    TotalPnLPercent = (decimal)generalInformationElement.Element("TotalPnLPercent"),
                    WinCount = (int)generalInformationElement.Element("WinCount"),
                    LossCount = (int)generalInformationElement.Element("LossCount"),
                    TradeWinRate = (decimal)generalInformationElement.Element("TradeWinRate")
                };

                var trades = new List<Trade>();

                foreach (var x in tradesElements)
                {
                    var trade = new Trade
                    {
                        Id = int.Parse(x.Element("Id")?.Value),
                        TradeStartDate = DateTime.Parse(x.Element("TradeDate").Value),
                        PositionSide = (PositionSide)Enum.Parse(typeof(PositionSide), x.Element("PositionSide").Value),
                        AverageEntryBalance  = decimal.Parse(x.Element("EntryBalance").Value, CultureInfo.InvariantCulture),
                        Leverage = int.Parse(x.Element("Leverage").Value),
                        AverageEntryPrice = decimal.Parse(x.Element("EntryPrice").Value, CultureInfo.InvariantCulture),
                        StopLossPrice = decimal.Parse(x.Element("StopLossPrice").Value, CultureInfo.InvariantCulture),
                        TakeProfitPrice = decimal.Parse(x.Element("TakeProfitPrice").Value, CultureInfo.InvariantCulture),
                        AveragePositionClosePrice = decimal.Parse(x.Element("PositionClosePrice").Value, CultureInfo.InvariantCulture),
                        RiskPercent = decimal.Parse(x.Element("RiskPercent").Value, CultureInfo.InvariantCulture),
                        RewardPercent = decimal.Parse(x.Element("RewardPercent").Value, CultureInfo.InvariantCulture),
                        RiskRewardRatio = decimal.Parse(x.Element("RiskRewardRatio").Value, CultureInfo.InvariantCulture),
                        ExpectedRiskValue = decimal.Parse(x.Element("ExpectedRiskValue").Value, CultureInfo.InvariantCulture),
                        ExpectedRewardValue = decimal.Parse(x.Element("ExpectedRewardValue").Value, CultureInfo.InvariantCulture),
                        PositionResult = (PositionResult)Enum.Parse(typeof(PositionResult), x.Element("PositionResult").Value),
                        ProfitOrLoss = decimal.Parse(x.Element("ProfitOrLoss").Value, CultureInfo.InvariantCulture),
                        ProfitOrLossPercent = decimal.Parse(x.Element("ProfitOrLossPercent").Value, CultureInfo.InvariantCulture),
                        Note = x.Element("Note").Value
                    };
                    trades.Add(trade);
                }

                var tradeModel = new TradeModel
                {
                    GeneralInformation = generalInformation,
                    Trades = trades
                };
                tradeModels.Add(tradeModel);
            }

            return tradeModels;
        }

    }
}
