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
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.btnImportation = new System.Windows.Forms.Button();
            this.btnExportation = new System.Windows.Forms.Button();
            this.btnAdjustment = new System.Windows.Forms.Button();
            this.btnMovePackage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem8);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItem3);
            this.menuItem1.MenuItems.Add(this.menuItem4);
            this.menuItem1.MenuItems.Add(this.menuItem5);
            this.menuItem1.MenuItems.Add(this.menuItem2);
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
            // menuItem2
            // 
            this.menuItem2.Text = "Packaging";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.MenuItems.Add(this.menuItem6);
            this.menuItem7.Text = "Setting";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Text = "Api url";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Text = "Exit";
            this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
            // 
            // btnImportation
            // 
            this.btnImportation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportation.BackColor = System.Drawing.Color.Aqua;
            this.btnImportation.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.btnImportation.Location = new System.Drawing.Point(9, 43);
            this.btnImportation.Name = "btnImportation";
            this.btnImportation.Size = new System.Drawing.Size(105, 47);
            this.btnImportation.TabIndex = 18;
            this.btnImportation.Text = "Importation";
            this.btnImportation.Click += new System.EventHandler(this.btnImportation_Click);
            // 
            // btnExportation
            // 
            this.btnExportation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportation.BackColor = System.Drawing.Color.OrangeRed;
            this.btnExportation.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.btnExportation.Location = new System.Drawing.Point(125, 43);
            this.btnExportation.Name = "btnExportation";
            this.btnExportation.Size = new System.Drawing.Size(105, 47);
            this.btnExportation.TabIndex = 19;
            this.btnExportation.Text = "Exportation";
            this.btnExportation.Click += new System.EventHandler(this.btnExportation_Click);
            // 
            // btnAdjustment
            // 
            this.btnAdjustment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdjustment.BackColor = System.Drawing.Color.Yellow;
            this.btnAdjustment.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.btnAdjustment.Location = new System.Drawing.Point(9, 103);
            this.btnAdjustment.Name = "btnAdjustment";
            this.btnAdjustment.Size = new System.Drawing.Size(105, 47);
            this.btnAdjustment.TabIndex = 20;
            this.btnAdjustment.Text = "Adjustment";
            this.btnAdjustment.Click += new System.EventHandler(this.btnAdjustment_Click);
            // 
            // btnMovePackage
            // 
            this.btnMovePackage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMovePackage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMovePackage.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
            this.btnMovePackage.Location = new System.Drawing.Point(125, 103);
            this.btnMovePackage.Name = "btnMovePackage";
            this.btnMovePackage.Size = new System.Drawing.Size(105, 47);
            this.btnMovePackage.TabIndex = 21;
            this.btnMovePackage.Text = "Move Package";
            this.btnMovePackage.Click += new System.EventHandler(this.btnMovePackage_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(240, 268);
            this.Controls.Add(this.btnMovePackage);
            this.Controls.Add(this.btnAdjustment);
            this.Controls.Add(this.btnExportation);
            this.Controls.Add(this.btnImportation);
            this.Menu = this.mainMenu1;
            this.Name = "Main";
            this.Text = "ERP System";
            this.Load += new System.EventHandler(this.Main_Load);
            this.Closed += new System.EventHandler(this.Main_Closed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.MenuItem menuItem7;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem8;
        private System.Windows.Forms.Button btnImportation;
        private System.Windows.Forms.Button btnExportation;
        private System.Windows.Forms.Button btnAdjustment;
        private System.Windows.Forms.Button btnMovePackage;
    }
}