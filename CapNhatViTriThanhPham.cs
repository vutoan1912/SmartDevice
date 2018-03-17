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
    public partial class CapNhatViTriThanhPham : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;


        TemThungThanhPham temThungThanhPham;
        TemVitri temViTri;

        int dangQuet = 0;

        int _soKien = 0;

        //bool _ready = false;

        List<TemThungThanhPham> listTemThungThanhPham = new List<TemThungThanhPham>();

        DataTable dtTemThungThanhPham = new DataTable();

        public CapNhatViTriThanhPham()
        {
            InitializeComponent();
            hDcd = new DecodeHandle(DecodeDeviceCap.Exists | DecodeDeviceCap.Barcode);

            // Initialize event
            DecodeRequest reqType = (DecodeRequest)1 | DecodeRequest.PostRecurring;
            dcdEvent = new DecodeEvent(hDcd, reqType, this);

            dcdEvent.Scanned += new DecodeScanned(dcdEvent_Scanned);

            dgListCuon.DataSource = dtTemThungThanhPham;
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

                
                temVitriTMP = new TemVitri(dcdData);

                if (temVitriTMP.ID != 0 && temVitriTMP.ID != null)
                {
                    dangQuet = 0;
                }
                else {
                    temThungThanhPham = new TemThungThanhPham(dcdData);
                    dangQuet = 1;
                }

                

                //quet tem Vitri
                if (dangQuet == 0)
                {
                    if (temVitriTMP.ID == 0 || temVitriTMP.ID == null)
                    {
                        MessageBox.Show("Ma vach khong hop le", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        if (temViTri != null)
                        {
                            if (temViTri.ID != temVitriTMP.ID)
                            {
                                MessageBox.Show("Bạn phải xóa vị trí cũ trước khi quét vị trí mới", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            }
                        }
                        else {
                            temViTri = temVitriTMP;

                            //load thong tin vi tri
                            lblViTri.Text = temViTri.ID + "-" + temViTri.Ten;
                            btnXoaVitri.Enabled = true;
                            dangQuet = 1;

                            lblStartMsgVitri.Visible = false;

                            if (dtTemThungThanhPham.Rows.Count > 0)
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
                    if (temThungThanhPham.IdThung == null)
                    {
                        MessageBox.Show("Ma vach khong hop le", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    else
                    {
                        //check exist in the added list
                        TemThungThanhPham tmp = listTemThungThanhPham.Find(x => x.IdThung == temThungThanhPham.IdThung); 
                        if (tmp == null)
                        {

                            listTemThungThanhPham.Add(temThungThanhPham);
                            
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
                            MessageBox.Show("Thùng này đã được quét", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
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
            dtTemThungThanhPham = Util.ToDataTable<TemThungThanhPham>(listTemThungThanhPham);

            dtTemThungThanhPham.Columns.Add("Action");

            foreach (DataRow row in dtTemThungThanhPham.Rows)
            {
                row["Action"] = "Xoa";
            }

            dgListCuon.DataSource = dtTemThungThanhPham;
            dgListCuon.TableStyles.Clear();

            DataGridTableStyle tableStyle = new DataGridTableStyle();
            tableStyle.MappingName = dtTemThungThanhPham.TableName;
            foreach (DataColumn item in dtTemThungThanhPham.Columns)
            {
                DataGridTextBoxColumn tbcName = new DataGridTextBoxColumn();

                switch (item.ColumnName)
                {
                    case "Action":
                        {
                            tbcName.Width = 60;
                        } break;
                    default:
                        {
                            tbcName.Width = 165;
                        } break;
                }

                tbcName.MappingName = item.ColumnName;
                tbcName.HeaderText = item.ColumnName;
                tableStyle.GridColumnStyles.Add(tbcName);
                
            }

            dgListCuon.TableStyles.Add(tableStyle);

            if (listTemThungThanhPham.Count == 0) {
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

            if (dtTemThungThanhPham.Rows.Count == 0) {
                btnReset.Enabled = false;
                btnSave.Enabled = false;
                lblStartMsgLinhKien.Visible = true;
                dgListCuon.Visible = false;
            }
        }
        

        private void btnReset_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show("Bạn có chắc muốn thao tác lại từ đầu?", "Ban co chac chan khong?", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

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
                MessageBox.Show("Hãy quét vào tem vị trí", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else if(listTemThungThanhPham.Count == 0){
                MessageBox.Show("Hãy quét vào tem thùng thành phẩm", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            else {
                try
                {
                    btnSave.Enabled = false;
                    btnReset.Enabled = false;
                    dgListCuon.Enabled = false;

                    string url = "update_location_by_code";

                    var temCuons = listCodeFromListObject(listTemThungThanhPham);

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
                            MessageBox.Show("Lưu thành công");
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
                    MessageBox.Show("Không có kết nối mạng", "Loi mang", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    btnSave.Enabled = true;
                    btnReset.Enabled = true;
                    dgListCuon.Enabled = true;
                }
            }
        }

        private string listCodeFromListObject(List<TemThungThanhPham> lsTemThungThanhPham) {

            List<string> lsCode = new List<string>();

            foreach (TemThungThanhPham tem in lsTemThungThanhPham)
            {
                lsCode.Add(tem.IdThung);
            }

            //string result = string.Join(",", lsCode.ToArray());
            string result = string.Join(",", lsCode.Select(i => i.ToString()).ToArray());
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
            listTemThungThanhPham.Clear();
            dangQuet = 0;
            SoKien = 0;

            dtTemThungThanhPham.Rows.Clear();

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
                            if (hti.Column == 1)
                            {
                                DialogResult confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa thùng?", "Chu y?", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

                                if (confirmResult == DialogResult.Yes)
                                {
                                    DataRow r = dtTemThungThanhPham.Rows[hti.Row];

                                    TemThungThanhPham t = listTemThungThanhPham.Find(x => x.IdThung == r["IdThung"].ToString());
                                    listTemThungThanhPham.Remove(t);

                                    updateGridLayout();
                                    SoKien--;

                                    if (t.IdThung != null && dtTemThungThanhPham.Rows.Count == 0) {
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