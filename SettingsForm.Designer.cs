namespace TradeNote
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.pBoxCummulativeStatistics = new System.Windows.Forms.PictureBox();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.cbxPairList = new System.Windows.Forms.ComboBox();
            this.cbxExchanges = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pBoxCummulativeStatistics)).BeginInit();
            this.SuspendLayout();
            // 
            // pBoxCummulativeStatistics
            // 
            this.pBoxCummulativeStatistics.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pBoxCummulativeStatistics.Image = global::TradeNote.Properties.Resources.pnl_base;
            this.pBoxCummulativeStatistics.Location = new System.Drawing.Point(208, 72);
            this.pBoxCummulativeStatistics.Margin = new System.Windows.Forms.Padding(2);
            this.pBoxCummulativeStatistics.Name = "pBoxCummulativeStatistics";
            this.pBoxCummulativeStatistics.Size = new System.Drawing.Size(910, 474);
            this.pBoxCummulativeStatistics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBoxCummulativeStatistics.TabIndex = 0;
            this.pBoxCummulativeStatistics.TabStop = false;
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSaveImage.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveImage.Image")));
            this.btnSaveImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveImage.Location = new System.Drawing.Point(72, 518);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(64, 28);
            this.btnSaveImage.TabIndex = 258;
            this.btnSaveImage.Text = "Kaydet";
            this.btnSaveImage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // cbxPairList
            // 
            this.cbxPairList.FormattingEnabled = true;
            this.cbxPairList.Items.AddRange(new object[] {
            "1000LUNCBUSDPERP",
            "1000LUNCUSDTPERP",
            "1000SHIBBUSDPERP",
            "1000SHIBUSDTPERP",
            "1000XECUSDTPERP",
            "1INCHUSDTPERP",
            "AAVEUSDTPERP",
            "ADABUSDPERP",
            "ADAUSDTPERP",
            "AGIXBUSDPERP",
            "ALGOUSDTPERP",
            "ALICEUSDTPERP",
            "ALPHAUSDTPERP",
            "AMBBUSDPERP",
            "ANCBUSDPERP",
            "ANKRUSDTPERP",
            "ANTUSDTPERP",
            "APEBUSDPERP",
            "APEUSDTPERP",
            "API3USDTPERP",
            "APTBUSDPERP",
            "APTUSDTPERP",
            "ARPAUSDTPERP",
            "ARUSDTPERP",
            "ATAUSDTPERP",
            "ATOMUSDTPERP",
            "AUCTIONBUSDPERP",
            "AUDIOUSDTPERP",
            "AVAXBUSDPERP",
            "AVAXUSDTPERP",
            "AXSUSDTPERP",
            "BAKEUSDTPERP",
            "BALUSDTPERP",
            "BANDUSDTPERP",
            "BATUSDTPERP",
            "BCHUSDTPERP",
            "BELUSDTPERP",
            "BLUEBIRDUSDTPERP",
            "BLZUSDTPERP",
            "BNBBUSDPERP",
            "BNBUSDTPERP",
            "BNXUSDTPERP",
            "BTCBUSDPERP",
            "BTCDOMUSDTPERP",
            "BTCSTUSDTPERP",
            "BTCUSDTPERP",
            "BTCUSDTPERP",
            "BTSUSDTPERP",
            "C98USDTPERP",
            "CELOUSDTPERP",
            "CELRUSDTPERP",
            "CHRUSDTPERP",
            "CHZUSDTPERP",
            "COMPUSDTPERP",
            "COTIUSDTPERP",
            "CRVUSDTPERP",
            "CTKUSDTPERP",
            "CTSIUSDTPERP",
            "CVCUSDTPERP",
            "CVXBUSDPERP",
            "CVXUSDTPERP",
            "DARUSDTPERP",
            "DASHUSDTPERP",
            "DEFIUSDTPERP",
            "DENTUSDTPERP",
            "DGBUSDTPERP",
            "DODOBUSDPERP",
            "DOGEBUSDPERP",
            "DOGEUSDTPERP",
            "DOTBUSDPERP",
            "DOTUSDTPERP",
            "DUSKUSDTPERP",
            "DYDXUSDTPERP",
            "EGLDUSDTPERP",
            "ENJUSDTPERP",
            "ENSUSDTPERP",
            "EOSUSDTPERP",
            "ETCBUSDPERP",
            "ETCUSDTPERP",
            "ETHBUSDPERP",
            "ETHUSDTPERP",
            "ETHUSDTPERP",
            "FETUSDTPERP",
            "FILBUSDPERP",
            "FILUSDTPERP",
            "FLMUSDTPERP",
            "FLOWUSDTPERP",
            "FOOTBALLUSDTPERP",
            "FTMBUSDPERP",
            "FTMUSDTPERP",
            "FTTBUSDPERP",
            "FTTUSDTPERP",
            "FXSUSDTPERP",
            "GALABUSDPERP",
            "GALAUSDTPERP",
            "GALBUSDPERP",
            "GALUSDTPERP",
            "GMTBUSDPERP",
            "GMTUSDTPERP",
            "GRTUSDTPERP",
            "GTCUSDTPERP",
            "HBARUSDTPERP",
            "HNTUSDTPERP",
            "HOOKUSDTPERP",
            "HOTUSDTPERP",
            "ICPBUSDPERP",
            "ICPUSDTPERP",
            "ICXUSDTPERP",
            "IMXUSDTPERP",
            "INJUSDTPERP",
            "IOSTUSDTPERP",
            "IOTAUSDTPERP",
            "IOTXUSDTPERP",
            "JASMYUSDTPERP",
            "KAVAUSDTPERP",
            "KLAYUSDTPERP",
            "KNCUSDTPERP",
            "KSMUSDTPERP",
            "LDOBUSDPERP",
            "LDOUSDTPERP",
            "LEVERBUSDPERP",
            "LINAUSDTPERP",
            "LINKBUSDPERP",
            "LINKUSDTPERP",
            "LITUSDTPERP",
            "LPTUSDTPERP",
            "LRCUSDTPERP",
            "LTCBUSDPERP",
            "LTCUSDTPERP",
            "LUNA2BUSDPERP",
            "LUNA2USDTPERP",
            "MAGICUSDTPERP",
            "MANAUSDTPERP",
            "MASKUSDTPERP",
            "MATICBUSDPERP",
            "MATICUSDTPERP",
            "MINAUSDTPERP",
            "MKRUSDTPERP",
            "MTLUSDTPERP",
            "NEARBUSDPERP",
            "NEARUSDTPERP",
            "NEOUSDTPERP",
            "NKNUSDTPERP",
            "OCEANUSDTPERP",
            "OGNUSDTPERP",
            "OMGUSDTPERP",
            "ONEUSDTPERP",
            "ONTUSDTPERP",
            "OPUSDTPERP",
            "PEOPLEUSDTPERP",
            "PHBBUSDPERP",
            "QNTUSDTPERP",
            "QTUMUSDTPERP",
            "RAYUSDTPERP",
            "REEFUSDTPERP",
            "RENUSDTPERP",
            "RLCUSDTPERP",
            "RNDRUSDTPERP",
            "ROSEUSDTPERP",
            "RSRUSDTPERP",
            "RUNEUSDTPERP",
            "RVNUSDTPERP",
            "SANDBUSDPERP",
            "SANDUSDTPERP",
            "SCUSDTPERP",
            "SFPUSDTPERP",
            "SKLUSDTPERP",
            "SNXUSDTPERP",
            "SOLBUSDPERP",
            "SOLUSDTPERP",
            "SPELLUSDTPERP",
            "SRMUSDTPERP",
            "STGUSDTPERP",
            "STMXUSDTPERP",
            "STORJUSDTPERP",
            "SUSHIUSDTPERP",
            "SXPUSDTPERP",
            "THETAUSDTPERP",
            "TLMBUSDPERP",
            "TLMUSDTPERP",
            "TOMOUSDTPERP",
            "TRBUSDTPERP",
            "TRXBUSDPERP",
            "TRXUSDTPERP",
            "TUSDTPERP",
            "UNFIUSDTPERP",
            "UNIBUSDPERP",
            "UNIUSDTPERP",
            "VETUSDTPERP",
            "WAVESBUSDPERP",
            "WAVESUSDTPERP",
            "WOOUSDTPERP",
            "XEMUSDTPERP",
            "XLMUSDTPERP",
            "XMRUSDTPERP",
            "XRPBUSDPERP",
            "XRPUSDTPERP",
            "XTZUSDTPERP",
            "YFIUSDTPERP",
            "ZECUSDTPERP",
            "ZENUSDTPERP",
            "ZILUSDTPERP",
            "ZRXUSDTPERP"});
            this.cbxPairList.Location = new System.Drawing.Point(23, 72);
            this.cbxPairList.Name = "cbxPairList";
            this.cbxPairList.Size = new System.Drawing.Size(166, 21);
            this.cbxPairList.TabIndex = 259;
            this.cbxPairList.SelectedIndexChanged += new System.EventHandler(this.cbxPairList_SelectedIndexChanged);
            // 
            // cbxExchanges
            // 
            this.cbxExchanges.FormattingEnabled = true;
            this.cbxExchanges.Items.AddRange(new object[] {
            "BINANCE",
            "OKEX",
            "BITHUMB",
            "COINBASE",
            "BITFINEX"});
            this.cbxExchanges.Location = new System.Drawing.Point(23, 113);
            this.cbxExchanges.Name = "cbxExchanges";
            this.cbxExchanges.Size = new System.Drawing.Size(166, 21);
            this.cbxExchanges.TabIndex = 260;
            this.cbxExchanges.Text = "BINANCE";
            this.cbxExchanges.SelectedIndexChanged += new System.EventHandler(this.cbxExchanges_SelectedIndexChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 658);
            this.Controls.Add(this.cbxExchanges);
            this.Controls.Add(this.cbxPairList);
            this.Controls.Add(this.btnSaveImage);
            this.Controls.Add(this.pBoxCummulativeStatistics);
            this.Name = "SettingsForm";
            this.Text = "SettingsForm";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pBoxCummulativeStatistics)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pBoxCummulativeStatistics;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.ComboBox cbxPairList;
        private System.Windows.Forms.ComboBox cbxExchanges;
    }
}