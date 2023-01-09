using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace TradeNote
{
    public class XmlRepository
    {
        public static void CreateXmlFile(string xmlFileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string subDirectory = Path.Combine(currentDirectory, "Trades");

            // Trades klasörünün olup olmadığını kontrol edin
            if (!Directory.Exists(subDirectory))
            {
                // Trades klasörünü oluşturun
                Directory.CreateDirectory(subDirectory);
            }



            // Trades klasörü içine xml dosyasını oluşturun
            string xmlFilePath = Path.Combine(subDirectory, xmlFileName + ".xml");



            if (!File.Exists(xmlFilePath))
            {
                // XML dosyasının içeriğini oluşturun
                const string xmlContent = "<Trades></Trades>";

                // XML dosyasını oluşturun ve içeriğini yazın
                File.WriteAllText(xmlFilePath, xmlContent);
                MessageBox.Show(xmlFileName+" listesi oluşturuldu!", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Bu isimde dosya mevcut. Lütfen dosya adını değiştiriniz!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void Add(Trade trade, string xmlFilePath)
        {
            // XML dosyasını okuma
            XDocument doc = XDocument.Load(xmlFilePath);

            if (doc.Root.Elements("Trade").Any(x => x.Element("Id").Value == trade.Id.ToString()))
            {
                MessageBox.Show("Id: " + trade.Id + " için zaten bir kayıt mevcut!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
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
                    new XElement("Note", trade.Note)
                );

                // <Trade> elementini <people> elementinin altına ekleme
                doc.Root.Add(element);

                // Değişiklikleri kaydetme
                doc.Save(xmlFilePath);
            }


        }

        public static List<Trade> ReadTrades(string xmlFilePath)
        {
            // XML dosyasını okuma
            XDocument doc = XDocument.Load(xmlFilePath);

            // <trade> elementlerini seçme ve Trade nesnelerine dönüştürme
            return doc.Root.Elements("Trade").Select(x => new Trade
            {
                Id = int.Parse(x.Element("Id")?.Value),
                TradeStartDate = DateTime.Parse(x.Element("TradeStartDate").Value),
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
                ExpectedRiskValue = decimal.Parse(x.Element("ExpectedRiskValue").Value,CultureInfo.InvariantCulture),
                ExpectedRewardValue = decimal.Parse(x.Element("ExpectedRewardValue").Value, CultureInfo.InvariantCulture),
                PositionResult = (PositionResult)Enum.Parse(typeof(PositionResult), x.Element("PositionResult").Value),
                ProfitOrLoss = decimal.Parse(x.Element("ProfitOrLoss").Value, CultureInfo.InvariantCulture),
                Note = x.Element("Note").Value
            }).ToList();
        }

        public static void Update(Trade trade, string xmlFilePath)
        {
            // XML dosyasını okuma
            XDocument doc = XDocument.Load(xmlFilePath);

            // İlgili <trade> elementini bulma
            XElement element = doc.Root.Elements("trade").FirstOrDefault(x => x.Element("Id").Value == trade.Id.ToString());
            if (element == null)
            {
                return;
            }

            // <trade> elementini güncelleme
            element.SetElementValue("Id", trade.Id);
            element.SetElementValue("TradeStartDate", trade.TradeStartDate);
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
            element.SetElementValue("Note", trade.Note);

            doc.Save(xmlFilePath);
        }

        public static void Delete(int id, string xmlFilePath)
        {
            XDocument doc = XDocument.Load(xmlFilePath);

            // İlgili <trade> elementini bulma ve silme
            XElement element = doc.Root.Elements("trade").FirstOrDefault(x => x.Element("Id").Value == id.ToString());
            if (element != null)
            {
                element.Remove();
            }

            // Değişiklikleri kaydetme
            doc.Save(xmlFilePath);
        }

        public static void DeleteXmlFile(string fileName)
        {
            string xmlFolderPath = Path.Combine(Application.StartupPath, "Trades");
            string xmlFilePath = Path.Combine(xmlFolderPath, fileName+".xml");
            if (File.Exists(xmlFilePath))
            {
                string deletedFolderPath = Path.Combine(Application.StartupPath, "Deleted");
                if (!Directory.Exists(deletedFolderPath))
                {
                    Directory.CreateDirectory(deletedFolderPath);
                }
                string deletedFileName = "Deleted_" + Path.GetFileNameWithoutExtension(fileName)+"_"+ DateTime.Now.ToString("dd-MM-yyyy-HH-mm")+ ".xml_XX";
                string deletedFilePath = Path.Combine(deletedFolderPath, deletedFileName);
                File.Move(xmlFilePath, deletedFilePath);
            }
        }
    }
}
