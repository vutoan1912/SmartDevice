using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ERP
{
    public class TransferDetail
    {
        public long created { get; set; }
        public long updated { get; set; }
        public string createdBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public int transferId { get; set; }
        public int transferItemId { get; set; }
        public string productName { get; set; }
        public int productId { get; set; }
        public string manPn { get; set; }
        public int doneQuantity { get; set; }
        public int srcLocationId { get; set; }
        public int destLocationId { get; set; }
        public string state { get; set; }
        public int destPackageId { get; set; }
        public string destPackageNumber { get; set; }
        public int srcPackageId { get; set; }
        public string srcPackageNumber { get; set; }
        public string traceNumber { get; set; }
        public int reserved { get; set; }
        public int manId { get; set; }
        public int lotId { get; set; }
        public string reference { get; set; }
        public string internalReference { get; set; }
        public string productDescription { get; set; }
        public string srcLocationName { get; set; }
        public string destLocationName { get; set; }
        public int available { get; set; }
        public string adjustmentType { get; set; }
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
        public int srcLocationId { get; set; }
        public int destLocationId { get; set; }
        public long scheduledDate { get; set; }
        public string state { get; set; }
        public int productVersionId { get; set; }
        public string sourceDocument { get; set; }
        public string priority { get; set; }
        public List<object> quantInfo { get; set; }
        public int? currentDemand { get; set; }
        public List<int> otherProjects { get; set; }
        public int? productionQuantity { get; set; }
        public int capacity { get; set; }
        public List<object> moItems { get; set; }
        public List<object> transferItems { get; set; }
        public List<object> transferDetails { get; set; }
        public List<object> removedTransferDetails { get; set; }
        public List<object> removedTransferItems { get; set; }
    }
}
