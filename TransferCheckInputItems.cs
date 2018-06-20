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
using ERP.Base;

namespace ERP
{
    public partial class TransferCheckInputItems : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;

        private DashBoardTransfer _frmDashBoardTransfer;
        public DashBoardTransfer FrmDashBoardTransfer
        {
            get { return _frmDashBoardTransfer; }
            set { _frmDashBoardTransfer = value; }
        }

        #region Properties
        private const string COL_INTERNAL_REFERENCE = "internalReference";
        private const string COL_MAN_PN = "manPn";
        private const string COL_INITIAL_QUANTITY = "initialQuantity";
        private const string COL_DONE_QUANTITY = "doneQuantity";

        //private const string PREFIX_LOT = "UID";
        //private const string PREFIX_PACK = "PACK";

        private TransferInfo TransferInfo;
        private int _transferId = 0;
        private int _transferIdRemove = 0;
        private int _typeTransfer = 0;
        private LocationInfo locationInfo;

        public int TransferId
        {
            get { return _transferId; }
            set { this._transferId = value; }
        }

        public int TransferIdRemove
        {
            get { return _transferIdRemove; }
            set { this._transferIdRemove = value; }
        }

        public int TypeTransfer
        {
            get { return _typeTransfer; }
            set { this._typeTransfer = value; }
        }

        private DataTable dtList = new DataTable();
        private List<TransferItem> ListTransferItem = new List<TransferItem>();        
        #endregion

        public TransferCheckInputItems()
        {
            InitializeComponent();

            //Initialize event
            initEventScan();

            dtList.Rows.Clear();
        }

        private void dcdEvent_Scanned(object sender, DecodeEventArgs e)
        {
            CodeId cID = CodeId.NoData;
            string dcdData = string.Empty;
            dcdData = hDcd.ReadString(e.RequestID, ref cID);

            ScanCode(dcdData);
        }

        private void initEventScan()
        {
            try
            {
                hDcd = new DecodeHandle(DecodeDeviceCap.Exists | DecodeDeviceCap.Barcode);
                DecodeRequest reqType = (DecodeRequest)1 | DecodeRequest.PostRecurring;
                dcdEvent = new DecodeEvent(hDcd, reqType, this);
                dcdEvent.Scanned += new DecodeScanned(dcdEvent_Scanned);
            }
            catch { }
        }

        private void closeEventScan()
        {
            if (dcdEvent.IsListening)
            {
                dcdEvent.StopScanListener();
            }

            if (hDcd != null)
            {
                hDcd.Dispose();
            }
        }

        private void TransferCheckInputItems_Load(object sender, EventArgs e)
        {
            int _idBack = this._transferId;
            if (!getTransferById()) { this._transferIdRemove = _idBack; closeEventScan(); this.Close(); }
        }

        private void TransferCheckInputItems_Closing(object sender, CancelEventArgs e)
        {
            closeEventScan();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clearData(true);
        }

        private bool getListTransferItem(int transferId)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;

