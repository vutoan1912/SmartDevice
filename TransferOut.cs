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
    public partial class TransferOut : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;
        
        private const string COL_SCANNED = "scanned";
        private const string COL_ID = "pack_id";
        private const string PREFIX_LOT = "LOT";
        private const string PREFIX_PACK = "PACK";

        private int _sl_thung_quet = 0;
        private int _sl_cuon_quet = 0;

        private int _transferId = 0;
        private TransferInfo TransferInfo;

        private DataTable dtList = new DataTable();
        //private List<TransferDetail> listScanned = new List<TransferDetail>();

        #region Properties

        #endregion

        public TransferOut()
        {
            InitializeComponent();

            //Initialize event
            try
            {
                hDcd = new DecodeHandle(DecodeDeviceCap.Exists | DecodeDeviceCap.Barcode);
                DecodeRequest reqType = (DecodeRequest)1 | DecodeRequest.PostRecurring;
                dcdEvent = new DecodeEvent(hDcd, reqType, this);
                dcdEvent.Scanned += new DecodeScanned(dcdEvent_Scanned);
            }
            catch { }

            btnReset.Enabled = false;
            btnSave.Enabled = false;
            dtList.Rows.Clear();
        }

        private void dcdEvent_Scanned(object sender, DecodeEventArgs e)
        {
            CodeId cID = CodeId.NoData;
            string dcdData = string.Empty;
            dcdData = hDcd.ReadString(e.RequestID, ref cID);

            ScanCode(dcdData);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //TEST
            //ScanCode("[)>@06@PEP2SPBTU000001@3SLOT00061@@");

            LoadData(_transferId);
        }

        private void LoadData(int _transferId)
        {
            dtList.Rows.Clear();

            ApiResponse res = new ApiResponse();
            res.Status = false;
            //Load danh sach linh kien
            try
            {
                string url = "transfer-details/search?query=transferId==" + _transferId.ToString() + "&size=2000";
                res = HTTP.GetJson(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Not exists transfer number !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            if (res.Status && Util.IsJson(res.RawText))
            {
                try
                {
                    List<TransferDetail> RootObject = JsonConvert.DeserializeObject<List<TransferDetail>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    List<TransferDetail> ListDetail = RootObject as List<TransferDetail>;

                    dtList = Util.ToDataTable<TransferDetail>(ListDetail);

                    
                    dtList.Columns.Add(COL_ID);
                    dtList.Columns.Add(COL_SCANNED);
                    foreach (DataRow row in dtList.Rows)
                    {
                        if (Convert.ToDouble(row["doneQuantity"]) > 0)
                            row[COL_SCANNED] = "OK";
                        else row[COL_SCANNED] = "";
                        try
                        {
                            if (Convert.ToString(row["traceNumber"]).Length > 0)
                                row[COL_ID] = row["traceNumber"];
                            else
                                row[COL_ID] = row["destPackageNumber"];
                        }
                        catch { row[COL_ID] = row["destPackageNumber"]; }
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
                            case "destLocationId":
                                {
                                    tbcName.MappingName = item.ColumnName;
                                    tbcName.HeaderText = "Location";
                                    tbcName.Width = 30;
                                } break;
                            case "internalReference":
                                {
                                    tbcName.MappingName = item.ColumnName;
                                    tbcName.HeaderText = "Product";
                                    tbcName.Width = 75;
                                } break;
                            case COL_ID:
                                {
                                    tbcName.MappingName = item.ColumnName;
                                    tbcName.HeaderText = "Package";
                                    tbcName.Width = 60;
                                } break;
                            //case "reserved":
                            //    {
                            //        tbcName.MappingName = item.ColumnName;
                            //        tbcName.HeaderText = "Quantity";
                            //        tbcName.Width = 30;
                            //    } break;
                            case COL_SCANNED:
                                {
                                    tbcName.MappingName = item.ColumnName;
                                    tbcName.HeaderText = "Scanned";
                                    tbcName.Width = 30;
                                } break;
                            default:
                                {
                                    tbcName.Width = 20;
                                } break;
                        }

                        //if (tbcName.MappingName != "")
                        //{
                        //    tbcName.MappingName = item.ColumnName;
                        //    tbcName.HeaderText = item.ColumnName;
                        //}

                        tableStyle.GridColumnStyles.Add(tbcName);
                    }

                    dgCuonList.TableStyles.Add(tableStyle);

                    dgCuonList.Refresh();
                    btnReset.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    //MessageBox.Show("Error during loading information !");
                }
            }
            else
            {
                MessageBox.Show(res.RawText);
            }
        }

        private void TansferOut_Closing(object sender, CancelEventArgs e)
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
            //dcdData = "[)>@06@P80N45HGJ456YU768@3SPKG3676573658@@";

            //Obtain the string and code id.
            //MessageBox.Show(dcdData);
            try
            {
                if (dcdData.StartsWith("["))
                {
                    #region scan package/lot
                    LabelPackage labelPackage = new LabelPackage(dcdData.Trim());
                    DataRow[] rs_product = null; DataRow[] rs_package = null;
                    DataRow dr = null;

                    bool _exists_product = false;
                    bool _exists_package = false;

                    if (labelPackage.ProductName != null && labelPackage.ProductName != "")
                    {
                        rs_product = dtList.Select("internalReference = '" + labelPackage.ProductName + "'");
                        DataRow dr_product = null;
                        if (rs_product.Length > 0)
                        {
                            _exists_product = true;
                        }

                        rs_package = dtList.Select(COL_ID + " = '" + labelPackage.PackageId + "'");
                        DataRow dr_package = null;
                        if (rs_package.Length > 0)
                        {
                            _exists_package = true;
                        }

                        DialogResult mgb = new DialogResult();
                        if (!_exists_product)
                        {
                            mgb = MessageBox.Show("Product not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb == DialogResult.Yes)
                            {
                                dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_ID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                                if (dr == null)
                                    addPackageNotInList(labelPackage.ProductName, labelPackage.PackageId);
                            }
                        }
                        else if (!_exists_package)
                        {
                            mgb = MessageBox.Show("Package not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb == DialogResult.Yes)
                            {
                                dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_ID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                                if(dr == null)
                                    addPackageNotInList(labelPackage.ProductName, labelPackage.PackageId);
                            }
                        }
                        else
                        {
                            //dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_ID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                            dr = dtList.Select(COL_ID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();

                            //check duplicate scan
                            //TransferDetail packDuplicate = this.listScanned.SingleOrDefault(entry => entry.id_cuon == temCuon.IdCuon);
                            //if (packDuplicate != null)
                            //{
                            //    MessageBox.Show("Thung/cuon da duoc quet truoc do!");
                            //    return;
                            //}

                            if (dr != null)
                            {
                                dr["doneQuantity"] = dr["reserved"];
                                dr[COL_SCANNED] = "X";
                            }
                        }

                        //enable button save
                        btnSave.Enabled = true;
                    }
                    else
                        MessageBox.Show("Error scan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    #endregion
                }
                else
                {
                    #region scan transfer
                    btnReset.Enabled = false;
                    //dtList.Rows.Clear();

                    try
                    {
                        string[] split = dcdData.Split('-');
                        _transferId = Convert.ToInt32(split[1]);
                        txtTransferNumber.Text = split[0].ToString();

                        if (_transferId != 0)
                        {
                            LoadData(_transferId);

                            ApiResponse res = new ApiResponse();
                            res.Status = false;

                            try
                            {
                                string url = "transfers/search?query=id==" + _transferId.ToString();
                                res = HTTP.GetJson(url);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Not exists transfer number !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            }

                            if (res.Status && Util.IsJson(res.RawText))
                            {
                                try
                                {
                                    List<TransferInfo> RootObject = JsonConvert.DeserializeObject<List<TransferInfo>>(res.RawText, new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore
                                    });

                                    if (RootObject.Count > 0)
                                    {
                                        TransferInfo = RootObject[0];
                                    }
                                    else
                                    {
                                        MessageBox.Show("Not exists transfer number !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error during loading information !");
                                }
                            }
                            else
                            {
                                MessageBox.Show(res.RawText);
                            }
                        }
                        else
                        {
                            txtTransferNumber.Focus();
                        }                        
                    }
                    catch (Exception ex) {
                        txtTransferNumber.Text = null;
                        btnReset.Enabled = false;
                        //listScanned.Clear();
                        dtList.Rows.Clear();
                        _transferId = 0;
                        MessageBox.Show("Transfer number wrong format !"); 
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
                //MessageBox.Show("Error during loading information !", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;

            try
            {
                string url = "transfers/" + _transferId.ToString();
 
                //transfer info
                string para_transfer = "{";
                para_transfer += "\"id\": " + _transferId + ", ";
                para_transfer += "\"created\": " + TransferInfo.created + ", ";
                para_transfer += "\"updated\": " + TransferInfo.updated + ", ";
                para_transfer += "\"createdBy\": \"" + TransferInfo.createdBy + "\", ";
                para_transfer += "\"transferNumber\": \"" + TransferInfo.transferNumber + "\", ";
                para_transfer += "\"originTransferNumber\": \"" + TransferInfo.originTransferNumber + "\", ";
                para_transfer += "\"operationTypeId\": " + TransferInfo.operationTypeId + ", ";
                para_transfer += "\"srcLocationId\": " + TransferInfo.srcLocationId + ", ";
                para_transfer += "\"destLocationId\": " + TransferInfo.destLocationId + ", ";
                para_transfer += "\"scheduledDate\": " + TransferInfo.scheduledDate + ", ";
                para_transfer += "\"state\": \"" + TransferInfo.state + "\", ";
                para_transfer += "\"productVersionId\": " + TransferInfo.productVersionId + ", ";
                para_transfer += "\"sourceDocument\": \"" + TransferInfo.sourceDocument + "\", ";
                para_transfer += "\"priority\": \"" + TransferInfo.priority + "\", ";
                para_transfer += "\"currentDemand\": " + TransferInfo.currentDemand + ", ";
                para_transfer += "\"productionQuantity\": " + TransferInfo.productionQuantity + ", ";
                para_transfer += "\"capacity\": " + TransferInfo.capacity + ", ";

                //transfer details
                para_transfer += "\"transferDetails\": [ ";
                foreach (DataRow row in dtList.Rows)
                {
                    if (row[COL_SCANNED] == "X")
                    {
                        para_transfer += "{ ";
                        if (row["destLocationId"] != null && Convert.ToString(row["destLocationId"]).Length > 0) para_transfer += "\"destLocationId\": " + row["destLocationId"] + ", ";
                        if (row["destPackageNumber"] != null && Convert.ToString(row["destPackageNumber"]).Length > 0) para_transfer += "\"destPackageNumber\": \"" + row["destPackageNumber"] + "\", ";
                        if (row["doneQuantity"] != null && Convert.ToString(row["doneQuantity"]).Length > 0) para_transfer += "\"doneQuantity\": " + row["doneQuantity"] + ", ";
                        if (row["manId"] != null && Convert.ToString(row["manId"]).Length > 0) para_transfer += "\"manId\": " + row["manId"] + ", ";
                        if (row["manPn"] != null && Convert.ToString(row["manPn"]).Length > 0) para_transfer += "\"manPn\": \"" + row["manPn"] + "\", ";
                        if (row["productId"] != null && Convert.ToString(row["productId"]).Length > 0) para_transfer += "\"productId\": " + row["productId"] + ", ";
                        if (row["reserved"] != null && Convert.ToString(row["reserved"]).Length > 0) para_transfer += "\"reserved\": " + row["reserved"] + ", ";
                        if (row["srcLocationId"] != null && Convert.ToString(row["srcLocationId"]).Length > 0) para_transfer += "\"srcLocationId\": " + row["srcLocationId"] + ", ";
                        if (row["traceNumber"] != null && Convert.ToString(row["traceNumber"]).Length > 0) para_transfer += "\"traceNumber\": \"" + row["traceNumber"] + "\", ";
                        if (row["transferId"] != null && Convert.ToString(row["transferId"]).Length > 0) para_transfer += "\"transferId\": " + row["transferId"] + ", ";
                        if (row["transferItemId"] != null && Convert.ToString(row["transferItemId"]).Length > 0) para_transfer += "\"transferItemId\": " + row["transferItemId"] + ", ";
                        if (row["id"] != null && Convert.ToString(row["id"]).Length > 0) para_transfer += "\"id\": " + row["id"] + ", ";
                        if (row["lotId"] != null && Convert.ToString(row["lotId"]).Length > 0) para_transfer += "\"lotId\": " + row["lotId"] + ", ";
                        if (row["reference"] != null && Convert.ToString(row["reference"]).Length > 0) para_transfer += "\"reference\": \"" + row["reference"] + "\", ";
                        if (row["level"] != null && Convert.ToString(row["level"]).Length > 0) para_transfer += "\"level\": \"" + row["level"] + "\", ";
                        if (row["created"] != null && Convert.ToString(row["created"]).Length > 0) para_transfer += "\"created\": " + row["created"] + ", ";
                        if (row["createdBy"] != null && Convert.ToString(row["createdBy"]).Length > 0) para_transfer += "\"createdBy\": \"" + row["createdBy"] + "\" ";
                        if (row["internalReference"] != null && Convert.ToString(row["internalReference"]).Length > 0) para_transfer += "\"internalReference\": \"" + row["internalReference"] + "\" ";
                        para_transfer += "}, ";
                    }
                }
                string end_char = para_transfer.Substring(para_transfer.Length - 2, 1);
                if (end_char == ",") para_transfer = para_transfer.Substring(0, para_transfer.Length - 2);
                para_transfer += " ] }";

                res = HTTP.Put(url, para_transfer);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during allocate !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            if (res.Status)
            {
                
            }
            else
            {
                //MessageBox.Show(res.RawText);
            }

            btnSave.Enabled = false;

            //refresh data
            LoadData(_transferId);
        }

        private void dgCuonList_CurrentCellChanged(object sender, EventArgs e)
        {
            
        }

        private void txtTransferNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string _transferNumber = txtTransferNumber.Text.Trim();
                ApiResponse res = new ApiResponse();
                res.Status = false;
                
                try
                {
                    string url = "transfers/search?query=transferNumber=='" + _transferNumber + "'";
                    res = HTTP.GetJson(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Not exists transfer number !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                try
                {
                    if (res.Status && Util.IsJson(res.RawText))
                    {

                        List<TransferInfo> RootObject = JsonConvert.DeserializeObject<List<TransferInfo>>(res.RawText, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                        if (RootObject.Count > 0)
                        {
                            TransferInfo = RootObject[0];
                            _transferId = TransferInfo.id;

                            if (_transferId != 0)
                            {
                                LoadData(_transferId);
                            }
                            else
                            {
                                txtTransferNumber.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Not exists transfer number !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                    }
                    else
                    {
                        MessageBox.Show(res.RawText);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during loading information !");
                }
            }
        }

        private void addPackageNotInList(string _productName, string _packageId)
        {
            try
            {
                DataRow dr = this.dtList.NewRow();
                dr[COL_ID] = _packageId;
                if(_packageId.StartsWith(PREFIX_LOT))
                    dr["traceNumber"] = _packageId;
                else
                    dr["destPackageNumber"] = _packageId;
                dr["internalReference"] = _productName;
                dr[COL_SCANNED] = "X";
                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;
                this.dgCuonList.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }

    public class TransferDetail
    {
        public object created { get; set; }
        public object updated { get; set; }
        public string createdBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public int transferId { get; set; }
        public int transferItemId { get; set; }
        public string productName { get; set; }
        public int productId { get; set; }
        public string manPn { get; set; }
        public int doneQuantity { get; set; }
        public int srcLocationId { get; set; }
        public int destLocationId { get; set; }
        public string state { get; set; }
        public int destPackageId { get; set; }
        public string destPackageNumber { get; set; }
        public string traceNumber { get; set; }
        public int reserved { get; set; }
        public int manId { get; set; }
        public int lotId { get; set; }
        public string reference { get; set; }
        public string level { get; set; }
        public string internalReference { get; set; }
        public string productDescription { get; set; }
        public int available { get; set; }
    }

    public class TransferInfo
    {
        public long created { get; set; }
        public long updated { get; set; }
        public string createdBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string transferNumber { get; set; }
        public string originTransferNumber { get; set; }
        public int operationTypeId { get; set; }
        public int srcLocationId { get; set; }
        public int destLocationId { get; set; }
        public long scheduledDate { get; set; }
        public string state { get; set; }
        public int productVersionId { get; set; }
        public string sourceDocument { get; set; }
        public string priority { get; set; }
        public List<object> quantInfo { get; set; }
        public int currentDemand { get; set; }
        public List<int> otherProjects { get; set; }
        public int productionQuantity { get; set; }
        public int capacity { get; set; }
        public List<object> moItems { get; set; }
        public List<object> transferItems { get; set; }
        public List<object> transferDetails { get; set; }
        public List<object> removedTransferDetails { get; set; }
        public List<object> removedTransferItems { get; set; }
    }

}