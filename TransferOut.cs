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
using System.Threading;

namespace ERP
{
    public partial class TransferOut : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;

        #region Properties
        private const string COL_SCANNED = "scanned";
        private const string COL_SCANNED_BAK = "scanned_bak";
        private const string COL_PACKID = "pack_id";
        private const string COL_ID = "id";
        private const string COL_LOCATION = "destLocationName";

        private const string PREFIX_LOT = "UID";
        private const string PREFIX_PACK = "PACK";

        private int _sl_thung_quet = 0;
        private int _sl_cuon_quet = 0;

        private int _transferId = 0;
        private TransferInfo TransferInfo;

        private DataTable dtList = new DataTable();
        private List<TransferItem> ListTI = new List<TransferItem>();
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
            LoadData(_transferId);

            //TEST
            //ScanCode("HQV/IN/00006-34");
        }

        private bool LoadData(int _transferId)
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
                MessageBox.Show("Get transfer details error: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
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

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Parse transfer details error: " + ex.Message.ToString());
                    return false;
                }

                try
                {
                    if (!dtList.Columns.Contains(COL_PACKID)) dtList.Columns.Add(COL_PACKID);
                    if (!dtList.Columns.Contains(COL_SCANNED)) dtList.Columns.Add(COL_SCANNED);
                    if (!dtList.Columns.Contains(COL_SCANNED_BAK)) dtList.Columns.Add(COL_SCANNED_BAK);
                    if (!dtList.Columns.Contains("level")) dtList.Columns.Add("level");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Add column table error: " + ex.Message.ToString());
                    return false;
                }

                try
                {
                    foreach (DataRow row in dtList.Rows)
                    {
                        if (Convert.ToDouble(row["doneQuantity"]) > 0)
                        {
                            row[COL_SCANNED] = "OK";
                            try
                            {
                                if (row["traceNumber"] != null && Convert.ToString(row["traceNumber"]).Length > 0)
                                    row[COL_PACKID] = row["traceNumber"];
                                else
                                    row[COL_PACKID] = row["destPackageNumber"];
                            }
                            catch { row[COL_PACKID] = row["destPackageNumber"]; }
                        }
                        else
                        {
                            row[COL_SCANNED] = "";
                            try
                            {
                                if (row["traceNumber"] != null && Convert.ToString(row["traceNumber"]).Length > 0)
                                    row[COL_PACKID] = row["traceNumber"];
                                else
                                    row[COL_PACKID] = row["srcPackageNumber"];
                            }
                            catch { row[COL_PACKID] = row["srcPackageNumber"]; }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Init table error: " + ex.Message.ToString());
                    return false;
                }

                try
                {
                    DataGridInitUpdater dataGridInitUpdater = DataGridInitValue;
                    this.dgCuonList.Invoke(dataGridInitUpdater, dtList);
                    
                    //dgCuonList.DataSource = dtList;
                    //dgCuonList.TableStyles.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Set value table error: " + ex.Message.ToString());
                    return false;
                }

                DataGridTableStyle tableStyle = new DataGridTableStyle();
                try
                {
                    tableStyle.MappingName = dtList.TableName;
                    foreach (DataColumn item in dtList.Columns)
                    {
                        DataGridTextBoxColumn tbcName = new DataGridTextBoxColumn();

                        switch (item.ColumnName)
                        {
                            case COL_LOCATION:
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
                            case COL_PACKID:
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Init style table error: " + ex.Message.ToString());
                    return false;
                }

                try
                {
                    //dgCuonList.TableStyles.Add(tableStyle);
                    //dgCuonList.Refresh();

                    DataGridAddStyleUpdater dataGridAddStyleUpdater = DataGridAddStyle;
                    this.dgCuonList.Invoke(dataGridAddStyleUpdater, tableStyle);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Set table style error: " + ex.Message.ToString());
                    return false;
                }

                try
                {
                    //btnReset.Enabled = true;
                    ControlEnableUpdater controlEnableUpdater = ControlEnable;
                    this.btnReset.Invoke(controlEnableUpdater, btnReset, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Enable button reset error: " + ex.Message.ToString());
                    return false;
                }
                
            }
            else
            {
                MessageBox.Show(res.RawText);
                return false;
            }
            return true;
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

            if (loadingThread != null) loadingThread.Abort();
            if (loadTransferThread != null) loadTransferThread.Abort();
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
                    #region scan package/uid new
                    if (this.TransferInfo == null)
                    {
                        MessageBox.Show("Request to enter the transfer name before scanning!");
                        return;
                    }

                    LabelPackage labelPackage = new LabelPackage(dcdData.Trim());
                    DataRow[] rs_product = null; DataRow[] rs_package = null;

                    bool _exists_product = false;
                    bool _exists_package = false;
                    bool _exists_many = false;

                    if (labelPackage.ProductName != null && labelPackage.ProductName != "")
                    {
                        rs_product = dtList.Select("internalReference = '" + labelPackage.ProductName + "'");
                        if (rs_product.Length > 0)
                        {
                            _exists_product = true;
                        }

                        rs_package = dtList.Select(COL_PACKID + " = '" + labelPackage.PackageId + "'");
                        if (rs_package.Length > 0)
                        {
                            _exists_package = true;
                        }
                        else
                        {
                            rs_package = dtList.Select("destPackageNumber" + " = '" + labelPackage.PackageId + "'");
                            if (rs_package.Length > 0)
                            {
                                _exists_package = true;
                                _exists_many = true;
                            }
                        }

                        DialogResult mgb = new DialogResult();
                        if (!_exists_product)
                        {
                            mgb = MessageBox.Show("Product not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb == DialogResult.Yes)
                            {
                                DataRow dr = null;
                                dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_PACKID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                                if (dr == null)
                                    addPackageNotInList(labelPackage.ProductName, labelPackage.PackageId);
                            }
                        }
                        else if (!_exists_package)
                        {
                            //mgb = MessageBox.Show("Package not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            //if (mgb == DialogResult.Yes)
                            //{
                            //    dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_PACKID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                            //    if(dr == null)
                                    addPackageNotInList(labelPackage.ProductName, labelPackage.PackageId);
                                    //enable button save
                                    btnSave.Enabled = true;
                            //}
                        }
                        else
                        {
                            if (!_exists_many)
                            {
                                DataRow dr = rs_package[0];
                                if (Convert.ToInt32(dr[COL_ID]) > 0)
                                {
                                    if (dr[COL_SCANNED] == "OK")
                                    {
                                        MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                        return;
                                    }
                                    if (dr[COL_SCANNED] == "X")
                                        MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    else checkOverQuantity(dr);

                                    dr["doneQuantity"] = dr["reserved"];
                                    dr[COL_SCANNED] = "X";
                                    dr[COL_SCANNED_BAK] = "X";
                                    dr["state"] = "available";
                                }
                                else
                                {
                                    if (dr[COL_SCANNED] == "N")
                                        MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                                    dr[COL_SCANNED] = "N";
                                    dr[COL_SCANNED_BAK] = "N";
                                }

                                int index = dtList.Rows.IndexOf(dr);
                                this.dgCuonList.CurrentRowIndex = index;
                            }
                            else
                            {
                                bool exported = true;
                                foreach (DataRow dr in rs_package)
                                {
                                    if (dr[COL_SCANNED] != "OK")
                                    {
                                        exported = false;
                                        break;
                                    }
                                }
                                if (exported)
                                {
                                    DialogResult mgb_export = new DialogResult();
                                    mgb_export = MessageBox.Show("Package " + Convert.ToString(rs_package[0]["destPackageNumber"]) + " was scanned. Are you sure want to scan again?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    if (mgb_export != DialogResult.Yes) return;
                                }

                                if (rs_package[0][COL_SCANNED] == "X" || rs_package[0][COL_SCANNED] == "N" || rs_package[0][COL_SCANNED] == "OK")
                                {
                                    MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                }

                                bool alertQuantity = false;
                                foreach (DataRow dr in rs_package)
                                {
                                    if (!alertQuantity && Convert.ToString(dr[COL_SCANNED]).Length == 0) alertQuantity = checkOverQuantity(dr);

                                    if (dr[COL_SCANNED] != "OK")
                                    {
                                        dr["doneQuantity"] = dr["reserved"];
                                        dr[COL_SCANNED] = "X";
                                        dr[COL_SCANNED_BAK] = "X";
                                        dr["state"] = "available";
                                    }
                                }
                                int index = dtList.Rows.IndexOf(rs_package[0]);
                                this.dgCuonList.CurrentRowIndex = index;
                            }
                        }

                        //enable button save
                        btnSave.Enabled = true;
                    }
                    else
                        MessageBox.Show("Error scan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    #endregion
                }
                else if (dcdData.Trim().StartsWith("PN:"))
                {
                    #region scan package/uid old
                    if (this.TransferInfo == null)
                    {
                        MessageBox.Show("Request to enter the transfer name before scanning!");
                        return;
                    }

                    TemCuon temCuon = new TemCuon(dcdData.Trim());
                    DataRow[] rs_product = null; DataRow[] rs_package = null;

                    bool _exists_product = false;
                    bool _exists_package = false;
                    bool _exists_many = false;

                    if (temCuon.VnptPn != null && temCuon.VnptPn != "")
                    {
                        rs_product = dtList.Select("internalReference = '" + temCuon.VnptPn + "'");
                        if (rs_product.Length > 0)
                        {
                            _exists_product = true;
                        }

                        rs_package = dtList.Select(COL_PACKID + " = '" + temCuon.IdCuon + "'");
                        if (rs_package.Length > 0)
                        {
                            _exists_package = true;
                        }
                        else
                        {
                            rs_package = dtList.Select("destPackageNumber" + " = '" + temCuon.IdCuon + "'");
                            if (rs_package.Length > 0)
                            {
                                _exists_package = true;
                                _exists_many = true;
                            }
                        }

                        DialogResult mgb = new DialogResult();
                        if (!_exists_product)
                        {
                            mgb = MessageBox.Show("Product not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb == DialogResult.Yes)
                            {
                                DataRow dr = null;
                                dr = dtList.Select("internalReference = '" + temCuon.VnptPn + "' AND " + COL_PACKID + " = '" + temCuon.IdCuon + "'").FirstOrDefault();
                                if (dr == null)
                                    addPackageOldNotInList(temCuon.VnptPn, temCuon.IdCuon, Convert.ToInt32(temCuon.Type));
                            }
                        }
                        else if (!_exists_package)
                        {
                            //mgb = MessageBox.Show("Package not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            //if (mgb == DialogResult.Yes)
                            //{
                            //    dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_PACKID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                            //    if(dr == null)
                                addPackageOldNotInList(temCuon.VnptPn, temCuon.IdCuon, Convert.ToInt32(temCuon.Type));
                                //enable button save
                                btnSave.Enabled = true;
                            //}
                        }
                        else
                        {
                            if (!_exists_many)
                            {
                                DataRow dr = rs_package[0];
                                if (Convert.ToInt32(dr[COL_ID]) > 0)
                                {
                                    if (dr[COL_SCANNED] == "OK")
                                    {
                                        MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                        return;
                                    }

                                    if (dr[COL_SCANNED] == "X")
                                        MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    else checkOverQuantity(dr);

                                    dr["doneQuantity"] = dr["reserved"];
                                    dr[COL_SCANNED] = "X";
                                    dr[COL_SCANNED_BAK] = "X";
                                    dr["state"] = "available";
                                }
                                else
                                {
                                    if (dr[COL_SCANNED] == "N")
                                        MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                                    dr[COL_SCANNED] = "N";
                                    dr[COL_SCANNED_BAK] = "N";
                                }

                                int index = dtList.Rows.IndexOf(dr);
                                this.dgCuonList.CurrentRowIndex = index;
                            }
                            else
                            {
                                bool exported = true;
                                foreach (DataRow dr in rs_package)
                                {
                                    if (dr[COL_SCANNED] != "OK")
                                    {
                                        exported = false;
                                        break;
                                    }
                                }
                                if (exported)
                                {
                                    DialogResult mgb_export = new DialogResult();
                                    mgb_export = MessageBox.Show("Package " + Convert.ToString(rs_package[0]["destPackageNumber"]) + " was scanned. Are you sure want to scan again?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    if (mgb_export != DialogResult.Yes) return;
                                }

                                if (rs_package[0][COL_SCANNED] == "X" || rs_package[0][COL_SCANNED] == "N" || rs_package[0][COL_SCANNED] == "OK")
                                    MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                                bool alertQuantity = false;
                                foreach (DataRow dr in rs_package)
                                {
                                    if (!alertQuantity && Convert.ToString(dr[COL_SCANNED]).Length == 0) alertQuantity = checkOverQuantity(dr);

                                    if (dr[COL_SCANNED] != "OK")
                                    {
                                        dr["doneQuantity"] = dr["reserved"];
                                        dr[COL_SCANNED] = "X";
                                        dr[COL_SCANNED_BAK] = "X";
                                        dr["state"] = "available";
                                    }
                                }
                                int index = dtList.Rows.IndexOf(rs_package[0]);
                                this.dgCuonList.CurrentRowIndex = index;
                            }
                        }

                        //enable button save
                        btnSave.Enabled = true;
                    }
                    else
                        MessageBox.Show("Error scan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    #endregion
                }
                else if (Util.OnlyHexInString(dcdData.Trim()))
                {
                    #region scan serial number
                    if (this.TransferInfo == null)
                    {
                        MessageBox.Show("Request to enter the transfer name before scanning!");
                        return;
                    }

                    string SerialNumber = dcdData.Trim();
                    DataRow[] rs_package = null;
                    bool _exists_package = false;
                    bool _exists_many = false;

                    rs_package = dtList.Select(COL_PACKID + " = '" + SerialNumber + "'");
                    if (rs_package.Length > 0)
                    {
                        _exists_package = true;
                    }
                    else
                    {
                        rs_package = dtList.Select("destPackageNumber" + " = '" + SerialNumber + "'");
                        if (rs_package.Length > 0)
                        {
                            _exists_package = true;
                            _exists_many = true;
                        }
                    }

                    if (!_exists_package)
                    {
                        addPackageNotInList(null, SerialNumber);
                        //enable button save
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        if (!_exists_many)
                        {
                            DataRow dr = rs_package[0];
                            if (Convert.ToInt32(dr[COL_ID]) > 0)
                            {
                                if (dr[COL_SCANNED] == "OK")
                                {
                                    MessageBox.Show("Package/Serial number was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                    return;
                                }

                                if (dr[COL_SCANNED] == "X")
                                    MessageBox.Show("Package/Serial number was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                else checkOverQuantity(dr);

                                dr["doneQuantity"] = dr["reserved"];
                                dr[COL_SCANNED] = "X";
                                dr[COL_SCANNED_BAK] = "X";
                                dr["state"] = "available";
                            }
                            else
                            {
                                if (dr[COL_SCANNED] == "N")
                                    MessageBox.Show("Package/Serial number was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                                dr[COL_SCANNED] = "N";
                                dr[COL_SCANNED_BAK] = "N";
                            }

                            int index = dtList.Rows.IndexOf(dr);
                            this.dgCuonList.CurrentRowIndex = index;
                        }
                        else
                        {
                            bool exported = true;
                            foreach (DataRow dr in rs_package)
                            {
                                if (dr[COL_SCANNED] != "OK")
                                {
                                    exported = false;
                                    break;
                                }
                            }
                            if (exported)
                            {
                                DialogResult mgb_export = new DialogResult();
                                mgb_export = MessageBox.Show("Package " + Convert.ToString(rs_package[0]["destPackageNumber"]) + " was scanned. Are you sure want to scan again?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                if (mgb_export != DialogResult.Yes) return;
                            }

                            if (rs_package[0][COL_SCANNED] == "X" || rs_package[0][COL_SCANNED] == "N" || rs_package[0][COL_SCANNED] == "OK")
                                MessageBox.Show("Package/Serial number was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

                            bool alertQuantity = false;
                            foreach (DataRow dr in rs_package)
                            {
                                if (!alertQuantity && Convert.ToString(dr[COL_SCANNED]).Length == 0) alertQuantity = checkOverQuantity(dr);

                                if (dr[COL_SCANNED] != "OK")
                                {
                                    dr["doneQuantity"] = dr["reserved"];
                                    dr[COL_SCANNED] = "X";
                                    dr[COL_SCANNED_BAK] = "X";
                                    dr["state"] = "available";
                                }
                            }
                            int index = dtList.Rows.IndexOf(rs_package[0]);
                            this.dgCuonList.CurrentRowIndex = index;
                        }

                        //enable button save
                        btnSave.Enabled = true;
                    }
                    #endregion
                }
                else
                {
                    #region scan transfer
                    btnReset.Enabled = false;
                    startLoading(dcdData, true);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.ToString());
            }
        }

        private bool checkOverQuantity(DataRow dr)
        {
            try
            {
                TransferItem result = this.ListTI.Find(x => x.productId == Convert.ToInt32(dr["productId"]));
                if (result != null)
                {
                    result.doneQuantity += Convert.ToDouble(dr["reserved"]);
                    if (result.initialQuantity < result.doneQuantity)
                    {
                        MessageBox.Show("Product " + result.productName + ": Scanned quantity greater than initial quantity !");
                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.TransferInfo.state == "done")
            {
                MessageBox.Show("The selected transaction has completed, actions are not allowed !");
                return;
            }

            ApiResponse res = new ApiResponse();
            res.Status = false;

            try
            {
                string url = "transfers/" + _transferId.ToString();
 
                //transfer info
                string para_transfer = "{";
                para_transfer += "\"id\": " + _transferId + ", ";
                if (TransferInfo.created != null) para_transfer += "\"created\": " + TransferInfo.created.ToString() + ", ";
                if (TransferInfo.updated != null) para_transfer += "\"updated\": " + TransferInfo.updated.ToString() + ", ";
                if (TransferInfo.createdBy != null) para_transfer += "\"createdBy\": \"" + TransferInfo.createdBy + "\", ";
                if (TransferInfo.transferNumber != null) para_transfer += "\"transferNumber\": \"" + TransferInfo.transferNumber + "\", ";
                if (TransferInfo.originTransferNumber != null) para_transfer += "\"originTransferNumber\": \"" + TransferInfo.originTransferNumber + "\", ";
                if (TransferInfo.operationTypeId != null) para_transfer += "\"operationTypeId\": " + TransferInfo.operationTypeId.ToString() + ", ";
                if (TransferInfo.srcLocationId != null) para_transfer += "\"srcLocationId\": " + TransferInfo.srcLocationId.ToString() + ", ";
                if (TransferInfo.destLocationId != null) para_transfer += "\"destLocationId\": " + TransferInfo.destLocationId.ToString() + ", ";
                if (TransferInfo.scheduledDate != null) para_transfer += "\"scheduledDate\": " + TransferInfo.scheduledDate.ToString() + ", ";
                if (TransferInfo.state != null) para_transfer += "\"state\": \"" + TransferInfo.state + "\", ";
                if (TransferInfo.productVersionId != null) para_transfer += "\"productVersionId\": " + TransferInfo.productVersionId.ToString() + ", ";
                if (TransferInfo.sourceDocument != null) para_transfer += "\"sourceDocument\": \"" + TransferInfo.sourceDocument + "\", ";
                if (TransferInfo.priority != null) para_transfer += "\"priority\": \"" + TransferInfo.priority + "\", ";
                if (TransferInfo.currentDemand != null) para_transfer += "\"currentDemand\": " + TransferInfo.currentDemand.ToString() + ", ";
                if (TransferInfo.productionQuantity != null) para_transfer += "\"productionQuantity\": " + TransferInfo.productionQuantity.ToString() + ", ";
                if (TransferInfo.capacity != null) para_transfer += "\"capacity\": " + TransferInfo.capacity.ToString() + ", ";
                if (TransferInfo.routing != null) para_transfer += "\"routing\": \"" + TransferInfo.routing.ToString() + "\", ";

                //transfer details
                para_transfer += "\"transferDetails\": [ ";
                foreach (DataRow row in dtList.Rows)
                {
                    if (row[COL_SCANNED] == "X" || row[COL_SCANNED] == "N")
                    {
                        para_transfer += "{ ";
                        if (row["destLocationId"] != null && Convert.ToString(row["destLocationId"]).Length > 0) para_transfer += "\"destLocationId\": " + row["destLocationId"] + ", ";
                        if (row["destPackageNumber"] != null && Convert.ToString(row["destPackageNumber"]).Length > 0) para_transfer += "\"destPackageNumber\": \"" + Convert.ToString(row["destPackageNumber"]).Trim() + "\", ";
                        if (row["srcPackageNumber"] != null && Convert.ToString(row["srcPackageNumber"]).Length > 0) para_transfer += "\"srcPackageNumber\": \"" + Convert.ToString(row["srcPackageNumber"]).Trim() + "\", ";
                        if (row["doneQuantity"] != null && Convert.ToString(row["doneQuantity"]).Length > 0) para_transfer += "\"doneQuantity\": " + row["doneQuantity"] + ", ";
                        if (row["manId"] != null && Convert.ToString(row["manId"]).Length > 0 && Convert.ToInt32(row["manId"]) > 0) para_transfer += "\"manId\": " + row["manId"] + ", ";
                        if (row["manPn"] != null && Convert.ToString(row["manPn"]).Length > 0) para_transfer += "\"manPn\": \"" + Convert.ToString(row["manPn"]).Trim() + "\", ";
                        if (row["productId"] != null && Convert.ToString(row["productId"]).Length > 0) para_transfer += "\"productId\": " + row["productId"] + ", ";
                        if (row["reserved"] != null && Convert.ToString(row["reserved"]).Length > 0) para_transfer += "\"reserved\": " + row["reserved"] + ", ";
                        if (row["srcLocationId"] != null && Convert.ToString(row["srcLocationId"]).Length > 0) para_transfer += "\"srcLocationId\": " + row["srcLocationId"] + ", ";
                        if (row["traceNumber"] != null && Convert.ToString(row["traceNumber"]).Length > 0) para_transfer += "\"traceNumber\": \"" + Convert.ToString(row["traceNumber"]).Trim() + "\", ";
                        if (row["transferId"] != null && Convert.ToString(row["transferId"]).Length > 0) para_transfer += "\"transferId\": " + row["transferId"] + ", ";
                        if (row["transferItemId"] != null && Convert.ToString(row["transferItemId"]).Length > 0) para_transfer += "\"transferItemId\": " + row["transferItemId"] + ", ";
                        if (row["id"] != null && Convert.ToInt32(row["id"]) > 0) para_transfer += "\"id\": " + row["id"] + ", ";
                        if (row["lotId"] != null && Convert.ToString(row["lotId"]).Length > 0) para_transfer += "\"lotId\": " + row["lotId"] + ", ";
                        if (row["reference"] != null && Convert.ToString(row["reference"]).Length > 0) para_transfer += "\"reference\": \"" + Convert.ToString(row["reference"]).Trim() + "\", ";
                        if (row["level"] != null && Convert.ToString(row["level"]).Length > 0) para_transfer += "\"level\": \"" + row["level"] + "\", ";
                        if (row["created"] != null && Convert.ToString(row["created"]).Length > 0) para_transfer += "\"created\": " + row["created"] + ", ";
                        if (row["createdBy"] != null && Convert.ToString(row["createdBy"]).Length > 0) para_transfer += "\"createdBy\": \"" + row["createdBy"] + "\", ";
                        if (row["internalReference"] != null && Convert.ToString(row["internalReference"]).Length > 0) para_transfer += "\"internalReference\": \"" + Convert.ToString(row["internalReference"]).Trim() + "\", ";

                        string end = para_transfer.Substring(para_transfer.Length - 2, 1);
                        if (end == ",") para_transfer = para_transfer.Substring(0, para_transfer.Length - 2);

                        para_transfer += "}, ";
                    }
                }
                string end_char = para_transfer.Substring(para_transfer.Length - 2, 1);
                if (end_char == ",") para_transfer = para_transfer.Substring(0, para_transfer.Length - 2);
                para_transfer += " ] }";

                para_transfer = para_transfer.Replace(System.Environment.NewLine, "").Trim();

                //Util.Logs(para_transfer);

                res = HTTP.Put(url, para_transfer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            if (res.Status)
            {
                MessageBox.Show("Export Success !");
                btnSave.Enabled = false;
                btnReset.Enabled = true;
                this.dtList.Rows.Clear();
                this.dgCuonList.Refresh();
                _transferId = 0;
                this.txtTransferNumber.Text = null;
                this.TransferInfo = null;
            }
            else
            {
                //MessageBox.Show(res.RawText);
            }

        }

        private void dgCuonList_CurrentCellChanged(object sender, EventArgs e)
        {
            
        }

        private void txtTransferNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string _transferNumber = this.txtTransferNumber.Text.Trim();
                startLoading(_transferNumber, false);
                //loadTransferHandleInput();
                //stopLoading();
            }
        }

        private bool getListTransferItem(int transferId)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;

            try
            {
                string url = "transfer-items/search?query=transferId==" + transferId.ToString();
                res = HTTP.GetJson(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get transfer items error: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }
            try
            {
                if (res.Status && Util.IsJson(res.RawText))
                {
                    this.ListTI = JsonConvert.DeserializeObject<List<TransferItem>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                }
                else
                {
                    MessageBox.Show(res.RawText);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load transfer items error: " + ex.Message.ToString());
                return false;
            }
            return true;
        }

        private void addPackageNotInList(string _productName, string _packageId)
        {
            try
            {
                DataColumnCollection columns = dtList.Columns;
                if (!columns.Contains(COL_LOCATION)) dtList.Columns.Add(COL_LOCATION);

                DataRow dr = this.dtList.NewRow();
                dr[COL_ID] = -1;
                dr[COL_LOCATION] = null;
                dr[COL_PACKID] = _packageId;
                if (Util.getTypePackage(_packageId.Trim(), null) % 2 == 0)
                {
                    dr["traceNumber"] = _packageId;
                }
                else
                {
                    dr["destPackageNumber"] = _packageId;
                    dr["srcPackageNumber"] = _packageId;
                }
                dr["internalReference"] = _productName;
                dr[COL_SCANNED] = "N";
                dr[COL_SCANNED_BAK] = "N";
                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;
                this.dgCuonList.Refresh();

                this.dgCuonList.CurrentRowIndex = this.dtList.Rows.Count-1;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void addPackageOldNotInList(string _productName, string _packageId, int _type)
        {
            try
            {
                DataColumnCollection columns = dtList.Columns;
                if (!columns.Contains(COL_LOCATION)) dtList.Columns.Add(COL_LOCATION);

                DataRow dr = this.dtList.NewRow();
                dr[COL_ID] = -1;
                dr[COL_LOCATION] = null;
                dr[COL_PACKID] = _packageId;
                if (_type == 0)
                    dr["traceNumber"] = _packageId;
                else
                {
                    dr["destPackageNumber"] = _packageId;
                    dr["srcPackageNumber"] = _packageId;
                }
                dr["internalReference"] = _productName;
                dr[COL_SCANNED] = "N";
                dr[COL_SCANNED_BAK] = "N";
                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;
                this.dgCuonList.Refresh();

                this.dgCuonList.CurrentRowIndex = this.dtList.Rows.Count - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dgCuonList_Click(object sender, EventArgs e)
        {
            try
            {
                int index_column_scan = 0;
                foreach (DataColumn item in dtList.Columns)
                {
                    if (item.ColumnName == COL_SCANNED)
                    {
                        break;
                    }
                    index_column_scan++;
                }

                //MessageBox.Show(index_column_scan.ToString());

                DataGridCell row = dgCuonList.CurrentCell;
                //MessageBox.Show(row.ColumnNumber.ToString());
                if (row.ColumnNumber == index_column_scan)
                {
                    if (dtList.Rows[row.RowNumber][COL_SCANNED].ToString().Length > 0)
                    {
                        dtList.Rows[row.RowNumber][COL_SCANNED] = "";
                    }
                    else
                    {
                        dtList.Rows[row.RowNumber][COL_SCANNED] = dtList.Rows[row.RowNumber][COL_SCANNED_BAK];
                    }
                    dgCuonList.Refresh();
                }
            }
            catch { }
        }

        private void TransferOut_Load(object sender, EventArgs e)
        {

        }

        private Thread loadingThread;
        private Thread loadTransferThread;
        bool stillRun = false;

        private delegate void ProgressBarUpdater(int value);
        private void ProgressBarUpdateValue(int value)
        {
            progressLoading.Value = value;
        }

        private delegate void ProgressBarHideUpdater();
        private void ProgressBarHide()
        {
            this.progressLoading.Visible = false;
            this.progressLoading.Value = 0;
            this.Refresh();
        }

        private delegate void ControlEnableUpdater(Control uiControl, bool enable);
        private void ControlEnable(Control uiControl,bool enable)
        {
            uiControl.Enabled = enable;
        }

        private delegate void ControlTextUpdater(Control uiControl, string text);
        private void ControlTextUpdate(Control uiControl, string text)
        {
            uiControl.Text = text;
        }

        private delegate void DataGridInitUpdater(DataTable dt);
        private void DataGridInitValue(DataTable dt)
        {
            this.dgCuonList.DataSource = dt;
            this.dgCuonList.TableStyles.Clear();
        }

        private delegate void DataGridAddStyleUpdater(DataGridTableStyle tableStyle);
        private void DataGridAddStyle(DataGridTableStyle tableStyle)
        {
            dgCuonList.TableStyles.Add(tableStyle);
            dgCuonList.Refresh();
        }

        private delegate void FormRefreshController();
        private void FormRefresh()
        {
            this.Refresh();
        }

        private void loadingWork()
        {
            ProgressBarUpdater progressBarUpdater = ProgressBarUpdateValue;

            int i = 1;
            while (!stillRun)
            {
                if (i >= 100) i = 1;
                //this.Invoke(progressBarUpdater, i);
                this.progressLoading.Invoke(progressBarUpdater, i);
                Thread.Sleep(50);
                i++;
            }

            this.progressLoading.BeginInvoke(progressBarUpdater, 0);
        }

        private void startLoading(string transferNumber, bool isScan)
        {
            progressLoading.Visible = true;
            progressLoading.Value = 0;
            this.Refresh();
            
            loadingThread = new Thread(loadingWork);
            stillRun = false;
            loadingThread.Start();

            if (isScan)
            {
                loadTransferThread = new Thread(() => loadTransferHandleScan(transferNumber));
                loadTransferThread.Start();
            }
            else
            {
                loadTransferThread = new Thread(() => loadTransferHandleInput(transferNumber));
                loadTransferThread.Start();
            }
        }

        private void stopLoading()
        {
            ProgressBarHideUpdater progressBarHideUpdater = ProgressBarHide;
            this.progressLoading.Invoke(progressBarHideUpdater);
            stillRun = true;
            loadingThread.Abort();
            loadTransferThread.Abort();
            
            //this.Refresh();
            //FormRefreshController formRefreshController = FormRefresh;
            //this.Invoke(formRefreshController);
        }

        private void loadTransferHandleInput(string _transferNumber)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;

            try
            {
                string url = "transfers/search?query=transferNumber=='" + _transferNumber + "'";
                res = HTTP.GetJson(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get transfer error: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                stopLoading();
                return;
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
                            if (!getListTransferItem(_transferId)) { stopLoading(); return; }
                            if (!LoadData(_transferId)) { stopLoading(); return; }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Not exists transfer number !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                }
                else
                {
                    MessageBox.Show("Get transfer info error: " + res.RawText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.Message.ToString());
            }

            stopLoading();
        }

        private void loadTransferHandleScan(string dcdData)
        {
            try
            {
                if (dcdData.Trim().Length > 0)
                {
                    ApiResponse res = new ApiResponse();
                    res.Status = false;

                    try
                    {
                        string url = "transfers/search?query=transferNumber=='" + dcdData.Trim() + "'";
                        res = HTTP.GetJson(url);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
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
                                _transferId = TransferInfo.id;
                                if (!getListTransferItem(_transferId)) { stopLoading(); return; }
                                if (!LoadData(_transferId)) { stopLoading(); return; }

                                ControlTextUpdater controlTextUpdater = ControlTextUpdate;
                                this.txtTransferNumber.Invoke(controlTextUpdater, txtTransferNumber, dcdData.Trim());
                                //txtTransferNumber.Text = dcdData.Trim();
                            }
                            else
                            {
                                ControlTextUpdater controlTextUpdater = ControlTextUpdate;
                                this.txtTransferNumber.Invoke(controlTextUpdater, txtTransferNumber, null);
                                MessageBox.Show("Not exists transfer number !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            }

                        }
                        catch (Exception ex)
                        {
                            _transferId = 0;
                            ControlTextUpdater controlTextUpdater = ControlTextUpdate;
                            this.txtTransferNumber.Invoke(controlTextUpdater, txtTransferNumber, null);
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                    else
                    {
                        _transferId = 0;
                        ControlTextUpdater controlTextUpdater = ControlTextUpdate;
                        this.txtTransferNumber.Invoke(controlTextUpdater, txtTransferNumber, null);
                        MessageBox.Show(res.RawText);
                    }
                }
                else
                {
                    _transferId = 0;
                    ControlTextUpdater controlTextUpdater = ControlTextUpdate;
                    this.txtTransferNumber.Invoke(controlTextUpdater, txtTransferNumber, null);
                    //txtTransferNumber.Focus();
                }
            }
            catch (Exception ex)
            {
                ControlTextUpdater controlTextUpdater = ControlTextUpdate;
                this.txtTransferNumber.Invoke(controlTextUpdater, txtTransferNumber, null);

                //btnReset.Enabled = false;
                ControlEnableUpdater controlEnableUpdater = ControlEnable;
                this.btnReset.Invoke(controlEnableUpdater, btnReset, false);
                
                dtList.Rows.Clear();
                _transferId = 0;
                MessageBox.Show("Transfer number wrong format !");
            }

            stopLoading();
        }
    }

}