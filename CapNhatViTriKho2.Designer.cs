namespace ERP
{
    partial class CapNhatViTriKho2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CapNhatViTriKho2));
            this.lblViTri = new System.Windows.Forms.Label();
            this.lblVitriLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pbArrowVitri = new System.Windows.Forms.PictureBox();
            this.pbArrowLinhKien = new System.Windows.Forms.PictureBox();
            this.dgListCuon = new System.Windows.Forms.DataGrid();
            this.btnXoaVitri = new System.Windows.Forms.Button();
            this.lblStartMsgVitri = new System.Windows.Forms.Label();
            this.lblStartMsgLinhKien = new System.Windows.Forms.Label();
            this.lblSoKien = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblViTri
            // 
            this.lblViTri.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.lblViTri.Location = new System.Drawing.Point(48, 20);
            this.lblViTri.Name = "lblViTri";
            this.lblViTri.Size = new System.Drawing.Size(138, 23);
            this.lblViTri.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblVitriLabel
            // 
            this.lblVitriLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVitriLabel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.lblVitriLabel.Location = new System.Drawing.Point(78, 0);
            this.lblVitriLabel.Name = "lblVitriLabel";
            this.lblVitriLabel.Size = new System.Drawing.Size(86, 20);
            this.lblVitriLabel.Text = "Vi Tri";
            this.lblVitriLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(78, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.Text = "Linh Kien";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(35, 244);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(72, 22);
            this.btnReset.TabIndex = 12;
            this.btnReset.Text = "Huy";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.Location = new System.Drawing.Point(139, 244);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 22);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Luu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pbArrowVitri
            // 
            this.pbArrowVitri.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbArrowVitri.BackColor = System.Drawing.Color.Transparent;
            this.pbArrowVitri.Image = ((System.Drawing.Image)(resources.GetObject("pbArrowVitri.Image")));
            this.pbArrowVitri.Location = new System.Drawing.Point(34, 0);
            this.pbArrowVitri.Name = "pbArrowVitri";
            this.pbArrowVitri.Size = new System.Drawing.Size(43, 20);
            this.pbArrowVitri.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbArrowVitri.Visible = false;
            // 
            // pbArrowLinhKien
            // 
            this.pbArrowLinhKien.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pbArrowLinhKien.BackColor = System.Drawing.Color.Transparent;
            this.pbArrowLinhKien.Image = ((System.Drawing.Image)(resources.GetObject("pbArrowLinhKien.Image")));
            this.pbArrowLinhKien.Location = new System.Drawing.Point(34, 48);
            this.pbArrowLinhKien.Name = "pbArrowLinhKien";
            this.pbArrowLinhKien.Size = new System.Drawing.Size(43, 20);
            this.pbArrowLinhKien.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbArrowLinhKien.Visible = false;
            // 
            // dgListCuon
            // 
            this.dgListCuon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgListCuon.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgListCuon.Location = new System.Drawing.Point(3, 71);
            this.dgListCuon.Name = "dgListCuon";
            this.dgListCuon.RowHeadersVisible = false;
            this.dgListCuon.Size = new System.Drawing.Size(233, 164);
            this.dgListCuon.TabIndex = 15;
            this.dgListCuon.Visible = false;
            this.dgListCuon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgListCuon_MouseDown);
            // 
            // btnXoaVitri
            // 
            this.btnXoaVitri.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXoaVitri.Enabled = false;
            this.btnXoaVitri.Location = new System.Drawing.Point(191, 20);
            this.btnXoaVitri.Name = "btnXoaVitri";
            this.btnXoaVitri.Size = new System.Drawing.Size(45, 22);
            this.btnXoaVitri.TabIndex = 12;
            this.btnXoaVitri.Text = "Xoa";
            this.btnXoaVitri.Click += new System.EventHandler(this.btnXoaVitri_Click);
            // 
            // lblStartMsgVitri
            // 
            this.lblStartMsgVitri.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.lblStartMsgVitri.Location = new System.Drawing.Point(3, 19);
            this.lblStartMsgVitri.Name = "lblStartMsgVitri";
            this.lblStartMsgVitri.Size = new System.Drawing.Size(234, 23);
            this.lblStartMsgVitri.Text = "Moi ban quet ma vi tri";
            this.lblStartMsgVitri.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblStartMsgLinhKien
            // 
            this.lblStartMsgLinhKien.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.lblStartMsgLinhKien.Location = new System.Drawing.Point(17, 88);
            this.lblStartMsgLinhKien.Name = "lblStartMsgLinhKien";
            this.lblStartMsgLinhKien.Size = new System.Drawing.Size(205, 23);
            this.lblStartMsgLinhKien.Text = "Moi ban quet ma linh kien";
            this.lblStartMsgLinhKien.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSoKien
            // 
            this.lblSoKien.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.lblSoKien.Location = new System.Drawing.Point(170, 48);
            this.lblSoKien.Name = "lblSoKien";
            this.lblSoKien.Size = new System.Drawing.Size(66, 23);
            this.lblSoKien.Text = "0 kien";
            this.lblSoKien.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CapNhatViTriKho2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.lblSoKien);
            this.Controls.Add(this.lblStartMsgLinhKien);
            this.Controls.Add(this.dgListCuon);
            this.Controls.Add(this.pbArrowLinhKien);
            this.Controls.Add(this.pbArrowVitri);
            this.Controls.Add(this.lblStartMsgVitri);
            this.Controls.Add(this.lblViTri);
            this.Controls.Add(this.btnXoaVitri);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblVitriLabel);
            this.Name = "CapNhatViTriKho2";
            this.Text = "Nhap Kho Linh Kien";
            this.Load += new System.EventHandler(this.CapNhatViTriKho2_Load);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.CapNhatViTriKho2_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblViTri;
        private System.Windows.Forms.Label lblVitriLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.PictureBox pbArrowVitri;
        private System.Windows.Forms.PictureBox pbArrowLinhKien;
        private System.Windows.Forms.DataGrid dgListCuon;
        private System.Windows.Forms.Button btnXoaVitri;
        private System.Windows.Forms.Label lblStartMsgVitri;
        private System.Windows.Forms.Label lblStartMsgLinhKien;
        private System.Windows.Forms.Label lblSoKien;
    }
}