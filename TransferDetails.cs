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
    public partial class TransferDetails : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;

        #region Properties
        private const string COL_SCANNED = "scanned";
        private const string COL_SCANNED_BAK = "scanned_bak";
        private const string COL_PACKID = "destPackageNumber";
        private const string COL_UID = "traceNumber";
        private const string COL_LOCATION = "destLocationName";
        private const string COL_INTERNAL_REFERENCE = "internalReference";
        private const string COL_RESERVED = "reserved";
        private const string COL_DONE = "doneQuantity";
        private const string COL_ID = "id";
        private const string COL_PRODUCTID = "productId";
        private const string COL_MANID = "manId";
        private const string COL_MANPN = "manPn";

        private const string PREFIX_LOT = "UID";
        private const string PREFIX_PACK = "PACK";

        private int _transferId = 0;
        private int _typeTransfer = 0;
        private LocationInfo locationInfo;

        public int TransferId
        {
            get { return _transferId; }
            set { this._transferId = value; }
        }
        public int TypeTransfer
        {
            get { return _typeTransfer; }
            set { this._typeTransfer = value; }
        }

        public TransferInfo TransferInfo;
        private DataTable dtList = new DataTable();
        private List<TransferItem> listTransferItem = new List<TransferItem>();
        private List<TransferDetail> listTransferDetail = new List<TransferDetail>();

        public List<TransferItem> ListTransferItem
        {
            get { return listTransferItem; }
            set { this.listTransferItem = value; }
        }
        public List<TransferDetail> ListTransferDetail
        {
            get { return listTransferDetail; }
            set { this.listTransferDetail = value; }
        }

        public LocationInfo LocationInfo
        {
            get { return this.locationInfo; }
            set { this.locationInfo = value; }
        }

        private bool _closeStatus;
        public bool CloseStatus
        {
            get { return this._closeStatus; }
            set { this._closeStatus = value; }
        }
        #endregion

        public TransferDetails()
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

            //btnReset.Enabled = false;
            //btnSave.Enabled = false;
            //dtList.Rows.Clear();
        }

        private void dcdEvent_Scanned(object sender, DecodeEventArgs e)
        {
            CodeId cID = CodeId.NoData;
            string dcdData = string.Empty;
            dcdData = hDcd.ReadString(e.RequestID, ref cID);

            ScanCode(dcdData);
        }

        // --------------------------LOAD----------------------------------------

        private void TransferDetails_Load(object sender, EventArgs e)
        {
            this._closeStatus = false;

            if (this.TypeTransfer == 1)
            {
                lblScanValue.Text = TransferInfo.transferNumber;
                lblScan.Text = "TRANSFER";
            }
            else
            {
                if (this.locationInfo != null) lblScanValue.Text = this.locationInfo.name; else lblScanValue.Text = null;
                lblScan.Text = "LOCATION";
            }

            if (this.listTransferDetail.Count > 0)
            {
                dtList.Rows.Clear();
                dtList = Util.ToDataTable<TransferDetail>(listTransferDetail);
                loadingStart(false);
            }
            else
            {
                loadingStart(true);
            }
        }

        #region Thread

        private Thread loadingThread;
        private Thread loadTransferThread;
        bool stillRun = false;

        private void loadingWork()
        {
            ProgressBarUpdater progressBarUpdater = ProgressBarUpdateValue;

            int i = 1;
            while (!stillRun)
            {
                if (i >= 100) i = 1;
                this.progressLoading.Invoke(progressBarUpdater, i);
                Thread.Sleep(50);
                i++;
            }
            this.progressLoading.BeginInvoke(progressBarUpdater, 0);
        }

        private void loadingStart(bool isFirst)
        {
            progressLoading.Visible = true;
            progressLoading.Value = 0;
            this.Refresh();

            loadingThread = new Thread(loadingWork);
            stillRun = false;
            loadingThread.Start();

            if (isFirst)
            {
                loadTransferThread = new Thread(() => LoadData(TransferInfo.id));
                loadTransferThread.Start();
            }
            else
            {
                loadTransferThread = new Thread(() => renderDataGrid(dtList));
                loadTransferThread.Start();
            }
        }

        private void loadingStop()
        {
            ProgressBarHideUpdater progressBarHideUpdater = ProgressBarHide;
            this.progressLoading.Invoke(progressBarHideUpdater);
            stillRun = true;
            loadingThread.Abort();
            loadTransferThread.Abort();
        }

        #endregion

        private bool LoadData(int _transferId)
        {
            dtList.Rows.Clear();

            ApiResponse res = new ApiResponse();
            res.Status = false;
            try
            {
                string url = "transfer-details/search?query=transferId==" + _transferId.ToString() + "&size=2000";
                res = HTTP.GetJson(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get transfer details error: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                loadingStop();
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

                    listTransferDetail = RootObject as List<TransferDetail>;
                    foreach (TransferDetail item in listTransferDetail)
                    {
                        if (item.doneQuantity > 0)
                        {
                            item.scanned = "OK";
                            item.scanned_bak = "OK";
                        }
                        else
                        {
                            item.scanned = "";
                            item.scanned_bak = "";
                        }
                    }
                    dtList = Util.ToDataTable<TransferDetail>(listTransferDetail);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Parse transfer details error: " + ex.Message.ToString());
                    loadingStop();
                    return false;
                }

                if (!renderDataGrid(dtList)) { loadingStop(); return false; }

            }
            else
            {
                MessageBox.Show(res.RawText);
                loadingStop();
                return false;
            }
            return true;
        }

        private bool renderDataGrid(DataTable dtList)
        {
            try
            {
                if (!dtList.Columns.Contains(COL_SCANNED)) dtList.Columns.Add(COL_SCANNED);
                if (!dtList.Columns.Contains(COL_SCANNED_BAK)) dtList.Columns.Add(COL_SCANNED_BAK);
                foreach (DataRow row in dtList.Rows)
                {
                    //MessageBox.Show("COL_INTERNAL_REFERENCE: " + row[COL_INTERNAL_REFERENCE]);
                    //MessageBox.Show("productName: " + row["productName"]);
                    if (row[COL_INTERNAL_REFERENCE] == null || row[COL_INTERNAL_REFERENCE].ToString().Length == 0)
                        row[COL_INTERNAL_REFERENCE] = row["productName"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Init table error: " + ex.Message.ToString());
                loadingStop();
                return false;
            }

            try
            {
                DataGridInitUpdater dataGridInitUpdater = DataGridInitValue;
                this.dgCuonList.Invoke(dataGridInitUpdater, dtList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Set value table error: " + ex.Message.ToString());
                loadingStop();
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
                        case COL_INTERNAL_REFERENCE:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Product";
                                tbcName.Width = 70;
                            } break;
                        case COL_PACKID:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Package";
                                tbcName.Width = 60;
                            } break;
                        case COL_UID:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "UID";
                                tbcName.Width = 60;
                            } break;
                        case COL_RESERVED:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Reserved";
                                tbcName.Width = 30;
                            } break;
                        case COL_DONE:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Done";
                                tbcName.Width = 30;
                            } break;
                        case COL_SCANNED:
                            {
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Status";
                                tbcName.Width = 30;
                            } break;
                        default:
                            {
                                tbcName.Width = 20;
                            } break;
                    }
                    tableStyle.GridColumnStyles.Add(tbcName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Init style table error: " + ex.Message.ToString());
                loadingStop();
                return false;
            }

            try
            {
                DataGridAddStyleUpdater dataGridAddStyleUpdater = DataGridAddStyle;
                this.dgCuonList.Invoke(dataGridAddStyleUpdater, tableStyle);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Set table style error: " + ex.Message.ToString());
                loadingStop();
                return false;
            }

            try
            {
                ControlEnableUpdater controlEnableUpdater = ControlEnable;
                this.btnSave.Invoke(controlEnableUpdater, btnSave, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Enable button reset error: " + ex.Message.ToString());
                loadingStop();
                return false;
            }

            loadingStop();
            return true;
        }

        private void TransferDetails_Closing(object sender, CancelEventArgs e)
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

        #region Defined delegate function

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
        private void ControlEnable(Control uiControl, bool enable)
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

        #endregion

        // --------------------------CLEAR & CLICK-------------------------------

        private void clearListScan()
        {
            foreach (TransferDetail item in ListTransferDetail)
            {
                if (item.scanned == "X" || item.scanned_bak == "X" || item.scanned == "N" || item.scanned_bak == "N")
                {
                    item.scanned = "";
                    item.scanned_bak = "";
                }
            }
            foreach (DataRow dr in this.dtList.Rows)
            {
                if (dr[COL_SCANNED] == "X" || dr[COL_SCANNED_BAK] == "X" || dr[COL_SCANNED] == "N" || dr[COL_SCANNED_BAK] == "N")
                {
                    dr[COL_SCANNED] = "";
                    dr[COL_SCANNED_BAK] = "";

                    calculateQuantity(dr, false);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearListScan();
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

                DataGridCell row = dgCuonList.CurrentCell;
                if (row.ColumnNumber == index_column_scan)
                {
                    if (dtList.Rows[row.RowNumber][COL_SCANNED].ToString().Length > 0 && dtList.Rows[row.RowNumber][COL_SCANNED].ToString() != "OK")
                    {
                        dtList.Rows[row.RowNumber][COL_SCANNED] = "";
                        this.listTransferDetail[row.RowNumber].scanned = "";

                        //TransferItem result = this.listTransferItem.Find(x => x.productId == Convert.ToInt32(dtList.Rows[row.RowNumber][COL_PRODUCTID]));
                        //result.doneQuantity -= Convert.ToDouble(dtList.Rows[row.RowNumber][COL_RESERVED]);

                        calculateQuantity(dtList.Rows[row.RowNumber],false);
                    }
                    else
                    {
                        dtList.Rows[row.RowNumber][COL_SCANNED] = dtList.Rows[row.RowNumber][COL_SCANNED_BAK];
                        this.listTransferDetail[row.RowNumber].scanned = this.listTransferDetail[row.RowNumber].scanned_bak;

                        calculateQuantity(dtList.Rows[row.RowNumber], true);
                    }
                    dgCuonList.Refresh();
                }
            }
            catch { }
        }

        private void calculateQuantity(DataRow dr, bool plus)
        {
            double _quantity = Convert.ToDouble(dr[COL_RESERVED]) > 0 ? Convert.ToDouble(dr[COL_RESERVED]) : Convert.ToDouble(dr[COL_DONE]);
            try
            {
                if (dr[COL_MANID] != null && Convert.ToString(dr[COL_MANID]).Length > 0 &&
                    dr[COL_MANPN] != null && Convert.ToString(dr[COL_MANPN]).Length > 0)
                {
                    TransferItem result = this.listTransferItem.Find(x => (x.productId == Convert.ToInt32(dr[COL_PRODUCTID]) && x.manId == Convert.ToInt32(dr[COL_MANID]) && x.manPn == dr[COL_MANPN]));
                    if (result != null)
                    {
                        if (plus)
                            result.doneQuantity += _quantity;
                        else result.doneQuantity -= _quantity;
                    }
                }
                else if (dr[COL_MANID] != null && Convert.ToString(dr[COL_MANID]).Length > 0)
                {
                    TransferItem result = this.listTransferItem.Find(x => (x.productId == Convert.ToInt32(dr[COL_PRODUCTID]) && x.manId == Convert.ToInt32(dr[COL_MANID])));
                    if (result != null)
                    {
                        if (plus)
                            result.doneQuantity += _quantity;
                        else result.doneQuantity -= _quantity;
                    }
                }
                else
                {
                    TransferItem result = this.listTransferItem.Find(x => x.productId == Convert.ToInt32(dr[COL_PRODUCTID]));
                    if (result != null)
                    {
                        if (plus)
                            result.doneQuantity += _quantity;
                        else result.doneQuantity -= _quantity;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Calculate orver quantity error: " + ex.Message.ToString()); }
        }

        private void calculateQuantity(int productId, double quantity, int? manId, string manPn, bool plus)
        {
            TransferItem result;
            try
            {
                if (manId != null && manPn != null)
                {
                    result = this.listTransferItem.Find(x => (x.productId == productId) && (x.manId == manId) && (x.manPn == manPn));
                    if (result != null)
                    {
                        if (plus)
                            result.doneQuantity += quantity;
                        else result.doneQuantity -= quantity;
                    }
                }
                else if (manId != null)
                {
                    result = this.listTransferItem.Find(x => (x.productId == productId) && (x.manId == manId));
                    if (result != null)
                    {
                        if (plus)
                            result.doneQuantity += quantity;
                        else result.doneQuantity -= quantity;
                    }
                }
                else
                {
                    result = this.listTransferItem.Find(x => x.productId == productId);
                    if (result != null && result.manId == null)
                    {
                        if (plus)
                            result.doneQuantity += quantity;
                        else result.doneQuantity -= quantity;
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show("Calculate orver quantity error: " + ex.Message.ToString()); }
        }

        // --------------------------SCAN----------------------------------------

        private bool checkReservedInOtherList(string _packageId, bool _isPackage)
        {
            if (this.TypeTransfer != 1) return false;
            string url = "";
            if (_isPackage)
                url = "transfer-details/search?query=srcPackageNumber=='" + _packageId + "';state!='done';transferId!=" + TransferInfo.id.ToString() + ";lotId==null";
            else
                url = "transfer-details/search?query=traceNumber=='" + _packageId + "';state!='done';transferId!=" + TransferInfo.id.ToString();
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

        private int checkOverQuantity(DataRow dr, bool alert)
        {
            // 0: không quá sl
            // 1: quá sl và đồng ý add package
            // 2: quá sl và bỏ qua package
            int resultCheck = 0;
            try
            {
                if (dr[COL_MANID] != null && Convert.ToString(dr[COL_MANID]).Length > 0 &&
                    dr[COL_MANPN] != null && Convert.ToString(dr[COL_MANPN]).Length > 0)
                {
                    TransferItem result = this.listTransferItem.Find(x => (x.productId == Convert.ToInt32(dr[COL_PRODUCTID]) && x.manId == Convert.ToInt32(dr[COL_MANID]) && x.manPn == dr[COL_MANPN]));
                    if (result != null)
                    {
                        result.doneQuantity += Convert.ToDouble(dr[COL_RESERVED]);
                        if (result.initialQuantity < result.doneQuantity && alert)
                        {
                            DialogResult mgb = new DialogResult();
                            mgb = MessageBox.Show("Product " + result.internalReference + " (ManPn " + result.manPn + "): Scanned quantity greater than initial quantity !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb != DialogResult.Yes)
                                resultCheck = 2;
                            else resultCheck = 1;
                        }
                    }
                }
                else if (dr[COL_MANID] != null && Convert.ToString(dr[COL_MANID]).Length > 0)
                {
                    TransferItem result = this.listTransferItem.Find(x => (x.productId == Convert.ToInt32(dr[COL_PRODUCTID]) && x.manId == Convert.ToInt32(dr[COL_MANID])));
                    if (result != null)
                    {
                        result.doneQuantity += Convert.ToDouble(dr[COL_RESERVED]);
                        if (result.initialQuantity < result.doneQuantity && alert)
                        {
                            DialogResult mgb = new DialogResult();
                            mgb = MessageBox.Show("Product " + result.internalReference + ": Scanned quantity greater than initial quantity !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb != DialogResult.Yes)
                                resultCheck = 2;
                            else resultCheck = 1;
                        }
                    }
                }
                else
                {
                    TransferItem result = this.listTransferItem.Find(x => x.productId == Convert.ToInt32(dr[COL_PRODUCTID]));
                    if (result != null)
                    {
                        result.doneQuantity += Convert.ToDouble(dr[COL_RESERVED]);
                        if (result.initialQuantity < result.doneQuantity && alert)
                        {
                            DialogResult mgb = new DialogResult();
                            mgb = MessageBox.Show("Product " + result.productName + ": Scanned quantity greater than initial quantity !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb != DialogResult.Yes)
                                resultCheck = 2;
                            else resultCheck = 1;
                        }
                    }
                }
            }
            catch { }
            return resultCheck;
        }

        private bool checkOverQuantity(int productId, double quantity, int? manId, string manPn)
        {
            bool checkOver = false;
            TransferItem result;
            try
            {
                if (manId != null && manPn != null)
                {
                    result = this.listTransferItem.Find(x => (x.productId == productId) && (x.manId == manId) && (x.manPn == manPn));
                    if (result != null)
                    {
                        double total_done = result.doneQuantity;
                        total_done += quantity;
                        if (result.initialQuantity < total_done)
                        {
                            DialogResult mgb = new DialogResult();
                            mgb = MessageBox.Show("Product " + result.internalReference + " (ManPn " + result.manPn + "): Scanned quantity greater than initial quantity !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb != DialogResult.Yes)
                            {
                                checkOver = true;
                                return checkOver;
                            }
                        }
                    }
                }
                else if (manId != null)
                {
                    result = this.listTransferItem.Find(x => (x.productId == productId) && (x.manId == manId));
                    if (result != null)
                    {
                        double total_done = result.doneQuantity;
                        total_done += quantity;
                        if (result.initialQuantity < total_done)
                        {
                            DialogResult mgb = new DialogResult();
                            mgb = MessageBox.Show("Product " + result.internalReference + ": Scanned quantity greater than initial quantity !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb != DialogResult.Yes)
                            {
                                checkOver = true;
                                return checkOver;
                            }
                        }
                    }
                }
                else
                {
                    result = this.listTransferItem.Find(x => x.productId == productId);
                    if (result != null && result.manId == null)
                    {
                        double total_done = result.doneQuantity;
                        total_done += quantity;
                        if (result.initialQuantity < total_done)
                        {
                            DialogResult mgb = new DialogResult();
                            mgb = MessageBox.Show("Product " + result.productName + ": Scanned quantity greater than initial quantity !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb != DialogResult.Yes)
                            {
                                checkOver = true;
                                return checkOver;
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex) { MessageBox.Show("Check orver quantity error: " + ex.Message.ToString()); return false; }
            return checkOver;
        }

        private bool checkOverQuantity(List<Quant> ListQuant)
        {
            bool checkOver = false;
            TransferItem result;
            //List<TransferItem> listTransferItemSpace = new List<TransferItem>();
            //foreach (TransferItem item in this.listTransferItem)
            //{
            //    listTransferItemSpace.Add(item);
            //}

            //MessageBox.Show(listTransferItemSpace[0].doneQuantity.ToString());
            //MessageBox.Show(listTransferItem[0].doneQuantity.ToString());

            try
            {
                foreach (Quant item in ListQuant)
                {
                    if (item.manId != null && item.manPn != null)
                    {
                        result = this.listTransferItem.Find(x => (x.productId == item.productId) && (x.manId == item.manId) && (x.manPn == item.manPn));
                        if (result != null)
                        {
                            result.doneQuantity += item.onHand;
                            if (result.initialQuantity < result.doneQuantity)
                            {
                                DialogResult mgb = new DialogResult();
                                mgb = MessageBox.Show("Product " + result.internalReference + " (ManPn " + result.manPn + "): Scanned quantity greater than initial quantity !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                if (mgb != DialogResult.Yes)
                                {
                                    return true;
                                }
                                else return false;
                            }
                        }
                    }
                    else if (item.manId != null)
                    {
                        result = this.listTransferItem.Find(x => (x.productId == item.productId) && (x.manId == item.manId));
                        if (result != null)
                        {
                            result.doneQuantity += item.onHand;
                            if (result.initialQuantity < result.doneQuantity)
                            {
                                DialogResult mgb = new DialogResult();
                                mgb = MessageBox.Show("Product " + result.internalReference + ": Scanned quantity greater than initial quantity !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                if (mgb != DialogResult.Yes)
                                {
                                    return true;
                                }
                                else return false;
                            }
                        }
                    }
                    else
                    {
                        result = this.listTransferItem.Find(x => x.productId == item.productId);
                        if (result != null && result.manId == null)
                        {
                            result.doneQuantity += item.onHand;
                            if (result.initialQuantity < result.doneQuantity)
                            {
                                DialogResult mgb = new DialogResult();
                                mgb = MessageBox.Show("Product " + result.productName + ": Scanned quantity greater than initial quantity !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                if (mgb != DialogResult.Yes)
                                {
                                    return true;
                                }
                                else return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Check orver quantity error: " + ex.Message.ToString()); return false; }
            return checkOver;
        }

        private void checkScan(string productName, string ScanId, string type)
        {
            if (this.locationInfo == null && this.TypeTransfer == 0)
            {
                MessageBox.Show("Request scan location before scan the package!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            bool isPackage = true;
            if (Util.getTypePackage(ScanId, type) % 2 == 0) isPackage = false;
            if (!isPackage)
            {
                if (checkReservedInOtherList(ScanId, isPackage))
                {
                    MessageBox.Show("Package/Uid already exists in another reserved list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }
            }

            //DataRow[] rs_product = null; 
            DataRow[] rs_package = null;
            bool _exists_product = false; bool _exists_package = false;

            //check exist product
            if (productName != null)
            {
                string productSearch = productName;
                try
                {
                    if (productName.Substring(productName.Length - 2, 2) == "NA") productSearch = productName.Substring(0, productName.Length - 2);
                }
                catch (Exception ex) { MessageBox.Show("Product search subtring error: " + ex.Message.ToString()); }
                //rs_product = dtList.Select(COL_INTERNAL_REFERENCE + " = '" + productSearch + "'");
                //if (rs_product.Length > 0)
                //{
                //    _exists_product = true;
                //}

                List<TransferItem> listTransferItemSearch = this.listTransferItem;
                foreach(TransferItem item in listTransferItemSearch) 
                {
                    if(item.internalReference == null) item.internalReference = item.productName;
                }

                TransferItem result = listTransferItemSearch.Find(x => x.internalReference == productSearch);
                if (result != null)
                {
                    _exists_product = true;
                }
            }
            else _exists_product = true;

            //check exists package
            if (isPackage)
            {
                rs_package = dtList.Select(COL_PACKID + " = '" + ScanId + "'");
                if (rs_package.Length > 0)
                {
                    _exists_package = true;
                }
            }
            else
            {
                rs_package = dtList.Select(COL_UID + " = '" + ScanId + "'");
                if (rs_package.Length > 0)
                {
                    _exists_package = true;
                }
            }

            DialogResult mgb = new DialogResult();
            if (!_exists_product)
            {
                mgb = MessageBox.Show("Product not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (mgb == DialogResult.Yes)
                {
                    addPackageCheckProduct(productName, ScanId, type);
                }
            }
            else if (!_exists_package)
            {
                mgb = MessageBox.Show("Package/uid not exists in list !", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (mgb == DialogResult.Yes)
                {
                    addPackageCheckProduct(productName, ScanId, type);
                }
            }
            else
            {
                if (!isPackage)
                {
                    DataRow dr = rs_package[0];
                    if (Convert.ToInt32(dr[COL_ID]) > 0)
                    {
                        if (dr[COL_SCANNED] == "OK")
                        {
                            mgb = MessageBox.Show("Package/Unit ID was already scanned! Are you sure want to scan again?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            if (mgb != DialogResult.Yes) return;
                        }
                        else if (dr[COL_SCANNED] == "X")
                            MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        else
                        {
                            int checkOver = checkOverQuantity(dr, true);
                            if (checkOver == 2) { calculateQuantity(dr, false); return; }
                        }

                        dr[COL_DONE] = dr[COL_RESERVED];
                        dr[COL_SCANNED] = "X";
                        dr[COL_SCANNED_BAK] = "X";
                        dr["state"] = "available";

                        foreach (TransferDetail item in listTransferDetail)
                        {
                            if (item.traceNumber == ScanId)
                            {
                                item.doneQuantity = item.reserved;
                                item.scanned = "X";
                                item.scanned_bak = "X";
                                item.state = "available";
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (dr[COL_SCANNED] == "N")
                            MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        else
                        {
                            int checkOver = checkOverQuantity(dr, true);
                            if (checkOver == 2) { calculateQuantity(dr, false); return; }
                        }

                        dr[COL_SCANNED] = "N";
                        dr[COL_SCANNED_BAK] = "N";

                        foreach (TransferDetail item in listTransferDetail)
                        {
                            if (item.traceNumber == ScanId)
                            {
                                item.scanned = "N";
                                item.scanned_bak = "N";
                                item.state = "available";
                                break;
                            }
                        }
                    }

                    int index = dtList.Rows.IndexOf(dr);
                    this.dgCuonList.CurrentRowIndex = index;
                }
                else
                {
                    bool exported = true;
                    foreach (DataRow dr in rs_package)
                    {
                        if (dr[COL_SCANNED] == "" || dr[COL_SCANNED].ToString().Length == 0)
                        {
                            exported = false;
                            break;
                        }
                    }
                    if (exported)
                    {
                        DialogResult mgb_export = new DialogResult();
                        mgb_export = MessageBox.Show("Package " + Convert.ToString(rs_package[0][COL_PACKID]) + " was scanned. Are you sure want to scan again?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        if (mgb_export != DialogResult.Yes) return;
                    }

                    //if (rs_package[0][COL_SCANNED] == "X" || rs_package[0][COL_SCANNED] == "N" || rs_package[0][COL_SCANNED] == "OK")
                    //{
                    //    MessageBox.Show("Package/Unit ID was already scanned !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    //}

                    bool alert = true; int result = 0;
                    foreach (DataRow dr in rs_package)
                    {
                        if (Convert.ToString(dr[COL_SCANNED]).Length == 0 || dr[COL_SCANNED] == "")
                        {
                            //MessageBox.Show(Convert.ToString(dr[COL_SCANNED]));
                            result = checkOverQuantity(dr, alert);
                            if (result == 1) alert = false; else if (result == 2) break;
                        }

                        //if (dr[COL_SCANNED] != "OK")
                        //{
                            if (Convert.ToInt32(dr[COL_ID]) > 0)
                            {
                                dr[COL_SCANNED] = "X";
                                dr[COL_SCANNED_BAK] = "X";
                                dr[COL_DONE] = dr[COL_RESERVED];
                            }
                            else
                            {
                                dr[COL_SCANNED] = "N";
                                dr[COL_SCANNED_BAK] = "N";
                            }
                            dr["state"] = "available";
                        //}
                    }

                    if (result == 2)
                    {
                        foreach (DataRow dr in rs_package)
                        {
                            if (dr[COL_SCANNED] == "X" || dr[COL_SCANNED] == "N")
                            {
                                calculateQuantity(dr, false);
                                dr[COL_SCANNED] = "";
                                dr[COL_SCANNED_BAK] = "";
                            }
                        }
                        return;
                    }

                    foreach (TransferDetail item in listTransferDetail)
                    {
                        if (item.destPackageNumber == ScanId)
                        {
                            if (item.id != null)
                            {
                                item.doneQuantity = item.reserved;
                                item.scanned = "X";
                                item.scanned_bak = "X";
                            }
                            else
                            {
                                item.scanned = "N";
                                item.scanned_bak = "N";
                            }
                            item.state = "available";
                        }
                    }

                    int index = dtList.Rows.IndexOf(rs_package[0]);
                    this.dgCuonList.CurrentRowIndex = index;
                }
            }
        }

        private void addPackageCheckProduct(string _productName, string _packageId, string type)
        {
            bool isPackage = true;
            bool isOverQuantity = false;
            List<Quant> ListQuant = new List<Quant>();
            DialogResult mgb = new DialogResult();

            try
            {
                if (Util.getTypePackage(_packageId.Trim(), type) % 2 == 0)
                {
                    //StockQuants StockUid = StockQuantBusiness.getStockUid(_packageId);
                    //if (StockUid != null) _quantity = StockUid.onHand;
                    Lots lot = LotBusiness.getInfo(_packageId);
                    if (lot == null)
                    {
                        mgb = MessageBox.Show("Uid scanned is not exists !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    Lots lotInfo = LotBusiness.getFullInfo(lot.id);
                    //Check uid location exists in source location transfer ?
                    if (!lotInfo.locationName.Contains(this.TransferInfo.srcLocationName))
                    {
                        mgb = MessageBox.Show("Uid location is not exists in source location transfer !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (lotInfo.quants == null)
                    {
                        mgb = MessageBox.Show("Uid scanned is not exists in inventory!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        //if (mgb != DialogResult.Yes) return;
                        //addPackageToList(null, _productName, null, _packageId, 0, null, null);
                        return;
                    }
                    else
                    {
                        isOverQuantity = checkOverQuantity(lotInfo.quants[0].productId, lotInfo.quants[0].onHand, lotInfo.quants[0].manId, lotInfo.quants[0].manPn);
                        if (!isOverQuantity)
                        {
                            calculateQuantity(lotInfo.quants[0].productId, lotInfo.quants[0].onHand, lotInfo.quants[0].manId, lotInfo.quants[0].manPn, true);
                        }
                        else return;

                        ListQuant.Add(lotInfo.quants[0]);
                    }
                    isPackage = false;
                }
                else
                {
                    //_quantity = StockQuantBusiness.getStockQuantPackage(_packageId);
                    Packages package = PackageBusiness.getInfo(_packageId);
                    if (package == null)
                    {
                        mgb = MessageBox.Show("Package scanned is not exists !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    Packages packageInfo = PackageBusiness.getFullInfo(package.id);
                    //Check package location exists in source location transfer ?
                    if (!packageInfo.locationName.Contains(this.TransferInfo.srcLocationName))
                    {
                        mgb = MessageBox.Show("Package location is not exists in source location transfer !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    if (packageInfo.quants == null)
                    {
                        mgb = MessageBox.Show("Package scanned is not exists in inventory!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        //if (mgb != DialogResult.Yes) return;
                        //addPackageToList(null, _productName, _packageId, null, 0, null, null);
                        return;
                    }
                    else
                    {
                        foreach (TransferItem item in this.listTransferItem)
                        {
                            item.doneQuantityBackup = item.doneQuantity;
                        }

                        isOverQuantity = checkOverQuantity(packageInfo.quants);

                        foreach (TransferItem item in this.listTransferItem)
                        {
                            item.doneQuantity = item.doneQuantityBackup;
                        }

                        //MessageBox.Show(listTransferItem[0].doneQuantity.ToString());

                        //check over quantity with package/lot is not in list reserved
                        if (!isOverQuantity)
                        {
                            foreach (Quant item in packageInfo.quants)
                            {
                                calculateQuantity(item.productId, item.onHand, item.manId, item.manPn, true);
                            }
                        }
                        else return;

                        ListQuant = packageInfo.quants;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get full info & Check quantity Error: " + ex.Message.ToString(), "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            try
            {
                if (!isPackage)
                {
                    addPackageToList(ListQuant[0].productId, _productName, null, _packageId, ListQuant[0].onHand, ListQuant[0].manId, ListQuant[0].manPn, false);
                }
                else if (ListQuant.Count == 1 && ListQuant[0].lotId == null)
                {
                    //check reserved in other list
                    if (checkReservedInOtherList(_packageId, isPackage))
                    {
                        MessageBox.Show("Package/Uid already exists in another reserved list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        return;
                    }

                    addPackageToList(ListQuant[0].productId, _productName, _packageId, null, ListQuant[0].onHand, ListQuant[0].manId, ListQuant[0].manPn, false);
                }
                else
                {
                    foreach (Quant item in ListQuant)
                    {
                        if (item.lotId != null)
                        {
                            Lots Lot = LotBusiness.getFullInfo(Convert.ToInt32(item.lotId));

                            //check reserved in other list
                            if (checkReservedInOtherList(Lot.lotNumber, false))
                            {
                                MessageBox.Show("Uid " + Lot.lotNumber + " in package " + _packageId + " already exists in another reserved list!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            }
                            else
                            {
                                addPackageToList(item.productId, Lot.internalReference, _packageId, Lot.lotNumber, item.onHand, item.manId, item.manPn, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add package to list: " + ex.Message.ToString(), "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
        }

        private void addPackageToList(int? _productId, string _productName, string _packageId, string _uid, double _quantity, int? _manId, string _manPn, bool _add)
        {
            try
            {
                if (_uid != null && _uid.Length > 0)
                {
                    DataRow[] rs_package = null;
                    rs_package = dtList.Select(COL_UID + " = '" + _uid.Trim() + "'");
                    if (rs_package.Length > 0)
                    {
                        if (_add)
                        {
                            rs_package[0][COL_SCANNED] = rs_package[0][COL_SCANNED_BAK];
                        }
                        else return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check exists uid error: " + ex.Message.ToString());
            }

            try
            {
                DataRow dr = this.dtList.NewRow();
                dr[COL_ID] = -1;
                dr[COL_PACKID] = _packageId;
                dr[COL_UID] = _uid;
                dr[COL_INTERNAL_REFERENCE] = _productName;
                dr[COL_SCANNED] = "N";
                dr[COL_SCANNED_BAK] = "N";
                //gán thêm số lượng
                dr[COL_RESERVED] = _quantity;
                dr[COL_DONE] = _quantity;
                dr[COL_PRODUCTID] = _productId;
                //lấy thông tin manId, manPn
                dr[COL_MANID] = _manId;
                dr[COL_MANPN] = _manPn;

                this.dtList.Rows.Add(dr);
                this.dgCuonList.DataSource = this.dtList;
                this.dgCuonList.Refresh();

                TransferDetail newItem = new TransferDetail();
                
                if(this.TypeTransfer ==1)
                    newItem.srcPackageNumber = _packageId;
                else
                    newItem.destPackageNumber = _packageId;

                newItem.traceNumber = _uid;
                newItem.internalReference = _productName;
                newItem.scanned = "N";
                newItem.scanned_bak = "N";
                this.ListTransferDetail.Add(newItem);

                this.dgCuonList.CurrentRowIndex = this.dtList.Rows.Count - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add package new error: " + ex.ToString());
            }
        }

        private void ScanCode(string dcdData)
        {
            if (dcdData.StartsWith("["))
            {
                #region scan package/uid new

                LabelPackage labelPackage = new LabelPackage(dcdData.Trim());

                if (labelPackage.PackageId != null && labelPackage.PackageId != "")
                {
                    checkScan(labelPackage.ProductName, labelPackage.PackageId, null);
                }
                else
                    MessageBox.Show("Format label is incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                #endregion
            }
            else if (dcdData.Trim().StartsWith("PN:"))
            {
                #region scan package/uid old

                TemCuon temCuon = new TemCuon(dcdData.Trim());

                if (temCuon.IdCuon != null && temCuon.IdCuon != "")
                {
                    checkScan(temCuon.VnptPn, temCuon.IdCuon, temCuon.Type);
                }
                else
                    MessageBox.Show("Format label is incorrect!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                
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
                        if (ListLocations.Count > 0)
                        {
                            if (this.locationInfo != null)
                            {
                                DialogResult mgb = new DialogResult();
                                mgb = MessageBox.Show("Are you sure you want to update new scan location?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                if (mgb != DialogResult.Yes) return;
                            }

                            this.locationInfo = ListLocations[0];
                            this.lblScanValue.Text = this.locationInfo.name;
                        }
                        else
                        {
                            this.locationInfo = null;
                            this.lblScanValue.Text = null;
                            MessageBox.Show("Location is not exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.locationInfo = null;
                        this.lblScanValue.Text = null;
                        MessageBox.Show(ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    }
                }
                #endregion
            }
        }

        // --------------------------SAVE-----------------------------------------

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.TransferInfo.state == "done")
            {
                MessageBox.Show("The selected transaction has completed, actions are not allowed !");
                return;
            }
            //check location null
            if (this.locationInfo == null && this.TypeTransfer == 0)
            {
                MessageBox.Show("Request scan location before scan the package!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }

            try
            {
                this.lblLoading.Visible = true;
                this.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Enable loading error: " + ex.Message.ToString());
            }

            List<TransferDetail> ListPut = new List<TransferDetail>();
            try
            {
                //lấy các phần tử khác X N
                foreach (TransferDetail item in this.ListTransferDetail)
                {
                    if (item.scanned == "X" || item.scanned == "N")
                    {
                        ListPut.Add(item);
                    }
                }
                //Gán vị trí nếu là phần nhập
                if (this.TypeTransfer == 0)
                {
                    foreach (TransferDetail item in ListPut)
                    {
                        item.destLocationId = locationInfo.id;
                        item.destLocationName = locationInfo.name;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Prepare put error: " + ex.Message.ToString());
            }

            string param = "";
            try
            {
                this.TransferInfo.transferDetails = ListPut;
                param = JsonConvert.SerializeObject(TransferInfo);
                param = param.Replace(System.Environment.NewLine, "").Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Client error: " + ex.Message.ToString());
                return;
            }

            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "transfers/" + _transferId.ToString();

            //Util.Logs(param);

            try
            {
                res = HTTP.Put(url, param);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Server error: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            if (res.Status)
            {
                MessageBox.Show("Success !");
                this._closeStatus = true;
                this.Close();

                this.lblLoading.Visible = false;
                this.Refresh();
            }
            else
            {
                this.lblLoading.Visible = false;
                this.Refresh();
                MessageBox.Show(res.Message.ToString());
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}