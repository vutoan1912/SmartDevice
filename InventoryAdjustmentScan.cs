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
using System.IO;
using ERP.Base;

namespace ERP
{
    public partial class InventoryAdjustmentScan : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;
        
        private const string COL_DEL = "delete";
        private const string COL_LOT = "traceNumber";
        private const string COL_PACK = "packageNumber";
        private const string COL_QUANT = "realQuantity";
        private const string COL_PRODUCT_NAME = "internalReference";
        private const string COL_BARCODE = "barcode";
        private const string PREFIX_LOT = "LOT";
        private const string PREFIX_PACK = "PACK";

        private DataTable dtList = new DataTable();
        private string locationBarcode;
        private Adjustment _Adjustment;
        public Adjustment Adjustment { set { _Adjustment = value; } get { return _Adjustment; } }

        private List<AdjustmentDetail> _ListSpace = new List<AdjustmentDetail>();
        public List<AdjustmentDetail> ListSpace { set { _ListSpace = value; } get { return _ListSpace; } }
        //public string _action = "";

        #region Properties

        #endregion

        public InventoryAdjustmentScan()
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

            //btnClear.Enabled = false;
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
            _ListSpace.Clear();
            dtList.Rows.Clear();
            this.dgCuonList.Refresh();
            //_action = "cancel";
            //this.Close();

            //ScanCode("241-B.4");
            //scancode("1482-sd13-4");

