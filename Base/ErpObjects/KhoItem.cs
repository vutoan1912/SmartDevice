using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ERP.Base.ErpObjects
{
    class KhoItem
    {
        private string _vnpt_pn;
        private int _quantity;

        public string VnptPn
        {
            get { return _vnpt_pn; }
            set { _vnpt_pn = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
    }


    class CuonModel
    {
        private string _vnpt_pn;
        private string _code;
        private double _quantity;
        private int _thung;
        private int _scanned;
        private int _thungType;

        public string vnpt_pn
        {
            get { return _vnpt_pn; }
            set { _vnpt_pn = value; }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public double Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        public int Thung
        {
            get { return _thung; }
            set { _thung = value; }
        }
        public int Scanned
        {
            get { return _scanned; }
            set { _scanned = value; }
        }

        public int ThungType
        {
            get { return _thungType; }
            set { _thungType = value; }
        }
    }

    class ThungThanhPhamModel {
        private string _product_code;
        private string _id_thung;
        private int _quantity;
        private int _scanned;

        public string ProductCode
        {
            get { return _product_code; }
            set { _product_code = value; }
        }

        public string IdThung
        {
            get { return _id_thung; }
            set { _id_thung = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }
        
        public int Scanned
        {
            get { return _scanned; }
            set { _scanned = value; }
        }
    }
    
}
