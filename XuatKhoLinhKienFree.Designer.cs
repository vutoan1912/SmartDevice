namespace ERP
{
    partial class XuatKhoLinhKienFree
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
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblDaXuat = new System.Windows.Forms.Label();
            this.lblDaQuet = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMaXuatKho = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgCuonList = new System.Windows.Forms.DataGrid();
            this.btnListID = new System.Windows.Forms.Button();
            this.checkOther = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(172, 24);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(65, 20);
            this.btnReset.TabIndex = 16;
            this.btnReset.Text = "Huy";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(172, 46);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(65, 20);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Luu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Enabled = false;
            this.btnLoad.Location = new System.Drawing.Point(172, 2);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(65, 20);
            this.btnLoad.TabIndex = 13;
            this.btnLoad.Text = "Tai DL";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblDaXuat
            // 
            this.lblDaXuat.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDaXuat.Location = new System.Drawing.Point(82, 61);
            this.lblDaXuat.Name = "lblDaXuat";
            this.lblDaXuat.Size = new System.Drawing.Size(84, 29);
            this.lblDaXuat.Text = "0/0";
            this.lblDaXuat.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDaQuet
            // 
            this.lblDaQuet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDaQuet.Location = new System.Drawing.Point(3, 61);
            this.lblDaQuet.Name = "lblDaQuet";
            this.lblDaQuet.Size = new System.Drawing.Size(73, 29);
            this.lblDaQuet.Text = "0/0";
            this.lblDaQuet.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(82, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.Text = "Đã xuất";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.Text = "Đang quét";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtMaXuatKho
            // 
            this.txtMaXuatKho.Location = new System.Drawing.Point(11, 23);
            this.txtMaXuatKho.Name = "txtMaXuatKho";
            this.txtMaXuatKho.Size = new System.Drawing.Size(155, 21);
            this.txtMaXuatKho.TabIndex = 14;
            this.txtMaXuatKho.Text = "PART-EXPORT-REQ-0";
            this.txtMaXuatKho.TextChanged += new System.EventHandler(this.txtMaXuatKho_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 21);
            this.label1.Text = "Số phiếu XK";
            // 
            // dgCuonList
            // 
            this.dgCuonList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgCuonList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgCuonList.Location = new System.Drawing.Point(4, 90);
            this.dgCuonList.Name = "dgCuonList";
            this.dgCuonList.Size = new System.Drawing.Size(233, 227);
            this.dgCuonList.TabIndex = 15;
            this.dgCuonList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgCuonList_MouseDown);
            // 
            // btnListID
            // 
            this.btnListID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnListID.Enabled = false;
            this.btnListID.Location = new System.Drawing.Point(172, 68);
            this.btnListID.Name = "btnListID";
            this.btnListID.Size = new System.Drawing.Size(65, 20);
            this.btnListID.TabIndex = 23;
            this.btnListID.Text = "List ID";
            this.btnListID.Click += new System.EventHandler(this.btnListID_Click);
            // 
            // checkOther
            // 
            this.checkOther.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular);
            this.checkOther.Location = new System.Drawing.Point(112, 3);
            this.checkOther.Name = "checkOther";
            this.checkOther.Size = new System.Drawing.Size(56, 20);
            this.checkOther.TabIndex = 29;
            this.checkOther.Text = "Other";
            this.checkOther.Click += new System.EventHandler(this.checkOther_Click);
            // 
            // XuatKhoLinhKienFree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.Controls.Add(this.checkOther);
            this.Controls.Add(this.btnListID);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.lblDaXuat);
            this.Controls.Add(this.lblDaQuet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMaXuatKho);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgCuonList);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.Name = "XuatKhoLinhKienFree";
            this.Text = "Xuat kho linh kien";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.XuatKhoLinhKienFree_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblDaXuat;
        private System.Windows.Forms.Label lblDaQuet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMaXuatKho;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGrid dgCuonList;
        private System.Windows.Forms.Button btnListID;
        private System.Windows.Forms.CheckBox checkOther;

    }
}