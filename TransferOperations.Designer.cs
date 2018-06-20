namespace ERP
{
    partial class TransferOperations
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
            this.btnScan = new System.Windows.Forms.Button();
            this.txtTransferNumber = new System.Windows.Forms.TextBox();
            this.lblTransferNumber = new System.Windows.Forms.Label();
            this.dgList = new System.Windows.Forms.DataGrid();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(173, 27);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(60, 20);
            this.btnReset.TabIndex = 16;
            this.btnReset.Text = "Clear";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Location = new System.Drawing.Point(173, 4);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(60, 20);
            this.btnScan.TabIndex = 17;
            this.btnScan.Text = "Scan";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // txtTransferNumber
            // 
            this.txtTransferNumber.Location = new System.Drawing.Point(4, 24);
            this.txtTransferNumber.Name = "txtTransferNumber";
            this.txtTransferNumber.Size = new System.Drawing.Size(155, 21);
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
            // dgList
            // 
            this.dgList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgList.Location = new System.Drawing.Point(4, 51);
            this.dgList.Name = "dgList";
            this.dgList.Size = new System.Drawing.Size(232, 217);
            this.dgList.TabIndex = 15;
            // 
            // TransferOperations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.txtTransferNumber);
            this.Controls.Add(this.lblTransferNumber);
            this.Controls.Add(this.dgList);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.Name = "TransferOperations";
            this.Text = "TRANSFER OPERATIONS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TransferOperations_Load);
            this.Activated += new System.EventHandler(this.TransferOperations_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TansferOut_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.TextBox txtTransferNumber;
        private System.Windows.Forms.Label lblTransferNumber;
        private System.Windows.Forms.DataGrid dgList;

    }
}