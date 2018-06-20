using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Datalogic.API;
using ERP.Base;
using Newtonsoft.Json;

namespace ERP
{
    public partial class DashBoardTransfer : Form
    {
        private DecodeEvent dcdEvent;
        private DecodeHandle hDcd;
        TransferInfo _transferInfo;

        public DashBoardTransfer()
        {
            InitializeComponent();

            //Initialize event
            initEventScan();
        }

        private void dcdEvent_Scanned(object sender, DecodeEventArgs e)
        {
            CodeId cID = CodeId.NoData;
            string dcdData = string.Empty;
            dcdData = hDcd.ReadString(e.RequestID, ref cID);

            ScanCode(dcdData);
        }

        private void DashBoardTransfer_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void DashBoardTransfer_Closed(object sender, EventArgs e)
        {
            //closeEventScan();
            this.Close();
        }

        private bool ScanCode(string dcdData)
        {
            try
            {
                string barcode = dcdData.Trim();
                List<TransferInfo> listTransfer;
                try
                {
                    listTransfer = TransferBusiness.getListTransfer("search?query=transferNumber=='" + barcode + "'");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message.ToString()); return false; }

                if (listTransfer.Count == 0)
                {
                    MessageBox.Show("Transfer number is not exists");
                    return false;
                }
                else
                {
                    _transferInfo = listTransfer[0];
                    txtTransferNumber.Text = _transferInfo.transferNumber;
                    alertStateTransfer(_transferInfo.state);
                    txtTransferNumber.Focus();
                }
            }
            catch (Exception ex)
            {
                _transferInfo = null;
                MessageBox.Show(ex.ToString());
                return false;
            }
            return true;
        }

        #region Load data

        private int typeTransfer; // 0 : IN , 1 : OUT
        public int TypeTransfer
        {
            get { return typeTransfer; }
            set { this.typeTransfer = value; }
        }

        private DataTable dtListBot = new DataTable();
        private DataTable dtListTop = new DataTable();

        private const string COL_TRANSFER = "transferNumber";
        private const string COL_ID = "id";
        private const string COL_ACTION = "action";
        private const string COL_STATE = "state";

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
                        case COL_TRANSFER:
                            {
                                DataGridExtendedTextBoxColumn tbcName = new DataGridExtendedTextBoxColumn(backColor, Color.Black);
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Transfer number";
                                tbcName.Width = 143;
                                tbcName.Format = "{0:0000}";
                                tableStyle.GridColumnStyles.Add(tbcName);
                            } break;
                        case COL_ACTION:
                            {
                                DataGridExtendedTextBoxColumn tbcName = new DataGridExtendedTextBoxColumn();
                                tbcName.MappingName = item.ColumnName;
                                tbcName.HeaderText = "Action";
                                tbcName.Width = 50;
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
            if (!columns.Contains(COL_TRANSFER)) dtList.Columns.Add(COL_TRANSFER);
            if (!columns.Contains(COL_ACTION)) dtList.Columns.Add(COL_ACTION);
            if (!columns.Contains(COL_ID)) dtList.Columns.Add(COL_ID);
            if (!columns.Contains(COL_STATE)) dtList.Columns.Add(COL_STATE);

            foreach (DataRow dr in dtList.Rows)
            {
                dr[COL_ACTION] = "VIEW";
            }
            return dtList;
        }

        private void loadData()
        {
            string listOperation = getListOperations(this.typeTransfer);
            if (listOperation.Length == 0)
            {
                return;
            }

            if (this.typeTransfer == 0)
            {
                //this.dtListTop = getListTransfer("get-draft-not-finish");
                this.dtListTop = getListTransfer("search?query=operationTypeId=in=(" + listOperation + ");state=='draft'&size=15&sort=id,desc");
                this.dtListTop = buildTable(dtListTop);
                loadList(dgListTransferTop, dtListTop, Color.Red);

                //this.dtListBot = getListTransfer("get-draft-finish");
                this.dtListBot = getListTransfer("search?query=operationTypeId=in=(" + listOperation + ");state=='ready'&size=15&sort=id,desc");
                this.dtListBot = buildTable(dtListBot);
                loadList(dgListTransferBot, dtListBot, Color.Yellow);
            }
            else
            {
                this.dtListTop = getListTransfer("search?query=operationTypeId=in=(" + listOperation + ");state=='draft'&size=15&sort=id,desc");
                this.dtListBot = getListTransfer("search?query=operationTypeId=in=(" + listOperation + ");state=='waiting'&size=15&sort=id,desc");
                Color backColorTop = Color.Red;
                Color backColorBot = Color.Yellow;
                if (dtListBot.Rows.Count == 0)
                {
                    this.dtListBot = getListTransfer("search?query=operationTypeId=in=(" + listOperation + ");state=='ready'&size=15&sort=id,desc");
                    backColorBot = Color.AliceBlue;
                }
                else if (dtListTop.Rows.Count == 0)
                {
                    this.dtListTop = getListTransfer("search?query=operationTypeId=in=(" + listOperation + ");state=='ready'&size=15&sort=id,desc");
                    backColorTop = Color.AliceBlue;
                }
                this.dtListTop = buildTable(dtListTop);
                loadList(dgListTransferTop, dtListTop, backColorTop);
                this.dtListBot = buildTable(dtListBot);
                loadList(dgListTransferBot, dtListBot, backColorBot);
            }
        }

