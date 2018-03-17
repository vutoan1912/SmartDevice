using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ERP
{
    public partial class XuatKhoLinhKienOption : Form
    {
        public XuatKhoLinhKienOption()
        {
            InitializeComponent();
        }

        private void btnXuatKhoChiDinh_Click(object sender, EventArgs e)
        {
            XuatKhoLinhKien frmXuatKhoLK = new XuatKhoLinhKien();
            frmXuatKhoLK.ShowDialog();
        }

        private void btnXuatKhoTuDo_Click(object sender, EventArgs e)
        {
            XuatKhoLinhKienFree frmXuatKhoLKFree = new XuatKhoLinhKienFree();
            frmXuatKhoLKFree.ShowDialog();
        }
    }
}