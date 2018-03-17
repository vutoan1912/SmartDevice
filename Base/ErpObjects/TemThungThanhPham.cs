using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ERP;

namespace ERP.Base.ErpObjects
{
    class TemThungThanhPham
    {
        private const int ID_THUNG_LENGTH = 7;
        private string _id_thung;

        //private char _splitter = '.'; 

        public string IdThung
        {
            get { return _id_thung; }
            set { _id_thung = value; }
        }


        public TemThungThanhPham(string MaTemThungThanhPham)
        {

            MaTemThungThanhPham = MaTemThungThanhPham.Trim();
            //string[] location_barcode = MaTemThungThanhPham.Split(_splitter);
            if (MaTemThungThanhPham.Length == ID_THUNG_LENGTH)
            {
                _id_thung = MaTemThungThanhPham;
            }
        }
    }
}
