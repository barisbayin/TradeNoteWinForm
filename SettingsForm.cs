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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            
        }

        public static string ListOfTradeXmls { get; set; }

        private readonly TradeModelManager _tradeModelManager = new TradeModelManager(new TradeModelXmlRepository());

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            GenerateCumulativeStatisticImage();
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
                g.DrawString("Example Text Example Text Example Text", font: new Font("Matura MT Script Capitals", 36), Brushes.LightGoldenrodYellow, new PointF(10, 100));

                g.Save();
            }

            using (Graphics g = Graphics.FromImage(image))
            {
                g.DrawEllipse(Pens.Red, 10, 10, 100, 50);
                g.FillRectangle(Brushes.Green, 120, 10, 100, 50);
            }
            pBoxCummulativeStatistics.Image=image;
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            // SaveFileDialog nesnesini kullanarak kullanıcının resmi kaydetmesini isteyin
            //using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            //{
            //    saveFileDialog.Filter = "JPEG dosyası (*.jpg)|*.jpg|PNG dosyası (*.png)|*.png|Tüm dosyalar (*.*)|*.*";
            //    saveFileDialog.FilterIndex = 1;

            //    if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //    {
            //        image.Save(saveFileDialog.FileName);
            //    }
            //}
        }
    }
}
