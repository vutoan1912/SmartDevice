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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // btnNoiLinhKien
            // 
            this.btnNoiLinhKien.Location = new System.Drawing.Point(39, 160);
            this.btnNoiLinhKien.Name = "btnNoiLinhKien";
            this.btnNoiLinhKien.Size = new System.Drawing.Size(166, 20);
            this.btnNoiLinhKien.TabIndex = 0;
            this.btnNoiLinhKien.Text = "Nối Linh Kiện";
            this.btnNoiLinhKien.Visible = false;
            this.btnNoiLinhKien.Click += new System.EventHandler(this.btnNoiLinhKien_Click);
            // 
            // btnXuatKhoLinhKien
            // 
            this.btnXuatKhoLinhKien.Location = new System.Drawing.Point(39, 180);
            this.btnXuatKhoLinhKien.Name = "btnXuatKhoLinhKien";
            this.btnXuatKhoLinhKien.Size = new System.Drawing.Size(166, 20);
            this.btnXuatKhoLinhKien.TabIndex = 1;
            this.btnXuatKhoLinhKien.Text = "Xuất Kho LK";
            this.btnXuatKhoLinhKien.Visible = false;
            this.btnXuatKhoLinhKien.Click += new System.EventHandler(this.btnXuatKhoLinhKien_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(39, 220);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(166, 20);
            this.button1.TabIndex = 3;
            this.button1.Text = "Nhập Kho Linh Kiện";
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnVTTP
            // 
            this.btnVTTP.Location = new System.Drawing.Point(39, 240);
            this.btnVTTP.Name = "btnVTTP";
            this.btnVTTP.Size = new System.Drawing.Size(166, 20);
            this.btnVTTP.TabIndex = 4;
            this.btnVTTP.Text = "Nhập Kho Thành Phẩm";
            this.btnVTTP.Visible = false;
            this.btnVTTP.Click += new System.EventHandler(this.btnVTTP_Click);
            // 
            // btnXuatKhoThanhPham
            // 
            this.btnXuatKhoThanhPham.Location = new System.Drawing.Point(39, 200);
            this.btnXuatKhoThanhPham.Name = "btnXuatKhoThanhPham";
            this.btnXuatKhoThanhPham.Size = new System.Drawing.Size(166, 20);
            this.btnXuatKhoThanhPham.TabIndex = 5;
            this.btnXuatKhoThanhPham.Text = "Xuất Kho Thành Phẩm";
            this.btnXuatKhoThanhPham.Visible = false;
            // 
            // btnProductOut
            // 
            this.btnProductOut.Location = new System.Drawing.Point(39, 58);
            this.btnProductOut.Name = "btnProductOut";
            this.btnProductOut.Size = new System.Drawing.Size(166, 20);
            this.btnProductOut.TabIndex = 6;
            this.btnProductOut.Text = "Xuất Kho Hàng Hóa";
            this.btnProductOut.Click += new System.EventHandler(this.btnProductOut_Click);
            // 
            // btnNKHH
            // 
            this.btnNKHH.Location = new System.Drawing.Point(38, 95);
            this.btnNKHH.Name = "btnNKHH";
            this.btnNKHH.Size = new System.Drawing.Size(166, 20);
            this.btnNKHH.TabIndex = 7;
            this.btnNKHH.Text = "Nhập Kho Hàng Hóa";
            this.btnNKHH.Click += new System.EventHandler(this.btnNKHH_Click);
            // 
            // btnInventoryAdjustment
            // 
            this.btnInventoryAdjustment.Location = new System.Drawing.Point(38, 133);
            this.btnInventoryAdjustment.Name = "btnInventoryAdjustment";
            this.btnInventoryAdjustment.Size = new System.Drawing.Size(166, 20);
            this.btnInventoryAdjustment.TabIndex = 8;
            this.btnInventoryAdjustment.Text = "Kiểm Kê Hàng Hóa";
            this.btnInventoryAdjustment.Click += new System.EventHandler(this.btnInventoryAdjustment_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItem3);
            this.menuItem1.MenuItems.Add(this.menuItem4);
            this.menuItem1.MenuItems.Add(this.menuItem5);
            this.menuItem1.MenuItems.Add(this.menuItem7);
            this.menuItem1.Text = "Menu";
            // 
            // menuItem3
            // 
            this.menuItem3.Text = "Input";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Text = "Output";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Text = "Adjustment";
            this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Text = "Setting";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.btnInventoryAdjustment);
            this.Controls.Add(this.btnNKHH);
            this.Controls.Add(this.btnProductOut);
            this.Controls.Add(this.btnXuatKhoThanhPham);
            this.Controls.Add(this.btnVTTP);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnXuatKhoLinhKien);
            this.Controls.Add(this.btnNoiLinhKien);
            this.Menu = this.mainMenu1;
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
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem7;
    }
}