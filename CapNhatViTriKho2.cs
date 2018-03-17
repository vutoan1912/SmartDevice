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
using Newtonsoft.Json;

namespace ERP
{
    public partial class CapNhatViTriKho2 : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;


        TemCuon temCuon;
        TemVitri temViTri;

        int dangQuet = 0;

        int _soKien = 0;

        bool _ready = false;

        List<TemCuon> listTemCuon = new List<TemCuon>();

        DataTable dtCuon = new DataTable();

        public CapNhatViTriKho2()
        {
            InitializeComponent();
            hDcd = new DecodeHandle(DecodeDeviceCap.Exists | DecodeDeviceCap.Barcode);

            // Initialize event
            DecodeRequest reqType = (DecodeRequest)1 | DecodeRequest.PostRecurring;
            dcdEvent = new DecodeEvent(hDcd, reqType, this);

            dcdEvent.Scanned += new DecodeScanned(dcdEvent_Scanned);

            dgListCuon.DataSource = dtCuon;
        }

        private void dcdEvent_Scanned(object sender, DecodeEventArgs e)
        {
            CodeId cID = CodeId.NoData;
            string dcdData = string.Empty;

            TemVitri temVitriTMP = null;

            // Obtain the string and code id.
            try
            {
                dcdData = hDcd.ReadString(e.RequestID, ref cID);

                if (dcdData.Length < 20)
                {
                    dangQuet = 0;
                    temVitriTMP = new TemVitri(dcdData);
                }
                else {
                    dangQuet = 1;
                    temCuon = new TemCuon(dcdData);
                }


                //quet tem Vitri
                if (dangQuet == 0)
                {
                    if (temVitriTMP.ID == 0)
                    {
                        MessageBox.Show("Ma vach khong hop le", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        if (temViTri != null)
                        {
                            if (temViTri.ID != temVitriTMP.ID)
                            {
                                MessageBox.Show("Ban phai xoa vi tri cu truoc khi quet vi tri moi", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            }
                        }
                        else {
                            temViTri = temVitriTMP;

                            //load thong tin vi tri
                            lblViTri.Text = temViTri.ID + "-" + temViTri.Ten;
                            btnXoaVitri.Enabled = true;
                            dangQuet = 1;

                            lblStartMsgVitri.Visible = false;

                            if (dtCuon.Rows.Count > 0)
                            {
                                btnSave.Enabled = true;
                                btnReset.Enabled = true;
                            }
                        }

                    }
                }
                //Quet tem Cuon
                else if (dangQuet == 1 || dangQuet == 2)
                {
                    dgListCuon.Visible = true;
                    //temCuon = new TemCuon(dcdData);
                    lblStartMsgLinhKien.Visible = false;
                    if (temCuon.IdCuon == null)
                    {
                        MessageBox.Show("Ma vach khong hop le", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        //check exist in the added list
                        TemCuon tmp = listTemCuon.Find(x => x.IdCuon == temCuon.IdCuon); 
                        if (tmp == null)
                        {
                       
                            listTemCuon.Add(temCuon);
                            
                            updateGridLayout();

                            dangQuet = 2;

                            lblStartMsgLinhKien.Visible = false;
                            SoKien++;

                            if(temViTri != null){
                                btnSave.Enabled = true;
                                btnReset.Enabled = true;
                            }
                            
                        }
                        else {
                            MessageBox.Show("Cuon/Thung nay da duoc quet", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                MessageBox.Show("Co loi xay ra, hay quet lai tu dau", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

        }



        private void CapNhatViTriKho2_Load(object sender, EventArgs e)
        {
            
        }

        private void updateGridLayout() {
            dtCuon = Util.ToDataTable<TemCuon>(listTemCuon);

            dtCuon.Columns.Add("Action");

            foreach (DataRow row in dtCuon.Rows)
            {
                row["Action"] = "Xoa";
            }

            dgListCuon.DataSource = dtCuon;
            dgListCuon.TableStyles.Clear();

            DataGridTableStyle tableStyle = new DataGridTableStyle();
            tableStyle.MappingName = dtCuon.TableName;
            foreach (DataColumn item in dtCuon.Columns)
            {
                DataGridTextBoxColumn tbcName = new DataGridTextBoxColumn();

                switch (item.ColumnName)
                {
                    case "VnptPn":
                        {
                            tbcName.Width = -1;
                            
                        } break;
                    case "IdCuon":
                        {
                            tbcName.MappingName = "IdCuon";
                            tbcName.HeaderText = "Id Kien";
                            tbcName.Width = 85;
                            tableStyle.GridColumnStyles.Add(tbcName);
                        } break;
                    case "Description":
                        {
                            tbcName.Width = -1;
                        } break;
                    case "SoDonHang":
                        {
                            tbcName.Width = -1;
                        } break;
                    case "SoPhieuNhapKho":
                        {
                            tbcName.Width = -1;
                        } break;
                    case "NgayNhapKho":
                        {
                            tbcName.Width = -1;
                        } break;
                    case "Type":
                        {
                            tbcName.Width = -1;
                        } break;
                    case "Action":
                        {
                            tbcName.Width = 60;
                        } break;
                    default:
                        {
                            tbcName.Width = 85;
                        } break;
                }

                if (item.ColumnName != "IdCuon") {
                    tbcName.MappingName = item.ColumnName;
                    tbcName.HeaderText = item.ColumnName;
                    tableStyle.GridColumnStyles.Add(tbcName);
                }
                
            }

            dgListCuon.TableStyles.Add(tableStyle);

            if (listTemCuon.Count == 0) {
                btnSave.Enabled = false;
            }
        }


        #region Button Click
        private void btnXoaVitri_Click(object sender, EventArgs e)
        {
            lblViTri.Text = "";
            dangQuet = 0;
            //pbArrowVitri.Visible = true;
            //pbArrowLinhKien.Visible = false;
            lblStartMsgLinhKien.Visible = false;
            lblStartMsgVitri.Visible = true;
            temViTri = null;
            btnXoaVitri.Enabled = false;
            btnReset.Enabled = false;
            btnSave.Enabled = false;

            if (dtCuon.Rows.Count == 0) {
                btnReset.Enabled = false;
                btnSave.Enabled = false;
                lblStartMsgLinhKien.Visible = true;
                dgListCuon.Visible = false;
            }
        }
        

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("Ban co chac chan muon thao tac lai tu dau?", "Ban co chac chan khong?", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

            if (confirmResult == DialogResult.Yes)
            {
                ResetForm();
            }
        }

        

        private void btnSave_Click(object sender, EventArgs e)
        {
            //check vi tri
            if (temViTri == null)
            {
                MessageBox.Show("Hay quet vao tem vi tri", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else if(listTemCuon.Count == 0){
                MessageBox.Show("Hay quet vao tem cuon/thung", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else {
                try
                {
                    btnSave.Enabled = false;
                    btnReset.Enabled = false;
                    dgListCuon.Enabled = false;

                    string url = "update_location_by_code";

                    var temCuons = listCodeFromListObject(listTemCuon);

                    var paras = new
                    {
                        code = temCuons,
                        inventory_location_id = temViTri.ID
                    };

                    ApiResponse res = HTTP.Post(url, paras);

                    if (res.Status)
                    {
                        ApiModel resApi = JsonConvert.DeserializeObject<ApiModel>(res.RawText, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                        if (resApi.code == 200) {
                            MessageBox.Show("Luu thanh cong");
                            ResetForm();
                        } else {
                            //MessageBox.Show(resApi.message, "Co loi xay ra", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            MessageBox.Show(resApi.message);
                            btnSave.Enabled = true;
                            btnReset.Enabled = true;
                            dgListCuon.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Co loi xay ra, hay thu lai");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Khong co ket noi mang", "Loi mang", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    btnSave.Enabled = true;
                    btnReset.Enabled = true;
                    dgListCuon.Enabled = true;
                }
            }
        }

        private string listCodeFromListObject(List<TemCuon> lsTemCuon) {

            List<string> lsCode = new List<string>();

            foreach (TemCuon tem in lsTemCuon)
            {
                lsCode.Add(tem.IdCuon);
            }

            string result = string.Join(",", lsCode.ToArray());
            return result;
        }
        #endregion

        

        private void ResetForm()
        {
            //pbArrowVitri.Visible = true;
            //pbArrowLinhKien.Visible = false;
            lblStartMsgVitri.Visible = true;
            lblStartMsgLinhKien.Visible = true;

            btnReset.Enabled = false;
            btnSave.Enabled = false;
            btnXoaVitri.Enabled = false;
            lblViTri.Text = "";
            temViTri = null;
            dgListCuon.DataSource = null;
            listTemCuon.Clear();
            dangQuet = 0;
            SoKien = 0;

            dtCuon.Rows.Clear();

            dgListCuon.Visible = false;
        }

        private void dgListCuon_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                var hti = dgListCuon.HitTest(e.X, e.Y);

                switch (hti.Type)
                {
                    case System.Windows.Forms.DataGrid.HitTestType.Cell:
                        {
                            if (hti.Column == 8)
                            {
                                DialogResult confirmResult = MessageBox.Show("Ban co chac chan muon xoa linh kien?", "Ban co chac chan khong?", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

                                if (confirmResult == DialogResult.Yes)
                                {
                                    DataRow r = dtCuon.Rows[hti.Row];

                                    TemCuon t = listTemCuon.Find(x => x.IdCuon == r["IdCuon"].ToString());
                                    listTemCuon.Remove(t);

                                    updateGridLayout();
                                    SoKien--;

                                    if (t.IdCuon != null && dtCuon.Rows.Count == 0) {
                                        lblStartMsgLinhKien.Visible = true;
                                        btnSave.Enabled = false;
                                        btnReset.Enabled = false;
                                        dgListCuon.Visible = false;
                                    }
                                }
                            }
                        } break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }


        private void CapNhatViTriKho2_Closing(object sender, CancelEventArgs e)
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

        public int SoKien
        {
            get { return _soKien; }
            set { 
                _soKien = value;
                lblSoKien.Text = _soKien.ToString() + " kien";
            }
        }
    }
}