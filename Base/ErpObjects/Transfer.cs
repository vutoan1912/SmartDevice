using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ERP
{
    public class TransferDetail
    {
        public long created { get; set; }
        public long updated { get; set; }
        public string createdBy { get; set; }
        public bool active { get; set; }
        public int? id { get; set; }
        public int? transferId { get; set; }
        public int? transferItemId { get; set; }
        public string internalReference { get; set; }
        public string productName { get; set; }
        public int? productId { get; set; }
        public int? destPackageId { get; set; }
        public string destPackageNumber { get; set; }
        public int? srcPackageId { get; set; }
        public string srcPackageNumber { get; set; }
        public int? lotId { get; set; }
        public string traceNumber { get; set; }
        public string manPn { get; set; }
        public int? srcLocationId { get; set; }
        public int? destLocationId { get; set; }
        public string state { get; set; }
        public double reserved { get; set; }
        public double doneQuantity { get; set; }
        public int? manId { get; set; }
        public string reference { get; set; }
        public string productDescription { get; set; }
        public string srcLocationName { get; set; }
        public string destLocationName { get; set; }
        public int available { get; set; }
        public string adjustmentType { get; set; }
        public string scanned { get; set; }
        public string scanned_bak { get; set; }
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
        public int? srcLocationId { get; set; }
        public int? destLocationId { get; set; }
        public long scheduledDate { get; set; }
        public string state { get; set; }
        public int? productVersionId { get; set; }
        public string sourceDocument { get; set; }
        public string priority { get; set; }
        public List<object> quantInfo { get; set; }
        public int? currentDemand { get; set; }
        public List<int> otherProjects { get; set; }
        public int? productionQuantity { get; set; }
        public int capacity { get; set; }
        public List<object> moItems { get; set; }
        public List<TransferItem> transferItems { get; set; }
        public List<TransferDetail> transferDetails { get; set; }
        public List<object> removedTransferDetails { get; set; }
        public List<object> removedTransferItems { get; set; }
        public string routing { get; set; }
        public string srcLocationName { get; set; }
    }

    public class TransferItem
    {
        public object created { get; set; }
        public object updated { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string internalReference { get; set; }
        public int? transferId { get; set; }
        public int? productId { get; set; }
        public string productName { get; set; }
        public int? manId { get; set; }
        public string manPn { get; set; }
        public int? srcLocationId { get; set; }
        public int? destLocationId { get; set; }
        public string state { get; set; }
        public double reserved { get; set; }
        public string productDescription { get; set; }
        public double initialQuantity { get; set; }
        public double doneQuantity { get; set; }
        public double doneQuantityBackup { get; set; }
        public bool selected { get; set; }
        public double foc { get; set; }
    }

    public class TransferBusiness
    {
        public static List<TransferInfo> getListTransfer(string api)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            List<TransferInfo> ListTransfer = new List<TransferInfo>();
            try
            {
                string url = "transfers/" + api;
                res = HTTP.GetJson(url);

                if (res.Status && Util.IsJson(res.RawText))
                {
                    List<TransferInfo> RootObject = JsonConvert.DeserializeObject<List<TransferInfo>>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    ListTransfer = RootObject as List<TransferInfo>;
                }
            }
            catch (Exception ex)
            {

            }
            return ListTransfer;
        }

        public static TransferInfo getTransferInfo(int transferId)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            TransferInfo TransferInfo = new TransferInfo();
            try
            {
                string url = "transfers/" + transferId.ToString().Trim();
                res = HTTP.GetJson(url);

                if (res.Status && Util.IsJson(res.RawText))
                {
                    TransferInfo RootObject = JsonConvert.DeserializeObject<TransferInfo>(res.RawText, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    TransferInfo = RootObject as TransferInfo;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return TransferInfo;
        }
    }
}
