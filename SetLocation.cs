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
    public partial class SetLocation : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;
        
        private const string COL_DEL = "delete";
        private const string COL_ID = "pack_id";
        private const string COL_PRODUCT_NAME = "product_name";
        private const string COL_STATUS = "status";
        private const string COL_ERROR_CODE = "error_code";
        private const string COL_TYPE = "type";
        private const string PREFIX_LOT = "UID";
        private const string PREFIX_PACK = "PACK";

        private DataTable dtList = new DataTable();
        private string BarcodeLocation;

        #region Properties

        #endregion

        public SetLocation()
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
            dtList.Rows.Clear();

            //TEST
            //ScanCode(" PN:KBEFDQV50000058M*SPNK:PART-IMP-17*SDH:VNPT/TECH-BO-04/2017-97*NNK:2017-04-28*ID:0000203252*SL:1000000000*TYPE:1*");

            //ScanCode("1256-SA4-3");
        }

        private void InitData()
        {
            try
            {
                dtList.Rows.Clear();

                dtList.Columns.Add(COL_PRODUCT_NAME);
                dtList.Columns.Add(COL_ID);
                dtList.Columns.Add(COL_DEL);
                dtList.Columns.Add(COL_STATUS);
                dtList.Columns.Add(COL_ERROR_CODE);
                dtList.Columns.Add(COL_TYPE);

                dgCuonList.DataSource = dtList;
                dgCuonList.TableStyles.Clear();

                DataGridTableStyle tableStyle = new DataGridTableStyle();
                tableStyle.MappingName = dtList.TableName;

                foreach (DataColumn item in dtList.Columns)
                {
                    DataGridTextBoxColumn tbcName = new DataGridTextBoxColumn();

                    switch (item.ColumnName)
                    {
                        //case COL_PRODUCT_NAME:
                        //    {
                        //        tbcName.MappingName = item.ColumnName;
                        //        tbcName.HeaderText = "Product";
                        //        tbcName.Width = 80;
                        //    } break;
                        case COL_ID:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Package/Unit ID";
                                tbcName.Width = 100;
                            } break;
                        case COL_DEL:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Delete";
                                tbcName.Width = 50;
                            } break;
                        case COL_STATUS:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Status";
                                tbcName.Width = 55;
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

        private void ScanCode(string dcdData)
        {
            if (dcdData.StartsWith("["))
            {
                #region Scan Package/Unit ID new
                try
                {
                    LabelPackage labelPackage = new LabelPackage(dcdData.Trim());
                    DataRow[] rs_package = null;

                    bool _exists_package = false;

                    if (labelPackage.PackageId != null && labelPackage.PackageId != "")
                    {
                        rs_package = dtList.Select(COL_ID + " = '" + labelPackage.PackageId + "'");
                        if (rs_package.Length > 0)
                        {
                            _exists_package = true;
                        }

                        //DialogResult mgb = new DialogResult();
                        if (_exists_package)
                        {
                            //mgb = MessageBox.Show("Package not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            //if (mgb == DialogResult.Yes)
                            //{
                            //    dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_ID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                            //    if(dr == null)
                            //        addPackageNotInList(labelPackage.ProductName, labelPackage.PackageId);
                            //}
                            MessageBox.Show("Package already exists in the list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                        else
                        {
                            addPackageToList(labelPackage.ProductName, labelPackage.PackageId);

                            if (BarcodeLocation !=  null && BarcodeLocation.Trim().Length > 0 && !btnSave.Enabled)
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
                    MessageBox.Show("Wrong Package/Unit ID QRCode format !");
                    //MessageBox.Show(ex.InnerException.ToString());
                    //MessageBox.Show("Error during loading information !", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                #endregion
            }
            else if (dcdData.Trim().StartsWith("PN:"))
            {
                #region Scan Package/Unit ID old
                try
                {
                    TemCuon temCuon = new TemCuon(dcdData.Trim());
                    DataRow[] rs_package = null;

                    bool _exists_package = false;

                    if (temCuon.IdCuon != null && temCuon.IdCuon != "")
                    {
                        rs_package = dtList.Select(COL_ID + " = '" + temCuon.IdCuon + "'");
                        if (rs_package.Length > 0)
                        {
                            _exists_package = true;
                        }

                        //DialogResult mgb = new DialogResult();
                        if (_exists_package)
                        {
                            //mgb = MessageBox.Show("Package not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            //if (mgb == DialogResult.Yes)
                            //{
                            //    dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_ID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                            //    if(dr == null)
                            //        addPackageNotInList(labelPackage.ProductName, labelPackage.PackageId);
                            //}
                            MessageBox.Show("Package already exists in the list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                        else
                        {
                            addPackageToList(temCuon.VnptPn, temCuon.IdCuon, Convert.ToInt32(temCuon.Type));

                            if (BarcodeLocation != null && BarcodeLocation.Trim().Length > 0 && !btnSave.Enabled)
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
                    MessageBox.Show("Wrong Package/Unit ID QRCode format !");
                }
                #endregion
            }
            else if (Util.OnlyHexInString(dcdData.Trim()))
            {
                #region Scan Serial number
                try
                {
                    string SerialNumber = dcdData.Trim();
                    DataRow[] rs_package = null;
                    bool _exists_package = false;

                    if (SerialNumber != null && SerialNumber != "")
                    {
                        rs_package = dtList.Select(COL_ID + " = '" + SerialNumber + "'");
                        if (rs_package.Length > 0)
                        {
                            _exists_package = true;
                        }

                        //DialogResult mgb = new DialogResult();
                        if (_exists_package)
                        {
                            //mgb = MessageBox.Show("Package not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            //if (mgb == DialogResult.Yes)
                            //{
                            //    dr = dtList.Select("internalReference = '" + labelPackage.ProductName + "' AND " + COL_ID + " = '" + labelPackage.PackageId + "'").FirstOrDefault();
                            //    if(dr == null)
                            //        addPackageNotInList(labelPackage.ProductName, labelPackage.PackageId);
                            //}
                            MessageBox.Show("Package/Serial number already exists in the list !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                        else
                        {
                            addPackageToList(null, SerialNumber);

                            if (BarcodeLocation != null && BarcodeLocation.Trim().Length > 0 && !btnSave.Enabled)
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
                    MessageBox.Show("Wrong Package/Unit ID QRCode format !");
                    //MessageBox.Show(ex.InnerException.ToString());
                    //MessageBox.Show("Error during loading information !", "Chu y", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                #endregion
            }
            else
            {
                #region Scan location

                ApiResponse res = new ApiResponse();
                res.Status = false;
                try
                {
                    string url = "locations/search?query=barcode==\"" + dcdData.Trim() + "\"";
                    res = HTTP.GetJson(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during load location information !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
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
                        this.lblLocationBarcode.Text = ListLocations[0].name;
                        BarcodeLocation = dcdData;

                        if (this.dtList.Rows.Count > 0 && !btnSave.Enabled)
                        {
                            //enable button save
                            btnSave.Enabled = true;
                            btnClear.Enabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error during load location information !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                }
                #endregion
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
                    string url = "smart-devices/update-locations";

                    string param = "[";
                    foreach (DataRow row in dtList.Rows)
                    {
                        param += "{";
                        if (BarcodeLocation != null && BarcodeLocation.Length > 0) param += "\"locationCode\": \"" + BarcodeLocation + "\", ";
                        if (Util.getTypePackage(row[COL_ID].ToString().Trim(), row[COL_TYPE].ToString()) % 2 == 0)
                        {
                            param += "\"lotNumber\": \"" + row[COL_ID] + "\"";
                        }
                        else
                        {
                            param += "\"packageNumber\": \"" + row[COL_ID] + "\"";
                        }
                        param += "}, ";
                    }
                    string end_char = param.Substring(param.Length - 2, 1);
                    if (end_char == ",") param = param.Substring(0, param.Length - 2);
                    param += " ]";

                    param = param.Replace(System.Environment.NewLine, "").Trim();

                    //Util.Logs(param);

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
                        List<LocationUpdate> RootObject = JsonConvert.DeserializeObject<List<LocationUpdate>>(res.RawText, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                        List<LocationUpdate> ListDetail = RootObject as List<LocationUpdate>;
                        //dtList.Rows.Clear();

                        foreach (LocationUpdate item in ListDetail)
                        {
                            string packIdResponse = item.lotNumber != null && item.lotNumber.Length > 0 ? item.lotNumber : item.packageNumber;
                            DataRow[] rs_package = dtList.Select(COL_ID + " = '" + packIdResponse.Trim() + "'");
                            if (rs_package.Length > 0)
                            {
                                rs_package[0][COL_STATUS] = item.errorCode != null && item.errorCode.Length > 0 ? "error" : "";
                                rs_package[0][COL_ERROR_CODE] = item.errorCode != null && item.errorCode.Length > 0 ? item.errorCode : "";
                            }
                        }

                        this.dtList.DefaultView.Sort = COL_STATUS + " DESC";
                        this.dtList = this.dtList.DefaultView.ToTable();

                        this.dgCuonList.DataSource = this.dtList;
                        this.dgCuonList.Refresh();
                    }
                    catch { }

                    MessageBox.Show("Update location done !");
                }
                else
                {
                    //MessageBox.Show(res.RawText);
                }

                btnSave.Enabled = false;
                btnClear.Enabled = true;
                
            }
            else
            {
                MessageBox.Show("Request scanned package before saving !");
            }
        }

        private void dgCuonList_CurrentCellChanged(object sender, EventArgs e)
        {
            
        }

        private void addPackageToList(string _productName, string _packageId)
        {
            try
            {
                DataRow dr = this.dtList.NewRow();
                dr[COL_ID] = _packageId;
                dr[COL_PRODUCT_NAME] = _productName;
                //if(_packageId.StartsWith(PREFIX_LOT))
                //    dr["traceNumber"] = _packageId;
                //else
                //    dr["destPackageNumber"] = _packageId;
                //dr["internalReference"] = _productName;
                dr[COL_DEL] = "X";
                dr[COL_STATUS] = "";
                dr[COL_ERROR_CODE] = "";
                dr[COL_TYPE] = null;
                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;
                this.dgCuonList.Refresh();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void addPackageToList(string _productName, string _packageId, int _type)
        {
            try
            {
                DataRow dr = this.dtList.NewRow();
                dr[COL_ID] = _packageId;
                dr[COL_PRODUCT_NAME] = _productName;
                dr[COL_DEL] = "X";
                dr[COL_STATUS] = "";
                dr[COL_ERROR_CODE] = "";
                dr[COL_TYPE] = _type;
                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;
                this.dgCuonList.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SetLocation_Closing(object sender, CancelEventArgs e)
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

        private void SetLocation_Load(object sender, EventArgs e)
        {
            InitData();
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
                }else if(row.ColumnNumber == 3)
                {
                    if(dtList.Rows[row.RowNumber][COL_ERROR_CODE].ToString().Length > 0)
                        MessageBox.Show(dtList.Rows[row.RowNumber][COL_ERROR_CODE].ToString());
                }
            }
            catch { }
        }
    }

}