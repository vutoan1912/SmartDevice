namespace ERP
{
    partial class XuatKhoLinhKienOption
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
            this.btnXuatKhoTuDo = new System.Windows.Forms.Button();
            this.btnXuatKhoChiDinh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnXuatKhoTuDo
            // 
            this.btnXuatKhoTuDo.Location = new System.Drawing.Point(37, 131);
            this.btnXuatKhoTuDo.Name = "btnXuatKhoTuDo";
            this.btnXuatKhoTuDo.Size = new System.Drawing.Size(166, 20);
            this.btnXuatKhoTuDo.TabIndex = 3;
            this.btnXuatKhoTuDo.Text = "Xuất kho tự do";
            this.btnXuatKhoTuDo.Click += new System.EventHandler(this.btnXuatKhoTuDo_Click);
            // 
            // btnXuatKhoChiDinh
            // 
            this.btnXuatKhoChiDinh.Location = new System.Drawing.Point(37, 87);
            this.btnXuatKhoChiDinh.Name = "btnXuatKhoChiDinh";
            this.btnXuatKhoChiDinh.Size = new System.Drawing.Size(166, 20);
            this.btnXuatKhoChiDinh.TabIndex = 2;
            this.btnXuatKhoChiDinh.Text = "Xuất kho chỉ định";
            this.btnXuatKhoChiDinh.Click += new System.EventHandler(this.btnXuatKhoChiDinh_Click);
            // 
            // XuatKhoLinhKienOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 294);
            this.Controls.Add(this.btnXuatKhoTuDo);
            this.Controls.Add(this.btnXuatKhoChiDinh);
            this.MinimizeBox = false;
            this.Name = "XuatKhoLinhKienOption";
            this.Text = "Xuat kho linh kien";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnXuatKhoTuDo;
        private System.Windows.Forms.Button btnXuatKhoChiDinh;
    }
}