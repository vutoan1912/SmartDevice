using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace ERP
{
    public partial class XuatKhoLinhKienListID : Form
    {
        private const string COL_ACTION = "action";
        private List<PackageInfo> _listScanned;
        private List<PackageInfo> _listExport = new List<PackageInfo>();
        private List<PackageInfo> _listScanCancel = new List<PackageInfo>();
        private List<PackageInfo> _listExportCancel = new List<PackageInfo>();
        private List<PackageInfo> _listSaveFail = new List<PackageInfo>();
        private List<PackageInfo> ListPackage;
        private DataTable dtList = new DataTable();
        private string _maXuatKho;
        private int _checkOther = 0;

        public List<PackageInfo> listScanned
        {
            get { return _listScanned; }
            set { _listScanned = value; }
        }

        public List<PackageInfo> listSaveFail
        {
            get { return _listSaveFail; }
            set { _listSaveFail = value; }
        }

        public List<PackageInfo> listExportCancel
        {
            get { return _listExportCancel; }
            set { _listExportCancel = value; }
        }

        public List<PackageInfo> listScanCancel
        {
            get { return _listScanCancel; }
        }

        public string maXuatKho
        {
            get { return _maXuatKho; }
            set { _maXuatKho = value; }
        }

        public int checkOther
        {
            get { return _checkOther; }
            set { _checkOther = value; }
        }

        public XuatKhoLinhKienListID()
        {
            InitializeComponent();
        }

        private void XuatKhoLinhKienListID_Load(object sender, EventArgs e)
        {
            GetListPackageDaXuat();
            initData();
            LoadData();
        }

        private void GetListPackageDaXuat()
        {
            if (maXuatKho != "")
            {
                ApiResponse res = new ApiResponse();
                res.Status = false;
                //Load danh sach package
                try
                {
                    string url = "getPackageXuatKho";
                    if(this._checkOther == 1)
                        url = "getPackageXuatKhoOther";
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
                        _listExport = JsonConvert.DeserializeObject<List<PackageInfo>>(res.RawText, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });
                        foreach (PackageInfo item in _listExport)
                        {
                            item.status = "DaXuat";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Khong lay duoc package da xuat");
                    }
                }
                else
                {
                    if (Util.CleanStr(res.RawText) != "") MessageBox.Show(Util.CleanStr(res.RawText));
                }
            }
        }

        private void initData()
        {
            ListPackage = _listSaveFail.Concat(_listExport).Concat(_listScanned).ToList();
        }

        private void LoadData()
        {
            dtList = Util.ToDataTable<PackageInfo>(ListPackage);

            dtList.Columns.Add(COL_ACTION);
            foreach (DataRow row in dtList.Rows)
            {
                row[COL_ACTION] = "huy";
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
                            tbcName.Width = 95;
                        } break;
                    case "id_cuon":
                        {
                            tbcName.MappingName = item.ColumnName;
                            tbcName.HeaderText = "ID";
                            tbcName.Width = 65;
                        } break;
                    case "status":
                        {
                            tbcName.MappingName = item.ColumnName;
                            tbcName.HeaderText = "Status";
                            tbcName.Width = 40;
                        } break;
                    case COL_ACTION:
                        {
                            tbcName.MappingName = item.ColumnName;
                            tbcName.HeaderText = "Action";
                            tbcName.Width = 25;
                        } break;
                    case "quantity":
                        {
                            tbcName.Width = -1;
                        } break;
                    case "type":
                        {
                            tbcName.Width = -1;
                        } break;
                    default:
                        {
                            tbcName.Width = 30;
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
                            if (hti.Column == 5)
                            {
                                DataRow r = dtList.Rows[hti.Row];
                                if (r[COL_ACTION] != "")
                                {
                                    DialogResult confirmResult = MessageBox.Show("Ban co chac chan muon huy?", "Ban co chac chan khong?", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);

                                    if (confirmResult == DialogResult.Yes)
                                    {
                                        string _id_move = r["id_cuon"].ToString();

                                        if (r["status"].ToString() == "DaXuat")
                                        {
                                            PackageInfo packRemove = _listExport.SingleOrDefault(entry => entry.id_cuon == _id_move);

                                            //_listScanned
                                            if (packRemove == null || !ListPackage.Remove(packRemove) || !_listExport.Remove(packRemove))
                                            {
                                                MessageBox.Show("Thao tac Huy Thung/Cuon that bai");
                                                return;
                                            }

                                            //remove package from list export
                                            _listExportCancel.Add(packRemove);
                                        }
                                        else if (r["status"].ToString() == "Loi")
                                        {
                                            PackageInfo packRemove = _listSaveFail.SingleOrDefault(entry => entry.id_cuon == _id_move);
                                            //_listSaveFail
                                            if (packRemove == null || !ListPackage.Remove(packRemove) || !_listSaveFail.Remove(packRemove))
                                            {
                                                MessageBox.Show("Thao tac Huy Thung/Cuon that bai");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            PackageInfo packRemove = _listScanned.SingleOrDefault(entry => entry.id_cuon == _id_move);

                                            //_listScanned
                                            if (packRemove == null || !ListPackage.Remove(packRemove) || !_listScanned.Remove(packRemove))
                                            {
                                                MessageBox.Show("Thao tac Huy Thung/Cuon that bai");
                                                return;
                                            }

                                            //remove package from list Scanned
                                            _listScanCancel.Add(packRemove);
                                        }

                                        //Reload
                                        LoadData();
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}