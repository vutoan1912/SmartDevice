namespace ERP
{
    partial class PackagingItems
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
            this.txtDestPackageNumber = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblDestinatonPackage = new System.Windows.Forms.Label();
            this.dgCuonList = new System.Windows.Forms.DataGrid();
            this.SuspendLayout();
            // 
            // txtDestPackageNumber
            // 
            this.txtDestPackageNumber.Location = new System.Drawing.Point(4, 28);
            this.txtDestPackageNumber.Name = "txtDestPackageNumber";
            this.txtDestPackageNumber.Size = new System.Drawing.Size(159, 21);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(169, 27);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(60, 20);
            this.btnClear.TabIndex = 21;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(169, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(60, 20);
            this.btnSave.TabIndex = 22;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblDestinatonPackage
            // 
            this.lblDestinatonPackage.Location = new System.Drawing.Point(3, 6);
            this.lblDestinatonPackage.Name = "lblDestinatonPackage";
            this.lblDestinatonPackage.Size = new System.Drawing.Size(134, 21);
            this.lblDestinatonPackage.Text = "Destination package";
            // 
            // dgCuonList
            // 
            this.dgCuonList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgCuonList.Location = new System.Drawing.Point(4, 51);
            this.dgCuonList.Name = "dgCuonList";
            this.dgCuonList.Size = new System.Drawing.Size(232, 217);
            this.dgCuonList.TabIndex = 20;
            this.dgCuonList.Click += new System.EventHandler(this.dgCuonList_Click);
            // 
            // PackagingItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.Controls.Add(this.txtDestPackageNumber);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblDestinatonPackage);
            this.Controls.Add(this.dgCuonList);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.Name = "PackagingItems";
            this.Text = "Packaging items";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PackagingItems_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.PackagingItems_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label txtDestPackageNumber;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblDestinatonPackage;
        private System.Windows.Forms.DataGrid dgCuonList;

    }
}