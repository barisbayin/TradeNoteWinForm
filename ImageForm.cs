using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradeNote.Business;
using TradeNote.Helpers;
using TradeNote.Repositories;

namespace TradeNote
{
    public partial class ImageForm : Form
    {
        public ImageForm()
        {
            InitializeComponent();

        }

        public static string ListOfTradeXmls { get; set; }
        public static int SelectedTradeId { get; set; }
        public static string Exchange { get; set; }
        public static string CurrencyPair { get; set; }
        public static string ReferralLink { get; set; }
        public static string ReferralId { get; set; }
        public static Image BaseImage { get; set; }



        private readonly TradeModelManager _tradeModelManager = new TradeModelManager(new TradeModelXmlRepository());

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            GenerateTradeStatisticImage();
        }

        private void GenerateCumulativeStatisticImage()
        {
            if (string.IsNullOrEmpty(ListOfTradeXmls))
            {
                MessageBox.Show("Lütfen trade listesini seçiniz!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var xmlFilePath = GeneralHelper.GetXmlFilePath(ListOfTradeXmls);

            var generalInformation = _tradeModelManager.GetGeneralInformation(xmlFilePath);


            Bitmap image = new Bitmap(pBoxCummulativeStatistics.Image);


            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawString("Example Text Example Text Example Text", font: new Font("28 Days Later", 36),
                    Brushes.DodgerBlue, new PointF(10, 100));

                g.Save();
            }


            pBoxCummulativeStatistics.Image = image;
        }


        private void GenerateTradeStatisticImage()
        {
            decimal averageEntryPrice = 0;
            decimal averageClosePrice = 0;

            if (string.IsNullOrEmpty(ListOfTradeXmls) && string.IsNullOrEmpty(SelectedTradeId.ToString())) return;

            var xmlFilePath = GeneralHelper.GetXmlFilePath(ListOfTradeXmls);

            var selectedTrade = _tradeModelManager.GetTradeById(SelectedTradeId, xmlFilePath);

            CurrencyPair = selectedTrade.CurrencyPair;

            if (selectedTrade.AverageEntryPrice < 1)
            {
                averageEntryPrice = Math.Round(selectedTrade.AverageEntryPrice, 5);
                averageClosePrice = Math.Round(selectedTrade.AveragePositionClosePrice, 5);
            }
            else if (selectedTrade.AverageEntryPrice >= 1 && selectedTrade.AverageEntryPrice < 100)
            {
                averageEntryPrice = Math.Round(selectedTrade.AverageEntryPrice, 3);
                averageClosePrice = Math.Round(selectedTrade.AveragePositionClosePrice, 3);
            }
            else if (selectedTrade.AverageEntryPrice >= 100)
            {
                averageEntryPrice = Math.Round(selectedTrade.AverageEntryPrice, 2);
                averageClosePrice = Math.Round(selectedTrade.AveragePositionClosePrice, 2);
            }


            Bitmap image = new Bitmap(pBoxCummulativeStatistics.Image);

            var referralLinkQrCode = GeneralHelper.GenerateQrCodeImageByGivenString(ReferralLink);

            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawString(CurrencyPair, font: new Font("Hell Finland", 24), Brushes.Black,
                    new PointF(200, 60));

                switch (selectedTrade.PositionSide)
                {
                    case PositionSide.Long:
                        g.DrawString(selectedTrade.PositionSide.ToString() + "  |  " + selectedTrade.Leverage + "X", font: new Font("Hell Finland", 30),
                            Brushes.ForestGreen, new PointF(200, 100));
                        break;
                    case PositionSide.Short:
                        g.DrawString(selectedTrade.PositionSide.ToString() + "  |  " + selectedTrade.Leverage + "X", font: new Font("Hell Finland", 30),
                            Brushes.IndianRed, new PointF(200, 100));
                        break;
                }

                //g.DrawString(selectedTrade.Leverage + "X", font: new Font("Hell Finland", 30), Brushes.Black,
                //    new PointF(350, 100));

                switch (selectedTrade.PositionResult)
                {
                    case PositionResult.TP:
                        g.DrawString("%" + Math.Round(selectedTrade.ProfitOrLossPercent, 2),
                            font: new Font("Hell Finland", 48), Brushes.Green, new PointF(200, 140));
                        break;
                    case PositionResult.SL:
                        g.DrawString("%" + Math.Round(selectedTrade.ProfitOrLossPercent, 2),
                            font: new Font("Hell Finland", 48), Brushes.Red, new PointF(200, 140));
                        break;
                }

                g.DrawString("Entry Price:  " + averageEntryPrice, font: new Font("Hell Finland", 16), Brushes.Black,
                    new PointF(200, 210));

                g.DrawString("Close Price:  " + averageClosePrice, font: new Font("Hell Finland", 16), Brushes.Black,
                    new PointF(200, 235));

                g.DrawString("Start Date: " + selectedTrade.TradeStartDate.ToString("dd/MM/yyyy HH:mm") + "  |  " + "End Date: " + Convert.ToDateTime(selectedTrade.TradeEndDate).ToString("dd/MM/yyyy HH:mm"), font: new Font("Hell Finland", 12), Brushes.DarkGray,
                    new PointF(100, 275));

                g.DrawImage(referralLinkQrCode, new PointF(40, 350));


                g.DrawString(Exchange, font: new Font("Hell Finland", 18), Brushes.Black, new PointF(135, 365));


                g.DrawString(ReferralId, font: new Font("Hell Finland", 18), Brushes.Black, new PointF(135, 400));


            }

            pBoxCummulativeStatistics.Image = image;
            BaseImage = image;


        }


        private void btnSaveImage_Click(object sender, EventArgs e)
        {

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "JPEG dosyası (*.jpg)|*.jpg|PNG dosyası (*.png)|*.png|Tüm dosyalar (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    BaseImage.Save(saveFileDialog.FileName);
                }
            }
        }

    }
}
