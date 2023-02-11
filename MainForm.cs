using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using TradeNote.Business;
using TradeNote.Entities;
using TradeNote.Enums;
using TradeNote.Helpers;
using TradeNote.Properties;
using TradeNote.Repositories;
using ZXing;
using Excel = Microsoft.Office.Interop.Excel;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace TradeNote
{
    public partial class TradeList : Form
    {
        public TradeList()
        {
            InitializeComponent();
        }


        private readonly TradeModelManager _tradeModelManager = new TradeModelManager(new TradeModelXmlRepository());

        private int tradeClickedColumnIndex;
        private int tradeClickedRowIndex;
        private int tradeDetailClickedColumnIndex;
        private int tradeDetailClickedRowIndex;

        public void TradeList_Load(object sender, EventArgs e)
        {
            GetWithXmlFilesIntoComboBox();
            LoadTradeCheckedListBoxCheckStates();
        }



        public void GetWithXmlFilesIntoComboBox()
        {
            cbxListOfTradeXmls.Items.Clear();

            string currentDirectory = Directory.GetCurrentDirectory();
            string subDirectory = Path.Combine(currentDirectory, "TradeNotes");

            if (Directory.Exists(subDirectory))
            {
                string[] xmlFiles = Directory.GetFiles(subDirectory, "*.xml");

                List<string> xmlFileList = xmlFiles.Select(Path.GetFileNameWithoutExtension).ToList();

                cbxListOfTradeXmls.Items.AddRange(xmlFileList.OrderBy(x => x).ToArray());
            }
            else
            {
                Directory.CreateDirectory(subDirectory);
            }
        }


        private void btnNewTradeXml_Click_1(object sender, EventArgs e)
        {
            tbxNewTradeXmlName.Enabled = true;
            btnNewTradeXml.Enabled = false;
            btnCancelToSaveTradeXml.Enabled = true;
            btnSaveTradeXml.Enabled = true;
        }

        private void btnCancelToSaveTradeXml_Click(object sender, EventArgs e)
        {
            tbxNewTradeXmlName.Enabled = false;
            btnNewTradeXml.Enabled = true;
            btnCancelToSaveTradeXml.Enabled = false;
            btnSaveTradeXml.Enabled = false;
        }

        private void btnSaveTradeXml_Click_1(object sender, EventArgs e)
        {
            _tradeModelManager.CreateEmptyXmlFile(tbxNewTradeXmlName.Text);

            MessageBox.Show(tbxNewTradeXmlName.Text + " listesi oluşturuldu. \n Trade işlemlerine başlamadan önce lütfen genel ayarlarınız yapınız!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            tbxNewTradeXmlName.Text = "";
            tbxNewTradeXmlName.Enabled = false;
            btnNewTradeXml.Enabled = true;
            btnCancelToSaveTradeXml.Enabled = false;
            btnSaveTradeXml.Enabled = false;

            GetWithXmlFilesIntoComboBox();
        }

        private void LoadTradeDataGridView()
        {

            var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);
            try
            {
                if (cbxListOfTradeXmls.Text != null)
                {
                    gbForList.Text = cbxListOfTradeXmls.Text;
                    var dataList = _tradeModelManager.GetTradeList(xmlFilePath);

                    List<Trade> tradeList = new List<Trade>(dataList);

                    dgvTradeList.DataSource = tradeList;
                    dgvTradeList.ReadOnly = true;

                    var columnSettings = GetColumnSettings();

                    foreach (var columnSetting in columnSettings)
                    {
                        if (columnSetting.Key == "Id")
                        {
                            dgvTradeList.Columns["Id"].Visible = columnSetting.Value;
                        }

                        if (columnSetting.Key == "İşlem Çifti")
                        {
                            dgvTradeList.Columns["CurrencyPair"].Visible = columnSetting.Value;
                        }

                        if (columnSetting.Key == "Trade Başlangıç Tarihi")
                        {
                            dgvTradeList.Columns["TradeStartDate"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Trade Bitiş Tarihi")
                        {
                            dgvTradeList.Columns["TradeEndDate"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Posizyon Yönü")
                        {
                            dgvTradeList.Columns["PositionSide"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Ortalama Giriş Miktarı")
                        {
                            dgvTradeList.Columns["AverageEntryBalance"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Ortalama Giriş Adedi")
                        {
                            dgvTradeList.Columns["AverageEntryLotCount"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Kaldıraç")
                        {
                            dgvTradeList.Columns["Leverage"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Hedeflenen Giriş Fiyatı")
                        {
                            dgvTradeList.Columns["TargetedEntryPrice"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "StopLoss Fiyatı")
                        {
                            dgvTradeList.Columns["StopLossPrice"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Take Profit Fiyatı")
                        {
                            dgvTradeList.Columns["TakeProfitPrice"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Ortalama Giriş Fiyatı")
                        {
                            dgvTradeList.Columns["AverageEntryPrice"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Ortalama Pozisyon Kapama Fiyatı")
                        {
                            dgvTradeList.Columns["AveragePositionClosePrice"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Risk Yüzdesi")
                        {
                            dgvTradeList.Columns["RiskPercent"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Kazanç Yüzdesi")
                        {
                            dgvTradeList.Columns["RewardPercent"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Risk/Kazanç Yüzdesi")
                        {
                            dgvTradeList.Columns["RiskRewardRatio"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Tahmini Risk Miktarı")
                        {
                            dgvTradeList.Columns["ExpectedRiskValue"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Tahmini Kazanç Miktarı")
                        {
                            dgvTradeList.Columns["ExpectedRewardValue"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Ortalama Posizyon Kapama Miktarı")
                        {
                            dgvTradeList.Columns["AverageCloseBalance"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Ortalama Pozisyon Kapama Adedi")
                        {
                            dgvTradeList.Columns["AverageCloseLotCount"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Posizyon Sonucu")
                        {
                            dgvTradeList.Columns["PositionResult"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "PnL")
                        {
                            dgvTradeList.Columns["ProfitOrLoss"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "PnL Yüzdesi")
                        {
                            dgvTradeList.Columns["ProfitOrLossPercent"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Toplam Komisyon")
                        {
                            dgvTradeList.Columns["CommissionSum"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Toplam Fonlama Maliyeti")
                        {
                            dgvTradeList.Columns["FundingFeeSum"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Notlar")
                        {
                            dgvTradeList.Columns["Note"].Visible = columnSetting.Value;
                        }
                        if (columnSetting.Key == "Trade Sonlandı Mı?")
                        {
                            dgvTradeList.Columns["EndTrade"].Visible = columnSetting.Value;
                        }
                    }

                    //dgvTradeList.Columns["AverageEntryBalance"].DefaultCellStyle.Format = "$#.##";
                    //dgvTradeList.Columns["TargetedEntryPrice"].DefaultCellStyle.Format = "$#.##";
                    //dgvTradeList.Columns[6].DefaultCellStyle.Format = "$#.##";
                    //dgvTradeList.Columns[7].DefaultCellStyle.Format = "$#.##";
                    //dgvTradeList.Columns[8].DefaultCellStyle.Format = "$#.##";
                    //dgvTradeList.Columns[9].DefaultCellStyle.Format = "#.##%";
                    //dgvTradeList.Columns[10].DefaultCellStyle.Format = "#.##%";
                    //dgvTradeList.Columns[12].DefaultCellStyle.Format = "$#.##";
                    //dgvTradeList.Columns[13].DefaultCellStyle.Format = "$#.##";
                    //dgvTradeList.Columns[15].DefaultCellStyle.Format = "$#.##";
                    //dgvTradeList.Columns[16].DefaultCellStyle.Format = "#.##%";

                    // _tradeModelManager.CalculateGeneralInformation(xmlFilePath);
                    // LoadGeneralInformation(GetGeneralInformation());
                }

                bool columnExists = dgvTradeList.Columns.Cast<DataGridViewColumn>().Any(column => column.Name == "shareImageButton");

                if (!columnExists)
                {
                    DataGridViewButtonColumn shareButtonColumn = new DataGridViewButtonColumn();

                    shareButtonColumn.HeaderText = "Paylaş";
                    shareButtonColumn.Name = "shareImageButton";
                    shareButtonColumn.Text = "Paylaş";
                    shareButtonColumn.DefaultCellStyle.SelectionBackColor = Color.Transparent;
                    shareButtonColumn.UseColumnTextForButtonValue = true;
                    dgvTradeList.Columns.Add(shareButtonColumn);
                }

                ClearTrade();
                ClearTradeDetails();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Hata", MessageBoxButtons.OK);
            }
        }

        private void LoadTradeDetailDataGridView(int tradeId)
        {
            var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);

            if (cbxListOfTradeXmls.Text != null)
            {
                gbTradeDetails.Text = cbxListOfTradeXmls.Text + " İşlem Detayları..";

                var dataList = _tradeModelManager.GetTradeDetailList(tradeId, xmlFilePath);

                dgvTradeDetails.DataSource = dataList;
                dgvTradeDetails.ReadOnly = true;

                //dgvTradeList.Columns["AverageEntryBalance"].DefaultCellStyle.Format = "$#.##";
                //dgvTradeList.Columns["TargetedEntryPrice"].DefaultCellStyle.Format = "$#.##";
                //dgvTradeList.Columns[6].DefaultCellStyle.Format = "$#.##";
                //dgvTradeList.Columns[7].DefaultCellStyle.Format = "$#.##";
                //dgvTradeList.Columns[8].DefaultCellStyle.Format = "$#.##";
                //dgvTradeList.Columns[9].DefaultCellStyle.Format = "#.##%";
                //dgvTradeList.Columns[10].DefaultCellStyle.Format = "#.##%";
                //dgvTradeList.Columns[12].DefaultCellStyle.Format = "$#.##";
                //dgvTradeList.Columns[13].DefaultCellStyle.Format = "$#.##";
                //dgvTradeList.Columns[15].DefaultCellStyle.Format = "$#.##";
                //dgvTradeList.Columns[16].DefaultCellStyle.Format = "#.##%";

                // _tradeModelManager.CalculateGeneralInformation(xmlFilePath);
                // LoadGeneralInformation(GetGeneralInformation());
            }

        }


        private void cbxListOfTradeXmls_SelectedValueChanged_1(object sender, EventArgs e)
        {
            LoadTradeDataGridView();
            LoadGeneralInformation();
            dgvTradeDetails.DataSource = null;
        }

        private void btnDeleteXmlFile_Click(object sender, EventArgs e)
        {
            var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);

            if (cbxListOfTradeXmls.Text == null || cbxListOfTradeXmls.Text != "")
            {
                var result = MessageBox.Show(cbxListOfTradeXmls.Text + " listesini silmek istiyor musunuz?", "Uyarı",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _tradeModelManager.DeleteXmlFileByPath(xmlFilePath);
                    MessageBox.Show(cbxListOfTradeXmls.Text + " silindi!", "Bilgi",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    GetWithXmlFilesIntoComboBox();

                    ClearTrade();
                    ClearTradeDetails();
                    ClearGeneralInformation();
                    ClearCurrencyPairStatistic();
                    dgvTradeList.DataSource = null;
                    dgvTradeDetails.DataSource = null;
                    cbxListOfTradeXmls.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Herhangi bir dosya seçilmemiş...", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbxListOfTradeXmls.Text))
            {
                var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);

                LoadTradeDataGridView();
                _tradeModelManager.CalculateGeneralInformation(xmlFilePath);
                LoadGeneralInformation();
                GetWithXmlFilesIntoComboBox();
                ClearCurrencyPairStatistic();
                ClearTradeDetails();
                ClearTrade();
                dgvTradeDetails.DataSource = null;
                MakeEnableComponents();
            }


        }

        private void dgvTradeList_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);

            if (dgvTradeList.Columns[e.ColumnIndex].Name == "shareImageButton")
            {
                int tradeId = (int)dgvTradeList.Rows[e.RowIndex].Cells["Id"].Value;

                var settingsForm = new ImageForm();
                ImageForm.ListOfTradeXmls = cbxListOfTradeXmls.Text;

                ImageForm.SelectedTradeId = tradeId;

                var generalSettings = _tradeModelManager.GetGeneralSettings(xmlFilePath);

                ImageForm.ReferralLink = generalSettings.ReferralLink;
                ImageForm.ReferralId = generalSettings.ReferralId;
                ImageForm.Exchange = generalSettings.Exchange;

                settingsForm.Show();
            }

            try
            {
                tradeClickedColumnIndex = e.ColumnIndex;
                tradeClickedRowIndex = e.RowIndex;

                int tradeId = (int)dgvTradeList.Rows[e.RowIndex].Cells["Id"].Value;

                var tradeData = _tradeModelManager.GetTradeById(tradeId, xmlFilePath);

                lblTradeIdLabel.Text = tradeData.Id.ToString();
                cbxCurrencyPairList.Text = tradeData.CurrencyPair;
                cbxCurrencyPairList2.Text = tradeData.CurrencyPair;
                cbxPositionSide.Text = tradeData.PositionSide.ToString();
                cbxLeverage.Text = tradeData.Leverage.ToString();
                tbxTargetedEntryPrice.Text = tradeData.TargetedEntryPrice.ToString();
                tbxStopPrice.Text = tradeData.StopLossPrice.ToString(CultureInfo.InvariantCulture);
                tbxTakeProfitPrice.Text = tradeData.TakeProfitPrice.ToString(CultureInfo.InvariantCulture);
                tbxTotalFundingFee.Text = tradeData.FundingFeeSum.ToString(CultureInfo.InvariantCulture);
                rtbxTradeNote.Text = tradeData.Note;
                lblTradeDetailTradeIdLabel.Text = tradeData.Id.ToString();
                gbCurrencyPairStatistics.Text = tradeData.CurrencyPair + " İST..";

                LoadTradeDetailDataGridView(tradeData.Id);

                if (tradeData.PositionSide == PositionSide.Long)
                {
                    cbxTradeType.Items.Clear();
                    cbxTradeType.Items.Add("OpenLong");
                    cbxTradeType.Items.Add("CloseLong");
                }
                if (tradeData.PositionSide == PositionSide.Short)
                {
                    cbxTradeType.Items.Clear();
                    cbxTradeType.Items.Add("OpenShort");
                    cbxTradeType.Items.Add("CloseShort");
                }

                if (tradeData.TradeDetails.Count > 0)
                {
                    chckEndTrade.Enabled = true;
                }

                if (tradeData.EndTrade)
                {
                    MakeDisableComponentsForEndedTrade();

                }
                else
                {
                    MakeEnableComponents();
                }

                LoadCurrencyPairStatistic(tradeData.CurrencyPair);
            }
            catch (Exception exception)
            {

            }
        }

        private void MakeDisableComponentsForEndedTrade()
        {
            cbxPositionSide.Enabled = false;
            cbxLeverage.Enabled = false;
            tbxTargetedEntryPrice.Enabled = false;
            tbxStopPrice.Enabled = false;
            tbxTakeProfitPrice.Enabled = false;
            rtbxTradeNote.Enabled = false;
            cbxCurrencyPairList.Enabled = false;

            cbxTradeType.Enabled = false;
            cbxOrderType.Enabled = false;
            tbxTradeEntryPrice.Enabled = false;
            tbxTradeEntryBalance.Enabled = false;
            chckEntryLotCount.Enabled = false;
            chckEndTrade.Enabled = false;
            btnNewTradeDetail.Enabled = false;
            btnDeleteTradeDetail.Enabled = false;
            btnSaveTradeDetail.Enabled = false;
        }

        private void MakeEnableComponents()
        {
            cbxPositionSide.Enabled = true;
            cbxLeverage.Enabled = true;
            tbxTargetedEntryPrice.Enabled = true;
            tbxStopPrice.Enabled = true;
            tbxTakeProfitPrice.Enabled = true;
            rtbxTradeNote.Enabled = true;

            cbxTradeType.Enabled = true;
            cbxOrderType.Enabled = true;
            tbxTradeEntryPrice.Enabled = true;
            tbxTradeEntryBalance.Enabled = true;
            chckEntryLotCount.Enabled = true;
            chckEndTrade.Enabled = true;
            btnNewTradeDetail.Enabled = true;
            btnDeleteTradeDetail.Enabled = true;
            btnSaveTradeDetail.Enabled = true;
        }

        private void ClearTrade()
        {
            lblTradeIdLabel.Text = "";
            cbxPositionSide.Text = "";
            cbxLeverage.Text = "";
            tbxTargetedEntryPrice.Text = "";
            tbxStopPrice.Text = "";
            tbxTakeProfitPrice.Text = "";
            tbxTotalFundingFee.Text = "";
            rtbxTradeNote.Text = "";
            cbxCurrencyPairList.Text = "";
        }
        private void ClearTradeDetails()
        {
            lblTradeDetailIdLabel.Text = "";
            lblTradeDetailTradeIdLabel.Text = "";
            dateTradeDetailDate.Value = DateTime.Now;
            cbxTradeType.Text = "";
            cbxOrderType.Text = "";
            tbxTradeEntryBalance.Text = "";
            tbxTradeEntryPrice.Text = "";
            tbxTradeEntryLotCount.Text = "";
        }

        private void ClearGeneralInformation()
        {
            lblStartBalanceText.Text = "";
            lblLastBalanceLabel.Text = "";
            lblInTradeBalanceLabel.Text = "";
            lblAvailableBalanceLabel.Text = "";
            lblProfitsSumLabel.Text = "";
            lblLossesSumLabel.Text = "";
            lblTotalCommissionLabel.Text = "";
            lblTotalFundingFeeLabel.Text = "";
            lblTotalPnLLabel.Text = "";
            lblTotalPnLPercentLabel.Text = "";
            lblWinCountLabel.Text = "";
            lblLossCountLabel.Text = "";
            lblWinrateLabel.Text = "";
        }

        private void ClearCurrencyPairStatistic()
        {
            lblCurrencyPairInTradeBalanceLabel.Text = "";
            lblCurrencyPairProfitSumLabel.Text = "";
            lblCurrencyPairLosesSumLabel.Text = "";
            lblCurrencyPairTotalPnLLabel.Text = "";
            lblCurrencyPairWinTradeCountLabel.Text = "";
            lblCurrencyPairLoseTradeCountLabel.Text = "";
            lblCurrencyPairWinRateLabel.Text = "";
            lblCurrencyPairCommissionSumLabel.Text = "";
            lblCurrencyPairFundingFeeSumLabel.Text = "";
            cbxCurrencyPairList2.Text = "";
            gbCurrencyPairStatistics.Text = "İşlem Çifti İstatistiği..";
        }



        private GeneralInformation GetGeneralInformation()
        {
            var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);
            var generalInformation = new GeneralInformation();
            try
            {
                if (cbxListOfTradeXmls.Text != null)
                {
                    generalInformation = _tradeModelManager.GetGeneralInformation(xmlFilePath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Genel istatistik Bilgisi okunamadı." + "\nSistem Mesajı: " + e.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return generalInformation;

        }

        private GeneralSettings GetGeneralSettings()
        {
            var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);
            var generalSettings = new GeneralSettings();
            try
            {
                if (cbxListOfTradeXmls.Text != null)
                {
                    generalSettings = _tradeModelManager.GetGeneralSettings(xmlFilePath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Genel istatistik Bilgisi okunamadı." + "\nSistem Mesajı: " + e.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return generalSettings;

        }

        private void LoadGeneralInformation()
        {
            var generalInformation = GetGeneralInformation();
            var generalSettings = GetGeneralSettings();

            cbxExchanges.Text = generalSettings.Exchange;
            tbxReferralLink.Text = generalSettings.ReferralLink;
            tbxReferralId.Text = generalSettings.ReferralId;
            lblStartBalanceText.Text = "$" + generalInformation.StartingBalance.ToString(CultureInfo.InvariantCulture);
            lblLastBalanceLabel.Text = "$" + generalInformation.LastBalance.ToString(CultureInfo.InvariantCulture);
            lblInTradeBalanceLabel.Text = "$" + generalInformation.InTradeBalance.ToString(CultureInfo.InvariantCulture);
            lblAvailableBalanceLabel.Text = "$" + generalInformation.AvailableBalance.ToString(CultureInfo.InvariantCulture);
            lblProfitsSumLabel.Text = "$" + generalInformation.ProfitsSum.ToString(CultureInfo.InvariantCulture);
            lblLossesSumLabel.Text = "$" + generalInformation.LossesSum.ToString(CultureInfo.InvariantCulture);
            lblTotalPnLLabel.Text = "$" + generalInformation.TotalPnL.ToString(CultureInfo.InvariantCulture);
            lblWinCountLabel.Text = generalInformation.WinCount.ToString();
            lblLossCountLabel.Text = generalInformation.LossCount.ToString();
            lblWinrateLabel.Text = "%" + generalInformation.TradeWinRate.ToString(CultureInfo.InvariantCulture);
            lblTotalPnLPercentLabel.Text = "%" + generalInformation.TotalPnLPercent.ToString(CultureInfo.InvariantCulture);
            tbxMakerCommission.Text = generalSettings.MakerCommission.ToString(CultureInfo.InvariantCulture);
            tbxTakerCommission.Text = generalSettings.TakerCommission.ToString(CultureInfo.InvariantCulture);
            lblTotalCommissionLabel.Text = "$" + generalInformation.TotalCommission.ToString(CultureInfo.InvariantCulture);
            lblTotalFundingFeeLabel.Text = "$" + generalInformation.TotalFundingFee.ToString(CultureInfo.InvariantCulture);
            lblStartingBalanceLabel2.Text = "$" + generalInformation.StartingBalance.ToString(CultureInfo.InvariantCulture);

            if (generalInformation.TotalPnL >= 0)
            {
                lblTotalPnLLabel.ForeColor = Color.SeaGreen;
                lblTotalPnLPercentLabel.ForeColor = Color.SeaGreen;
            }
            else
            {
                lblTotalPnLLabel.ForeColor = Color.IndianRed;
                lblTotalPnLPercentLabel.ForeColor = Color.IndianRed;
            }

            if (generalInformation.TradeWinRate >= 50)
            {
                lblWinrateLabel.ForeColor = Color.SeaGreen;
            }
            else
            {
                lblWinrateLabel.ForeColor = Color.IndianRed;
            }
        }

        private void LoadCurrencyPairStatistic(string currencyPair)
        {
            var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);
            var currencyPairStatistic = _tradeModelManager.GetCurrencyPairStatisticByCurrencyPair(currencyPair, xmlFilePath);


            lblCurrencyPairInTradeBalanceLabel.Text = "$" + currencyPairStatistic.InTradeBalance.ToString(CultureInfo.InvariantCulture);
            lblCurrencyPairProfitSumLabel.Text = "$" + currencyPairStatistic.ProfitsSum.ToString(CultureInfo.InvariantCulture);
            lblCurrencyPairLosesSumLabel.Text = "$" + currencyPairStatistic.LossesSum.ToString(CultureInfo.InvariantCulture);
            lblCurrencyPairTotalPnLLabel.Text = "$" + currencyPairStatistic.TotalPnL.ToString(CultureInfo.InvariantCulture);
            lblCurrencyPairWinTradeCountLabel.Text = currencyPairStatistic.WinCount.ToString();
            lblCurrencyPairLoseTradeCountLabel.Text = currencyPairStatistic.LossCount.ToString();
            lblCurrencyPairWinRateLabel.Text = "%" + currencyPairStatistic.TradeWinRate.ToString(CultureInfo.InvariantCulture);
            lblCurrencyPairCommissionSumLabel.Text = "$" + currencyPairStatistic.TotalCommission.ToString(CultureInfo.InvariantCulture);
            lblCurrencyPairFundingFeeSumLabel.Text = "$" + currencyPairStatistic.TotalFundingFee.ToString(CultureInfo.InvariantCulture);

            if (currencyPairStatistic.TotalPnL >= 0)
            {
                lblCurrencyPairTotalPnLLabel.ForeColor = Color.SeaGreen;
            }
            else
            {
                lblCurrencyPairTotalPnLLabel.ForeColor = Color.IndianRed;
            }

            if (currencyPairStatistic.TradeWinRate >= 50)
            {
                lblCurrencyPairWinRateLabel.ForeColor = Color.SeaGreen;
            }
            else
            {
                lblCurrencyPairWinRateLabel.ForeColor = Color.IndianRed;
            }
        }

        private void btnSaveTrade_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbxListOfTradeXmls.Text))
            {
                string xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);

                var generalInformation = GetGeneralInformation();

                if (generalInformation.StartingBalance == 0)
                {
                    MessageBox.Show("Lütfen önce başlangıç bakiyenizi giriniz!", "Uyarı", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    if (string.IsNullOrEmpty(lblTradeIdLabel.Text))
                    {
                        Trade newTrade = new Trade();

                        newTrade.CurrencyPair = cbxCurrencyPairList.Text;
                        newTrade.TradeStartDate = DateTime.Now;
                        newTrade.PositionSide = (PositionSide)Enum.Parse(typeof(PositionSide), cbxPositionSide.Text);
                        newTrade.AverageEntryBalance = 0;
                        newTrade.Leverage = Convert.ToInt32(cbxLeverage.Text);
                        newTrade.AverageEntryPrice = 0;
                        newTrade.TargetedEntryPrice = Convert.ToDecimal(tbxTargetedEntryPrice.Text.Replace(".", ","));
                        newTrade.StopLossPrice = Convert.ToDecimal(tbxStopPrice.Text.Replace(".", ","));
                        newTrade.TakeProfitPrice = Convert.ToDecimal(tbxTakeProfitPrice.Text.Replace(".", ","));
                        newTrade.AveragePositionClosePrice = 0;
                        newTrade.PositionResult = PositionResult.SO;
                        newTrade.Note = rtbxTradeNote.Text;
                        newTrade.FundingFeeSum = 0;


                        switch (newTrade.PositionSide)
                        {
                            case PositionSide.Long:
                                newTrade.RiskPercent = Math.Round((newTrade.StopLossPrice / newTrade.TargetedEntryPrice - 1) * 100, 2);
                                newTrade.RewardPercent = Math.Round((newTrade.TakeProfitPrice / newTrade.TargetedEntryPrice - 1) * 100, 2);
                                newTrade.RiskRewardRatio = Math.Abs(Math.Round(newTrade.RewardPercent / newTrade.RiskPercent, 2));
                                break;
                            case PositionSide.Short:
                                newTrade.RiskPercent = Math.Round((1 - newTrade.StopLossPrice / newTrade.TargetedEntryPrice) * 100, 2);
                                newTrade.RewardPercent = Math.Round((1 - newTrade.TakeProfitPrice / newTrade.TargetedEntryPrice) * 100, 2);
                                newTrade.RiskRewardRatio = Math.Abs(Math.Round(newTrade.RewardPercent / newTrade.RiskPercent, 2));
                                break;
                        }

                        newTrade.ExpectedRiskValue =
                            Math.Round(newTrade.AverageEntryBalance * newTrade.Leverage * newTrade.RiskPercent / 100, 2);
                        newTrade.ExpectedRewardValue =
                            Math.Round(newTrade.AverageEntryBalance * newTrade.Leverage * newTrade.RewardPercent / 100, 2);

                        newTrade.EndTrade = false;

                        _tradeModelManager.AddTrade(newTrade, xmlFilePath);

                        _tradeModelManager.AddCurrencyPairStatistic(new CurrencyPairStatistic()
                        {
                            CurrencyPair = newTrade.CurrencyPair,
                            InTradeBalance = 0,
                            LossCount = 0,
                            LossesSum = 0,
                            ProfitsSum = 0,
                            TotalCommission = 0,
                            TotalFundingFee = 0,
                            TotalPnL = 0,
                            TotalTradeCount = 0,
                            TradeWinRate = 0,
                            WinCount = 0

                        }, xmlFilePath);

                        _tradeModelManager.CalculateCurrencyPairStatisticByCurrencyPair(newTrade.CurrencyPair, xmlFilePath);

                        LoadCurrencyPairStatistic(newTrade.CurrencyPair);
                    }
                    else
                    {
                        var foundTrade = _tradeModelManager.GetTradeById(Convert.ToInt32(lblTradeIdLabel.Text), xmlFilePath);

                        if (foundTrade.TradeDetails.Count == 0)
                        {
                            foundTrade.CurrencyPair = cbxCurrencyPairList.Text;
                            foundTrade.Leverage = Convert.ToInt32(cbxLeverage.Text);
                            foundTrade.TargetedEntryPrice = Convert.ToDecimal(tbxTargetedEntryPrice.Text.Replace(".", ","));
                            foundTrade.StopLossPrice = Convert.ToDecimal(tbxStopPrice.Text.Replace(".", ","));
                            foundTrade.TakeProfitPrice = Convert.ToDecimal(tbxTakeProfitPrice.Text.Replace(".", ","));
                            foundTrade.Note = rtbxTradeNote.Text;
                            foundTrade.FundingFeeSum = Convert.ToDecimal(tbxTotalFundingFee.Text.Replace(".", ","));

                            switch (foundTrade.PositionSide)
                            {
                                case PositionSide.Long:
                                    foundTrade.RiskPercent = Math.Round((foundTrade.StopLossPrice / foundTrade.TargetedEntryPrice - 1) * 100, 2);
                                    foundTrade.RewardPercent = Math.Round((foundTrade.TakeProfitPrice / foundTrade.TargetedEntryPrice - 1) * 100, 2);
                                    foundTrade.RiskRewardRatio = Math.Abs(Math.Round(foundTrade.RewardPercent / foundTrade.RiskPercent, 2));
                                    break;
                                case PositionSide.Short:
                                    foundTrade.RiskPercent = Math.Round((1 - foundTrade.StopLossPrice / foundTrade.TargetedEntryPrice) * 100, 2);
                                    foundTrade.RewardPercent = Math.Round((1 - foundTrade.TakeProfitPrice / foundTrade.TargetedEntryPrice) * 100, 2);
                                    foundTrade.RiskRewardRatio = Math.Abs(Math.Round(foundTrade.RewardPercent / foundTrade.RiskPercent, 2));
                                    break;
                            }

                            foundTrade.ExpectedRiskValue =
                                Math.Round(foundTrade.AverageEntryBalance * foundTrade.Leverage * foundTrade.RiskPercent / 100, 2);
                            foundTrade.ExpectedRewardValue =
                                Math.Round(foundTrade.AverageEntryBalance * foundTrade.Leverage * foundTrade.RewardPercent / 100, 2);


                            _tradeModelManager.UpdateTrade(foundTrade, xmlFilePath);

                        }
                        else
                        {

                            foundTrade.FundingFeeSum = Convert.ToDecimal(tbxTotalFundingFee.Text.Replace(".", ","));
                            _tradeModelManager.UpdateTrade(foundTrade, xmlFilePath);
                            var calculatedTrade = _tradeModelManager.CalculateTrade(foundTrade.Id, xmlFilePath);
                            _tradeModelManager.UpdateTrade(calculatedTrade, xmlFilePath);

                            MessageBox.Show("İşlem girişi yapıldıktan sonra sadece fonlama maliyeti güncellenebilir!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        _tradeModelManager.CalculateCurrencyPairStatisticByCurrencyPair(foundTrade.CurrencyPair, xmlFilePath);

                        LoadCurrencyPairStatistic(foundTrade.CurrencyPair);
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lütfen yeni bir trade girişi için tüm alanları doldurunuz. Mevcut bir trade'i güncellemek için ilgili trade satırını seçiniz." + "\n(Sistem mesajı: " + exception.Message + ")", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                _tradeModelManager.CalculateGeneralInformation(xmlFilePath);
                LoadTradeDataGridView();
                LoadGeneralInformation();
            }
            else
            {
                MessageBox.Show("Lütfen trade listesini yükleyiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnNewTrade_Click_1(object sender, EventArgs e)
        {
            ClearTrade();
            ClearTradeDetails();
            dgvTradeDetails.DataSource = null;
            ClearCurrencyPairStatistic();
            MakeEnableComponents();
        }


        private void dgvTradeList_RowPrePaint_1(object sender, DataGridViewRowPrePaintEventArgs e)
        {

            // Verileri Trade sınıfından oluşan bir liste olarak düşünün
            var data = (List<Trade>)dgvTradeList.DataSource;

            // Seçili satırın Trade nesnesini al
            var trade = data[e.RowIndex];

            switch (trade.PositionResult)
            {
                // Eğer PositionResult "TP" ise, satırı yeşil olarak boya
                case PositionResult.TP:
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.LimeGreen;
                    break;
                // Eğer PositionResult "SL" ise, satırı kırmızı olarak boya
                case PositionResult.SL:
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.OrangeRed;
                    break;
                case PositionResult.SO:
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
                    break;
            }

            switch (trade.PositionSide)
            {
                case PositionSide.Long:
                    dgvTradeList.Rows[e.RowIndex].Cells["PositionSide"].Style.BackColor = Color.LightSkyBlue;
                    dgvTradeList.Rows[e.RowIndex].Cells["PositionSide"].Style.SelectionBackColor = Color.LightSkyBlue;
                    break;
                case PositionSide.Short:
                    dgvTradeList.Rows[e.RowIndex].Cells["PositionSide"].Style.BackColor = Color.DeepPink;
                    dgvTradeList.Rows[e.RowIndex].Cells["PositionSide"].Style.SelectionBackColor = Color.DeepPink;
                    break;
            }

        }

        private void btnSaveTradeDetail_Click_1(object sender, EventArgs e)
        {
            string xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);


            try
            {
                int tradeId = 0;
                int tradeDetailId = 0;


                if (string.IsNullOrEmpty(lblTradeDetailTradeIdLabel.Text))
                {
                    MessageBox.Show("Lütfen işlem yapacağınız trade'i seçiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                tradeId = Convert.ToInt32(lblTradeDetailTradeIdLabel.Text);

                var tradeData = _tradeModelManager.GetTradeById(tradeId, xmlFilePath);
                var generalInformation = GetGeneralInformation();


                if (cbxTradeType.Text == "CloseLong" || cbxTradeType.Text == "CloseShort")
                {
                    if (tradeData.AverageEntryBalance == 0)
                    {
                        MessageBox.Show("İşlem açılmadan posizyon kapatma yapılamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(lblTradeDetailIdLabel.Text))
                {
                    tradeDetailId = Convert.ToInt32(lblTradeDetailIdLabel.Text);
                }

                TradeDetail newTradeDetail = new TradeDetail();

                if (string.IsNullOrEmpty(lblTradeDetailIdLabel.Text))
                {
                    if (string.IsNullOrEmpty(tbxTradeEntryPrice.Text) || string.IsNullOrEmpty(cbxTradeType.Text))
                    {
                        MessageBox.Show("Lütfen gerekli tüm bilgileri giriniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (cbxTradeType.Text == "OpenLong" || cbxTradeType.Text == "OpenShort")
                    {
                        var entryBalance = Convert.ToDecimal(tbxTradeEntryBalance.Text);
                        var availableBalance = generalInformation.AvailableBalance;
                        if (availableBalance - entryBalance < 0)
                        {
                            MessageBox.Show("Mevcut işlem bakiyeniz: $" + tbxTradeEntryBalance.Text + "\n" + "Boştaki bakiyeniz: $" + availableBalance + "\n" + "İşlem bakiyeniz boştaki bakiyenizden büyük olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    newTradeDetail.TradeId = Convert.ToInt32(lblTradeDetailTradeIdLabel.Text);
                    newTradeDetail.TradeDate = Convert.ToDateTime(dateTradeDetailDate.Value);
                    newTradeDetail.TradeType = (TradeType)Enum.Parse(typeof(TradeType), cbxTradeType.Text);
                    newTradeDetail.EntryPrice = Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ","));
                    newTradeDetail.OrderType = (OrderType)Enum.Parse(typeof(OrderType), cbxOrderType.Text);

                    if (!chckEndTrade.Checked)
                    {
                        if (!chckEntryLotCount.Checked)
                        {
                            newTradeDetail.EntryBalance = Convert.ToDecimal(tbxTradeEntryBalance.Text.Replace(".", ","));
                            newTradeDetail.EntryLotCount = Math.Round(Convert.ToDecimal(tbxTradeEntryBalance.Text.Replace(".", ",")) / Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ",")), 8);
                        }
                        else
                        {
                            newTradeDetail.EntryLotCount = Convert.ToDecimal(tbxTradeEntryLotCount.Text.Replace(".", ","));
                            newTradeDetail.EntryBalance = Math.Round(Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ",")) *
                                                                     Convert.ToDecimal(tbxTradeEntryLotCount.Text.Replace(".", ",")), 2);
                        }
                    }
                    else
                    {
                        switch (tradeData.PositionSide)
                        {
                            case PositionSide.Long:
                                {
                                    var openLongLotCountSum = tradeData.TradeDetails.Where(x => x.TradeType == TradeType.OpenLong)
                                        .Sum(x => x.EntryLotCount);

                                    var closeLongLotCountSum = tradeData.TradeDetails.Where(x => x.TradeType == TradeType.CloseLong)
                                        .Sum(x => x.EntryLotCount);

                                    var lastLotCount = openLongLotCountSum - closeLongLotCountSum;

                                    newTradeDetail.EntryLotCount = lastLotCount;
                                    newTradeDetail.EntryBalance = Math.Round(Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ",")) * lastLotCount, 2);
                                    break;
                                }
                            case PositionSide.Short:
                                {
                                    var openShortLotCountSum = tradeData.TradeDetails.Where(x => x.TradeType == TradeType.OpenShort)
                                        .Sum(x => x.EntryLotCount);

                                    var closeShortLotCountSum = tradeData.TradeDetails.Where(x => x.TradeType == TradeType.CloseShort)
                                        .Sum(x => x.EntryLotCount);

                                    var lastLotCount = openShortLotCountSum - closeShortLotCountSum;

                                    newTradeDetail.EntryLotCount = lastLotCount;
                                    newTradeDetail.EntryBalance = Math.Round(Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ",")) *
                                                                            lastLotCount, 2);
                                    break;
                                }
                        }

                        tradeData.EndTrade = true;
                        _tradeModelManager.UpdateTrade(tradeData, xmlFilePath);
                    }

                    _tradeModelManager.AddTradeDetail(newTradeDetail.TradeId, newTradeDetail, xmlFilePath);

                    var updatedTrade = _tradeModelManager.CalculateTrade(tradeId, xmlFilePath);

                    _tradeModelManager.UpdateTrade(updatedTrade, xmlFilePath);

                }
                else
                {

                    TradeDetail foundTradeDetail = _tradeModelManager.GetTradeDetailById(tradeId, tradeDetailId, xmlFilePath);

                    if (cbxTradeType.Text == "OpenLong" || cbxTradeType.Text == "OpenShort")
                    {
                        var entryBalance = Convert.ToDecimal(tbxTradeEntryBalance.Text);
                        var availableBalance = generalInformation.AvailableBalance + foundTradeDetail.EntryBalance;
                        if (availableBalance - entryBalance < 0)
                        {
                            MessageBox.Show("Mevcut işlem bakiyeniz: $" + tbxTradeEntryBalance.Text + "\n" + "Boştaki bakiyeniz: $" + (availableBalance - foundTradeDetail.EntryBalance) + "\n" + "İşlem bakiyeniz boştaki bakiyenizden büyük olamaz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    foundTradeDetail.TradeId = Convert.ToInt32(lblTradeDetailTradeIdLabel.Text);
                    foundTradeDetail.TradeDate = Convert.ToDateTime(dateTradeDetailDate.Value);
                    foundTradeDetail.TradeType = (TradeType)Enum.Parse(typeof(TradeType), cbxTradeType.Text);
                    foundTradeDetail.EntryPrice = Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ","));
                    foundTradeDetail.OrderType = (OrderType)Enum.Parse(typeof(OrderType), cbxOrderType.Text);

                    if (!chckEntryLotCount.Checked)
                    {
                        foundTradeDetail.EntryBalance = Convert.ToDecimal(tbxTradeEntryBalance.Text.Replace(".", ","));
                        foundTradeDetail.EntryLotCount = Math.Round(Convert.ToDecimal(tbxTradeEntryBalance.Text.Replace(".", ",")) / Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ",")), 8);
                    }
                    else
                    {
                        foundTradeDetail.EntryLotCount = Convert.ToDecimal(tbxTradeEntryLotCount.Text.Replace(".", ","));
                        foundTradeDetail.EntryBalance = Math.Round(Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ",")) * Convert.ToDecimal(tbxTradeEntryLotCount.Text.Replace(".", ",")), 2);
                    }

                    _tradeModelManager.UpdateTradeDetail(foundTradeDetail.TradeId, foundTradeDetail, xmlFilePath);



                    var updatedTrade = _tradeModelManager.CalculateTrade(tradeId, xmlFilePath);

                    _tradeModelManager.UpdateTrade(updatedTrade, xmlFilePath);


                }

                _tradeModelManager.CalculateCurrencyPairStatisticByCurrencyPair(tradeData.CurrencyPair, xmlFilePath);

                chckEndTrade.CheckState = CheckState.Unchecked;
                LoadTradeDataGridView();
                LoadTradeDetailDataGridView(tradeId);
                _tradeModelManager.CalculateGeneralInformation(xmlFilePath);
                LoadGeneralInformation();
                LoadCurrencyPairStatistic(tradeData.CurrencyPair);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void btnNewTradeDetail_Click_1(object sender, EventArgs e)
        {
            ClearTradeDetails();
        }


        private void chckEntryLotCount_CheckedChanged(object sender, EventArgs e)
        {
            if (chckEntryLotCount.Checked)
            {
                tbxTradeEntryLotCount.Enabled = true;
                tbxTradeEntryBalance.Enabled = false;
            }
            else
            {
                tbxTradeEntryLotCount.Enabled = false;
                tbxTradeEntryBalance.Enabled = true;
            }
        }


        private void dgvTradeDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);
            try
            {

                tradeClickedColumnIndex = e.ColumnIndex;
                tradeDetailClickedRowIndex = e.RowIndex;

                int tradeId = (int)dgvTradeDetails.Rows[e.RowIndex].Cells["TradeId"].Value;
                int tradeDetailId = (int)dgvTradeDetails.Rows[e.RowIndex].Cells["Id"].Value;

                var tradeDetailData = _tradeModelManager.GetTradeDetailById(tradeId, tradeDetailId, xmlFilePath);

                lblTradeDetailIdLabel.Text = tradeDetailData.Id.ToString();
                dateTradeDetailDate.Text = tradeDetailData.TradeDate.ToString(CultureInfo.CurrentCulture);
                cbxTradeType.Text = tradeDetailData.TradeType.ToString();
                cbxOrderType.Text = tradeDetailData.OrderType.ToString();
                tbxTradeEntryBalance.Text = tradeDetailData.EntryBalance.ToString(CultureInfo.InvariantCulture);
                tbxTradeEntryLotCount.Text = tradeDetailData.EntryLotCount.ToString(CultureInfo.InvariantCulture);
                tbxTradeEntryPrice.Text = tradeDetailData.EntryPrice.ToString(CultureInfo.InvariantCulture);
                lblTradeDetailTradeIdLabel.Text = tradeDetailData.TradeId.ToString();

            }
            catch
            {

            }
        }

        private void dgvTradeDetails_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var data = (List<TradeDetail>)dgvTradeDetails.DataSource;
            var trade = data[e.RowIndex];

            switch (trade.TradeType)
            {
                case TradeType.OpenLong:
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.BackColor = Color.LightGreen;
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.SelectionBackColor = Color.LimeGreen;
                    break;
                case TradeType.OpenShort:
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.BackColor = Color.LightGreen;
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.SelectionBackColor = Color.LimeGreen;
                    break;
                case TradeType.CloseLong:
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.BackColor = Color.LightCoral;
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.SelectionBackColor = Color.OrangeRed;
                    break;
                case TradeType.CloseShort:
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.BackColor = Color.LightCoral;
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.SelectionBackColor = Color.OrangeRed;
                    break;
            }
        }

        private void TradeList_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Control && e.KeyCode == Keys.F9)
            //{
            //    var settingsForm = new SettingsForm();
            //    settingsForm.Show();

            //}
        }


        private void btnTradeDelete_Click(object sender, EventArgs e)
        {
            var tradeId = Convert.ToInt32(lblTradeIdLabel.Text);
            var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);


            if (!string.IsNullOrEmpty(cbxListOfTradeXmls.Text))
            {
                if (!string.IsNullOrEmpty(lblTradeIdLabel.Text))
                {
                    var result = MessageBox.Show(lblTradeIdLabel.Text + " numaralı trade'i silmek istiyor musunuz?", "Uyarı",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        _tradeModelManager.RemoveTradeById(tradeId, xmlFilePath);
                        MessageBox.Show(lblTradeIdLabel.Text + " id numaralı işlem silindi!", "Uyarı",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        _tradeModelManager.CalculateGeneralInformation(xmlFilePath);

                        LoadGeneralInformation();
                        LoadTradeDataGridView();
                        LoadTradeDetailDataGridView(tradeId);
                    }

                }
                else
                {
                    MessageBox.Show("Lütfen işlem yapmak istediğiniz trade satırına tıklayınız!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Lütfen trade listesini seçiniz!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteTradeDetail_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblTradeDetailTradeIdLabel.Text) && !string.IsNullOrEmpty(lblTradeDetailIdLabel.Text))
            {
                var result = MessageBox.Show(lblTradeIdLabel.Text + " numaralı işlemi silmek istiyor musunuz?", "Uyarı",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var tradeId = Convert.ToInt32(lblTradeDetailTradeIdLabel.Text);
                    var tradeDetailId = Convert.ToInt32(lblTradeDetailIdLabel.Text);
                    var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);

                    var tradeData = _tradeModelManager.GetTradeById(tradeId, xmlFilePath);

                    if (tradeData.EndTrade)
                    {
                        MessageBox.Show("Sonlandırılmış bir trade işleminden detay silinemez!", "Uyarı",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (!string.IsNullOrEmpty(cbxListOfTradeXmls.Text))
                    {

                        _tradeModelManager.RemoveTradeDetailById(tradeId, tradeDetailId, xmlFilePath);
                        MessageBox.Show(lblTradeDetailIdLabel.Text + " id numaralı işlem detayı silindi!", "Uyarı",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        var updatedTrade = _tradeModelManager.CalculateTrade(tradeId, xmlFilePath);
                        _tradeModelManager.UpdateTrade(updatedTrade, xmlFilePath);
                        _tradeModelManager.CalculateGeneralInformation(xmlFilePath);

                        LoadGeneralInformation();
                        LoadTradeDataGridView();
                        LoadTradeDetailDataGridView(tradeId);
                    }
                    else
                    {
                        MessageBox.Show("Lütfen trade listesini seçiniz!", "Uyarı",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

            }
            else
            {
                MessageBox.Show("Lütfen işlem yapmak istediğiniz trade satırına tıklayınız!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void chckEndTrade_CheckedChanged(object sender, EventArgs e)
        {
            if (chckEndTrade.Checked)
            {
                var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);
                if (!string.IsNullOrEmpty(lblTradeDetailTradeIdLabel.Text))
                {
                    var tradeData = _tradeModelManager.GetTradeById(Convert.ToInt32(lblTradeDetailTradeIdLabel.Text), xmlFilePath);
                    ClearTradeDetails();

                    lblTradeDetailTradeIdLabel.Text = tradeData.Id.ToString();

                    switch (tradeData.PositionSide)
                    {
                        case PositionSide.Long:
                            cbxTradeType.Text = "CloseLong";
                            cbxTradeType.Enabled = false;
                            tbxTradeEntryBalance.Enabled = false;
                            chckEntryLotCount.Enabled = false;
                            break;
                        case PositionSide.Short:
                            cbxTradeType.Text = "CloseShort";
                            cbxTradeType.Enabled = false;
                            tbxTradeEntryBalance.Enabled = false;
                            chckEntryLotCount.Enabled = false;
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen işlem yapmak istediğiniz trade satırına tıklayınız!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                cbxTradeType.Enabled = true;
                tbxTradeEntryBalance.Enabled = true;
                chckEntryLotCount.Enabled = true;
                ClearTradeDetails();
            }


        }

        private void cbxTradeType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbxTradeType.Text == "CloseLong" || cbxTradeType.Text == "CloseShort")
            {
                var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);

                if (!string.IsNullOrEmpty(lblTradeDetailTradeIdLabel.Text))
                {
                    var tradeData = _tradeModelManager.GetTradeById(Convert.ToInt32(lblTradeDetailTradeIdLabel.Text), xmlFilePath);

                    if (tradeData.TradeDetails.Count == 0)
                    {
                        MessageBox.Show("Posizyon henüz açılmamış görünüyor. Lütfen önce posizyon açma parametrelerini kaydediniz!", "Uyarı",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbxTradeType.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen işlem yapmak istediğiniz trade satırına tıklayınız!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }



            }

        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {

            Point screenPoint = btnExport.PointToScreen(new Point(btnExport.Left, btnExport.Bottom));
            if (screenPoint.Y + cmsExport.Size.Height > Screen.PrimaryScreen.WorkingArea.Height)
            {
                cmsExport.Show(btnExport, new Point(0, -cmsExport.Size.Height));
            }
            else
            {
                cmsExport.Show(btnExport, new Point(0, btnExport.Height));
            }

        }

        private void ExportToExcel(DataGridView dataGridView)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Sheets[1];

            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dataGridView.Columns[i].HeaderText;
            }
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = dataGridView.Rows[i].Cells[j].Value.ToString();
                }
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                workbook.SaveAs(saveFileDialog.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            workbook.Close();
            excel.Quit();
        }

        private void ExportToPdf(DataGridView dataGridView)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A3.Rotate(), 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count);
                    pdfTable.DefaultCell.Padding = 3;
                    pdfTable.WidthPercentage = 100;
                    pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;

                    float[] widths = new float[dataGridView.Columns.Count];
                    for (int i = 0; i < dataGridView.Columns.Count; i++)
                    {
                        widths[i] = dataGridView.Columns[i].Width;
                    }
                    pdfTable.SetWidths(widths);

                    for (int i = 0; i < dataGridView.Columns.Count; i++)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(dataGridView.Columns[i].HeaderText));
                        cell.BackgroundColor = new BaseColor(240, 240, 240);
                        pdfTable.AddCell(cell);
                    }

                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView.Columns.Count; j++)
                        {
                            pdfTable.AddCell(dataGridView.Rows[i].Cells[j].Value.ToString());
                        }
                    }
                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();
                    stream.Close();
                }
            }
        }

        private void tsmiExportToExcel_Click(object sender, EventArgs e)
        {
            if (dgvTradeList.DataSource != null)
            {
                ExportToExcel(dgvTradeList);
            }
            else
            {
                MessageBox.Show("Trade listesi boş görünüyor. Lütfen trade listesini seçtikten sonra excele aktarmayı deneyin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsmiExportToPdf_Click(object sender, EventArgs e)
        {
            if (dgvTradeList.DataSource != null)
            {
                ExportToPdf(dgvTradeList);
            }
            else
            {
                MessageBox.Show("Trade listesi boş görünüyor. Lütfen trade listesini seçtikten sonra excele aktarmayı deneyin!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsmiExportStatisticsImage_Click(object sender, EventArgs e)
        {
            var settingsForm = new ImageForm();
            ImageForm.ListOfTradeXmls = cbxListOfTradeXmls.Text;
            settingsForm.Show();
        }

        private void tsmiExportTradeStatistics_Click(object sender, EventArgs e)
        {
            var settingsForm = new ImageForm();
            ImageForm.ListOfTradeXmls = cbxListOfTradeXmls.Text;

            if (!string.IsNullOrEmpty(lblTradeIdLabel.Text))
            {
                ImageForm.SelectedTradeId = Convert.ToInt32(lblTradeIdLabel.Text);
            }
            else
            {
                MessageBox.Show("Lütfen çıktı almak istediğiniz trade'i satırına tıklayarak seçiniz!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            var generalSettings = GetGeneralSettingsFromTextFile();

            if (generalSettings.ContainsKey("ReferralLink"))
            {
                ImageForm.ReferralLink = generalSettings["ReferralLink"];
            }
            if (generalSettings.ContainsKey("ReferralId"))
            {
                ImageForm.ReferralId = generalSettings["ReferralId"];
            }


            settingsForm.Show();
        }



        private void TradeList_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*
            checkedState = new bool[checkedListBox1.Items.Count];
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedState[i] = checkedListBox1.GetItemChecked(i);
            }
            */
        }


        private void LoadTradeCheckedListBoxCheckStates()
        {
            if (File.Exists(Properties.Settings.Default.CheckedListBoxFile))
            {
                var columnSettings = GetColumnSettings();

                for (int i = 0; i < chcklbTradeColumns.Items.Count; i++)
                {
                    var item = chcklbTradeColumns.Items[i].ToString();
                    var checkState = columnSettings.TryGetValue(item, out bool value);
                    chcklbTradeColumns.SetItemChecked(i, value);
                }
            }
        }

        bool[] checkedState;

        private void btnsaveTradeCheckedListBox_Click(object sender, EventArgs e)

        {
            using (StreamWriter sw = new StreamWriter(path: Properties.Settings.Default.CheckedListBoxFile))
            {
                foreach (var item in chcklbTradeColumns.Items)
                {
                    sw.WriteLine(item + ":" + chcklbTradeColumns.GetItemChecked(chcklbTradeColumns.Items.IndexOf(item)));
                }
            }
            MessageBox.Show("Gösterilecek sütunlar kaydedildi!", "Bilgi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            LoadTradeCheckedListBoxCheckStates();

            if (!string.IsNullOrEmpty(cbxListOfTradeXmls.Text))
            {
                LoadTradeDataGridView();
            }
        }

        private Dictionary<string, bool> GetColumnSettings()
        {
            Dictionary<string, bool> columnSettings = new Dictionary<string, bool>();

            using (var reader = new StreamReader(Properties.Settings.Default.CheckedListBoxFile))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] lineData = line.Split(':');
                    string key = lineData[0];
                    bool value = Convert.ToBoolean(lineData[1]);

                    columnSettings.Add(key, value);

                }
                return columnSettings;
            }
        }

        private Dictionary<string, string> GetGeneralSettingsFromTextFile()
        {
            Dictionary<string, string> generalSettings = new Dictionary<string, string>();

            using (var reader = new StreamReader(Properties.Settings.Default.GeneralSettings))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] lineData = line.Split('=');
                    string key = lineData[0];
                    string value = lineData[1];

                    generalSettings.Add(key, value);

                }
                return generalSettings;
            }
        }


        private void chckGeneralInformationEntries_CheckedChanged(object sender, EventArgs e)
        {
            if (chckGeneralInformationEntries.CheckState == CheckState.Checked)
            {
                cbxExchanges.Enabled = true;
                cbxCurrencyPairList.Enabled = true;
                tbxReferralLink.Enabled = true;
                tbxReferralId.Enabled = true;
                tbxMakerCommission.Enabled = true;
                tbxTakerCommission.Enabled = true;
                btnSaveGeneralInformationEntries.Enabled = true;
            }
            else
            {
                cbxExchanges.Enabled = false;
                cbxCurrencyPairList.Enabled = false;
                tbxReferralLink.Enabled = false;
                tbxReferralId.Enabled = false;
                tbxMakerCommission.Enabled = false;
                tbxTakerCommission.Enabled = false;
                btnSaveGeneralInformationEntries.Enabled = false;
            }
        }

        private void btnSaveGeneralInformationEntries_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbxListOfTradeXmls.Text))
            {
                var xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);

                var generalSettings = _tradeModelManager.GetGeneralSettings(xmlFilePath);

                if (!string.IsNullOrEmpty(cbxExchanges.Text))
                {
                    generalSettings.Exchange = cbxExchanges.Text;
                }

                if (!string.IsNullOrEmpty(tbxReferralLink.Text))
                {
                    generalSettings.ReferralLink = tbxReferralLink.Text;
                }
                if (!string.IsNullOrEmpty(tbxReferralId.Text))
                {
                    generalSettings.ReferralId = tbxReferralId.Text;
                }

                generalSettings.TakerCommission = Convert.ToDecimal(tbxTakerCommission.Text.Replace(".", ","));
                generalSettings.MakerCommission = Convert.ToDecimal(tbxMakerCommission.Text.Replace(".", ","));


                _tradeModelManager.UpdateGeneralSettings(generalSettings, xmlFilePath);

                MessageBox.Show("Genel ayarlar kaydedildi!", "Bilgi",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                tbxTakerCommission.Enabled = false;
                btnSaveGeneralInformationEntries.Enabled = false;
                chckGeneralInformationEntries.Checked = false;

                LoadGeneralInformation();
            }
            else
            {
                MessageBox.Show("Lütfen trade listesini seçiniz!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void dgvTradeList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (e.RowIndex < 0)
            //    return;

            ////I supposed your button column is at index 0
            //if (e.ColumnIndex == 0)
            //{
            //    e.Paint(e.CellBounds, DataGridViewPaintParts.All);

            //    var w = Properties.Resources.SomeImage.Width;
            //    var h = Properties.Resources.SomeImage.Height;
            //    var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
            //    var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

            //    e.Graphics.DrawImage(someImage, new Rectangle(x, y, w, h));
            //    e.Handled = true;
            //}
        }

        private void chckPlusMinus_CheckedChanged(object sender, EventArgs e)
        {
            if (chckPlusMinus.CheckState == CheckState.Checked)
            {
                tbxPlusMinus.Enabled = true;
                btnPlusMinus.Enabled = true;
            }
            else
            {
                tbxPlusMinus.Enabled = false;
                btnPlusMinus.Enabled = false;
            }
        }

        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            string xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);
            try
            {
                var currentGeneralInformation = GetGeneralInformation();

                var newStartingBalance = Convert.ToDecimal(currentGeneralInformation.StartingBalance) +
                                         Convert.ToDecimal(tbxPlusMinus.Text.Replace(".", ","));

                if (newStartingBalance > 0)
                {
                    currentGeneralInformation.StartingBalance = newStartingBalance;


                    _tradeModelManager.UpdateGeneralInformation(currentGeneralInformation, xmlFilePath);
                    _tradeModelManager.CalculateGeneralInformation(xmlFilePath);
                    LoadGeneralInformation();

                    tbxPlusMinus.Text = "";
                    chckPlusMinus.CheckState = CheckState.Unchecked;
                }
                else
                {
                    MessageBox.Show("Başlangıç bakiyesi 0'dan küçük olamaz", "Uyarı!", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void cbxCurrencyPairList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbxListOfTradeXmls.Text))
            {
                string xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);
                string currencyPair = cbxCurrencyPairList2.Text;

                var currencyPairStatistic = _tradeModelManager.GetCurrencyPairStatisticByCurrencyPair(currencyPair, xmlFilePath);


                if (currencyPairStatistic != null)
                {
                    LoadCurrencyPairStatistic(currencyPair);
                }
                else
                {
                    ClearCurrencyPairStatistic();
                }
            }
            else
            {
                MessageBox.Show("Lütfen trade listesini seçiniz!", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
