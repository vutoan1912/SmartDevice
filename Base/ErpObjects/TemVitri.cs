using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ERP.Base.ErpObjects
{
    class TemVitri
    {

        #region Properties
        private int _id;
        private string _ten;
        private char _splitter = '-'; 

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Ten
        {
            get { return _ten; }
            set { _ten = value; }
        }
        #endregion

        public TemVitri(string MaTemVitri)
        {
            string[] location_barcode = MaTemVitri.Split(_splitter);
            
            if (location_barcode.Length >= 2) {
                _id = ParseToInt(location_barcode[0]);

                if (location_barcode.Length > 2) {
                    List<string> tmp = new List<string>(location_barcode);
                    tmp.RemoveAt(0);
                    _ten = string.Join("-", tmp.ToArray());
                }
                else {
                    _ten = location_barcode[1];
                }
            }
        }

        private int ParseToInt(string sID) {
            int id = 0;
            try {
                id = Int32.Parse(sID);
            }
            catch(Exception ex){
                
            }
            return id;
        }

        public TemVitri(int id, string ten)
        {
            _id = id;
            _ten = ten;
        }

    }
}
