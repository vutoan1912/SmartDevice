namespace ERP
{
    partial class Main
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
            this.btnNoiLinhKien = new System.Windows.Forms.Button();
            this.btnXuatKhoLinhKien = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnVTTP = new System.Windows.Forms.Button();
            this.btnXuatKhoThanhPham = new System.Windows.Forms.Button();
            this.btnProductOut = new System.Windows.Forms.Button();
            this.btnNKHH = new System.Windows.Forms.Button();
            this.btnInventoryAdjustment = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnNoiLinhKien
            // 
            this.btnNoiLinhKien.Location = new System.Drawing.Point(39, 26);
            this.btnNoiLinhKien.Name = "btnNoiLinhKien";
            this.btnNoiLinhKien.Size = new System.Drawing.Size(166, 20);
            this.btnNoiLinhKien.TabIndex = 0;
            this.btnNoiLinhKien.Text = "Nối Linh Kiện";
            this.btnNoiLinhKien.Click += new System.EventHandler(this.btnNoiLinhKien_Click);
            // 
            // btnXuatKhoLinhKien
            // 
            this.btnXuatKhoLinhKien.Location = new System.Drawing.Point(39, 54);
            this.btnXuatKhoLinhKien.Name = "btnXuatKhoLinhKien";
            this.btnXuatKhoLinhKien.Size = new System.Drawing.Size(166, 20);
            this.btnXuatKhoLinhKien.TabIndex = 1;
            this.btnXuatKhoLinhKien.Text = "Xuất Kho LK";
            this.btnXuatKhoLinhKien.Click += new System.EventHandler(this.btnXuatKhoLinhKien_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(38, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 20);
            this.button1.TabIndex = 3;
            this.button1.Text = "Nhập Kho Linh Kiện";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnVTTP
            // 
            this.btnVTTP.Location = new System.Drawing.Point(39, 141);
            this.btnVTTP.Name = "btnVTTP";
            this.btnVTTP.Size = new System.Drawing.Size(166, 20);
            this.btnVTTP.TabIndex = 4;
            this.btnVTTP.Text = "Nhập Kho Thành Phẩm";
            this.btnVTTP.Click += new System.EventHandler(this.btnVTTP_Click);
            // 
            // btnXuatKhoThanhPham
            // 
            this.btnXuatKhoThanhPham.Location = new System.Drawing.Point(39, 83);
            this.btnXuatKhoThanhPham.Name = "btnXuatKhoThanhPham";
            this.btnXuatKhoThanhPham.Size = new System.Drawing.Size(166, 20);
            this.btnXuatKhoThanhPham.TabIndex = 5;
            this.btnXuatKhoThanhPham.Text = "Xuất Kho Thành Phẩm";
            // 
            // btnProductOut
            // 
            this.btnProductOut.Location = new System.Drawing.Point(38, 172);
            this.btnProductOut.Name = "btnProductOut";
            this.btnProductOut.Size = new System.Drawing.Size(166, 20);
            this.btnProductOut.TabIndex = 6;
            this.btnProductOut.Text = "Xuất Kho Hàng Hóa";
            this.btnProductOut.Click += new System.EventHandler(this.btnProductOut_Click);
            // 
            // btnNKHH
            // 
            this.btnNKHH.Location = new System.Drawing.Point(37, 201);
            this.btnNKHH.Name = "btnNKHH";
            this.btnNKHH.Size = new System.Drawing.Size(166, 20);
            this.btnNKHH.TabIndex = 7;
            this.btnNKHH.Text = "Nhập Kho Hàng Hóa";
            this.btnNKHH.Click += new System.EventHandler(this.btnNKHH_Click);
            // 
            // btnInventoryAdjustment
            // 
            this.btnInventoryAdjustment.Location = new System.Drawing.Point(37, 231);
            this.btnInventoryAdjustment.Name = "btnInventoryAdjustment";
            this.btnInventoryAdjustment.Size = new System.Drawing.Size(166, 20);
            this.btnInventoryAdjustment.TabIndex = 8;
            this.btnInventoryAdjustment.Text = "Kiểm Kê Hàng Hóa";
            this.btnInventoryAdjustment.Click += new System.EventHandler(this.btnInventoryAdjustment_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.btnInventoryAdjustment);
            this.Controls.Add(this.btnNKHH);
            this.Controls.Add(this.btnProductOut);
            this.Controls.Add(this.btnXuatKhoThanhPham);
            this.Controls.Add(this.btnVTTP);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnXuatKhoLinhKien);
            this.Controls.Add(this.btnNoiLinhKien);
            this.Name = "Main";
            this.Text = "ERP System";
            this.Closed += new System.EventHandler(this.Main_Closed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNoiLinhKien;
        private System.Windows.Forms.Button btnXuatKhoLinhKien;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnVTTP;
        private System.Windows.Forms.Button btnXuatKhoThanhPham;
        private System.Windows.Forms.Button btnProductOut;
        private System.Windows.Forms.Button btnNKHH;
        private System.Windows.Forms.Button btnInventoryAdjustment;
    }
}