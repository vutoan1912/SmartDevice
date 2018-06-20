using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ERP
{
    class Operation
    {
        public object created { get; set; }
        public object updated { get; set; }
        public string createdBy { get; set; }
        public string updatedBy { get; set; }
        public bool active { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int refSequenceId { get; set; }
        public int defaultSrcLocationId { get; set; }
        public int defaultDestLocationId { get; set; }
        public int warehouseId { get; set; }
        public int returnOperationId { get; set; }
        public string displayName { get; set; }
    }
}