            //ScanCode("[)>@06@PKBEFJGYB0000336X@3SLOT00106@@");
            //ScanCode("[)>@06@PKBEFJGYB0000336X@3SLOT00107@@");
            //ScanCode("[)>@06@PKBEFJGYB0000336X@3SLOT00108@@");
            //ScanCode("[)>@06@PEP2SPBTU000001@3SLOT00060@@");
            //ScanCode("[)>@06@PEP2SPBTU000001@3SLOT00061@@");
        }

        //private void SetReadOnly()
        //{
        //    DataColumnCollection myDataColumns;
        //    // Get the columns for a table bound to a DataGrid.
        //    myDataColumns = dtList.Columns;
        //    foreach (DataColumn dataColumn in myDataColumns)
        //    {
        //        dgCuonList.TableStyles[0].GridColumnStyles[dataColumn.ColumnName].ReadOnly = dataColumn.ReadOnly;
        //    }
        //}

        private void InitData()
        {
            try
            {
                dtList.Rows.Clear();
                dtList = Util.ToDataTable<AdjustmentDetail>(_ListSpace);
                //dtList.Columns.Add(COL_PRODUCT_NAME);
                //dtList.Columns.Add(COL_ID);
                //dtList.Columns.Add(COL_QUANT);
                //dtList.Columns.Add(COL_BARCODE);
                dtList.Columns.Add(COL_DEL);

                foreach (DataRow row in dtList.Rows)
                {
                    row[COL_DEL] = "X";
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
                        case COL_QUANT:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Quantity";
                                tbcName.NullText = "";
                                tbcName.Width = 50;
                            } break;
                        case COL_PACK:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Package";
                                tbcName.NullText = "";
                                tbcName.Width = 70;
                            } break;
                        case COL_LOT:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Lot";
                                tbcName.NullText = "";
                                tbcName.Width = 60;
                            } break;
                        case COL_DEL:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.NullText = "";
                                tbcName.HeaderText = "Cancel";
                                tbcName.Width = 40;
                            } break;
                        default:
                            {
                                tbcName.NullText = "";
                                tbcName.Width = 20;
                            } break;
                    }

                    tableStyle.GridColumnStyles.Add(tbcName);
                }

                dgCuonList.TableStyles.Add(tableStyle);
                
                dgCuonList.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private bool checkExistsInTable(string packageId)
        {
            //Check exists package/lot in table
            DataRow[] rs_package = null;
            if (packageId.StartsWith(PREFIX_LOT))
                rs_package = dtList.Select(COL_LOT + " = '" + packageId + "'");
            else if (packageId.StartsWith(PREFIX_PACK))
                rs_package = dtList.Select(COL_PACK + " = '" + packageId + "'");
            if (rs_package.Length > 0)
            {
                MessageBox.Show("Package already exists in the list !");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ScanCode(string dcdData)
        {
            if (dcdData.StartsWith("["))
            {
                #region Scan package/lot
                try
                {
                    string scanValue = dcdData.Replace(System.Environment.NewLine, "").Trim();
                    LabelPackage labelPackage = new LabelPackage(scanValue);

                    //Util.Logs(labelPackage.PackageId);

                    if (labelPackage.PackageId != null && labelPackage.PackageId != "")
                    {
                        //Check package exists in inventory
                        ApiResponse res = new ApiResponse();
                        res.Status = false;
                        string url = "";
                        if (labelPackage.PackageId.StartsWith(PREFIX_LOT))
                        {
                            Lots lot = new Lots();
                            Lots lotInfo = lot.getInfo(labelPackage.PackageId);
                            if (lotInfo == null)
                            {
                                MessageBox.Show("Lot is not exists in inventory !");
                                return;
                            }
                            else
                            {
                                url = "adjustment-details/search?query=adjustmentId==" + _Adjustment.id.ToString() + ";lotId==" + lotInfo.id.ToString();
                            }
                        }
                        else if (labelPackage.PackageId.StartsWith(PREFIX_PACK))
                        {
                            Packages package = new Packages();
                            Packages packageInfo = package.getInfo(labelPackage.PackageId);
                            if (packageInfo == null)
                            {
                                MessageBox.Show("Package is not exists in inventory !");
                                return;
                            }
                            else
                            {
                                url = "adjustment-details/search?query=adjustmentId==" + _Adjustment.id.ToString() + ";packageId==" + packageInfo.id.ToString();
                            }
                        }
                        if (url.Length == 0) { MessageBox.Show("Wrong package/lot QRCode format !"); return; }

                        try
                        {
                            res = HTTP.GetJson(url);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Server error !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }
                        List<AdjustmentDetail> ListDetail;
                        if (res.Status && Util.IsJson(res.RawText))
                        {
                            
                            List<AdjustmentDetail> RootObject = JsonConvert.DeserializeObject<List<AdjustmentDetail>>(res.RawText, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

                            ListDetail = RootObject as List<AdjustmentDetail>;
                            
                            if (checkExistsInTable(labelPackage.PackageId))
                            {
                                return;
                            }
                            else
                            {
                                if (ListDetail.Count == 0)
                                {
                                    DialogResult mgb = new DialogResult();
                                    mgb = MessageBox.Show("Package/Lot is not exists in inventory adjustment! Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    if (mgb != DialogResult.Yes) return;
                                    addPackageToList(labelPackage.PackageId, 0, labelPackage.ProductName, locationBarcode);
                                    //enable button save
                                    btnSave.Enabled = true;
                                    btnClear.Enabled = true;
                                }
                                else
                                {
                                    if (labelPackage.PackageId.StartsWith(PREFIX_LOT))
                                    {
                                        addPackageToList(ListDetail[0]);
                                    }
                                    else if (labelPackage.PackageId.StartsWith(PREFIX_PACK))
                                    {
                                        foreach (AdjustmentDetail item in ListDetail)
                                        {
                                            addPackageToList(item);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Server error !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }

                    }
                    else
                        MessageBox.Show("Wrong package/lot QRCode format !");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error scan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    //MessageBox.Show(ex.InnerException.ToString());
                    //MessageBox.Show("Error during loading information !", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                #endregion
            }
            else
            {
                #region Scan location
                locationBarcode = dcdData.Replace(System.Environment.NewLine, "").Trim();
                try
                {
                    ApiResponse res = new ApiResponse();
                    res.Status = false;

                    string url = "locations/check-child";
                    var param = new
                    {
                        locationId = this._Adjustment.locationId,
                        barcode = locationBarcode
                    };
                    res = HTTP.Post(url, param);
                    if (res.Status)
                    {
                        if (Convert.ToBoolean(res.RawText))
                        {
                            try
                            {
                                string[] split = dcdData.Split('-');
                                this.lblLocationBarcode.Text = dcdData.Substring(split[0].Length + 1, dcdData.Length - (split[0].Length + 1));
                                btnSave.Enabled = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Wrong location barcode format !");
                                //MessageBox.Show(ex.InnerException.ToString());
                                //MessageBox.Show("Error during loading information !", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Location is not exists in inventory!");
                            btnSave.Enabled = false;
                            this.dtList.Rows.Clear();
                            this.dgCuonList.Refresh();
                        }
                    }
                    else
                    {
                        btnSave.Enabled = false;
                        this.dtList.Rows.Clear();
                        this.dgCuonList.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during process !");
                    //Util.Logs(ex.ToString());
                }
                #endregion
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //_action = "save";
            string param = "";
            try
            {
                _Adjustment.adjustmentDetails = this._ListSpace;//Util.DataTableToList<AdjustmentDetail>(this.dtList);
                param = JsonConvert.SerializeObject(_Adjustment);
                param = param.Replace(System.Environment.NewLine, "").Trim();
                //Util.Logs(param);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return;
            }

            try
            {
                ApiResponse res = new ApiResponse();
                res.Status = false;

                string url = "inventories/" + this.Adjustment.id.ToString();
                res = HTTP.Put(url, param);
                if (res.Status)
                {
                    MessageBox.Show("Success !");
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;
                    this.dtList.Rows.Clear();
                    this.dgCuonList.Refresh();
                    this._ListSpace.Clear();
                }
                else
                {
                    Util.Logs("res.Message: " + res.Message + " --- res.RawText: " + res.RawText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            
        }

        private void addPackageToList(string _packageId, double _realQuantity, string _internalReference, string _barcode)
        {
            try
            {
                DataRow dr = this.dtList.NewRow();
                if (_packageId.StartsWith(PREFIX_LOT))
                {
                    dr[COL_LOT] = _packageId;
                    dr[COL_PACK] = null;
                }
                else if (_packageId.StartsWith(PREFIX_PACK))
                {
                    dr[COL_PACK] = _packageId;
                    dr[COL_LOT] = null;
                }
                dr[COL_QUANT] = _realQuantity;
                dr[COL_DEL] = "X";
                dr[COL_PRODUCT_NAME] = _internalReference;
                dr[COL_BARCODE] = _barcode;
                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;

                AdjustmentDetail newScan = new AdjustmentDetail();
                newScan.barcode = _barcode;
                newScan.internalReference = _internalReference;
                if (_packageId.StartsWith(PREFIX_LOT))
                    newScan.traceNumber = _packageId;
                else if (_packageId.StartsWith(PREFIX_PACK))
                    newScan.packageNumber = _packageId;
                newScan.realQuantity = _realQuantity;
                newScan.id = null;
                newScan.adjustmentId = _Adjustment.id;
                newScan.productId = null;
                newScan.manId = null;
                newScan.locationId = null;

                this._ListSpace.Add(newScan);

                this.dgCuonList.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void addPackageToList(AdjustmentDetail _adjustmentDetail)
        {
            Lots lotInfo = null; Packages packInfo = null;
            
            try
            {
                if (_adjustmentDetail.lotId != null)
                {
                    Lots lot = new Lots();
                    lotInfo = lot.getInfo(Convert.ToInt32(_adjustmentDetail.lotId));
                }
                }
            catch { }
            try
            {
                if (_adjustmentDetail.packageId != null)
                {
                    Packages pack = new Packages();
                    packInfo = pack.getInfo(Convert.ToInt32(_adjustmentDetail.packageId));
                    //Util.Logs(packInfo.packageNumber);
                }
            }
            catch (Exception ex) { Util.Logs(ex.ToString()); }

            try
            {
                DataRow dr = this.dtList.NewRow();
                if (lotInfo != null) { dr[COL_LOT] = lotInfo.lotNumber; _adjustmentDetail.traceNumber = lotInfo.lotNumber; } 
                else dr[COL_LOT] = "";
                if (packInfo != null) { dr[COL_PACK] = packInfo.packageNumber; _adjustmentDetail.packageNumber = packInfo.packageNumber; } 
                else dr[COL_PACK] = "";
                dr[COL_QUANT] = _adjustmentDetail.realQuantity;
                dr[COL_DEL] = "X";
                dr[COL_PRODUCT_NAME] = _adjustmentDetail.internalReference;
                dr[COL_BARCODE] = _adjustmentDetail.barcode;
                dr["created"] = _adjustmentDetail.created;
                dr["updated"] = _adjustmentDetail.updated;
                dr["createdBy"] = _adjustmentDetail.createdBy;
                dr["active"] = _adjustmentDetail.active;
                dr["id"] = _adjustmentDetail.id;
                dr["adjustmentId"] = _adjustmentDetail.adjustmentId;
                dr["productId"] = _adjustmentDetail.productId;
                dr["manId"] = _adjustmentDetail.manId;
                dr["locationId"] = _adjustmentDetail.locationId;
                dr["locationName"] = _adjustmentDetail.locationName;
                dr["theoreticalQuantity"] = _adjustmentDetail.theoreticalQuantity;
                dr["state"] = _adjustmentDetail.state;
                dr["manPn"] = _adjustmentDetail.manPn;
                dr["productDescription"] = _adjustmentDetail.productDescription;
                dr["lotId"] = _adjustmentDetail.lotId;
                dr["packageId"] = _adjustmentDetail.packageId;                
                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;
                this._ListSpace.Add(_adjustmentDetail);
                this.dgCuonList.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SetLocation_Closing(object sender, CancelEventArgs e)
        {
            try
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
            catch { }
        }

        private void SetLocation_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void dgCuonList_Click(object sender, EventArgs e)
        {
            try
            {
                int index_column_quant = 0;
                int index_column_del = 0;
                foreach (DataColumn item in dtList.Columns)
                {
                    if (item.ColumnName == COL_QUANT)
                    {
                        break;
                    }
                    index_column_quant++;
                }
                foreach (DataColumn item in dtList.Columns)
                {
                    if (item.ColumnName == COL_DEL)
                    {
                        break;
                    }
                    index_column_del++;
                }

                DataGridCell row = dgCuonList.CurrentCell;
                //MessageBox.Show(row.ColumnNumber.ToString());
                if (row.ColumnNumber == index_column_del)
                {
                    dtList.Rows[row.RowNumber].Delete();
                    this._ListSpace.RemoveAt(row.RowNumber);
                    dgCuonList.Refresh();

                    //Delete in List
                    //int index = -1;
                    //if (dtList.Rows[row.RowNumber][COL_LOT].StartsWith(PREFIX_LOT))
                    //{
                    //    index = ListDetail.FindIndex(a => a.traceNumber == labelPackage.PackageId);
                    //}
                    //else
                    //{
                    //    index = ListDetail.FindIndex(a => a.packageNumber == labelPackage.PackageId);
                    //}
                    //if (index < 0)
                    //{
                    //    DialogResult mgb = new DialogResult();
                    //    mgb = MessageBox.Show("Package/Lot is not exists in inventory adjustment! Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    //    if (mgb != DialogResult.Yes) return;
                    //}
                }
                else if (row.ColumnNumber == index_column_quant)
                {
                    TextEditable textEditable = new TextEditable();
                    textEditable.ShowDialog();
                    try
                    {
                        double value = Convert.ToDouble(textEditable.valueEdit);
                        dtList.Rows[row.RowNumber][COL_QUANT] = value;
                        _ListSpace[row.RowNumber].realQuantity = value;
                        dgCuonList.Refresh();
                    }
                    catch { MessageBox.Show("Wrong format input !"); }
                }
            }
            catch { }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //_action = "back";
            this.Close();

            //ScanCode("583-B7-1");
        }
    }

}