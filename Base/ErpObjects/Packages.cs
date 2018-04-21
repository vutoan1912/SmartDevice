using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ERP
{
    class Packages
    {
        public long created { get; set; }
        public long updated { get; set; }
        public string createdBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string packageNumber { get; set; }

        public Packages getInfo(string packageNumber)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "packages/search?query=packageNumber==\"" + packageNumber + "\"";
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                List<Packages> RootObject = JsonConvert.DeserializeObject<List<Packages>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                if (RootObject.Count > 0)
                    return RootObject[0];
                else return null;
            }
            else
            {
                return null;
            }
        }

        public Packages getInfo(int packageId)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "packages/search?query=id==" + packageId.ToString();
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                List<Packages> RootObject = JsonConvert.DeserializeObject<List<Packages>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                if (RootObject.Count > 0)
                    return RootObject[0];
                else return null;
            }
            else
            {
                return null;
            }
        }
    }

    class Lots
    {
        public long created { get; set; }
        public long updated { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string lotNumber { get; set; }
        public int productId { get; set; }
        public string internalReference { get; set; }

        public Lots getInfo(string lotNumber)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "lots/search?query=lotNumber==\"" + lotNumber + "\"";
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                List<Lots> RootObject = JsonConvert.DeserializeObject<List<Lots>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                if (RootObject.Count > 0)
                    return RootObject[0];
                else return null;
            }
            else
            {
                return null;
            }
        }

        public Lots getInfo(int lotId)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "lots/search?query=id==" + lotId.ToString();
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                List<Lots> RootObject = JsonConvert.DeserializeObject<List<Lots>>(res.RawText, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                if (RootObject.Count > 0)
                    return RootObject[0];
                else return null;
            }
            else
            {
                return null;
            }
        }
    }
}
