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
    public partial class XuatKhoLinhKienFree : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;
        
        private string _exp_code = "";
        private const string COL_SCANNED = "scanned";
        //private int _currentScanned = 0;
        private int _cuonScanned = 0;
        private int _thungScanned = 0;
        private int _cuonHaventScanned = 0;
        private int _thungHaventScanned = 0;

        private int _sl_thung_xuat = 0;
        private int _sl_thung_quet = 0;
        private int _sl_cuon_xuat = 0;
        private int _sl_cuon_quet = 0;

        private DataTable dtList = new DataTable();
        List<PackageInfo> listScanned = new List<PackageInfo>();
        private List<PackageInfo> listSaveFail = new List<PackageInfo>();
        private List<PackageInfo> listExportCancel;
        private List<PackageInfo> listScanCancel;

        public const string DEFAULT_TXT_MAXK = "PART-EXPORT-REQ-0";
        public const string DEFAULT_TXT_MAXK_OTHER = "EXP-RE-PART-DIF-0";
        private int _checkOther = 0;

        #region Properties

        //public int currentScanned
        //{
        //    get { return _currentScanned; }
        //    set
        //    {
        //        _currentScanned = value;
        //        if (_currentScanned > 0)
        //        {
        //            btnFinish.Enabled = true;
        //        }
        //        else
        //        {
        //            btnFinish.Enabled = false;
        //        }
        //    }
        //}

        public int cuonScanned
        {
            get { return _cuonScanned; }
            set
            {
                _cuonScanned = value;
                lblDaQuet.Text = _cuonScanned.ToString() + "/" + _thungScanned.ToString();

            }
        }

        public int cuonHaventScanned
        {
            get { return _cuonHaventScanned; }
            set
            {
                _cuonHaventScanned = value;
                lblDaXuat.Text = _cuonHaventScanned.ToString() + "/" + _thungHaventScanned.ToString();
            }
        }
        #endregion

        public XuatKhoLinhKienFree()
        {
            InitializeComponent();
            btnListID.Enabled = false;

            //Initialize event
            try
            {
                hDcd = new DecodeHandle(DecodeDeviceCap.Exists | DecodeDeviceCap.Barcode);
                DecodeRequest reqType = (DecodeRequest)1 | DecodeRequest.PostRecurring;
                dcdEvent = new DecodeEvent(hDcd, reqType, this);
                dcdEvent.Scanned += new DecodeScanned(dcdEvent_Scanned);
            }
            catch { }
        }

        private void dcdEvent_Scanned(object sender, DecodeEventArgs e)
        {
            CodeId cID = CodeId.NoData;
            string dcdData = string.Empty;
            dcdData = hDcd.ReadString(e.RequestID, ref cID);

            ScanCode(dcdData);
        }

        private void dgCuonList_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //TEST
            //ScanCode("");

            listScanned.Clear();
            LoadData(txtMaXuatKho.Text.TrimStart().TrimEnd());

            //private List<PackageInfo> listSaveFail = new List<PackageInfo>();
            //private List<PackageInfo> listExportCancel;
            //private List<PackageInfo> listScanCancel;  
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            string maXuatKho = txtMaXuatKho.Text.TrimStart().TrimEnd();
            _exp_code = txtMaXuatKho.Text;

            btnLoad.Enabled = false;
            btnReset.Enabled = false;
            listScanned.Clear();
            dtList.Rows.Clear();

            if (maXuatKho != "")
            {
                LoadData(maXuatKho);
            }
            else
            {
                txtMaXuatKho.Focus();
            }
            btnListID.Enabled = true;

        }

        public void LoadFormByCode(string code)
        {
            string maXuatKho = "PART-EXPORT-REQ-" + code;
            txtMaXuatKho.Text = maXuatKho;
            _exp_code = txtMaXuatKho.Text;

            btnLoad.Enabled = false;
            btnReset.Enabled = false;
            listScanned.Clear();
            dtList.Rows.Clear();

            LoadData(maXuatKho);
            btnListID.Enabled = true;
        }

        private void LoadData(string maXuatKho)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            //Load danh sach linh kien
            try
            {
                string url = "getInfoXuatKho";
                if(this._checkOther == 1)
                    url = "getInfoXuatKhoOther";
                var paras = new
                {
                    ma_xuat_kho = maXuatKho
                };
                res = HTTP.Post(url, paras);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi tai du lieu xuat kho", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            if (res.Status && Util.IsJson(res.RawText))
            {
                try
                {
                    RootObject RootObject = JsonConvert.DeserializeObject<RootObject>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    _sl_thung_xuat = Convert.ToInt32(Util.ReflectPropertyValue(RootObject, "sl_thung"));
                    _sl_cuon_xuat = Convert.ToInt32(Util.ReflectPropertyValue(RootObject, "sl_cuon"));
                    this.lblDaXuat.Text = _sl_thung_xuat.ToString() + " thùng/" + _sl_cuon_xuat.ToString() + " cuon";

                    object details = Util.ReflectPropertyValue(RootObject, "detail");
                    List<Detail> ListDetail = details as List<Detail>;

                    dtList = Util.ToDataTable<Detail>(ListDetail);

                    dtList.Columns.Add(COL_SCANNED);
                    foreach (DataRow row in dtList.Rows)
                    {
                        row[COL_SCANNED] = 0;
                    }

                    dgCuonList.DataSource = dtList;

                    dgCuonList.TableStyles.Clear();
                    DataGridTableStyle tableStyle = new DataGridTableStyle();
                    tableStyle.MappingName = dtList.TableName;
                    foreach (DataColumn item in dtList.Columns)
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
                            case "total_export":
                                {
                                    tbcName.MappingName = item.ColumnName;
                                    tbcName.HeaderText = "Đã xuất";
                                    tbcName.Width = 40;
                                } break;
                            case "total_request":
                                {
                                    tbcName.MappingName = item.ColumnName;
                                    tbcName.HeaderText = "Tổng";
                                    tbcName.Width = 40;
                                } break;
                            case COL_SCANNED:
                                {
                                    tbcName.MappingName = item.ColumnName;
                                    tbcName.HeaderText = "Đã quét";
                                    tbcName.Width = 40;
                                } break;
                            case "part_id":
                                {
                                    tbcName.Width = -1;
                                } break;
                            default:
                                {
                                    tbcName.Width = 40;
                                } break;
                        }

                        if (tbcName.MappingName != "")
                        {
                            tbcName.MappingName = item.ColumnName;
                            tbcName.HeaderText = item.ColumnName;
                        }

                        tableStyle.GridColumnStyles.Add(tbcName);
                    }

                    dgCuonList.TableStyles.Add(tableStyle);

                    dgCuonList.Refresh();
                    btnReset.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Khong lay duoc du lieu");
                    txtMaXuatKho.Text = DEFAULT_TXT_MAXK;
                    _exp_code = "";
                }
            }
            else
            {
                //if (Util.CleanStr(res.RawText) != "") MessageBox.Show(Util.CleanStr(res.RawText));
                MessageBox.Show(res.RawText);
                txtMaXuatKho.Text = DEFAULT_TXT_MAXK;
                _exp_code = "";
            }
        }

        private void txtMaXuatKho_TextChanged(object sender, EventArgs e)
        {
            if (txtMaXuatKho.Text == "" || txtMaXuatKho.Text == DEFAULT_TXT_MAXK || txtMaXuatKho.Text == DEFAULT_TXT_MAXK_OTHER)
            {
                btnLoad.Enabled = false;
            }
            else
            {
                btnLoad.Enabled = true;
            }
        }

        private void XuatKhoLinhKienFree_Closing(object sender, CancelEventArgs e)
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

        private void ScanCode(string dcdData)
        {
            //Data test
            //dcdData = "PN:8NQMH3LM000001QJ*SPNK:PART-IMP-29*SDH:VNPT/TECH-BO-05/2017-112*NNK:2017-05-30*ID:0000211398*SL:50*TYPE:1*";
            //dcdData = "PN:K5BXDN4M000001K3*SPNK:IMP-RE-PART-DIF-55*SDH:*NNK:15/09/2017 9:53:22 AM*ID:0000306360*SL:2000.000000*TYPE:1*";

            //Obtain the string and code id.
            //MessageBox.Show(dcdData);
            try
            {
                TemCuon temCuon = new TemCuon(dcdData.Trim());
                DataRow[] rs = null;

                if (temCuon.IdCuon != null && temCuon.IdCuon != "")
                {                    
                    string _vnpt_pn_ok = temCuon.VnptPn;
                    if (this._checkOther != 1 && temCuon.VnptPn.Length > 14) _vnpt_pn_ok = _vnpt_pn_ok.Substring(0, 14);
                    
                    rs = dtList.Select("vnpt_pn = '" + _vnpt_pn_ok + "'");
                    DataRow dr = null;

                    if (rs.Length > 0)
                        dr = rs.FirstOrDefault();
                    else
                    {
                        MessageBox.Show("Linh kien nay khong co trong danh sach xuat kho", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }
                    
                    //check duplicate scan
                    PackageInfo packDuplicate = this.listScanned.SingleOrDefault(entry => entry.id_cuon == temCuon.IdCuon);
                    if (packDuplicate != null)
                    {
                        MessageBox.Show("Thung/cuon da duoc quet truoc do!");
                        return;
                    }

                    if (dr != null)
                    {
                        double _quantity_scan = Convert.ToDouble(dr[COL_SCANNED]);
                        double _total_export = Convert.ToDouble(dr["total_export"]);
                        double _total_request = Convert.ToDouble(dr["total_request"]);

                        //update table
                        _quantity_scan += Convert.ToDouble(temCuon.SoLuong);
                        dr[COL_SCANNED] = _quantity_scan;

                        //add code da quet vao list
                        PackageInfo package = new PackageInfo();
                        package.id_cuon = temCuon.IdCuon;
                        package.quantity = Convert.ToDouble(temCuon.SoLuong);
                        package.status = "DaQuet";
                        package.vnpt_pn = _vnpt_pn_ok;
                        package.type = Convert.ToInt32(temCuon.Type);

                        listScanned.Add(package);

                        //update quantity scanned
                        //check thung or cuon
                        int type = Util.ConvertInt(temCuon.Type);
                        if (type == 1) _sl_thung_quet++; else _sl_cuon_quet++;
                        lblDaQuet.Text = _sl_thung_quet.ToString() + " thùng/" + _sl_cuon_quet.ToString() + " cuon";

                        //enable button save
                        btnSave.Enabled = true;

                        if (_quantity_scan + _total_export > _total_request)
                        {
                            MessageBox.Show("So luong LK da quet vuot qua so luong yeu cau xuat cua linh kien");
                        }
                    }
                }
                else
                    MessageBox.Show("Ma thung/cuon khong dung", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
                //MessageBox.Show("Co loi xay ra", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnListID_Click(object sender, EventArgs e)
        {
            XuatKhoLinhKienListID frmListID = new XuatKhoLinhKienListID();
            frmListID.checkOther = this._checkOther;
            frmListID.listScanned = this.listScanned;
            frmListID.listSaveFail = this.listSaveFail;
            frmListID.maXuatKho = txtMaXuatKho.Text.TrimStart().TrimEnd();
            frmListID.ShowDialog();
            this.listScanned = frmListID.listScanned;
            this.listScanCancel = frmListID.listScanCancel;
            this.listExportCancel = frmListID.listExportCancel;
            if (this.listScanCancel.Count > 0) UpdateFormInfoScanned();
            if (this.listExportCancel.Count > 0) UpdateFormInfoExport();
            this.listSaveFail.Clear();
        }

        private void UpdateFormInfoScanned()
        {
            //Update Scan List
            foreach (PackageInfo package in this.listScanCancel)
            {
                DataRow[] rs = null;
                rs = dtList.Select("vnpt_pn = '" + package.vnpt_pn + "'");

                DataRow dr = null;

                if (rs.Length > 0)
                    dr = rs.FirstOrDefault();
                else return;

                if (dr != null)
                {
                    double _quantity_scan = Convert.ToDouble(dr[COL_SCANNED]);

                    if (_quantity_scan >= package.quantity)
                    {
                        //update table
                        _quantity_scan -= package.quantity;
                        dr[COL_SCANNED] = _quantity_scan;

                        //update quantity scanned
                        if (package.type == 1) _sl_thung_quet--; else _sl_cuon_quet--;
                        lblDaQuet.Text = _sl_thung_quet.ToString() + " thùng/" + _sl_cuon_quet.ToString() + " cuon";
                    }
                }
            }
        }

        private void UpdateFormInfoExport()
        {
            //Update Package Cancel to ERP
            string maXuatKho = txtMaXuatKho.Text.TrimStart().TrimEnd();
            ApiResponse res = new ApiResponse();
            res.Status = false;

            var listPackage = listCodeFromListObject(listExportCancel);
            try
            {
                string url = "removePackageXuatKho";
                if (this._checkOther == 1)
                    url = "removePackageXuatKhoOther";
                var paras = new
                {
                    ma_xuat_kho = maXuatKho,
                    list_package = listPackage
                };
                res = HTTP.Post(url, paras);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xay ra loi trong qua trinh huy thung/cuon", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            //Done
            this.listExportCancel.Clear();

            //refresh data
            LoadData(maXuatKho);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string maXuatKho = txtMaXuatKho.Text.TrimStart().TrimEnd();
            ApiResponse res = new ApiResponse();
            res.Status = false;

            var listPackage = listCodeFromListObject(listScanned);
            try
            {
                string url = "allocateXuatKho";
                if (this._checkOther == 1)
                    url = "allocateXuatKhoOther";
                var paras = new
                {
                    ma_xuat_kho = maXuatKho,
                    list_package = listPackage
                };
                res = HTTP.Post(url, paras);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Xay ra loi trong qua trinh allocate!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            if (res.Status && Util.IsJson(res.RawText))
            {
                try
                {
                    //Get list save fail
                    List<string> _listFail = JsonConvert.DeserializeObject<List<string>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    if (_listFail.Count > 0)
                    {
                        foreach (string _id in _listFail)
                        {
                            PackageInfo result = this.listScanned.SingleOrDefault(s => s.id_cuon == _id);
                            //add listSaveFail
                            if (result != null)
                            {
                                result.status = "Loi";
                                this.listSaveFail.Add(result);
                            }
                            //else
                            //{
                            //    PackageInfo _package_fail = new PackageInfo();
                            //    _package_fail.id_cuon = _id;
                            //    _package_fail.status = "Loi";
                            //    _package_fail.type = 0;
                            //    this.listSaveFail.Add(_package_fail);
                            //}
                        }
                        if (listScanned.Count > listSaveFail.Count)
                            MessageBox.Show("Da luu duoc " + (listScanned.Count - listSaveFail.Count).ToString() + " thung/cuon. Con " + listSaveFail.Count.ToString() + " thung/cuon da quet khong chinh xac!");
                        else
                            MessageBox.Show("Allocate khong thanh cong! Thung/cuon duoc quet khong chinh xac!");
                    }
                    else
                    {
                        MessageBox.Show("Luu thanh cong!");
                    }
                    _sl_thung_quet = 0;
                    _sl_cuon_quet = 0;
                    lblDaQuet.Text = _sl_thung_quet.ToString() + " thùng/" + _sl_cuon_quet.ToString() + " cuon";
                    btnSave.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xay ra loi trong qua trinh luu!");
                }
            }
            else
            {
                MessageBox.Show(res.RawText);
            }

            //Done
            this.listScanned.Clear();

            //refresh data
            LoadData(maXuatKho);
        }

        private string listCodeFromListObject(List<PackageInfo> lsTemCuon)
        {

            List<string> lsCode = new List<string>();

            foreach (PackageInfo tem in lsTemCuon)
            {
                lsCode.Add(tem.id_cuon);
            }

            string result = string.Join(",", lsCode.ToArray());
            return result;
        }

        private void checkOther_Click(object sender, EventArgs e)
        {
            if (this.checkOther.Checked)
            {
                this.txtMaXuatKho.Text = DEFAULT_TXT_MAXK_OTHER;
                this._checkOther = 1;
            }
            else
            {
                this.txtMaXuatKho.Text = DEFAULT_TXT_MAXK;
                this._checkOther = 0;
            }
        }
    }

    class PartModel
    {
        private string _vnpt_pn;
        private int _part_id;
        private double _total_export;
        private double _total_request;

        public string vnpt_pn
        {
            get { return _vnpt_pn; }
            set { _vnpt_pn = value; }
        }
        public int part_id
        {
            get { return _part_id; }
            set { _part_id = value; }
        }
        public double total_export
        {
            get { return _total_export; }
            set { _total_export = value; }
        }
        public double total_request
        {
            get { return _total_request; }
            set { _total_request = value; }
        }
    }

    public class Detail
    {
        public string vnpt_pn { get; set; }
        public int part_id { get; set; }
        public double total_request { get; set; }
        public double total_export { get; set; }
    }

    public class RootObject
    {
        public int id { get; set; }
        public string status { get; set; }
        public int product_id { get; set; }
        public int sl_thung { get; set; }
        public int sl_cuon { get; set; }
        public List<Detail> detail { get; set; }
    }

    public class PackageInfo
    {
        public string vnpt_pn { get; set; }
        public string id_cuon { get; set; }
        public string status { get; set; }
        public double quantity { get; set; }
        public int type { get; set; }
    }

}