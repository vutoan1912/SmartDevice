namespace ERP
{
    partial class TransferDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblScan = new System.Windows.Forms.Label();
            this.dgCuonList = new System.Windows.Forms.DataGrid();
            this.progressLoading = new System.Windows.Forms.ProgressBar();
            this.btnBack = new System.Windows.Forms.Button();
            this.lblLoading = new System.Windows.Forms.Label();
            this.lblScanValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(95, 28);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(60, 20);
            this.btnReset.TabIndex = 16;
            this.btnReset.Text = "Clear";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(6, 28);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 20);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblScan
            // 
            this.lblScan.Location = new System.Drawing.Point(4, 5);
            this.lblScan.Name = "lblScan";
            this.lblScan.Size = new System.Drawing.Size(73, 21);
            this.lblScan.Text = "TRANSFER";
            // 
            // dgCuonList
            // 
            this.dgCuonList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgCuonList.Location = new System.Drawing.Point(4, 51);
            this.dgCuonList.Name = "dgCuonList";
            this.dgCuonList.Size = new System.Drawing.Size(232, 217);
            this.dgCuonList.TabIndex = 15;
            this.dgCuonList.Click += new System.EventHandler(this.dgCuonList_Click);
            // 
            // progressLoading
            // 
            this.progressLoading.Location = new System.Drawing.Point(37, 100);
            this.progressLoading.Name = "progressLoading";
            this.progressLoading.Size = new System.Drawing.Size(164, 20);
            this.progressLoading.Visible = false;
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.Location = new System.Drawing.Point(178, 28);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(60, 20);
            this.btnBack.TabIndex = 19;
            this.btnBack.Text = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // lblLoading
            // 
            this.lblLoading.Location = new System.Drawing.Point(83, 81);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(79, 19);
            this.lblLoading.Text = "Processing...";
            this.lblLoading.Visible = false;
            // 
            // lblScanValue
            // 
            this.lblScanValue.Location = new System.Drawing.Point(64, 5);
            this.lblScanValue.Name = "lblScanValue";
            this.lblScanValue.Size = new System.Drawing.Size(73, 21);
            // 
            // TransferDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.Controls.Add(this.lblScanValue);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.progressLoading);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblScan);
            this.Controls.Add(this.dgCuonList);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.Name = "TransferDetails";
            this.Text = "TRANSFER DETAILS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TransferDetails_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TransferDetails_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblScan;
        private System.Windows.Forms.DataGrid dgCuonList;
        private System.Windows.Forms.ProgressBar progressLoading;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.Label lblScanValue;

    }
}