        private string getListOperations(int type)
        {
            string url;
            if (type == 0)
            {
                url = "operation-types/search?query=type=in=(\"incoming\")";
            }
            else
            {
                //url = "operation-types/search?query=type=in=(\"outcoming\",\"internal\",\"manufacturing\")";
                url = "operation-types/search?query=type=in=(\"outcoming\")";
            }

            ApiResponse res = new ApiResponse();
            res.Status = false;
            string result = "";
            try
            {
                res = HTTP.GetJson(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get list operations error: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return result;
            }

            try
            {
                if (res.Status && Util.IsJson(res.RawText))
                {
                    List<Operation> RootObject = JsonConvert.DeserializeObject<List<Operation>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    List<Operation> ListOperation = RootObject as List<Operation>;
                    foreach (Operation item in ListOperation)
                    {
                        if (result.Length > 0)
                            result += "," + item.id.ToString();
                        else result += item.id.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Load list operations error: " + res.RawText);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Parse operation error : " + ex.Message.ToString());
            }
            return result;
        }

        private DataTable getListTransfer(string api)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            DataTable dtList = new DataTable();
            try
            {
                string url = "transfers/" + api;
                res = HTTP.GetJson(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Get transfer error: " + ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return dtList;
            }

            try
            {
                if (res.Status && Util.IsJson(res.RawText))
                {
                    List<TransferInfo> RootObject = JsonConvert.DeserializeObject<List<TransferInfo>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                    List<TransferInfo> ListTransfer = RootObject as List<TransferInfo>;
                    dtList = Util.ToDataTable<TransferInfo>(ListTransfer);
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
            return dtList;
        }

        #endregion

        private DataTable removeRowDatatable(DataGrid dg, DataTable dt, int id)
        {
            try
            {
                DataRow[] rs = null;
                rs = dt.Select(COL_ID + " = " + id);
                if (rs.Length > 0)
                {
                    foreach (DataRow dr in rs)
                    {
                        dt.Rows.Remove(dr);
                    }
                }
                
                dg.DataSource = dt;
                dg.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Remove row table error: " + ex.Message.ToString());
            }
            return dt;
        }

        private void callTransferOperation(int id)
        {
            closeEventScan();
            TransferOperations transferOperations = new TransferOperations();
            transferOperations.FrmDashBoardTransfer = this;
            transferOperations.TransferId = id;
            transferOperations.TransferIdRemove = 0;
            transferOperations.TypeTransfer = this.typeTransfer;
            transferOperations.ShowDialog();
            initEventScan();

            _transferInfo = null;
            //this.txtTransferNumber.Text = null;

            int _id = transferOperations.TransferIdRemove;
            //MessageBox.Show(_id.ToString());
            if (_id > 0)
            {
                removeRowDatatable(this.dgListTransferTop, this.dtListTop, _id);
                removeRowDatatable(this.dgListTransferBot, this.dtListBot, _id);
            }
        }

        private void dgListTransferTop_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridCell row = dgListTransferTop.CurrentCell;
                if (row.ColumnNumber == 1)
                {
                    bool next = true;
                    if (dtListTop.Rows[row.RowNumber][COL_ID] != null && Convert.ToString(dtListTop.Rows[row.RowNumber][COL_STATE]).Length > 0)
                    {
                        next = alertStateTransfer(Convert.ToString(dtListTop.Rows[row.RowNumber][COL_STATE]));
                    }
                    if (next) callTransferOperation(Convert.ToInt32(dtListTop.Rows[row.RowNumber][COL_ID]));
                }
            }
            catch { }
        }

        private void dgListTransferBot_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridCell row = dgListTransferBot.CurrentCell;
                if (row.ColumnNumber == 1)
                {
                    bool next = true;
                    if (dtListBot.Rows[row.RowNumber][COL_ID] != null && Convert.ToString(dtListBot.Rows[row.RowNumber][COL_STATE]).Length > 0)
                    {
                        next = alertStateTransfer(Convert.ToString(dtListBot.Rows[row.RowNumber][COL_STATE]));
                    }
                    if (next) callTransferOperation(Convert.ToInt32(dtListBot.Rows[row.RowNumber][COL_ID]));
                }
            }
            catch { }
        }

        private void DashBoardTransfer_Closing(object sender, CancelEventArgs e)
        {
            closeEventScan();
        }

        public void closeEventScan()
        {
            if (this.dcdEvent.IsListening)
            {
                this.dcdEvent.StopScanListener();
            }

            if (this.hDcd != null)
            {
                this.hDcd.Dispose();
            }
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

        private void txtTransferNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_transferInfo != null)
                {
                    callTransferOperation(_transferInfo.id);
                }
                else
                {
                    if (!ScanCode(this.txtTransferNumber.Text.Trim())) return;
                    if (_transferInfo != null)
                    {
                        //MessageBox.Show(_transferInfo.id.ToString());
                        callTransferOperation(_transferInfo.id);
                    }
                }
            }
        }

        private bool alertStateTransfer(string state)
        {
            DialogResult mgb = new DialogResult();
            switch (state)
            {
                case "done":
                    //do something
                    mgb = MessageBox.Show("The transfer has been completed", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    if (mgb != DialogResult.Yes) return false;
                    break;
                case "draft":
                    //do something else
                    return true;
                    break;
                case "waiting":
                    //do something else
                    mgb = MessageBox.Show("The transfer is waiting for the material", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    if (mgb != DialogResult.Yes) return false;
                    break;
                case "waiting_other":
                    //do something else
                    mgb = MessageBox.Show("The transfer is waiting for the material", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    if (mgb != DialogResult.Yes) return false;
                    break;
                case "ready":
                    //do something else
                    mgb = MessageBox.Show("The transfer is on the ready list", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    if (mgb != DialogResult.Yes) return false;
                    break;
                default:
                    //do a different thing
                    return true;
                    break;
            }
            return true;
        }

    }

}