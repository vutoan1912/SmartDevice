using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ERP
{
    public class StockQuants
    {
        public object created { get; set; }
        public object updated { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public int? locationId { get; set; }
        public int? lotId { get; set; }
        public int? productId { get; set; }
        public int? manId { get; set; }
        public double reserved { get; set; }
        public double onHand { get; set; }
        public int? packageId { get; set; }
    }

    public class StockQuantBusiness
    {
        public static StockQuants getStockUid(string lotNumber)
        {
            Lots Lot = LotBusiness.getInfo(lotNumber);
            if (Lot == null) return null;

            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "quants/search?query=lotId==" + Lot.id.ToString() + ";onHand>0";
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                List<StockQuants> RootObject = JsonConvert.DeserializeObject<List<StockQuants>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                if (RootObject.Count > 0) return RootObject[0];
                else return null;
            }
            else
            {
                return null;
            }
        }

        public static StockQuants getStockUid(int lotId)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "quants/search?query=lotId==" + lotId.ToString() + ";onHand>0";
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                List<StockQuants> RootObject = JsonConvert.DeserializeObject<List<StockQuants>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                if (RootObject.Count > 0) return RootObject[0];
                else return null;
            }
            else
            {
                return null;
            }
        }

        public static List<StockQuants> getStockPackage(string PackageNumber)
        {
            List<StockQuants> ListStock = new List<StockQuants>();
            Packages Package = PackageBusiness.getInfo(PackageNumber);
            if (Package == null) return ListStock;

            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "quants/search?query=packageId==" + Package.id.ToString() + ";onHand>0";
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                ListStock = JsonConvert.DeserializeObject<List<StockQuants>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            return ListStock;
        }

        public static List<StockQuants> getStockPackage(int PackageId)
        {
            List<StockQuants> ListStock = new List<StockQuants>();
            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "quants/search?query=packageId==" + PackageId.ToString() + ";onHand>0";
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                ListStock = JsonConvert.DeserializeObject<List<StockQuants>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            return ListStock;
        }

        public static double getStockQuantPackage(string PackageNumber)
        {
            double total = 0;
            List<StockQuants> ListStock = new List<StockQuants>();
            Packages Package = PackageBusiness.getInfo(PackageNumber);
            if (Package == null) return total;

            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "quants/search?query=packageId==" + Package.id.ToString() + ";onHand>0";
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                ListStock = JsonConvert.DeserializeObject<List<StockQuants>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            foreach (StockQuants item in ListStock)
            {
                total += item.onHand;
            }
            return total;
        }

        public static double getStockQuantPackage(int PackageId)
        {
            double total = 0;
            List<StockQuants> ListStock = new List<StockQuants>();

            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "quants/search?query=packageId==" + PackageId.ToString() + ";onHand>0";
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                ListStock = JsonConvert.DeserializeObject<List<StockQuants>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }
            foreach (StockQuants item in ListStock)
            {
                total += item.onHand;
            }
            return total;
        }
    }
}
