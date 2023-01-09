using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TradeNote.Business;
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

        private int clickedColumnIndex;
        private int clickedRowIndex;

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

        private void LoadDataGridView()
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


        private void cbxListOfTradeXmls_SelectedValueChanged_1(object sender, EventArgs e)
        {
            LoadDataGridView();
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
            LoadDataGridView();
            PopulateComboBoxWithXmlFiles();
            _tradeModelManager.CalculateGeneralInformation(GeneralHelper.GetXmlFilePath(cbxListOfTradeXmls.Text));
        }

        private void dgvTradeList_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                clickedColumnIndex = e.ColumnIndex;
                clickedRowIndex = e.RowIndex;

                lblTradeIdLabel.Text = dgvTradeList.Rows[e.RowIndex].Cells["Id"].Value.ToString();
                cbxPositionSide.Text = dgvTradeList.Rows[e.RowIndex].Cells["PositionSide"].Value.ToString();
                cbxLeverage.Text = dgvTradeList.Rows[e.RowIndex].Cells["Leverage"].Value.ToString();
                tbxTargetedEntryPrice.Text = dgvTradeList.Rows[e.RowIndex].Cells["TargetedEntryPrice"].Value.ToString();
                tbxStopPrice.Text = dgvTradeList.Rows[e.RowIndex].Cells["StopLossPrice"].Value.ToString();
                tbxTakeProfitPrice.Text = dgvTradeList.Rows[e.RowIndex].Cells["TakeProfitPrice"].Value.ToString();
                rtbxTradeNote.Text = dgvTradeList.Rows[e.RowIndex].Cells["Note"].Value.ToString();
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
            dateTradeDate.Value = DateTime.Now;
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
                MessageBox.Show(exception.Message);
            }


            LoadDataGridView();

        }



        private void dgvTradeList_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {

            // Verileri Trade sınıfından oluşan bir liste olarak düşünün
            var data = (List<Trade>)dgvTradeList.DataSource;

            // Seçili satırın Trade nesnesini al
            var trade = data[e.RowIndex];

            // Eğer PositionResult "TP" ise, satırı yeşil olarak boya
            if (trade.PositionResult == PositionResult.TP)
            {
                dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.LightGreen;
            }
            // Eğer PositionResult "SL" ise, satırı kırmızı olarak boya
            else if (trade.PositionResult == PositionResult.SL)
            {
                dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                dgvTradeList.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.LightCoral;
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
    }
}
