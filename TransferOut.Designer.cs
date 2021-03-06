﻿namespace ERP
{
    partial class TransferOut
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
            this.txtTransferNumber = new System.Windows.Forms.TextBox();
            this.lblTransferNumber = new System.Windows.Forms.Label();
            this.dgCuonList = new System.Windows.Forms.DataGrid();
            this.progressLoading = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(171, 27);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(60, 20);
            this.btnReset.TabIndex = 16;
            this.btnReset.Text = "Reset";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(171, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 20);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtTransferNumber
            // 
            this.txtTransferNumber.Location = new System.Drawing.Point(4, 25);
            this.txtTransferNumber.Name = "txtTransferNumber";
            this.txtTransferNumber.Size = new System.Drawing.Size(161, 21);
            this.txtTransferNumber.TabIndex = 14;
            this.txtTransferNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransferNumber_KeyDown);
            // 
            // lblTransferNumber
            // 
            this.lblTransferNumber.Location = new System.Drawing.Point(3, 6);
            this.lblTransferNumber.Name = "lblTransferNumber";
            this.lblTransferNumber.Size = new System.Drawing.Size(134, 21);
            this.lblTransferNumber.Text = "TRANSFER NUMBER";
            // 
            // dgCuonList
            // 
            this.dgCuonList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgCuonList.Location = new System.Drawing.Point(4, 51);
            this.dgCuonList.Name = "dgCuonList";
            this.dgCuonList.Size = new System.Drawing.Size(232, 217);
            this.dgCuonList.TabIndex = 15;
            this.dgCuonList.CurrentCellChanged += new System.EventHandler(this.dgCuonList_CurrentCellChanged);
            this.dgCuonList.Click += new System.EventHandler(this.dgCuonList_Click);
            // 
            // progressLoading
            // 
            this.progressLoading.Location = new System.Drawing.Point(37, 93);
            this.progressLoading.Name = "progressLoading";
            this.progressLoading.Size = new System.Drawing.Size(164, 20);
            this.progressLoading.Visible = false;
            // 
            // TransferOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.Controls.Add(this.progressLoading);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtTransferNumber);
            this.Controls.Add(this.lblTransferNumber);
            this.Controls.Add(this.dgCuonList);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.Name = "TransferOut";
            this.Text = "TRANSFER OUT";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TransferOut_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TansferOut_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtTransferNumber;
        private System.Windows.Forms.Label lblTransferNumber;
        private System.Windows.Forms.DataGrid dgCuonList;
        private System.Windows.Forms.ProgressBar progressLoading;

    }
}