            try
            {
                string url = "transfer-items/search?query=transferId==" + transferId.ToString() + ";((optionOf!=null),(optionOf==null;level==null))&size=150";
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
                    this.ListTransferItem = JsonConvert.DeserializeObject<List<TransferItem>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    dtList = Util.ToDataTable<TransferItem>(ListTransferItem);
                    this.dtList = buildTable(dtList);
                    foreach (DataRow dr in this.dtList.Rows)
                    {
                        if (dr[COL_INTERNAL_REFERENCE] == null || dr[COL_INTERNAL_REFERENCE].ToString().Length == 0)
                            dr[COL_INTERNAL_REFERENCE] = dr["productName"].ToString();
                    }

                    loadList(dgList, dtList, Color.White);
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

        private void ScanCode(string dcdData)
        {
            #region scan transfer
            try
            {
                string _transferNumber = dcdData.Trim();
                if (_transferNumber.Length > 0)
                {
                    clearData(true);
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
                        return;
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
                                if (!getListTransferItem(_transferId)) { clearData(true); return; }
                                txtTransferNumber.Text = dcdData.Trim();
                            }
                            else
                            {
                                clearData(true);
                                MessageBox.Show("Not exists transfer number !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            }

                        }
                        catch (Exception ex)
                        {
                            clearData(true);
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                    else
                    {
                        clearData(true);
                        MessageBox.Show(res.RawText);
                    }
                }
                else
                {
                    clearData(true);
                }
            }
            catch (Exception ex)
            {
                clearData(true);
                MessageBox.Show("Transfer number wrong format !");
            }
            #endregion
        }

        private void clearData(bool isScan)
        {
            if (isScan) this.txtTransferNumber.Text = null;
            _transferId = 0;
            this.TransferInfo = null;
            dtList.Rows.Clear();
            ListTransferItem.Clear();
        }

        private void txtTransferNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                clearData(false);

                string _transferNumber = txtTransferNumber.Text.Trim();
                ApiResponse res = new ApiResponse();
                
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

                            if (!getListTransferItem(_transferId)) { clearData(false); return; }
                            alertStateTransfer(TransferInfo.state);
                        }
                        else
                        {
                            clearData(false);
                            MessageBox.Show("Not exists transfer number !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        }
                    }
                    else
                    {
                        clearData(false);
                        MessageBox.Show("res.RawText: " + res.RawText);
                    }
                }
                catch (Exception ex)
                {
                    clearData(false);
                    MessageBox.Show("Load data error: " + ex.Message.ToString());
                }
            }
        }

        private bool getTransferById()
        {
            if (_transferId > 0)
            {
                ApiResponse res = new ApiResponse();
                res.Status = false;

                try
                {
                    string url = "transfers/search?query=id==" + _transferId.ToString();
                    res = HTTP.GetJson(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Get transfer error: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return false;
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
                            if (!getListTransferItem(_transferId)) { clearData(true); return false; }
                            txtTransferNumber.Text = TransferInfo.transferNumber;
                            //alertStateTransfer(TransferInfo.state);
                        }
                        else
                        {
                            clearData(true);
                            MessageBox.Show("Not exists transfer number !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        clearData(true);
                        MessageBox.Show(ex.Message.ToString());
                        return false;
                    }
                }
                else
                {
                    clearData(true);
                    MessageBox.Show(res.RawText);
                    return false;
                }
                return true;
            }
            else return false;
        }

        private void alertStateTransfer(string state)
        {
            switch (state)
            {
                case "done":
                    //do something
                    MessageBox.Show("The transfer has been completed", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    break;
                case "draft":
                    //do something else
                    break;
                case "waiting":
                    //do something else
                    MessageBox.Show("The transfer is waiting for the material", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    break;
                case "waiting_other":
                    //do something else
                    MessageBox.Show("The transfer is waiting for the material", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    break;
                case "ready":
                    //do something else
                    MessageBox.Show("The transfer is on the ready list", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    break;
                default:
                    //do a different thing
                    break;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void loadList(DataGrid dgList, DataTable dtList, Color backColor)
        {
            try
            {
                dgList.TableStyles.Clear();
                dgList.DataSource = dtList;
                DataGridTableStyle tableStyle = new DataGridTableStyle();
                tableStyle.MappingName = dtList.TableName;

                foreach (DataColumn item in dtList.Columns)
                {
                    switch (item.ColumnName)
                    {
                        case COL_INTERNAL_REFERENCE:
                            {
                                DataGridExtendedTextBoxColumn tbcName = new DataGridExtendedTextBoxColumn();
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Product";
                                tbcName.Width = 90;
                                tbcName.Format = "{0:0000}";
                                tableStyle.GridColumnStyles.Add(tbcName);
                            } break;
                        case COL_MAN_PN:
                            {
                                DataGridExtendedTextBoxColumn tbcName = new DataGridExtendedTextBoxColumn();
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "ManPN";
                                tbcName.Width = 70;
                                tbcName.Format = "{0:0000}";
                                tableStyle.GridColumnStyles.Add(tbcName);
                            } break;
                        case COL_INITIAL_QUANTITY:
                            {
                                DataGridExtendedTextBoxColumn tbcName = new DataGridExtendedTextBoxColumn();
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Initial";
                                tbcName.Width = 40;
                                tbcName.Format = "{0:0000}";
                                tableStyle.GridColumnStyles.Add(tbcName);
                            } break;
                        case COL_DONE_QUANTITY:
                            {
                                DataGridExtendedTextBoxColumn tbcName = new DataGridExtendedTextBoxColumn();
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Done";
                                tbcName.Width = 40;
                                tbcName.Format = "{0:0000}";
                                tableStyle.GridColumnStyles.Add(tbcName);
                            } break;
                        default:
                            {
                                //DataGridExtendedTextBoxColumn tbcName = new DataGridExtendedTextBoxColumn();
                                //tbcName.Width = 20;
                                //tableStyle.GridColumnStyles.Add(tbcName);
                            } break;
                    }
                }

                dgList.TableStyles.Add(tableStyle);
                dgList.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private DataTable buildTable(DataTable dtList)
        {
            DataColumnCollection columns = dtList.Columns;
            if (!columns.Contains(COL_INTERNAL_REFERENCE)) dtList.Columns.Add(COL_INTERNAL_REFERENCE);
            //if (!columns.Contains(COL_PRODUCT_MAN)) dtList.Columns.Add(COL_PRODUCT_MAN);
            if (!columns.Contains(COL_MAN_PN)) dtList.Columns.Add(COL_MAN_PN);
            if (!columns.Contains(COL_INITIAL_QUANTITY)) dtList.Columns.Add(COL_INITIAL_QUANTITY);
            if (!columns.Contains(COL_DONE_QUANTITY)) dtList.Columns.Add(COL_DONE_QUANTITY);
            return dtList;
        }
    }

}