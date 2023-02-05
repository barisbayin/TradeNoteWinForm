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
            ((System.ComponentModel.ISupportInitialize)(this.pBoxCummulativeStatistics)).BeginInit();
            this.SuspendLayout();
            // 
            // pBoxCummulativeStatistics
            // 
            this.pBoxCummulativeStatistics.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pBoxCummulativeStatistics.Image = global::TradeNote.Properties.Resources.istockphoto_1299714007_612x612;
            this.pBoxCummulativeStatistics.Location = new System.Drawing.Point(180, 33);
            this.pBoxCummulativeStatistics.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pBoxCummulativeStatistics.Name = "pBoxCummulativeStatistics";
            this.pBoxCummulativeStatistics.Size = new System.Drawing.Size(948, 592);
            this.pBoxCummulativeStatistics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pBoxCummulativeStatistics.TabIndex = 0;
            this.pBoxCummulativeStatistics.TabStop = false;
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSaveImage.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveImage.Image")));
            this.btnSaveImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveImage.Location = new System.Drawing.Point(98, 418);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(64, 28);
            this.btnSaveImage.TabIndex = 258;
            this.btnSaveImage.Text = "Kaydet";
            this.btnSaveImage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1151, 658);
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
    }
}