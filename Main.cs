using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Datalogic.API;

namespace ERP
{
    public partial class Main : Form
    {
        private Form _frmLogin;
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;

        public Main()
        {
            InitializeComponent();

            //Initialize event
            //hDcd = new DecodeHandle(DecodeDeviceCap.Exists | DecodeDeviceCap.Barcode);
            //DecodeRequest reqType = (DecodeRequest)1 | DecodeRequest.PostRecurring;
            //dcdEvent = new DecodeEvent(hDcd, reqType, this);
            //dcdEvent.Scanned += new DecodeScanned(dcdEvent_Scanned);
        }

        private void dcdEvent_Scanned(object sender, DecodeEventArgs e)
        {
            //CodeId cID = CodeId.NoData;
            //string dcdData = string.Empty;
            //dcdData = hDcd.ReadString(e.RequestID, ref cID);

            //ScanCode(dcdData);
        }

        private void btnNoiLinhKien_Click(object sender, EventArgs e)
        {
            
        }

        private void btnXuatKhoLinhKien_Click(object sender, EventArgs e)
        {
            XuatKhoLinhKienOption frmXuatKhoLKOption = new XuatKhoLinhKienOption();
            frmXuatKhoLKOption.ShowDialog();
        }

        private void Main_Closed(object sender, EventArgs e)
        {
            frmLogin.Close();
            Application.Exit();
        }

        public Form frmLogin
        {
            get { return _frmLogin; }
            set { _frmLogin = value; }
        }

        private void btnCapNhatViTri_Click(object sender, EventArgs e)
        {
            CapNhatViTriKho frmCapNhatViTri = new CapNhatViTriKho();
            frmCapNhatViTri.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CapNhatViTriKho2 frmCapNhatViTri = new CapNhatViTriKho2();
            frmCapNhatViTri.ShowDialog();
        }

        private void btnVTTP_Click(object sender, EventArgs e)
        {
            CapNhatViTriThanhPham frmCapNhatViTriThanhPham = new CapNhatViTriThanhPham();
            frmCapNhatViTriThanhPham.ShowDialog();
        }

        private void btnProductOut_Click(object sender, EventArgs e)
        {
            TransferOut frmTransferOut = new TransferOut();
            frmTransferOut.ShowDialog();
        }

        //private void btnXuatKhoThanhPham_Click(object sender, EventArgs e)
        //{
        //    XuatKhoThanhPham frmXKTP = new XuatKhoThanhPham();
        //    frmXKTP.ShowDialog();
        //}

        private void ScanCode(string dcdData)
        {
            try
            {

                string barcode = dcdData.Substring(0, 6);
                string[] codes = dcdData.Split('-');
                string code = "";
                try
                {
                    code = codes[3];
                }
                catch { return; };

                MessageBox.Show(barcode);
                switch (barcode)
                {
                    case "PA-I-R":
                        MessageBox.Show(code);
                        break;
                    case "PA-I-P":
                        MessageBox.Show(code);
                        break;
                    case "PA-E-R":
                        MessageBox.Show(code);
                        if (code.Length > 0)
                        {
                            XuatKhoLinhKienFree frmXuatKhoLinhKienFree = new XuatKhoLinhKienFree();
                            frmXuatKhoLinhKienFree.ShowDialog();
                        }
                        break;
                    case "PA-E-P":
                        MessageBox.Show(code);
                        break;
                    case "SE-E-R":
                        MessageBox.Show(code);
                        break;
                    default:
                        MessageBox.Show("Not exists code");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnNKHH_Click(object sender, EventArgs e)
        {
            SetLocation frmSetLocation = new SetLocation();
            frmSetLocation.ShowDialog();
        }

        private void btnInventoryAdjustment_Click(object sender, EventArgs e)
        {
            InventoryAdjustment frmInventoryAdjustment = new InventoryAdjustment();
            frmInventoryAdjustment.ShowDialog();
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            SetLocation frmSetLocation = new SetLocation();
            frmSetLocation.ShowDialog();
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            TransferOut frmTransferOut = new TransferOut();
            frmTransferOut.ShowDialog();
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            InventoryAdjustment frmInventoryAdjustment = new InventoryAdjustment();
            frmInventoryAdjustment.ShowDialog();
        }

    }
}