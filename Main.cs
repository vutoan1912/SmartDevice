using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Datalogic.API;
using ERP.Base;

namespace ERP
{
    public partial class Main : Form
    {
        //private DecodeEvent dcdEvent;
        //private DecodeHandle hDcd;

        private Form _frmLogin;
        public Form frmLogin
        {
            get { return _frmLogin; }
            set { _frmLogin = value; }
        }

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

        private void Main_Load(object sender, EventArgs e)
        {
            //loadData();
        }

        private void Main_Closed(object sender, EventArgs e)
        {
            frmLogin.Close();
            Application.Exit();
        }

        private void btnProductOut_Click(object sender, EventArgs e)
        {
            TransferOut frmTransferOut = new TransferOut();
            frmTransferOut.ShowDialog();
        }

        private void ScanCode(string dcdData)
        {

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

        private void btnPackagingItems_Click(object sender, EventArgs e)
        {
            PackagingItems frmPackagingItems = new PackagingItems();
            frmPackagingItems.ShowDialog();
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            TextEditable textEditable = new TextEditable();
            textEditable.valueEdit = Config.API_URL;
            textEditable.ShowDialog();
            Config.API_URL = textEditable.valueEdit.Trim();
        }

        private void menuItem8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            PackagingItems frmPackagingItems = new PackagingItems();
            frmPackagingItems.ShowDialog();
        }

        private void btnImportation_Click(object sender, EventArgs e)
        {
            DashBoardTransfer dashBoardTransfer = new DashBoardTransfer();
            dashBoardTransfer.TypeTransfer = 0;
            dashBoardTransfer.ShowDialog();
        }

        private void btnAdjustment_Click(object sender, EventArgs e)
        {
            InventoryAdjustment InventoryAdjustment = new InventoryAdjustment();
            InventoryAdjustment.ShowDialog();
        }

        private void btnExportation_Click(object sender, EventArgs e)
        {
            DashBoardTransfer dashBoardTransfer = new DashBoardTransfer();
            dashBoardTransfer.TypeTransfer = 1;
            dashBoardTransfer.ShowDialog();
        }

        private void btnMovePackage_Click(object sender, EventArgs e)
        {
            PackagingItems PackagingItems = new PackagingItems();
            PackagingItems.ShowDialog();
        }

    }


}