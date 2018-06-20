namespace ERP
{
    partial class DashBoardTransfer
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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.dgListTransferBot = new System.Windows.Forms.DataGrid();
            this.dgListTransferTop = new System.Windows.Forms.DataGrid();
            this.txtTransferNumber = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // menuItem8
            // 
            this.menuItem8.Text = "";
            // 
            // dgListTransferBot
            // 
            this.dgListTransferBot.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgListTransferBot.Location = new System.Drawing.Point(4, 148);
            this.dgListTransferBot.Name = "dgListTransferBot";
            this.dgListTransferBot.Size = new System.Drawing.Size(232, 120);
            this.dgListTransferBot.TabIndex = 16;
            this.dgListTransferBot.Click += new System.EventHandler(this.dgListTransferBot_Click);
            // 
            // dgListTransferTop
            // 
            this.dgListTransferTop.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dgListTransferTop.Location = new System.Drawing.Point(4, 28);
            this.dgListTransferTop.Name = "dgListTransferTop";
            this.dgListTransferTop.Size = new System.Drawing.Size(232, 120);
            this.dgListTransferTop.TabIndex = 17;
            this.dgListTransferTop.Click += new System.EventHandler(this.dgListTransferTop_Click);
            // 
            // txtTransferNumber
            // 
            this.txtTransferNumber.Location = new System.Drawing.Point(4, 4);
            this.txtTransferNumber.Name = "txtTransferNumber";
            this.txtTransferNumber.Size = new System.Drawing.Size(165, 21);
            this.txtTransferNumber.TabIndex = 18;
            this.txtTransferNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTransferNumber_KeyDown);
            // 
            // DashBoardTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.Controls.Add(this.txtTransferNumber);
            this.Controls.Add(this.dgListTransferTop);
            this.Controls.Add(this.dgListTransferBot);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "DashBoardTransfer";
            this.Text = "DashBoard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.DashBoardTransfer_Load);
            this.Closed += new System.EventHandler(this.DashBoardTransfer_Closed);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.DashBoardTransfer_Closing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.DataGrid dgListTransferBot;
        private System.Windows.Forms.DataGrid dgListTransferTop;
        private System.Windows.Forms.TextBox txtTransferNumber;
    }
}