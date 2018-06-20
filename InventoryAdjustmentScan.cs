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
        private const string COL_QUANT_THEORETICAL = "theoreticalQuantity";
        private const string COL_PRODUCT_NAME = "internalReference";
        private const string COL_BARCODE = "barcode";
        private const string COL_STATUS = "Status";

        private const string PREFIX_LOT = "UID";
        private const string PREFIX_PACK = "PACK";

        private DataTable dtList = new DataTable();
        private string locationBarcode;
        private Adjustment _Adjustment;
        public Adjustment Adjustment { set { _Adjustment = value; } get { return _Adjustment; } }
        private LocationInfo _LocationInfo;

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
            //ScanCode("578-C7-1");
        }

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
                dtList.Columns.Add(COL_STATUS);

                foreach (DataRow row in dtList.Rows)
                {
                    row[COL_DEL] = "Delete";
                    row[COL_STATUS] = "";
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
                                tbcName.Width = 35;
                            } break;
                        case COL_QUANT_THEORETICAL:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Theoretical quantity";
                                tbcName.NullText = "";
                                tbcName.Width = 35;
                            } break;
                        case COL_PACK:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Package";
                                tbcName.NullText = "";
                                tbcName.Width = 60;
                            } break;
                        case COL_LOT:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "UnitID";
                                tbcName.NullText = "";
                                tbcName.Width = 60;
                            } break;
                        case COL_DEL:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.NullText = "";
                                tbcName.HeaderText = "Cancel";
                                tbcName.Width = 30;
                            } break;
                        case COL_STATUS:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.NullText = "";
                                tbcName.HeaderText = "Status";
                                tbcName.Width = 20;
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
            //Check exists Package/Unit ID in table
            DataRow[] rs_package = null;
            if (Util.getTypePackage(packageId, null) % 2 == 0)
                rs_package = dtList.Select(COL_LOT + " = '" + packageId + "'");
            else //if (Util.getTypePackage(packageId, null) % 2 != 0)
                rs_package = dtList.Select(COL_PACK + " = '" + packageId + "'");
            if (rs_package.Length > 0)
            {
                MessageBox.Show("Package/UID already exists in the list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkExistsInTable(string packageId, int type)
        {
            //Check exists package/uid in table
            DataRow[] rs_package = null;
            if (Util.getTypePackage(packageId, type.ToString()) % 2 == 0)
                rs_package = dtList.Select(COL_LOT + " = '" + packageId + "'");
            else //if (Util.getTypePackage(packageId, type.ToString()) % 2 != 0)
                rs_package = dtList.Select(COL_PACK + " = '" + packageId + "'");
            if (rs_package.Length > 0)
            {
                MessageBox.Show("Package already exists in the list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return true;
            }
            else
            {
                return false;
            }
        }

        private LocationInfo getLocationInfo(string locationBarcode)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            try
            {
                string url = "locations/search?query=barcode==\"" + locationBarcode + "\"";
                res = HTTP.GetJson(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }

            if (res.Status && Util.IsJson(res.RawText))
            {
                try
                {
                    List<LocationInfo> RootObject = JsonConvert.DeserializeObject<List<LocationInfo>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    List<LocationInfo> ListLocations = RootObject as List<LocationInfo>;
                    if (ListLocations.Count > 0)
                    {
                        return ListLocations[0];
                    }
                    else
                    {
                        MessageBox.Show("Location is not exists in inventory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    return null;
                }
            }
            else
            {
                MessageBox.Show("Error during load information location !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return null;
            }
        }

        private void addPackageFromStock(int _id, string _packageId, bool isPackage, string _productName, string _locationBarcode, string type, double realDefault)
        {
            List<Quant> ListQuant = new List<Quant>();
            if (!isPackage)
            {
                Lots lotInfo = LotBusiness.getFullInfo(_id);
                if (lotInfo.quants == null)
                {
                    addPackageToList(null, _packageId, realDefault, _productName, _locationBarcode, type);
                    return;
                }
                else
                {
                    ListQuant.Add(lotInfo.quants[0]);
                }
            }
            else
            {
                Packages packageInfo = PackageBusiness.getFullInfo(_id);
                if (packageInfo.quants == null)
                {
                    addPackageToList(_packageId, null, realDefault, _productName, _locationBarcode, type);
                    return;
                }
                else
                {
                    ListQuant = packageInfo.quants;
                }
            }

            try
            {
                if (!isPackage)
                {
                    addPackageToList(null, _packageId, ListQuant[0].onHand, _productName, _locationBarcode, type);
                }
                else if (ListQuant.Count == 1 && ListQuant[0].lotId == null)
                {
                    addPackageToList(_packageId, null, ListQuant[0].onHand, _productName, _locationBarcode, type);
                }
                else
                {
                    foreach (Quant item in ListQuant)
                    {
                        if (item.lotId != null)
                        {
                            Lots Lot = LotBusiness.getFullInfo(Convert.ToInt32(item.lotId));
                            addPackageToList(_packageId, Lot.lotNumber, item.onHand, Lot.internalReference, _locationBarcode, type);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add package from stock to list: " + ex.Message.ToString(), "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
        }

        private void ScanCode(string dcdData)
        {
            if (dcdData.StartsWith("["))
            {
                #region Scan Package/Unit ID new
                if (this._LocationInfo == null)
                {
                    MessageBox.Show("Request scan location before scan the package!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }

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
                        int _id = 0; bool isPackage = true;
                        if (Util.getTypePackage(labelPackage.PackageId, null) % 2 == 0)
                        {
                            isPackage = false;
                            Lots lot = new Lots();
                            Lots lotInfo = lot.getInfo(labelPackage.PackageId);
                            if (lotInfo == null)
                            {
                                MessageBox.Show("Unit ID is not exists in inventory !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }
                            else
                            {
                                url = "adjustment-details/search?query=adjustmentId==" + _Adjustment.id.ToString() + ";lotId==" + lotInfo.id.ToString();
                                _id = lotInfo.id;
                            }
                        }
                        else if (Util.getTypePackage(labelPackage.PackageId, null) % 2 != 0)
                        {
                            isPackage = true;
                            Packages package = new Packages();
                            Packages packageInfo = package.getInfo(labelPackage.PackageId);
                            if (packageInfo == null)
                            {
                                MessageBox.Show("Package is not exists in inventory !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }
                            else
                            {
                                url = "adjustment-details/search?query=adjustmentId==" + _Adjustment.id.ToString() + ";packageId==" + packageInfo.id.ToString();
                                _id = packageInfo.id;
                            }
                        }
                        if (url.Length == 0)
                        {
                            MessageBox.Show("Wrong Package/Unit ID QRCode format !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }

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
                                    mgb = MessageBox.Show("Package/Unit ID is not exists in inventory adjustment! Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    if (mgb != DialogResult.Yes) return;

                                    addPackageFromStock(_id, labelPackage.PackageId, isPackage, labelPackage.ProductName, _LocationInfo.barcode, null, 0);
                                    //addPackageToList(labelPackage.PackageId, 0, labelPackage.ProductName, _LocationInfo.barcode);

                                    //enable button save
                                    if (_LocationInfo != null && !btnSave.Enabled)
                                    {
                                        btnSave.Enabled = true;
                                    }
                                }
                                else
                                {
                                    if (labelPackage.PackageId.StartsWith(PREFIX_LOT) || ListDetail.Count == 1)
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

                                    //enable button save
                                    if (_LocationInfo != null && !btnSave.Enabled)
                                    {
                                        btnSave.Enabled = true;
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
                        MessageBox.Show("Wrong Package/Unit ID QRCode format !");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error scan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                #endregion
            }
            else if (dcdData.Trim().StartsWith("PN:"))
            {
                #region Scan Package/Unit ID old
                if (_LocationInfo == null)
                {
                    MessageBox.Show("Request scan location before scan the package!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }

                try
                {
                    string scanValue = dcdData.Replace(System.Environment.NewLine, "").Trim();
                    TemCuon temCuon = new TemCuon(scanValue);

                    if (temCuon.IdCuon != null && temCuon.IdCuon != "")
                    {
                        //Check package exists in inventory
                        ApiResponse res = new ApiResponse();
                        res.Status = false;
                        string url = "";
                        int _id = 0; bool isPackage = true;

                        if (Convert.ToInt32(temCuon.Type) == 0)
                        {
                            isPackage = false;
                            Lots lot = new Lots();
                            Lots lotInfo = lot.getInfo(temCuon.IdCuon);
                            if (lotInfo == null)
                            {
                                DialogResult mgb = new DialogResult();
                                mgb = MessageBox.Show("Uid is not exists in inventory! Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                if (mgb != DialogResult.Yes) return;
                                if (checkExistsInTable(temCuon.IdCuon, Convert.ToInt32(temCuon.Type))) return;
                                addPackageToList(null, temCuon.IdCuon, Convert.ToDouble(temCuon.SoLuong), temCuon.VnptPn, _LocationInfo.barcode, temCuon.Type);
                            }
                            url = "adjustment-details/search?query=adjustmentId==" + _Adjustment.id.ToString() + ";lotId==" + lotInfo.id.ToString();
                            _id = lotInfo.id;
                        }
                        else if (Convert.ToInt32(temCuon.Type) == 1)
                        {
                            isPackage = true;
                            Packages package = new Packages();
                            Packages packageInfo = package.getInfo(temCuon.IdCuon);
                            if (packageInfo == null)
                            {
                                DialogResult mgb = new DialogResult();
                                mgb = MessageBox.Show("Package is not exists in inventory! Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                if (mgb != DialogResult.Yes) return;
                                if (checkExistsInTable(temCuon.IdCuon, Convert.ToInt32(temCuon.Type))) return;
                                addPackageToList(temCuon.IdCuon, null, Convert.ToDouble(temCuon.SoLuong), temCuon.VnptPn, _LocationInfo.barcode, temCuon.Type);
                            }
                            url = "adjustment-details/search?query=adjustmentId==" + _Adjustment.id.ToString() + ";packageId==" + packageInfo.id.ToString();
                            _id = packageInfo.id;
                        }
                        if (url.Length == 0) { MessageBox.Show("Wrong Package/Unit ID QRCode format !"); return; }

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

                            if (checkExistsInTable(temCuon.IdCuon, Convert.ToInt32(temCuon.Type)))
                            {
                                return;
                            }
                            else
                            {
                                if (ListDetail.Count == 0)
                                {
                                    DialogResult mgb = new DialogResult();
                                    mgb = MessageBox.Show("Package/Unit ID is not exists in inventory adjustment! Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    if (mgb != DialogResult.Yes) return;

                                    addPackageFromStock(_id, temCuon.IdCuon, isPackage, temCuon.VnptPn, _LocationInfo.barcode, temCuon.Type, Convert.ToDouble(temCuon.SoLuong));
                                    //addPackageToList(temCuon.IdCuon, 0, temCuon.VnptPn, _LocationInfo.barcode, Convert.ToInt32(temCuon.Type));

                                    //enable button save
                                    if (_LocationInfo != null && !btnSave.Enabled)
                                    {
                                        btnSave.Enabled = true;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(temCuon.Type) == 0 || ListDetail.Count == 1)
                                    {
                                        addPackageToList(ListDetail[0]);
                                    }
                                    else if (Convert.ToInt32(temCuon.Type) == 1)
                                    {
                                        foreach (AdjustmentDetail item in ListDetail)
                                        {
                                            addPackageToList(item);
                                        }
                                    }

                                    //enable button save
                                    if (_LocationInfo != null && !btnSave.Enabled)
                                    {
                                        btnSave.Enabled = true;
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
                        MessageBox.Show("Wrong Package/Unit ID QRCode format !");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error scan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                #endregion
            }
            else if (Util.OnlyHexInString(dcdData.Trim()))
            {
                #region scan serial number

                string SerialNumber = dcdData.Trim();
                checkScan(null, SerialNumber, null);

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
                            if (_LocationInfo != null)
                            {
                                DialogResult mgb = new DialogResult();
                                mgb = MessageBox.Show("Are you sure you want to update new scan location?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                if (mgb != DialogResult.Yes) return;
                            }

                            _LocationInfo = getLocationInfo(locationBarcode);
                            if (_LocationInfo == null) return;

                            this.lblLocationBarcode.Text = _LocationInfo.name;

                            //enable button save
                            if (this.dtList.Rows.Count > 0 && !btnSave.Enabled)
                            {
                                btnSave.Enabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Location is not exists in adjustment name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            //btnSave.Enabled = false;
                            //this.dtList.Rows.Clear();
                            //this.dgCuonList.Refresh();
                            this.locationBarcode = null;
                            //this.lblLocationBarcode.Text = null;
                        }
                    }
                    else
                    {
                        //btnSave.Enabled = false;
                        //this.dtList.Rows.Clear();
                        //this.dgCuonList.Refresh();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during load location information !");
                }
                #endregion
            }
        }

        private bool checkScan(string _productName, string _packageId, string type)
        {
            if (_LocationInfo == null)
            {
                MessageBox.Show("Request scan location before scan the package!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }

            //Check package exists in inventory
            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "";
            int _id = 0; bool isPackage = true;

            if (Util.getTypePackage(_packageId, type) % 2 == 0)
            {
                isPackage = false;
                Lots lotInfo = LotBusiness.getInfo(_packageId);
                if (lotInfo == null)
                {
                    DialogResult mgb = new DialogResult();
                    mgb = MessageBox.Show("Uid is not exists in inventory! Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    //if (mgb != DialogResult.Yes) return false;
                    return false;
                }
                url = "adjustment-details/search?query=adjustmentId==" + _Adjustment.id.ToString() + ";lotId==" + lotInfo.id.ToString();
                _id = lotInfo.id;
            }
            else //if (Util.getTypePackage(_packageId, type) % 2 != 0)
            {
                isPackage = true;
                Packages packageInfo = PackageBusiness.getInfo(_packageId);
                if (packageInfo == null)
                {
                    DialogResult mgb = new DialogResult();
                    mgb = MessageBox.Show("Package is not exists in inventory! Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    //if (mgb != DialogResult.Yes) return false;
                    return false;
                }
                url = "adjustment-details/search?query=adjustmentId==" + _Adjustment.id.ToString() + ";packageId==" + packageInfo.id.ToString();
                _id = packageInfo.id;
            }
            if (url.Length == 0) { MessageBox.Show("Wrong Package/Unit ID QRCode format !"); return false; }

            try
            {
                res = HTTP.GetJson(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server error: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }
            List<AdjustmentDetail> ListDetail;
            if (res.Status && Util.IsJson(res.RawText))
            {
                List<AdjustmentDetail> RootObject = JsonConvert.DeserializeObject<List<AdjustmentDetail>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                ListDetail = RootObject as List<AdjustmentDetail>;

                if (checkExistsInTable(_packageId, Convert.ToInt32(type)))
                {
                    return false;
                }
                else
                {
                    if (ListDetail.Count == 0)
                    {
                        DialogResult mgb = new DialogResult();
                        mgb = MessageBox.Show("Package/UnitID is not exists in inventory adjustment! Do you want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        if (mgb != DialogResult.Yes) return false;

                        addPackageFromStock(_id, _packageId, isPackage, _productName, _LocationInfo.barcode, null, 1);

                        //enable button save
                        if (_LocationInfo != null && !btnSave.Enabled)
                        {
                            btnSave.Enabled = true;
                        }
                    }
                    else
                    {
                        if (Util.getTypePackage(_packageId, type) % 2 == 0 || ListDetail.Count == 1)
                        {
                            addPackageToList(ListDetail[0]);
                        }
                        else //Util.getTypePackage(_packageId, type) % 2 != 0
                        {
                            foreach (AdjustmentDetail item in ListDetail)
                            {
                                addPackageToList(item);
                            }
                        }

                        //enable button save
                        if (_LocationInfo != null && !btnSave.Enabled)
                        {
                            btnSave.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Server error !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            LocationInfo locationInfo = LocationBusiness.getInfo(_LocationInfo.id);
            if (locationInfo == null)
            {
                MessageBox.Show("Location is not exists in inventory!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            //_action = "save";
            string param = "";
            try
            {
                _Adjustment.adjustmentDetails = this._ListSpace;//Util.DataTableToList<AdjustmentDetail>(this.dtList);
                param = JsonConvert.SerializeObject(_Adjustment);
                param = param.Replace(System.Environment.NewLine, "").Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Client error: " + ex.ToString());
                return;
            }

            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "inventories/gen-details";
            //string url = "inventories/" + this.Adjustment.id.ToString();
            
            //Util.Logs(param);

            try
            {
                //res = HTTP.Put(url, param);
                res = HTTP.PostJson(url, param);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }
            
            if (res.Status)
            {
                try
                {
                    Adjustment RootObject = JsonConvert.DeserializeObject<Adjustment>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    Adjustment AdjustmentResponse = RootObject as Adjustment;
                    List<AdjustmentDetail> ListError = AdjustmentResponse.errorDetails;
                    foreach (AdjustmentDetail item in ListError)
                    {
                        if (item.id != null)
                        {
                            DataRow[] rs_package = dtList.Select("id" + " = " + item.id.ToString());
                            if (rs_package.Length > 0)
                            {
                                rs_package[0][COL_STATUS] = "error";
                            }
                        }
                        else
                        {
                            DataRow[] rs_package;
                            if (item.packageNumber != null && item.traceNumber != null)
                                rs_package = dtList.Select(COL_PACK + " = '" + item.packageNumber + "' AND " + COL_LOT + " = '" + item.traceNumber + "'");
                            else if (item.packageNumber != null)
                                rs_package = dtList.Select(COL_PACK + " = '" + item.packageNumber + "'");
                            else //if (item.traceNumber != null)
                                rs_package = dtList.Select(COL_LOT + " = '" + item.traceNumber + "'");
                            if (rs_package.Length > 0)
                            {
                                rs_package[0][COL_STATUS] = "error";
                            }
                        }
                    }

                    this.dtList.DefaultView.Sort = COL_STATUS + " DESC";
                    this.dtList = this.dtList.DefaultView.ToTable();

                    this.dgCuonList.DataSource = this.dtList;
                    this.dgCuonList.Refresh();
                }
                catch (Exception ex) { MessageBox.Show("View error: " + ex.Message.ToString()); }

                MessageBox.Show("Success !");
                btnSave.Enabled = false;
                //btnClear.Enabled = false;
                //this.dtList.Rows.Clear();
                //this.dgCuonList.Refresh();
                //this._ListSpace.Clear();
                this.locationBarcode = null;
                //this.lblLocationBarcode.Text = null;
            }
            else
            {
                //Util.Logs("res.Message: " + res.Message + " --- res.RawText: " + res.RawText);
            }
        }

        private void addPackageToList(string _packageId, string _uid, double _realQuantity, string _internalReference, string _barcode, string type)
        {
            try
            {
                DataRow dr = this.dtList.NewRow();
                dr[COL_PACK] = _packageId;
                dr[COL_LOT] = _uid;
                dr[COL_QUANT] = _realQuantity;
                dr[COL_DEL] = "Delete";
                dr[COL_PRODUCT_NAME] = _internalReference;
                dr[COL_BARCODE] = _barcode;
                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;

                AdjustmentDetail newScan = new AdjustmentDetail();
                newScan.barcode = _barcode;
                newScan.internalReference = _internalReference;
                newScan.traceNumber = _uid;
                newScan.packageNumber = _packageId;
                newScan.realQuantity = _realQuantity;
                newScan.id = null;
                newScan.adjustmentId = _Adjustment.id;
                newScan.productId = null;
                newScan.manId = null;
                newScan.locationId = _LocationInfo.id;
                newScan.locationName = _LocationInfo.name;
                newScan.type = "manually";
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
            Lots lotInfo = null; 
            Packages packInfo = null;
            
            try
            {
                if (_adjustmentDetail.lotId != null)
                {
                    Lots lot = new Lots();
                    lotInfo = lot.getInfo(Convert.ToInt32(_adjustmentDetail.lotId));
                }
            }
            catch (Exception ex) { }
            try
            {
                if (_adjustmentDetail.packageId != null)
                {
                    Packages pack = new Packages();
                    packInfo = pack.getInfo(Convert.ToInt32(_adjustmentDetail.packageId));
                    //Util.Logs(packInfo.packageNumber);
                }
            }
            catch (Exception ex) {  }

            try
            {
                if (_adjustmentDetail.type != "manually") _adjustmentDetail.realQuantity = _adjustmentDetail.theoreticalQuantity;

                DataRow dr = this.dtList.NewRow();
                if (lotInfo != null) { dr[COL_LOT] = lotInfo.lotNumber; _adjustmentDetail.traceNumber = lotInfo.lotNumber; } 
                else dr[COL_LOT] = "";
                if (packInfo != null) { dr[COL_PACK] = packInfo.packageNumber; _adjustmentDetail.packageNumber = packInfo.packageNumber; } 
                else dr[COL_PACK] = "";
                dr[COL_QUANT] = _adjustmentDetail.realQuantity;
                dr[COL_DEL] = "Delete";
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
                dr["locationId"] = _LocationInfo.id;
                dr["locationName"] = _LocationInfo.name;
                dr["theoreticalQuantity"] = _adjustmentDetail.theoreticalQuantity;
                dr["state"] = _adjustmentDetail.state;
                dr["manPn"] = _adjustmentDetail.manPn;
                dr["productDescription"] = _adjustmentDetail.productDescription;
                dr["lotId"] = _adjustmentDetail.lotId;
                dr["packageId"] = _adjustmentDetail.packageId;
                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;

                _adjustmentDetail.locationId = _LocationInfo.id;
                _adjustmentDetail.locationName = _LocationInfo.name;
                _adjustmentDetail.type = "manually";
                this._ListSpace.Add(_adjustmentDetail);
                this.dgCuonList.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
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
                int index_column_status = 0;
                foreach (DataColumn item in dtList.Columns)
                {
                    if (item.ColumnName == COL_QUANT) break;
                    index_column_quant++;
                }
                foreach (DataColumn item in dtList.Columns)
                {
                    if (item.ColumnName == COL_DEL) break;
                    index_column_del++;
                }
                foreach (DataColumn item in dtList.Columns)
                {
                    if (item.ColumnName == COL_STATUS) break;
                    index_column_status++;
                }

                DataGridCell row = dgCuonList.CurrentCell;
                //MessageBox.Show(row.ColumnNumber.ToString());
                if (row.ColumnNumber == index_column_del)
                {
                    dtList.Rows[row.RowNumber].Delete();
                    this._ListSpace.RemoveAt(row.RowNumber);
                    dgCuonList.Refresh();
                }
                else if (row.ColumnNumber == index_column_quant)
                {
                    TextEditable textEditable = new TextEditable();
                    textEditable.valueEdit = Convert.ToString(dtList.Rows[row.RowNumber][COL_QUANT]);
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
                else if (row.ColumnNumber == index_column_status)
                {
                    if (dtList.Rows[row.RowNumber][COL_STATUS].ToString().Length > 0)
                        MessageBox.Show("Package/Uid duplicate !");
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