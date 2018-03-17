namespace ERP
{
    partial class XuatKhoLinhKienListID
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
            this.dgCuonList = new System.Windows.Forms.DataGrid();
            this.btnBack = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dgCuonList
            // 
            this.dgCuonList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgCuonList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgCuonList.Location = new System.Drawing.Point(4, 26);
            this.dgCuonList.Name = "dgCuonList";
            this.dgCuonList.RowHeadersVisible = false;
            this.dgCuonList.Size = new System.Drawing.Size(233, 285);
            this.dgCuonList.TabIndex = 16;
            this.dgCuonList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgCuonList_MouseDown);
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(4, 3);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(72, 20);
            this.btnBack.TabIndex = 17;
            this.btnBack.Text = "Quay lai";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // XuatKhoLinhKienListID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.dgCuonList);
            this.Location = new System.Drawing.Point(0, 0);
            this.MinimizeBox = false;
            this.Name = "XuatKhoLinhKienListID";
            this.Text = "XuatKhoLinhKienListID";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.XuatKhoLinhKienListID_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGrid dgCuonList;
        private System.Windows.Forms.Button btnBack;
    }
}