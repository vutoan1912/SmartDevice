using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ERP
{
    public class LocationInfo
    {
        public long created { get; set; }
        public long updated { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string barcode { get; set; }
        public string completeName { get; set; }
        public int parentId { get; set; }
        public string type { get; set; }
        public string displayName { get; set; }
        public bool scrapLocation { get; set; }
        public bool returnedLocation { get; set; }
    }

    public class LocationUpdate
    {
        public string errorCode { get; set; }
        public string locationCode { get; set; }
        public string lotNumber { get; set; }
        public string packageNumber { get; set; }
    }

    class LocationBusiness
    {
        public static LocationInfo getInfo(int locationId)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "locations/" + locationId.ToString();
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                LocationInfo RootObject = JsonConvert.DeserializeObject<LocationInfo>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                return RootObject;
            }
            else
            {
                return null;
            }
        }
    }
}
