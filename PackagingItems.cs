using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Datalogic.API;
using ERP.Base.ErpObjects;
using Newtonsoft.Json;

namespace ERP
{
    public partial class PackagingItems : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;

        private const string COL_DEL = "delete";
        private const string COL_LOT_ID = "lot_number";
        private const string COL_PACK_ID = "pack_number";
        //private const string COL_PRODUCT_NAME = "product_name";
        private const string PREFIX_LOT = "UID";
        private const string PREFIX_PACK = "PACK";

        private DataTable dtList = new DataTable();
        private string DestPackageNumber;

        public PackagingItems()
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

        private void PackagingItems_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void InitData()
        {
            try
            {
                dtList.Rows.Clear();

                dtList.Columns.Add(COL_PACK_ID);
                dtList.Columns.Add(COL_LOT_ID);
                dtList.Columns.Add(COL_DEL);

                dgCuonList.DataSource = dtList;
                dgCuonList.TableStyles.Clear();

                DataGridTableStyle tableStyle = new DataGridTableStyle();
                tableStyle.MappingName = dtList.TableName;

                foreach (DataColumn item in dtList.Columns)
                {
                    DataGridTextBoxColumn tbcName = new DataGridTextBoxColumn();

                    switch (item.ColumnName)
                    {
                        case COL_PACK_ID:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Package";
                                tbcName.Width = 80;
                            } break;
                        case COL_LOT_ID:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Unit ID";
                                tbcName.Width = 80;
                            } break;
                        case COL_DEL:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Delete";
                                tbcName.Width = 45;
                            } break;
                        default:
                            {
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

        private void PackagingItems_Closing(object sender, CancelEventArgs e)
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

        private void dgCuonList_CurrentCellChanged(object sender, EventArgs e)
        {

        }

        private void dgCuonList_Click(object sender, EventArgs e)
        {
            try
            {
                //dgCuonList.Select(dgCuonList.CurrentRowIndex);
                DataGridCell row = dgCuonList.CurrentCell;
                if (row.ColumnNumber == 2)
                {
                    dtList.Rows[row.RowNumber].Delete();
                    dgCuonList.Refresh();
                }
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

        private bool checkExistsPackage(string packageNumber)
        {
            Packages package = PackageBusiness.getInfo(packageNumber);
            if (package == null)
            {
                MessageBox.Show("Package " + packageNumber + " is not exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }
            else
            {
                Packages packageFull = PackageBusiness.getFullInfo(package.id);
                if (packageFull == null || packageFull.quants == null)
                {
                    MessageBox.Show("Package " + packageNumber + " is not exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return false;
                }
            }
            return true;
        }

        private void ScanCode(string dcdData)
        {
            //Obtain the string and code id.
            //MessageBox.Show(dcdData);
            if (dcdData.StartsWith("["))
            {
                #region New label
                LabelPackage labelPackage = null;
                try
                {
                    labelPackage = new LabelPackage(dcdData.Trim());
                }
                catch
                {
                    MessageBox.Show("Wrong Package/UnitID QRCode format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); 
                    return;
                }

                if (labelPackage.PackageId.StartsWith(PREFIX_LOT))
                {
                    #region Scan uid
                    try
                    {
                        if (labelPackage.PackageId != null && labelPackage.PackageId != "")
                        {
                            if (checkReservedInOtherList(labelPackage.PackageId, false))
                            {
                                MessageBox.Show("Package/Uid already exists in another reserved list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }

                            DataRow[] rs_package = dtList.Select(COL_LOT_ID + " = '" + labelPackage.PackageId + "'");
                            if (rs_package.Length > 0)
                            {
                                //DialogResult mgb = new DialogResult();
                                //mgb = MessageBox.Show("Package not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                //if (mgb == DialogResult.Yes)
                                //{
                                //    dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_ID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                                //    if(dr == null)
                                //        addPackageNotInList(labelPackage.ProductName, labelPackage.PackageId);
                                //}
                                
                                MessageBox.Show("Unit ID " + labelPackage.PackageId + " already exists in the list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }
                            else
                            {
                                //Validate Unit ID
                                Lots lot = new Lots();
                                Lots lotInfo = lot.getInfo(labelPackage.PackageId);
                                if (lotInfo == null)
                                {
                                    MessageBox.Show("Unit ID " + labelPackage.PackageId + " is not exists in inventory !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    return;
                                }

                                Packages PackageInfo = validateLot(lotInfo.id);
                                if (PackageInfo == null)
                                {
                                    return;
                                }
                                
                                if (PackageInfo.packageNumber == null)
                                {
                                    if (this.dtList.Rows.Count == 0)
                                    {
                                        addPackageToList("", labelPackage.PackageId);
                                    }
                                    else
                                    {
                                        DataRow[] rs = dtList.Select(COL_PACK_ID + " = ''");
                                        if (rs.Length > 0)
                                        {
                                            addPackageToList("", labelPackage.PackageId);
                                        }
                                        else
                                        {
                                            if (Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]).Length > 0)
                                                MessageBox.Show("Request scan unit ID in package " + Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]));
                                            else
                                                MessageBox.Show("Request scan free unit ID !");
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    if (this.dtList.Rows.Count == 0)
                                    {
                                        addPackageToList(PackageInfo.packageNumber, labelPackage.PackageId);
                                    }
                                    else
                                    {
                                        DataRow[] rs = dtList.Select(COL_PACK_ID + " = '" + PackageInfo.packageNumber + "'");
                                        if (rs.Length > 0)
                                        {
                                            addPackageToList(PackageInfo.packageNumber, labelPackage.PackageId);
                                        }
                                        else
                                        {
                                            if (Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]).Length > 0)
                                                MessageBox.Show("Request scan unit ID in package " + Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]));
                                            else
                                                MessageBox.Show("Request scan free unit ID !");
                                            return;
                                        }
                                    }
                                }

                                if (DestPackageNumber != null && DestPackageNumber.Trim().Length > 0 && !btnSave.Enabled)
                                {
                                    //enable button save
                                    btnSave.Enabled = true;
                                    btnClear.Enabled = true;
                                }
                            }
                        }
                        else
                            MessageBox.Show("Error scan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.InnerException.ToString());
                        //MessageBox.Show("Error during loading information !", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    #endregion
                }
                else if (labelPackage.PackageId.StartsWith(PREFIX_PACK))
                {
                    #region Scan package
                    if (DestPackageNumber != null && DestPackageNumber.Length > 0)
                    {
                        DialogResult mgb = new DialogResult();
                        mgb = MessageBox.Show("Are you sure you want to update the package number?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        if (mgb != DialogResult.Yes) return;
                    }

                    if (!checkExistsPackage(labelPackage.PackageId)) return;

                    if (checkReservedInOtherList(labelPackage.PackageId, true))
                    {
                        MessageBox.Show("Package/Uid already exists in another reserved list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DestPackageNumber = labelPackage.PackageId;
                    txtDestPackageNumber.Text = DestPackageNumber;

                    if (this.dtList.Rows.Count > 0 && !btnSave.Enabled)
                    {
                        //enable button save
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("Wrong Package/UnitID QRCode format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }
                #endregion
            }
            else if (dcdData.Trim().StartsWith("PN:"))
            {
                #region Old label
                TemCuon labelPackage = null;
                try
                {
                    labelPackage = new TemCuon(dcdData.Trim());
                }
                catch { MessageBox.Show("Wrong Package/UnitID QRCode format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return; }

                if (Convert.ToInt32(labelPackage.Type) == 0)
                {
                    #region Scan Unit ID
                    try
                    {
                        if (labelPackage.IdCuon != null && labelPackage.IdCuon != "")
                        {
                            if (checkReservedInOtherList(labelPackage.IdCuon, false))
                            {
                                MessageBox.Show("Package/Uid already exists in another reserved list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }

                            DataRow[] rs_package = dtList.Select(COL_LOT_ID + " = '" + labelPackage.IdCuon + "'");
                            if (rs_package.Length > 0)
                            {
                                MessageBox.Show("Unit ID " + labelPackage.IdCuon + " already exists in the list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }
                            else
                            {
                                //Validate Unit ID
                                Lots lot = new Lots();
                                Lots lotInfo = lot.getInfo(labelPackage.IdCuon);
                                if (lotInfo == null)
                                {
                                    MessageBox.Show("Unit ID " + labelPackage.IdCuon + " is not exists in inventory !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    return;
                                }

                                Packages PackageInfo = validateLot(lotInfo.id);
                                if (PackageInfo == null)
                                {
                                    return;
                                }

                                if (PackageInfo.packageNumber == null)
                                {
                                    if (this.dtList.Rows.Count == 0)
                                    {
                                        addPackageToList("", labelPackage.IdCuon);
                                    }
                                    else
                                    {
                                        DataRow[] rs = dtList.Select(COL_PACK_ID + " = ''");
                                        if (rs.Length > 0)
                                        {
                                            addPackageToList("", labelPackage.IdCuon);
                                        }
                                        else
                                        {
                                            if (Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]).Length > 0)
                                                MessageBox.Show("Request scan unit ID in package " + Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]));
                                            else
                                                MessageBox.Show("Request scan free unit ID !");
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    if (this.dtList.Rows.Count == 0)
                                    {
                                        addPackageToList(PackageInfo.packageNumber, labelPackage.IdCuon);
                                    }
                                    else
                                    {
                                        DataRow[] rs = dtList.Select(COL_PACK_ID + " = '" + PackageInfo.packageNumber + "'");
                                        if (rs.Length > 0)
                                        {
                                            addPackageToList(PackageInfo.packageNumber, labelPackage.IdCuon);
                                        }
                                        else
                                        {
                                            if (Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]).Length > 0)
                                                MessageBox.Show("Request scan unit ID in package " + Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]));
                                            else
                                                MessageBox.Show("Request scan free unit ID !");
                                            return;
                                        }
                                    }
                                }

                                if (DestPackageNumber != null && DestPackageNumber.Trim().Length > 0 && !btnSave.Enabled)
                                {
                                    //enable button save
                                    btnSave.Enabled = true;
                                    btnClear.Enabled = true;
                                }
                            }
                        }
                        else
                            MessageBox.Show("Error scan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.InnerException.ToString());
                        //MessageBox.Show("Error during loading information !", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                    #endregion
                }
                else if (Convert.ToInt32(labelPackage.Type) == 1)
                {
                    #region Scan package
                    if (DestPackageNumber != null && DestPackageNumber.Length > 0)
                    {
                        DialogResult mgb = new DialogResult();
                        mgb = MessageBox.Show("Are you sure you want to update the package number?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        if (mgb != DialogResult.Yes) return;
                    }

                    if (!checkExistsPackage(labelPackage.IdCuon)) return;

                    if (checkReservedInOtherList(labelPackage.IdCuon, true))
                    {
                        MessageBox.Show("Package/Uid already exists in another reserved list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    DestPackageNumber = labelPackage.IdCuon;
                    txtDestPackageNumber.Text = DestPackageNumber;

                    if (this.dtList.Rows.Count > 0 && !btnSave.Enabled)
                    {
                        //enable button save
                        btnSave.Enabled = true;
                        btnClear.Enabled = true;
                    }
                    #endregion
                }
                else
                {
                    MessageBox.Show("Wrong Package/UnitID QRCode format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return;
                }
                #endregion
            }
            else if (Util.OnlyHexInString(dcdData.Trim()))
            {
                #region Serial number

                string SerialNumber = dcdData.Trim();
                checkScan(SerialNumber, null);

                #endregion
            }
            else { MessageBox.Show("Wrong Package/UnitID QRCode format!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return; }
        }

        private bool checkScan(string _packageId, string type)
        {
            if (Util.getTypePackage(_packageId, type) % 2 == 0)
            {
                #region Scan uid
                try
                {
                    if (_packageId != null && _packageId != "")
                    {
                        if (checkReservedInOtherList(_packageId, false))
                        {
                            MessageBox.Show("Package/Uid already exists in another reserved list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return false;
                        }

                        DataRow[] rs_package = dtList.Select(COL_LOT_ID + " = '" + _packageId + "'");
                        if (rs_package.Length > 0)
                        {
                            //DialogResult mgb = new DialogResult();
                            //mgb = MessageBox.Show("Package not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            //if (mgb == DialogResult.Yes)
                            //{
                            //    dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_ID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                            //    if(dr == null)
                            //        addPackageNotInList(labelPackage.ProductName, labelPackage.PackageId);
                            //}

                            MessageBox.Show("Unit ID " + _packageId + " already exists in the list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return false;
                        }
                        else
                        {
                            //Validate Unit ID
                            Lots lot = new Lots();
                            Lots lotInfo = lot.getInfo(_packageId);
                            if (lotInfo == null)
                            {
                                MessageBox.Show("Unit ID " + _packageId + " is not exists in inventory !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return false;
                            }

                            Packages PackageInfo = validateLot(lotInfo.id);
                            if (PackageInfo == null)
                            {
                                return false;
                            }

                            if (PackageInfo.packageNumber == null)
                            {
                                if (this.dtList.Rows.Count == 0)
                                {
                                    addPackageToList("", _packageId);
                                }
                                else
                                {
                                    DataRow[] rs = dtList.Select(COL_PACK_ID + " = ''");
                                    if (rs.Length > 0)
                                    {
                                        addPackageToList("", _packageId);
                                    }
                                    else
                                    {
                                        if (Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]).Length > 0)
                                            MessageBox.Show("Request scan unit ID in package " + Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]));
                                        else
                                            MessageBox.Show("Request scan free unit ID !");
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                if (this.dtList.Rows.Count == 0)
                                {
                                    addPackageToList(PackageInfo.packageNumber, _packageId);
                                }
                                else
                                {
                                    DataRow[] rs = dtList.Select(COL_PACK_ID + " = '" + PackageInfo.packageNumber + "'");
                                    if (rs.Length > 0)
                                    {
                                        addPackageToList(PackageInfo.packageNumber, _packageId);
                                    }
                                    else
                                    {
                                        if (Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]).Length > 0)
                                            MessageBox.Show("Request scan unit ID in package " + Convert.ToString(this.dtList.Rows[0][COL_PACK_ID]));
                                        else
                                            MessageBox.Show("Request scan free unit ID !");
                                        return false;
                                    }
                                }
                            }

                            if (DestPackageNumber != null && DestPackageNumber.Trim().Length > 0 && !btnSave.Enabled)
                            {
                                //enable button save
                                btnSave.Enabled = true;
                                btnClear.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error scan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.InnerException.ToString());
                    //MessageBox.Show("Error during loading information !", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return false;
                }
                #endregion
            }
            else //if (Util.getTypePackage(_packageId, type) % 2 != 0)
            {
                #region Scan package
                if (DestPackageNumber != null && DestPackageNumber.Length > 0)
                {
                    DialogResult mgb = new DialogResult();
                    mgb = MessageBox.Show("Are you sure you want to update the package number?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    if (mgb != DialogResult.Yes) return false;
                }

                if (!checkExistsPackage(_packageId)) return false;

                if (checkReservedInOtherList(_packageId, true))
                {
                    MessageBox.Show("Package/Uid already exists in another reserved list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return false;
                }

                DestPackageNumber = _packageId;
                txtDestPackageNumber.Text = DestPackageNumber;

                if (this.dtList.Rows.Count > 0 && !btnSave.Enabled)
                {
                    //enable button save
                    btnSave.Enabled = true;
                    btnClear.Enabled = true;
                }
                #endregion
            }
            return true;
        }

        private bool checkReservedInOtherList(string _packageId, bool _isPackage)
        {
            string url = "";
            if (_isPackage)
                url = "transfer-details/search?query=srcPackageNumber=='" + _packageId + "';state!='done'";
            else
                url = "transfer-details/search?query=traceNumber=='" + _packageId + "';state!='done'";
            ApiResponse res = new ApiResponse();
            res.Status = false;
            List<TransferDetail> ListTransferDetail = new List<TransferDetail>();
            try
            {
                res = HTTP.GetJson(url);

                if (res.Status && Util.IsJson(res.RawText))
                {
                    List<TransferDetail> RootObject = JsonConvert.DeserializeObject<List<TransferDetail>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    ListTransferDetail = RootObject as List<TransferDetail>;
                    if (ListTransferDetail.Count == 0) return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private Packages validateLot(int lotId)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            
            try
            {
                string url = "quants/search?query=lotId==" + lotId.ToString() + ";onHand>0&sort=created,desc";
                res = HTTP.GetJson(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Not exists unit ID number in inventory !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return null;
            }

            if (res.Status && Util.IsJson(res.RawText))
            {
                try
                {
                    List<StockQuants> RootObject = JsonConvert.DeserializeObject<List<StockQuants>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    List<StockQuants> ListStock = RootObject as List<StockQuants>;

                    if (ListStock.Count == 0)
                    {
                        MessageBox.Show("Not exists unit ID number in inventory !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return null;
                    }
                    else
                    {
                        if (ListStock[0].packageId == null)
                        {
                            return new Packages();
                        }
                        else
                        {
                            Packages package = new Packages();
                            Packages packageInfo = package.getInfo(Convert.ToInt32(ListStock[0].packageId));
                            if (packageInfo == null) return null; else return packageInfo;
                        }
                    }
                }
                catch (Exception ex) { return null; }
            }
            else
            {
                return null;
            }
        }

        private void addPackageToList(string srcPackageNumber, string lotNumber)
        {
            try
            {
                DataRow dr = this.dtList.NewRow();
                dr[COL_PACK_ID] = srcPackageNumber;
                dr[COL_LOT_ID] = lotNumber;
                dr[COL_DEL] = "X";
                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;
                this.dgCuonList.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.dtList.Rows.Count > 0)
            {
                ApiResponse res = new ApiResponse();
                res.Status = false;

                try
                {
                    string url = "smart-devices/move-packages";

                    string param = "{ \"destPackageNumber\": \""+ DestPackageNumber.Trim() +"\", \"lotNumbers\": [";
                    foreach (DataRow row in dtList.Rows)
                    {
                        param += "\"" + row[COL_LOT_ID] + "\",";
                    }
                    param = param.Replace(System.Environment.NewLine, "").Trim();
                    param = param.Substring(0, param.Length - 1);

                    //string end_char = param.Substring(param.Length - 2, 1);
                    //while (end_char != ",")
                    //{
                    //    param = param.Substring(0, param.Length - 2);
                    //    end_char = param.Substring(param.Length - 2, 1);
                    //}
                    //if (end_char == ",") param = param.Substring(0, param.Length - 2);

                    if (dtList.Rows[0][COL_PACK_ID] != null && dtList.Rows[0][COL_PACK_ID].ToString().Trim().Length > 0)
                        param += "], \"srcPackageNumber\": \"" + dtList.Rows[0][COL_PACK_ID] + "\" }";
                    else
                        param += "], \"srcPackageNumber\": null }";

                    param = param.Replace(System.Environment.NewLine, "").Trim();

                    res = HTTP.PostJson(url, param);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    //MessageBox.Show(ex.InnerException.ToString());
                }

                if (res.Status)
                {
                    try
                    {
                        PackagingResponse RootObject = JsonConvert.DeserializeObject<PackagingResponse>(res.RawText, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                        PackagingResponse Response = RootObject as PackagingResponse;
                        if (Response.invalidLotNumbers.Count > 0)
                        {
                            string message = "Error occurred with unit ID: ";
                            foreach (string item in Response.invalidLotNumbers)
                            {
                                message += item + ",";
                            }
                            string end_char = message.Substring(message.Length - 1, 1);
                            if (end_char == ",") message = message.Substring(0, message.Length - 1);
                            MessageBox.Show(message);
                        }
                        else
                        {
                            MessageBox.Show("Move unit ID to package successfully!");
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); }                    
                }
                else
                {
                    //MessageBox.Show(res.RawText);
                }

                //btnSave.Enabled = false;
                //btnClear.Enabled = true;

            }
            else
            {
                MessageBox.Show("Request scanned package before saving !");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            dtList.Rows.Clear();
            btnSave.Enabled = false;
        }
    }

    public class PackagingResponse
    {
        public List<string> invalidLotNumbers { get; set; }
    }
}