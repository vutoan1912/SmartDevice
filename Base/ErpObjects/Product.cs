using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ERP
{
    public class Product
    {
        public long created { get; set; }
        public long updated { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? categoryId { get; set; }
        public string oldName { get; set; }
        public int? oldId { get; set; }
        public int? categoryOldId { get; set; }
        public int? type { get; set; }

        public Product getInfo(int productId)
        {
            ApiResponse res = new ApiResponse();
            res.Status = false;
            string url = "products/search?query=id==" + productId.ToString();
            res = HTTP.GetJson(url);
            if (res.Status && Util.IsJson(res.RawText))
            {
                List<Product> RootObject = JsonConvert.DeserializeObject<List<Product>>(res.RawText, new JsonSerializerSettings
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
    }
}
