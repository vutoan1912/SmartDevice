namespace ERP
{
    partial class InventoryAdjustment
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.txtInventoryName = new System.Windows.Forms.TextBox();
            this.lblTransferNumber = new System.Windows.Forms.Label();
            this.dgCuonList = new System.Windows.Forms.DataGrid();
            this.lblInventoryLocation = new System.Windows.Forms.Label();
            this.lblInventoryLocationValue = new System.Windows.Forms.Label();
            this.lblInventoryOfValue = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblHiddenFieldValue = new System.Windows.Forms.Label();
            this.lblHiddenField = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(175, 27);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 20);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "Exit";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Enabled = false;
            this.btnScan.Location = new System.Drawing.Point(175, 4);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(60, 20);
            this.btnScan.TabIndex = 17;
            this.btnScan.Text = "Scan";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // txtInventoryName
            // 
            this.txtInventoryName.Location = new System.Drawing.Point(4, 26);
            this.txtInventoryName.Name = "txtInventoryName";
            this.txtInventoryName.Size = new System.Drawing.Size(165, 21);
            this.txtInventoryName.TabIndex = 14;
            this.txtInventoryName.Text = "INV";
            this.txtInventoryName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransferNumber_KeyDown);
            // 
            // lblTransferNumber
            // 
            this.lblTransferNumber.Location = new System.Drawing.Point(3, 6);
            this.lblTransferNumber.Name = "lblTransferNumber";
            this.lblTransferNumber.Size = new System.Drawing.Size(134, 21);
            this.lblTransferNumber.Text = "Inventory Name";
            // 
            // dgCuonList
            // 
            this.dgCuonList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgCuonList.Location = new System.Drawing.Point(4, 117);
            this.dgCuonList.Name = "dgCuonList";
            this.dgCuonList.Size = new System.Drawing.Size(232, 151);
            this.dgCuonList.TabIndex = 15;
            this.dgCuonList.Visible = false;
            this.dgCuonList.Click += new System.EventHandler(this.dgCuonList_Click);
            // 
            // lblInventoryLocation
            // 
            this.lblInventoryLocation.Location = new System.Drawing.Point(4, 52);
            this.lblInventoryLocation.Name = "lblInventoryLocation";
            this.lblInventoryLocation.Size = new System.Drawing.Size(59, 21);
            this.lblInventoryLocation.Text = "Location:";
            // 
            // lblInventoryLocationValue
            // 
            this.lblInventoryLocationValue.Location = new System.Drawing.Point(6, 69);
            this.lblInventoryLocationValue.Name = "lblInventoryLocationValue";
            this.lblInventoryLocationValue.Size = new System.Drawing.Size(230, 21);
            // 
            // lblInventoryOfValue
            // 
            this.lblInventoryOfValue.Location = new System.Drawing.Point(83, 92);
            this.lblInventoryOfValue.Name = "lblInventoryOfValue";
            this.lblInventoryOfValue.Size = new System.Drawing.Size(153, 21);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 21);
            this.label2.Text = "Inventory of:";
            // 
            // lblHiddenFieldValue
            // 
            this.lblHiddenFieldValue.Location = new System.Drawing.Point(5, 139);
            this.lblHiddenFieldValue.Name = "lblHiddenFieldValue";
            this.lblHiddenFieldValue.Size = new System.Drawing.Size(231, 21);
            this.lblHiddenFieldValue.Visible = false;
            // 
            // lblHiddenField
            // 
            this.lblHiddenField.Location = new System.Drawing.Point(5, 117);
            this.lblHiddenField.Name = "lblHiddenField";
            this.lblHiddenField.Size = new System.Drawing.Size(142, 21);
            this.lblHiddenField.Text = "Hidden field";
            this.lblHiddenField.Visible = false;
            // 
            // InventoryAdjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.Controls.Add(this.lblHiddenFieldValue);
            this.Controls.Add(this.lblHiddenField);
            this.Controls.Add(this.lblInventoryOfValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblInventoryLocationValue);
            this.Controls.Add(this.lblInventoryLocation);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.txtInventoryName);
            this.Controls.Add(this.lblTransferNumber);
            this.Controls.Add(this.dgCuonList);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.Name = "InventoryAdjustment";
            this.Text = "Inventory Adjustment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.TansferOut_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.TextBox txtInventoryName;
        private System.Windows.Forms.Label lblTransferNumber;
        private System.Windows.Forms.DataGrid dgCuonList;
        private System.Windows.Forms.Label lblInventoryLocation;
        private System.Windows.Forms.Label lblInventoryLocationValue;
        private System.Windows.Forms.Label lblInventoryOfValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblHiddenFieldValue;
        private System.Windows.Forms.Label lblHiddenField;

    }
}