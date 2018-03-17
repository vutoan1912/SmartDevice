using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ERP.Base.ErpObjects;
using Datalogic.API;

namespace ERP
{
    public partial class CapNhatViTriKho : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;


        TemCuon temCuon;
        TemVitri temViTri;

        int dangQuet = 0;

        //bool _ready = false;
        public CapNhatViTriKho()
        {
            InitializeComponent();

            hDcd = new DecodeHandle(DecodeDeviceCap.Exists | DecodeDeviceCap.Barcode);

            // Initialize event
            DecodeRequest reqType = (DecodeRequest)1 | DecodeRequest.PostRecurring;
            dcdEvent = new DecodeEvent(hDcd, reqType, this);

            dcdEvent.Scanned += new DecodeScanned(dcdEvent_Scanned); 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string url = "update_location_by_code";
                var paras = new
                {
                    code = temCuon.IdCuon,
                    inventory_location_id = temViTri.ID
                };
                ApiResponse res = HTTP.Post(url, paras);

                if (res.Status)
                {
                    MessageBox.Show("Da cap nhat vitri");
                    ResetForm();
                }
                else {
                    MessageBox.Show(res.Message);
                }

            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }


        private void dcdEvent_Scanned(object sender, DecodeEventArgs e)
        {
            CodeId cID = CodeId.NoData;
            string dcdData = string.Empty;

            // Obtain the string and code id.
            try
            {
                dcdData = hDcd.ReadString(e.RequestID, ref cID);
                
                //quet tem cuon
                if (dangQuet == 0) {
                    temCuon = new TemCuon(dcdData);
                    
                    if (temCuon.IdCuon == null)
                    {
                        MessageBox.Show("Hay quet vao tem cuon", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    else { 
                        //load thong tin tem cuon
                        lblVnptPN.Text = temCuon.VnptPn;
                        lblPhieuNhapKho.Text = temCuon.SoPhieuNhapKho;
                        lblSoDonHang.Text = temCuon.SoDonHang;
                        lblNgayNhapKho.Text = temCuon.NgayNhapKho;
                        lblSoLuong.Text = temCuon.SoLuong;
                        lblIdCuon.Text = temCuon.IdCuon;
                        dangQuet = 1;
                    }
                }
                

                //Quet tem vitri
                else if (dangQuet == 1)
                {
                    temViTri = new TemVitri(dcdData);

                    if (temViTri.ID == 0 || temViTri.ID == null)
                    {
                        MessageBox.Show("Hay quet vao tem vi tri", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        
                        //load thong tin tem cuon
                        lblViTri.Text = temViTri.ID + "-" + temViTri.Ten;
                        dangQuet = 2;
                        btnSave.Enabled = true;
                        btnReset.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                MessageBox.Show("Co loi xay ra, hay quet lai tu dau", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                ResetForm();
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm() {
            lblIdCuon.Text = "";
            lblNgayNhapKho.Text = "";
            lblPhieuNhapKho.Text = "";
            lblSoDonHang.Text = "";
            lblSoLuong.Text = "";
            lblVnptPN.Text = "";
            lblViTri.Text = "";
            btnReset.Enabled = false;
            btnSave.Enabled = false;
            dangQuet = 0;
        }

        private void CapNhatViTriKho_Closing(object sender, CancelEventArgs e)
        {
            // If our instance of DcdEvent is listening to the decoder, we need to make
            // sure we tell DcdEvent to stop listening.
            if (dcdEvent.IsListening)
            {
                dcdEvent.StopScanListener();
            }

            if (hDcd != null)
            {
                hDcd.Dispose();
            }
        }
    }
}