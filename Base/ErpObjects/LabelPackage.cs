using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ERP.Base.ErpObjects
{
    class LabelPackage
    {
        #region Properties
        private string _product_name;
        private string _package_id;

        public string ProductName
        {
            get { return _product_name; }
            set { _product_name = value; }
        }

        public string PackageId
        {
            get { return _package_id; }
            set { _package_id = value; }
        }
        #endregion

        public LabelPackage(string labelContent)
        {
            ParseString(labelContent);
        }

        public LabelPackage Clone()
        {
            LabelPackage newLabel = new LabelPackage();
            newLabel.ProductName = this.ProductName;
            newLabel.PackageId = this.PackageId;
            return newLabel;
        }

        private LabelPackage() { }

        public LabelPackage(string productName, string packageId)
        {
            _product_name = productName;
            _package_id = packageId;
        }

        public bool ParseString(string labelContent)
        {
            try
            {
                string endChar = labelContent.Substring(labelContent.Length - 2, 2);
                if (labelContent.StartsWith(ObjectDefine.Prefix) && (endChar == ObjectDefine.End))
                {
                    labelContent = labelContent.Substring(ObjectDefine.Prefix.Length, labelContent.Length - ObjectDefine.Prefix.Length);
                    labelContent = labelContent.Substring(0, labelContent.Length - 2);
                }
                else return false;

                string[] tem_data = labelContent.Split(ObjectDefine.Separator);
                foreach (string item in tem_data)
                {
                    if (item.StartsWith(ObjectDefine.productNo))
                        _product_name = item.Substring(ObjectDefine.productNo.Length, item.Length - ObjectDefine.productNo.Length);
                    else if (item.StartsWith(ObjectDefine.packageID))
                        _package_id = item.Substring(ObjectDefine.packageID.Length, item.Length - ObjectDefine.packageID.Length);
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;

        }

        public string TemToString()
        {
            //example: [)>@06@P80N45HGJ456YU768@3SPKG3676573658@@

            string qrString = "";
            qrString += ObjectDefine.Prefix + ObjectDefine.Separator + ObjectDefine.productNo + _product_name;
            qrString += ObjectDefine.Separator + ObjectDefine.packageID + _package_id + ObjectDefine.End;
            return qrString;
        }

        public List<KeyValuePair<string, string>> ToList()
        {
            var lstData = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>(ObjectDefine.productNo, _product_name),
                    new KeyValuePair<string, string>(ObjectDefine.packageID, _package_id)
                };

            return lstData;
        }
    }

    class ObjectDefine
    {
        //code char
        public static string packageID = "3S";
        public static string productNo = "P";
        public static char Separator = '@';
        public static string Prefix = "[)>@06";
        public static string End = "@@";
    }
}
