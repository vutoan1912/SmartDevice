namespace ERP
{
    partial class XuatKhoLinhKien
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
            this.dgCuonList = new System.Windows.Forms.DataGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMaXuatKho = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDaQuet = new System.Windows.Forms.Label();
            this.lblChuaQuet = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.dsCuonList = new System.Data.DataSet();
            this.btnFinish = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dsCuonList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgCuonList
            // 
            this.dgCuonList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgCuonList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgCuonList.Location = new System.Drawing.Point(4, 86);
            this.dgCuonList.Name = "dgCuonList";
            this.dgCuonList.RowHeadersVisible = false;
            this.dgCuonList.Size = new System.Drawing.Size(233, 231);
            this.dgCuonList.TabIndex = 3;
            this.dgCuonList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgCuonList_MouseDown);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.Text = "Ma XK:";
            // 
            // txtMaXuatKho
            // 
            this.txtMaXuatKho.Location = new System.Drawing.Point(58, 10);
            this.txtMaXuatKho.Name = "txtMaXuatKho";
            this.txtMaXuatKho.Size = new System.Drawing.Size(109, 25);
            this.txtMaXuatKho.TabIndex = 2;
            this.txtMaXuatKho.Text = "part-exp-no-0";
            this.txtMaXuatKho.TextChanged += new System.EventHandler(this.txtMaXuatKho_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.Text = "Da Quet";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(83, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.Text = "Chua Quet";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDaQuet
            // 
            this.lblDaQuet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblDaQuet.Location = new System.Drawing.Point(4, 63);
            this.lblDaQuet.Name = "lblDaQuet";
            this.lblDaQuet.Size = new System.Drawing.Size(73, 20);
            this.lblDaQuet.Text = "0/0";
            this.lblDaQuet.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblChuaQuet
            // 
            this.lblChuaQuet.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblChuaQuet.Location = new System.Drawing.Point(83, 63);
            this.lblChuaQuet.Name = "lblChuaQuet";
            this.lblChuaQuet.Size = new System.Drawing.Size(84, 20);
            this.lblChuaQuet.Text = "0/0";
            this.lblChuaQuet.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Enabled = false;
            this.btnLoad.Location = new System.Drawing.Point(181, 10);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(56, 20);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Tai DL";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dsCuonList
            // 
            this.dsCuonList.DataSetName = "dsCuonList";
            this.dsCuonList.Namespace = "";
            this.dsCuonList.Prefix = "";
            this.dsCuonList.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnFinish
            // 
            this.btnFinish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinish.Enabled = false;
            this.btnFinish.Location = new System.Drawing.Point(181, 60);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(56, 20);
            this.btnFinish.TabIndex = 7;
            this.btnFinish.Text = "Luu";
            this.btnFinish.Click += new System.EventHandler(this.btnFinish_Click);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(181, 34);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(56, 20);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "Huy";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // XuatKhoLinhKien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.lblChuaQuet);
            this.Controls.Add(this.lblDaQuet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMaXuatKho);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgCuonList);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.Name = "XuatKhoLinhKien";
            this.Text = "Xuat Kho Linh Kien";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.XuatKhoLinhKien_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.dsCuonList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid dgCuonList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtMaXuatKho;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDaQuet;
        private System.Windows.Forms.Label lblChuaQuet;
        private System.Windows.Forms.Button btnLoad;
        private System.Data.DataSet dsCuonList;
        private System.Windows.Forms.Button btnFinish;
        private System.Windows.Forms.Button btnReset;
    }
}