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
    public partial class InventoryAdjustment : Form
    {
        #region Properties
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;

        //private const string COL_CANCEL = "cancel";
        private const string COL_QUANT = "realQuantity";
        private const string PREFIX_LOT = "UID";
        private const string PREFIX_PACK = "PACK";

        private int _adjustmentId = 0;
        private Adjustment _Adjustment;

        private DataTable dtList = new DataTable();
        private List<AdjustmentInput> ListDetail = new List<AdjustmentInput>();

        private const string INVENTORY_OF_ALL = "all_product";
        private const string INVENTORY_OF_CATEGORY = "one_product_category";
        //private const string INVENTORY_OF_PRODUCT = "one_product_only";
        private const string INVENTORY_OF_PRODUCT_MANUALLY = "select_products_manually";
        private const string INVENTORY_OF_LOT = "one_lot_number";
        private const string INVENTORY_OF_PACKAGE = "one_package_number";
        //private const string INVENTORY_OF_RANDOM_SAMPLE = "random_sample";

        private List<AdjustmentDetail> ListSpace = new List<AdjustmentDetail>();
        #endregion

        public InventoryAdjustment()
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

            btnScan.Enabled = false;
            dtList.Rows.Clear();
        }

        private void dcdEvent_Scanned(object sender, DecodeEventArgs e)
        {
            CodeId cID = CodeId.NoData;
            string dcdData = string.Empty;
            dcdData = hDcd.ReadString(e.RequestID, ref cID);

            ScanCode(dcdData);
        }

        private void LoadData(Adjustment Adjustment)
        {
            switch (Adjustment.inventoryOf)
            {
                case INVENTORY_OF_CATEGORY:
                    {
                        this.lblHiddenField.Visible = true;
                        this.lblHiddenFieldValue.Visible = true;
                        this.dgCuonList.Visible = false;
                        this.lblHiddenField.Text = "Category name:";
                        this.lblHiddenFieldValue.Text = Adjustment.categoryName;
                    } break;
                case INVENTORY_OF_PRODUCT_MANUALLY:
                    {
                        this.lblHiddenField.Visible = false;
                        this.lblHiddenFieldValue.Visible = false;
                        this.dgCuonList.Visible = true;
                        loadProductsManually(Adjustment.id);
                    } break;
                case INVENTORY_OF_LOT:
                    {
                        this.lblHiddenField.Visible = true;
                        this.lblHiddenFieldValue.Visible = true;
                        this.dgCuonList.Visible = false;
                        this.lblHiddenField.Text = "Trace number:";
                        this.lblHiddenFieldValue.Text = Adjustment.traceNumber;
                    } break;
                case INVENTORY_OF_PACKAGE:
                    {
                        this.lblHiddenField.Visible = true;
                        this.lblHiddenFieldValue.Visible = true;
                        this.dgCuonList.Visible = false;
                        this.lblHiddenField.Text = "Package number:";
                        this.lblHiddenFieldValue.Text = Adjustment.packageNumber;
                    } break;
                default:
                    {
                        this.lblHiddenField.Visible = false;
                        this.lblHiddenFieldValue.Visible = false;
                        this.dgCuonList.Visible = false;
                    } break;
            }
        }

        private void checkScan(Adjustment _Adjustment)
        {
            if(_Adjustment.state == "in_progress")
                this.btnScan.Enabled = true;
            else
                this.btnScan.Enabled = false;
        }

        private void loadProductsManually(int _adjustmentId)
        {
            dtList.Rows.Clear();

            ApiResponse res = new ApiResponse();
            res.Status = false;
            //Load danh sach linh kien
            try
            {
                string url = "adjustment-inputs/search?query=adjustmentId==" + _adjustmentId + "&page=0&size=1000&sort=,asc";
                res = HTTP.GetJson(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load adjustment input data Fail !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            if (res.Status && Util.IsJson(res.RawText))
            {
                try
                {
                    List<AdjustmentInput> RootObject = JsonConvert.DeserializeObject<List<AdjustmentInput>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    ListDetail = RootObject as List<AdjustmentInput>;

                    foreach (AdjustmentInput item in ListDetail)
                    {
                        if (item.internalReference == null && item.productId != null)
                        {
                            Product product = new Product();
                            Product productInfo = product.getInfo(item.productId);
                            if (productInfo != null) item.internalReference = productInfo.name;
                        }
                    }

                    dtList = Util.ToDataTable<AdjustmentInput>(ListDetail);

                    dgCuonList.DataSource = dtList;
                    dgCuonList.TableStyles.Clear();

                    DataGridTableStyle tableStyle = new DataGridTableStyle();
                    tableStyle.MappingName = dtList.TableName;
                    foreach (DataColumn item in dtList.Columns)
                    {
                        DataGridTextBoxColumn tbcName = new DataGridTextBoxColumn();

                        switch (item.ColumnName)
                        {
                            case "internalReference":
                                {
                                    tbcName.MappingName = item.ColumnName;
                                    tbcName.HeaderText = "Product";
                                    tbcName.Width = 100;
                                } break;
                            case "packageNumber":
                                {
                                    tbcName.MappingName = item.ColumnName;
                                    tbcName.HeaderText = "Package";
                                    tbcName.Width = 60;
                                } break;
                            case "traceNumber":
                                {
                                    tbcName.MappingName = item.ColumnName;
                                    tbcName.HeaderText = "Unit ID";
                                    tbcName.Width = 60;
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
                    //MessageBox.Show("Error during loading information !");
                }
            }
            else
            {
                //MessageBox.Show(res.RawText);
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
            //dtList.Rows.Clear();

            try
            {
                if (dcdData.Trim().Length > 0)
                {
                    ApiResponse res = new ApiResponse();
                    res.Status = false;

                    try
                    {
                        string url = "inventories/search?query=reference==\"" + dcdData.Trim() + "\"";
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
                            List<Adjustment> RootObject = JsonConvert.DeserializeObject<List<Adjustment>>(res.RawText, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

                            if (RootObject.Count > 0)
                            {
                                _Adjustment = RootObject[0];
                                this.lblInventoryOfValue.Text = _Adjustment.inventoryOf;
                                this.lblInventoryLocationValue.Text = _Adjustment.locationName;

                                _adjustmentId = _Adjustment.id;
                                txtInventoryName.Text = _Adjustment.reference;
                                this.ListSpace.Clear();

                                checkScan(_Adjustment);

                                LoadData(_Adjustment);
                            }
                            else
                            {
                                MessageBox.Show("Not exists inventory name !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
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
                    txtInventoryName.Focus();
                }
            }
            catch (Exception ex)
            {
                txtInventoryName.Text = null;
                dtList.Rows.Clear();
                _adjustmentId = 0;
                MessageBox.Show("Inventory name wrong format !");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dcdEvent.IsListening)
                {
                    dcdEvent.StopScanListener();
                }
            }
            catch { }
            try
            {
                if (hDcd != null)
                {
                    hDcd.Dispose();
                }
            }
            catch { }
            this.Close();
        }

        private void txtTransferNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string _inventoryName = txtInventoryName.Text.Trim();
                ApiResponse res = new ApiResponse();
                res.Status = false;

                try
                {
                    string url = "inventories/search?query=reference=='" + _inventoryName + "'";
                    res = HTTP.GetJson(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Not exists inventory name !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                try
                {
                    if (res.Status && Util.IsJson(res.RawText))
                    {
                        List<Adjustment> RootObject = JsonConvert.DeserializeObject<List<Adjustment>>(res.RawText, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

                        if (RootObject.Count > 0)
                        {
                            _Adjustment = RootObject[0];
                            _adjustmentId = _Adjustment.id;
                            this.lblInventoryOfValue.Text = _Adjustment.inventoryOf;
                            this.lblInventoryLocationValue.Text = _Adjustment.locationName;
                            this.ListSpace.Clear();

                            checkScan(_Adjustment);

                            if (_adjustmentId != 0)
                            {
                                LoadData(_Adjustment);
                            }
                            else
                            {
                                txtInventoryName.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Not exists inventory name !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
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

        private void dgCuonList_Click(object sender, EventArgs e)
        {

            //try
            //{
            //    int index_column_scan = 0;
            //    foreach (DataColumn item in dtList.Columns)
            //    {
            //        if (item.ColumnName == COL_CANCEL)
            //        {
            //            break;
            //        }
            //        index_column_scan++;
            //    }
            //    DataGridCell row = dgCuonList.CurrentCell;
            //    if (row.ColumnNumber == index_column_scan)
            //    {
            //        dtList.Rows[row.RowNumber][COL_QUANT] = 0;
            //        dgCuonList.Refresh();
            //    }
            //}
            //catch { }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            try
            {
                if (dcdEvent.IsListening)
                {
                    dcdEvent.StopScanListener();
                }
            }
            catch { }
            try
            {
                if (hDcd != null)
                {
                    hDcd.Dispose();
                }
            }
            catch { }

            InventoryAdjustmentScan frmInventoryAdjustmentScan = new InventoryAdjustmentScan();
            frmInventoryAdjustmentScan.ListSpace = this.ListSpace;
            //frmInventoryAdjustmentScan.ListDetail = this.ListDetail;
            frmInventoryAdjustmentScan.Adjustment = this._Adjustment;
            frmInventoryAdjustmentScan.ShowDialog();

            this.ListSpace = frmInventoryAdjustmentScan.ListSpace;

            //Initialize event
            try
            {
                hDcd = new DecodeHandle(DecodeDeviceCap.Exists | DecodeDeviceCap.Barcode);
                DecodeRequest reqType = (DecodeRequest)1 | DecodeRequest.PostRecurring;
                dcdEvent = new DecodeEvent(hDcd, reqType, this);
                dcdEvent.Scanned += new DecodeScanned(dcdEvent_Scanned);
            }
            catch (Exception ex) { }
        }
    }

}