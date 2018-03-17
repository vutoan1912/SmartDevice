using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ERP.Base.ErpObjects;
using Newtonsoft.Json;
using Datalogic.API;

namespace ERP
{
    public partial class XuatKhoLinhKien : Form
    {
        public const string COL_STATUS = "T.Thai"; 
        public const string COL_ACTION = "Huy";
        public const string DEFAULT_TXT_MAXK = "part-exp-no-0";

        private bool _dangQuet = false;
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;
        private DataTable dtListCuon = new DataTable();

        private int _totalThung = 0;
        private int _thungHaventScanned = 0;
        private int _thungScanned = 0;

        private int _totalCuon = 0;
        private int _cuonHaventScanned = 0;
        private int _cuonScanned = 0;

        private int _currentScanned = 0;

        private string _exp_code = "";

        List<string> listScanned = new List<string>();

        public XuatKhoLinhKien()
        {
            InitializeComponent();

            //Initialize event
            hDcd = new DecodeHandle(DecodeDeviceCap.Exists | DecodeDeviceCap.Barcode);
            DecodeRequest reqType = (DecodeRequest)1 | DecodeRequest.PostRecurring;
            dcdEvent = new DecodeEvent(hDcd, reqType, this);
            dcdEvent.Scanned += new DecodeScanned(dcdEvent_Scanned);

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string maXuatKho = txtMaXuatKho.Text.TrimStart().TrimEnd();
            _exp_code = txtMaXuatKho.Text;

            btnLoad.Enabled = false;
            btnReset.Enabled = false;
            listScanned.Clear();
            dtListCuon.Rows.Clear();

            if (maXuatKho != "")
            {
                ApiResponse res = new ApiResponse();
                res.Status = false;
                //Load danh sach linh kien
                try
                {
                    //string url = "getListThungXuatKho";
                    string url = "getListCuonXuatKho";
                    var paras = new
                    {
                        ma_xuat_kho = maXuatKho
                    };
                    res = HTTP.Post(url, paras);
                }
                catch (Exception ex) {
                    MessageBox.Show("Loi tai du lieu xuat kho", "Loi mang", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }

                if (res.Status && Util.IsJson(res.RawText))
                {
                    try
                    {
                        List<CuonModel> listCuon = JsonConvert.DeserializeObject<List<CuonModel>>(res.RawText, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                        btnReset.Enabled = true;
                        
                        //lblChuaQuet.Text = chuaQuet.ToString();

                        dtListCuon = Util.ToDataTable<CuonModel>(listCuon);

                        dtListCuon.Columns.Add(COL_STATUS);
                        dtListCuon.Columns.Add(COL_ACTION);

                        foreach (DataRow row in dtListCuon.Rows)
                        {
                            if (row["Scanned"].ToString() == "1")
                            {
                                row[COL_STATUS] = "DX";
                            }
                            else
                                row[COL_STATUS] = "";
                            row[COL_ACTION] = "";
                        }

                        dgCuonList.DataSource = dtListCuon;
                        dgCuonList.TableStyles.Clear();
                        DataGridTableStyle tableStyle = new DataGridTableStyle();
                        tableStyle.MappingName = dtListCuon.TableName;
                        foreach (DataColumn item in dtListCuon.Columns)
                        {
                            DataGridTextBoxColumn tbcName = new DataGridTextBoxColumn();

                            switch (item.ColumnName)
                            {
                                case "vnpt_pn":
                                    {
                                        tbcName.MappingName = item.ColumnName;
                                        tbcName.HeaderText = "VNPT P/N";
                                        tbcName.Width = 100;
                                    } break;
                                case "Code":
                                    {
                                        tbcName.MappingName = item.ColumnName;
                                        tbcName.HeaderText = "Id Cuon";
                                        tbcName.Width = 65;
                                    } break;
                                case "Quantity":
                                    {
                                        tbcName.Width = -1;
                                    } break;
                                case "Thung":
                                    {
                                        tbcName.Width = 65;
                                    } break;
                                case "Scanned":
                                    {
                                        tbcName.Width = -1;
                                    } break;
                                case "ThungType":
                                    {
                                        tbcName.Width = -1;
                                    } break;
                                case COL_STATUS:
                                    {
                                        tbcName.MappingName = item.ColumnName;
                                        tbcName.HeaderText = "Trang thai";
                                        tbcName.Width = 30;
                                    } break;
                                case COL_ACTION:
                                    {
                                        tbcName.MappingName = item.ColumnName;
                                        tbcName.HeaderText = "Huy";
                                        tbcName.Width = 25;
                                    } break;
                                default:
                                    {
                                        tbcName.Width = 65;
                                    } break;
                            }

                            if (tbcName.MappingName != "") {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = item.ColumnName;
                            }

                            tableStyle.GridColumnStyles.Add(tbcName);
                            
                            dangQuet = true;
                        }

                        dgCuonList.TableStyles.Add(tableStyle);

                        dgCuonList.Refresh();

                        ScanInfo();
                    }
                    catch (Exception ex)
                    {
                        //btnLoad.Enabled = true;
                        MessageBox.Show("Khong lay duoc du lieu");
                        txtMaXuatKho.Text = DEFAULT_TXT_MAXK;
                        _exp_code = "";
                    }
                }
                else
                {
                    //btnLoad.Enabled = true;
                    if (Util.CleanStr(res.RawText) != "")
                        MessageBox.Show(Util.CleanStr(res.RawText));
                    txtMaXuatKho.Text = DEFAULT_TXT_MAXK;
                    _exp_code = "";
                }
            }
            else {
                txtMaXuatKho.Focus();
            }
        }

        private void ScanCode(string dcdData)
        {
            // Obtain the string and code id.
            try
            {
                TemCuon temCuon = new TemCuon(dcdData);

                DataRow[] rs = null;

                if (temCuon.IdCuon != null && temCuon.IdCuon != "")
                {
                    //check thung or cuon
                    int type = Util.ConvertInt(temCuon.Type);

                    if (type == 0)
                    { //cuon
                        rs = dtListCuon.Select("Code = " + temCuon.IdCuon);

                        DataRow dr = null;

                        if (rs.Length > 0)
                            dr = rs.FirstOrDefault();
                        else
                        {
                            MessageBox.Show("Cuon nay khong co trong danh sach xuat kho", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }


                        if (dr != null && dr[COL_STATUS] == "")
                        {

                            listScanned.Add(temCuon.IdCuon); //add code da quet vao list
                            dr[COL_STATUS] = "DQ";
                            dr[COL_ACTION] = "huy";
                            currentScanned++;
                        }
                        else
                        {
                            MessageBox.Show("Cuon nay da quet", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                    }
                    else
                    {   //thung
                        rs = dtListCuon.Select("Code = " + temCuon.IdCuon);

                        //var text = string.Join(",", dtListCuon.AsEnumerable()
                        //         .Select(x => x["Code"].ToString())
                        //         .ToArray());

                        if (rs.Length > 0)
                        {

                            string listIdCuon = "";
                            int ThungType = Util.ConvertInt(rs[0]["ThungType"].ToString());
                            if (ThungType == 1)
                            {
                                //Thong bao cac cuon trong thung
                                foreach (DataRow r in rs)
                                {
                                    if (r[COL_STATUS] == "")
                                    {
                                        if (listIdCuon == "")
                                            listIdCuon += r["Code"].ToString();
                                        else
                                            listIdCuon += "-" + r["Code"].ToString();
                                    }
                                }
                                MessageBox.Show("Co " + rs.Length + " cuon trong thung. " + listIdCuon, "Thong Bao", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                            }
                            else if (ThungType == 2)
                            {
                                foreach (DataRow r in rs)
                                {
                                    if (r[COL_STATUS] == "")
                                    {
                                        r[COL_STATUS] = "DQ";
                                        r[COL_ACTION] = "huy";
                                        currentScanned++;
                                        listScanned.Add(r["Code"].ToString());

                                        if (listIdCuon == "")
                                            listIdCuon += r["Code"].ToString();
                                        else
                                            listIdCuon += "-" + r["Code"].ToString();
                                    }
                                }
                                MessageBox.Show("Co " + rs.Length + " cuon trong thung. " + listIdCuon, "Thong Bao", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                            }
                            else
                            {
                                DataRow dr = rs.FirstOrDefault();
                                if (dr[COL_STATUS] == "")
                                {
                                    listScanned.Add(temCuon.IdCuon); //add code da quet vao list
                                    dr[COL_STATUS] = "DQ";
                                    dr[COL_ACTION] = "huy";
                                    currentScanned++;
                                }
                                else
                                {
                                    MessageBox.Show("Thung nay da quet", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                }
                            }
                        }
                        else
                            MessageBox.Show("Thung khong nam trong danh sach xuat kho.", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    ScanInfo();
                }
                else
                    MessageBox.Show("Ma thung/cuon khong dung", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Co loi xay ra", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void dcdEvent_Scanned(object sender, DecodeEventArgs e)
        {
            CodeId cID = CodeId.NoData;
            string dcdData = string.Empty;
            dcdData = hDcd.ReadString(e.RequestID, ref cID);
            //dcdData = "PN:KBUAA7SK0000048M*SPNK:PART-IMP-17*SDH:VNPT/TECH-BO-04/2017-97*NNK:2017-04-28*ID:0000207503*SL:1000.000000*TYPE:1*";

            ScanCode(dcdData);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (dangQuet)
            {
                DialogResult dialogResult = MessageBox.Show("Ban co muon thuc hien giao dich xuat kho khac khong", "Thuc hien lai giao dich?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.Yes)
                {
                    dtListCuon.Rows.Clear();
                    txtMaXuatKho.Text = DEFAULT_TXT_MAXK;
                    _exp_code = "";
                    dangQuet = false;

                }
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            string url = "updateListXuatKhoLK";

            string dsCuon = string.Join("-", listScanned.ToArray());

            var paras = new
            {
                ma_xuat_kho = _exp_code,
                list_cuon = dsCuon
            };

            try
            {
                ApiResponse res = HTTP.Post(url, paras);

                if (res.Status)
                {
                    //update trang thai cua lenh export kho
                    MessageBox.Show("Da luu thanh cong");
                    ResetForm();
                    dtListCuon.Rows.Clear();
                }
                else
                {
                    MessageBox.Show("Loi mang, hay thu lai lan nua");
                }
            }catch(Exception ex){
                MessageBox.Show("Khong co ket noi mang", "Loi mang", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void ResetForm() {
            _thungScanned = 0;
            _thungHaventScanned = 0;
            currentScanned = 0;
            cuonScanned = 0;
            cuonHaventScanned = 0;
            
            btnFinish.Enabled = false;
            btnReset.Enabled = false;
            txtMaXuatKho.Text = DEFAULT_TXT_MAXK;
            _exp_code = "";
            dangQuet = false;
            dtListCuon.Rows.Clear();
            listScanned.Clear();
            txtMaXuatKho.Focus();
        }

        /*
        private void btnFinish_Click(object sender, EventArgs e)
        {
            string url = "updateXuatKhoLK";
            var paras = new
            {
                ma_xuat_kho = txtMaXuatKho.Text
            };
            ApiResponse res = HTTP.Post(url, paras);

            if (res.Status)
            {
                //update trang thai cua lenh export kho
                MessageBox.Show("Da hoan thanh giao dich xuat kho");
                btnFinish.Enabled = false;
                btnReset.Enabled = false;
                txtMaXuatKho.Text = "";
                dangQuet = false;
                //daQuet = 0;
                chuaQuet = 0;
                dtListCuon.Rows.Clear();
            }
            string aaa = string.Join("-", listScanned.ToArray());
            MessageBox.Show(aaa);
        }
         * */

        #region Properties
       
        public int currentScanned{
           get { return _currentScanned; }
           set { 
               _currentScanned = value;
               if (_currentScanned > 0)
               {
                   btnFinish.Enabled = true;
               }
               else
               {
                   btnFinish.Enabled = false;
               }
           }
       }

        public int cuonScanned
        {
            get { return _cuonScanned; }
            set {
                _cuonScanned = value;
                lblDaQuet.Text = _cuonScanned.ToString() + "/" + _thungScanned.ToString();
                
            }
        }
        
        public int cuonHaventScanned
        {
            get { return _cuonHaventScanned; }
            set {
                _cuonHaventScanned = value;
                lblChuaQuet.Text = _cuonHaventScanned.ToString() + "/" + _thungHaventScanned.ToString();
            }
        }
        

        public bool dangQuet
        {
            get { return _dangQuet; }
            set
            {
                _dangQuet = value;

                if (value == true)
                {
                    btnReset.Enabled = true;
                    btnLoad.Enabled = false;
                }
                else
                {
                    btnReset.Enabled = false;
                    btnLoad.Enabled = true;
                }
            }
        }
        #endregion

        private void XuatKhoLinhKien_Closing(object sender, CancelEventArgs e)
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

        private void dgCuonList_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                var hti = dgCuonList.HitTest(e.X, e.Y);

                switch (hti.Type)
                {
                    case System.Windows.Forms.DataGrid.HitTestType.Cell:
                        {
                            if (hti.Column == 7)
                            {
                                DataRow r = dtListCuon.Rows[hti.Row];
                                if (r[COL_ACTION] != "")
                                {
                                    DialogResult confirmResult = MessageBox.Show("Ban co chac chan muon huy?", "Ban co chac chan khong?", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

                                    if (confirmResult == DialogResult.Yes)
                                    {
                                        r[COL_STATUS] = "";
                                        r[COL_ACTION] = "";
                                        listScanned.Remove(r["Code"].ToString());
                                        ScanInfo();
                                        currentScanned--;
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

        private void txtMaXuatKho_TextChanged(object sender, EventArgs e)
        {
            if (txtMaXuatKho.Text == "")
            {
                btnLoad.Enabled = false;
            }
            else {
                btnLoad.Enabled = true;
            }
        }

        private void ScanInfo() { 
            //count all thung
            DataTable distinctValues = dtListCuon.DefaultView.ToTable(true, "Thung");
            _totalThung = distinctValues.Rows.Count;

            //count thung that haven't scan
            DataRow[] countCuonRemain = dtListCuon.Select(COL_STATUS + "= ''").ToArray();
            if (countCuonRemain.Length > 0)
            {
                DataTable tTMP = dtListCuon.Select(COL_STATUS + "= ''").CopyToDataTable();

                DataTable dtThungHaventScanned = tTMP.DefaultView.ToTable(true, "Thung");
                _thungHaventScanned = dtThungHaventScanned.Rows.Count;

                _thungScanned = _totalThung - _thungHaventScanned;
            }
            else {
                _thungHaventScanned = 0;
                _thungScanned = _totalThung;
            }
            

            //count all cuon
            _totalCuon = dtListCuon.Rows.Count;

            //count cuon that scanned
            DataRow[] rCuonHaventScanned = dtListCuon.Select(COL_STATUS + "= ''").ToArray();
            cuonHaventScanned = rCuonHaventScanned.Count();

            cuonScanned = _totalCuon - _cuonHaventScanned;

            
        }
    }
}