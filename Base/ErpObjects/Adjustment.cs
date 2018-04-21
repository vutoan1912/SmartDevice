using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ERP
{
    public class Adjustment
    {
        public long created { get; set; }
        public long updated { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string reference { get; set; }
        public int locationId { get; set; }
        public string locationName { get; set; }
        public string inventoryOf { get; set; }
        public int requesterId { get; set; }
        public int assigneeId { get; set; }
        public bool exhaustedProducts { get; set; }
        public string state { get; set; }
        public List<AdjustmentDetail> adjustmentDetails { get; set; }
        public List<object> adjustmentInputs { get; set; }
        public List<object> deletedAdjustmentInputs { get; set; }
        public List<object> errorDetails { get; set; }
        public List<object> errorInputs { get; set; }
        public List<object> transferDetails { get; set; }
        public string categoryName { get; set; }
        public string traceNumber { get; set; }
        public string packageNumber { get; set; }
    }

    public class AdjustmentDetail
    {
        public object created { get; set; }
        public object updated { get; set; }
        public string createdBy { get; set; }
        public bool active { get; set; }
        public int? id { get; set; }
        public int adjustmentId { get; set; }
        public int? productId { get; set; }
        public int? manId { get; set; }
        public string internalReference { get; set; }
        public int? locationId { get; set; }
        public string locationName { get; set; }
        public double theoreticalQuantity { get; set; }
        public string state { get; set; }
        public string productDescription { get; set; }
        public string manPn { get; set; }
        public int? packageId { get; set; }
        public string packageNumber { get; set; }
        public int? lotId { get; set; }
        public string traceNumber { get; set; }
        public double realQuantity { get; set; }
        public string barcode { get; set; }
    }

    public class AdjustmentInput
    {
        public object created { get; set; }
        public object updated { get; set; }
        public string createdBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public int adjustmentId { get; set; }
        public int productId { get; set; }
        public int manId { get; set; }
        public string internalReference { get; set; }
        public int locationId { get; set; }
        public string locationName { get; set; }
        public int lotId { get; set; }
        public string traceNumber { get; set; }
        public int packageId { get; set; }
        public string productDescription { get; set; }
        public string packageNumber { get; set; }
    }

    public class AdjustmentDetailScan
    {
        public string internalReference { get; set; }
        public string pack_id { get; set; }
        public double realQuantity { get; set; }
        public string barcode { get; set; }
    }
}
