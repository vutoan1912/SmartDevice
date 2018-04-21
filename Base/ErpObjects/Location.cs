using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

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
}
