﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TradeNote.Business;
using TradeNote.Entities;
using TradeNote.Enums;
using TradeNote.Helpers;
using TradeNote.Repositories;

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
            PopulateComboBoxWithXmlFiles();
        }


        public void PopulateComboBoxWithXmlFiles()
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

            tbxNewTradeXmlName.Text = "";
            tbxNewTradeXmlName.Enabled = false;
            btnNewTradeXml.Enabled = true;
            btnCancelToSaveTradeXml.Enabled = false;
            btnSaveTradeXml.Enabled = false;

            PopulateComboBoxWithXmlFiles();
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

                    dgvTradeList.Columns["EndTrade"].Visible = false;
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
            dgvTradeDetails.DataSource = null;
        }

        private void btnDeleteXmlFile_Click(object sender, EventArgs e)
        {
            if (cbxListOfTradeXmls.Text == null || cbxListOfTradeXmls.Text != "")
            {
                var result = MessageBox.Show(cbxListOfTradeXmls.Text + " listesini silmek istiyor musunuz?", "Uyarı",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    XmlRepository.DeleteXmlFile(cbxListOfTradeXmls.Text);
                    MessageBox.Show(cbxListOfTradeXmls.Text + " silindi!", "Bilgi",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    PopulateComboBoxWithXmlFiles();
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
            LoadTradeDataGridView();
            PopulateComboBoxWithXmlFiles();
            _tradeModelManager.CalculateGeneralInformation(GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text));
        }

        private void dgvTradeList_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                tradeClickedColumnIndex = e.ColumnIndex;
                tradeClickedRowIndex = e.RowIndex;

                lblTradeIdLabel.Text = dgvTradeList.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                cbxPositionSide.Text = dgvTradeList.Rows[e.RowIndex].Cells["PositionSide"].Value.ToString();
                cbxLeverage.Text = dgvTradeList.Rows[e.RowIndex].Cells["Leverage"].Value.ToString();
                tbxTargetedEntryPrice.Text = dgvTradeList.Rows[e.RowIndex].Cells["TargetedEntryPrice"].Value.ToString();
                tbxStopPrice.Text = dgvTradeList.Rows[e.RowIndex].Cells["StopLossPrice"].Value.ToString();
                tbxTakeProfitPrice.Text = dgvTradeList.Rows[e.RowIndex].Cells["TakeProfitPrice"].Value.ToString();
                rtbxTradeNote.Text = dgvTradeList.Rows[e.RowIndex].Cells["Note"].Value.ToString();
                lblTradeDetailTradeIdLabel.Text = dgvTradeList.Rows[e.RowIndex].Cells["Id"].Value.ToString();

                LoadTradeDetailDataGridView((int)dgvTradeList.Rows[e.RowIndex].Cells["Id"].Value);


                if (dgvTradeList.Rows[e.RowIndex].Cells["PositionSide"].Value.ToString() == "Long")
                {
                    cbxTradeType.Items.Clear();
                    cbxTradeType.Items.Add("OpenLong");
                    cbxTradeType.Items.Add("CloseLong");
                }
                if (dgvTradeList.Rows[e.RowIndex].Cells["PositionSide"].Value.ToString() == "Short")
                {
                    cbxTradeType.Items.Clear();
                    cbxTradeType.Items.Add("OpenShort");
                    cbxTradeType.Items.Add("CloseShort");
                }

            }
            catch (Exception exception)
            {

            }
        }

        private void ClearTrade()
        {
            lblTradeIdLabel.Text = "";
            cbxPositionSide.Text = "";
            cbxLeverage.Text = "";
            tbxTargetedEntryPrice.Text = "";
            tbxStopPrice.Text = "";
            tbxTakeProfitPrice.Text = "";
            rtbxTradeNote.Text = "";
        }
        private void ClearTradeDetails()
        {
            lblTradeDetailIdLabel.Text = "";
            lblTradeDetailTradeIdLabel.Text = "";
            dateTradeDetailDate.Value = DateTime.Now;
            cbxTradeType.Text = "";
            tbxTradeEntryBalance.Text = "";
            tbxTradeEntryPrice.Text = "";
            tbxTradeEntryLotCount.Text = "";
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

                ClearTrade();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Hata", MessageBoxButtons.OK);
            }
            return generalInformation;

        }

        private void LoadGeneralInformation(GeneralInformation generalInformation)
        {
            lblStartBalanceText.Text = "$" + generalInformation.StartingBalance.ToString();
            lblLastBalanceLabel.Text = "$" + generalInformation.LastBalance.ToString();
            lblProfitsSumLabel.Text = "$" + generalInformation.ProfitsSum.ToString();
            lblLossesSumLabel.Text = "$" + generalInformation.LossesSum.ToString();
            lblTotalPnLLabel.Text = "$" + generalInformation.TotalPnL.ToString();
            lblWinCountLabel.Text = generalInformation.WinCount.ToString();
            lblLossCountLabel.Text = generalInformation.LossCount.ToString();
            lblWinrateLabel.Text = "% " + generalInformation.TradeWinRate.ToString();
            lblTotalPnLPercentLabel.Text = "% " + generalInformation.TotalPnLPercent.ToString();
        }



        private void btnTradeSave_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbxListOfTradeXmls.Text))
            {
                string xmlFilePath = GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text);

                try
                {
                    if (string.IsNullOrEmpty(lblTradeIdLabel.Text))
                    {
                        Trade newTrade = new Trade();

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

                    }
                    else
                    {
                        var foundTrade = _tradeModelManager.GetTradeById(Convert.ToInt32(lblTradeIdLabel.Text), xmlFilePath);

                        if (foundTrade.TradeDetails.Count == 0)
                        {
                            foundTrade.Leverage = Convert.ToInt32(cbxLeverage.Text);
                            foundTrade.TargetedEntryPrice = Convert.ToDecimal(tbxTargetedEntryPrice.Text.Replace(".", ","));
                            foundTrade.StopLossPrice = Convert.ToDecimal(tbxStopPrice.Text.Replace(".", ","));
                            foundTrade.TakeProfitPrice = Convert.ToDecimal(tbxTakeProfitPrice.Text.Replace(".", ","));
                            foundTrade.Note = rtbxTradeNote.Text;

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
                            foundTrade.EndTrade = chckEndTrade.Checked;
                            _tradeModelManager.UpdateTrade(foundTrade, xmlFilePath);
                            var calculatedTrade = _tradeModelManager.CalculateTrade(foundTrade.Id, xmlFilePath);
                            _tradeModelManager.UpdateTrade(calculatedTrade, xmlFilePath);

                            if (cbxPositionSide.Text != foundTrade.PositionSide.ToString())
                            {
                                MessageBox.Show("İşlem başladıktan sonra posizyon yönü değiştirilemez!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }


                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Lütfen yeni bir trade girişi için tüm alanları doldurunuz. Mevcut bir trade'i güncellemek için ilgili trade satırını seçiniz." + "\n(Sistem mesajı: " + exception.Message + ")", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


                LoadTradeDataGridView();
            }
            else
            {
                MessageBox.Show("Lütfen trade listesini yükleyiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void LoadGeneralInformation()
        {

        }


        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            try
            {
                var currentGeneralInformation = GetGeneralInformation();

                var newStartingBalance = Convert.ToDecimal(currentGeneralInformation.StartingBalance) +
                                         Convert.ToDecimal(tbxPlusMinus.Text.Replace(".", ","));
                if (newStartingBalance > 0)
                {
                    var newGeneralInformation = new GeneralInformation()
                    {
                        StartingBalance = newStartingBalance,
                        LastBalance = currentGeneralInformation.LastBalance,
                        ProfitsSum = currentGeneralInformation.ProfitsSum,
                        LossesSum = currentGeneralInformation.LossesSum,
                        TotalPnL = currentGeneralInformation.TotalPnL,
                        TotalPnLPercent = currentGeneralInformation.TotalPnLPercent,
                        WinCount = currentGeneralInformation.WinCount,
                        LossCount = currentGeneralInformation.LossCount,
                        TradeWinRate = currentGeneralInformation.TradeWinRate
                    };

                    _tradeModelManager.UpdateGeneralInformation(newGeneralInformation, GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text));
                    LoadGeneralInformation(newGeneralInformation);

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


        private void SetDataGridColumns()
        {
            /*
            dgvTradeList.AutoGenerateColumns = false;

            DataGridViewColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "Id";
            idColumn.HeaderText = "ID";
            dgvTradeList.Columns.Add(idColumn);

            DataGridViewColumn tradeDateColumn = new DataGridViewTextBoxColumn();
            tradeDateColumn.DataPropertyName = "TradeDate";
            tradeDateColumn.HeaderText = "Tarih";
            dgvTradeList.Columns.Add(tradeDateColumn);

            DataGridViewColumn positionSideColumn = new DataGridViewTextBoxColumn();
            positionSideColumn.DataPropertyName = "PositionSide";
            positionSideColumn.HeaderText = "Yön";
            dgvTradeList.Columns.Add(positionSideColumn);

            DataGridViewColumn entryBalanceColumn = new DataGridViewTextBoxColumn();
            entryBalanceColumn.DataPropertyName = "EntryBalance";
            entryBalanceColumn.HeaderText = "Giriş Bakiyesi";
            dgvTradeList.Columns.Add(entryBalanceColumn);

            DataGridViewColumn leverageColumn = new DataGridViewTextBoxColumn();
            leverageColumn.DataPropertyName = "Leverage";
            leverageColumn.HeaderText = "Kaldıraç";
            dgvTradeList.Columns.Add(leverageColumn);

            DataGridViewColumn entryPriceColumn = new DataGridViewTextBoxColumn();
            entryPriceColumn.DataPropertyName = "EntryPrice";
            entryPriceColumn.HeaderText = "Giriş Fiyatı";
            dgvTradeList.Columns.Add(entryPriceColumn);

            DataGridViewColumn stopLossPriceColumn = new DataGridViewTextBoxColumn();
            stopLossPriceColumn.DataPropertyName = "StopLossPrice";
            stopLossPriceColumn.HeaderText = "Stop Fiyatı";
            dgvTradeList.Columns.Add(stopLossPriceColumn);

            DataGridViewColumn takeProfitPriceColumn = new DataGridViewTextBoxColumn();
            takeProfitPriceColumn.DataPropertyName = "TakeProfitPrice";
            takeProfitPriceColumn.HeaderText = "TP Fiyatı";
            dgvTradeList.Columns.Add(takeProfitPriceColumn);

            DataGridViewColumn positionClosePriceColumn = new DataGridViewTextBoxColumn();
            positionClosePriceColumn.DataPropertyName = "PositionClosePrice";
            positionClosePriceColumn.HeaderText = "Poz. Kap. Fiyatı";
            dgvTradeList.Columns.Add(positionClosePriceColumn);

            DataGridViewColumn riskPercentColumn = new DataGridViewTextBoxColumn();
            riskPercentColumn.DataPropertyName = "RiskPercent";
            riskPercentColumn.HeaderText = "Risk %";
            dgvTradeList.Columns.Add(riskPercentColumn);

            DataGridViewColumn rewardPercentColumn = new DataGridViewTextBoxColumn();
            rewardPercentColumn.DataPropertyName = "RewardPercent";
            rewardPercentColumn.HeaderText = "Kazanç %";
            dgvTradeList.Columns.Add(rewardPercentColumn);

            DataGridViewColumn riskRewardRatioColumn = new DataGridViewTextBoxColumn();
            riskRewardRatioColumn.DataPropertyName = "RiskRewardRatio";
            riskRewardRatioColumn.HeaderText = "Risk/Kazanç";
            dgvTradeList.Columns.Add(riskRewardRatioColumn);

            DataGridViewColumn expectedRiskValueColumn = new DataGridViewTextBoxColumn();
            expectedRiskValueColumn.DataPropertyName = "ExpectedRiskValue";
            expectedRiskValueColumn.HeaderText = "Tahmini Risk $";
            dgvTradeList.Columns.Add(expectedRiskValueColumn);

            DataGridViewColumn expectedRewardValueColumn = new DataGridViewTextBoxColumn();
            expectedRewardValueColumn.DataPropertyName = "ExpectedRewardValue";
            expectedRewardValueColumn.HeaderText = "Tahmini Kazanç $";
            dgvTradeList.Columns.Add(expectedRewardValueColumn);

            DataGridViewColumn positionResultColumn = new DataGridViewTextBoxColumn();
            positionResultColumn.DataPropertyName = "PositionResult";
            positionResultColumn.HeaderText = "Poz. Sonuç";
            dgvTradeList.Columns.Add(positionResultColumn);

            DataGridViewColumn profitOrLossColumn = new DataGridViewTextBoxColumn();
            profitOrLossColumn.DataPropertyName = "ProfitOrLoss";
            profitOrLossColumn.HeaderText = "PnL $";
            dgvTradeList.Columns.Add(profitOrLossColumn);

            DataGridViewColumn profitOrLossPercentColumn = new DataGridViewTextBoxColumn();
            profitOrLossPercentColumn.DataPropertyName = "ProfitOrLossPercent";
            profitOrLossPercentColumn.HeaderText = "PnL %";
            dgvTradeList.Columns.Add(profitOrLossPercentColumn);

            DataGridViewColumn noteColumn = new DataGridViewTextBoxColumn();
            noteColumn.DataPropertyName = "Note";
            noteColumn.HeaderText = "Not";
            dgvTradeList.Columns.Add(noteColumn);
            */
        }

        private void btnNewTrade_Click(object sender, EventArgs e)
        {
            ClearTrade();
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
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.DarkOliveGreen;
                    break;
                // Eğer PositionResult "SL" ise, satırı kırmızı olarak boya
                case PositionResult.SL:
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.OrangeRed;
                    break;
                case PositionResult.SO:
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.LightSlateGray;
                    break;
            }

        }

        private void btnSaveTradeDetail_Click(object sender, EventArgs e)
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
                else
                {
                    tradeId = Convert.ToInt32(lblTradeDetailTradeIdLabel.Text);
                }


                if (!string.IsNullOrEmpty(lblTradeDetailIdLabel.Text))
                {
                    tradeDetailId = Convert.ToInt32(lblTradeDetailIdLabel.Text);
                }

                TradeDetail newTradeDetail = new TradeDetail();

                if (string.IsNullOrEmpty(lblTradeDetailIdLabel.Text))
                {

                    newTradeDetail.TradeId = Convert.ToInt32(lblTradeDetailTradeIdLabel.Text);
                    newTradeDetail.TradeDate = Convert.ToDateTime(dateTradeDetailDate.Value);
                    newTradeDetail.TradeType = (TradeType)Enum.Parse(typeof(TradeType), cbxTradeType.Text);
                    newTradeDetail.EntryPrice = Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ","));

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

                    _tradeModelManager.AddTradeDetail(newTradeDetail.TradeId, newTradeDetail, xmlFilePath);

                    var updatedTrade = _tradeModelManager.CalculateTrade(tradeId, xmlFilePath);

                    _tradeModelManager.UpdateTrade(updatedTrade, xmlFilePath);
                }
                else
                {
                    TradeDetail foundTradeDetail = _tradeModelManager.GetTradeDetailById(tradeId, tradeDetailId, xmlFilePath);

                    foundTradeDetail.TradeId = Convert.ToInt32(lblTradeDetailTradeIdLabel.Text);
                    foundTradeDetail.TradeDate = Convert.ToDateTime(dateTradeDetailDate.Value);
                    foundTradeDetail.TradeType = (TradeType)Enum.Parse(typeof(TradeType), cbxTradeType.Text);
                    foundTradeDetail.EntryPrice = Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ","));

                    if (!chckEntryLotCount.Checked)
                    {
                        foundTradeDetail.EntryBalance = Convert.ToDecimal(tbxTradeEntryBalance.Text.Replace(".", ","));
                        foundTradeDetail.EntryLotCount = Math.Round(Convert.ToDecimal(tbxTradeEntryBalance.Text.Replace(".", ",")) / Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ",")), 8);
                    }
                    else
                    {
                        foundTradeDetail.EntryLotCount = Convert.ToDecimal(tbxTradeEntryLotCount.Text.Replace(".", ","));
                        foundTradeDetail.EntryBalance = Math.Round(Convert.ToDecimal(tbxTradeEntryPrice.Text.Replace(".", ",")) *
                                                                   Convert.ToDecimal(tbxTradeEntryLotCount.Text.Replace(".", ",")), 2);
                    }

                    _tradeModelManager.UpdateTradeDetail(foundTradeDetail.TradeId, foundTradeDetail, xmlFilePath);


                    var updatedTrade = _tradeModelManager.CalculateTrade(tradeId, xmlFilePath);

                    _tradeModelManager.UpdateTrade(updatedTrade, xmlFilePath);
                }

                LoadTradeDataGridView();
                LoadTradeDetailDataGridView(tradeId);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void btnNewTradeDetail_Click(object sender, EventArgs e)
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
            try
            {
                tradeClickedColumnIndex = e.ColumnIndex;
                tradeDetailClickedRowIndex = e.RowIndex;

                lblTradeDetailIdLabel.Text = dgvTradeDetails.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                dateTradeDetailDate.Text = dgvTradeDetails.Rows[e.RowIndex].Cells["TradeDate"].Value.ToString();
                cbxTradeType.Text = dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Value.ToString();
                tbxTradeEntryBalance.Text = dgvTradeDetails.Rows[e.RowIndex].Cells["EntryBalance"].Value.ToString();
                tbxTradeEntryLotCount.Text = dgvTradeDetails.Rows[e.RowIndex].Cells["EntryLotCount"].Value.ToString();
                tbxTradeEntryPrice.Text = dgvTradeDetails.Rows[e.RowIndex].Cells["EntryPrice"].Value.ToString();
                lblTradeDetailTradeIdLabel.Text = dgvTradeDetails.Rows[e.RowIndex].Cells["TradeId"].Value.ToString();

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
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.SelectionBackColor = Color.DarkOliveGreen;
                    break;
                case TradeType.OpenShort:
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.BackColor = Color.LightGreen;
                    dgvTradeDetails.Rows[e.RowIndex].Cells["TradeType"].Style.SelectionBackColor = Color.DarkOliveGreen;
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
    }
}
