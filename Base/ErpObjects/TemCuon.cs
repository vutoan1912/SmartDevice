using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ERP.Base.ErpObjects
{
    class TemCuon
    {
        #region Properties
        private string _vnpt_pn;
        private string _so_phieu_nhap_kho;
        private string _so_don_hang;
        private string _ngay_nhap_kho;
        private string _id_cuon;
        private string _so_luong;
        private string _type = "0";
        private string _description;

        public string VnptPn
        {
            get { return _vnpt_pn; }
            set { _vnpt_pn = value; }
        }

        

        public string SoPhieuNhapKho
        {
            get { return _so_phieu_nhap_kho; }
            set { _so_phieu_nhap_kho = value; }
        }

        public string SoDonHang
        {
            get { return _so_don_hang; }
            set { _so_don_hang = value; }
        }

        public string NgayNhapKho
        {
            get { return _ngay_nhap_kho; }
            set { _ngay_nhap_kho = value; }
        }

        public string IdCuon
        {
            get { return _id_cuon; }
            set { _id_cuon = value; }
        }

        public string SoLuong
        {
            get { return _so_luong; }
            set { _so_luong = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        #endregion

        public TemCuon(string labelContent)
        {
            ParseString(labelContent);
        }

        public TemCuon Clone()
        {
            TemCuon newCuon = new TemCuon();
            newCuon.VnptPn = this.VnptPn;
            newCuon.SoPhieuNhapKho = this.SoPhieuNhapKho;

            newCuon.SoDonHang = this.SoDonHang;

            newCuon.NgayNhapKho = this.NgayNhapKho;

            newCuon.IdCuon = this.IdCuon;

            newCuon.SoLuong = this.SoLuong;

            newCuon.Type = this.Type;

            return newCuon;
        }

        private TemCuon() { }

        public TemCuon(string vnptPn, string soDonHang, string soPhieuNhapKho, string ngayNhapKho, string idCuon, string soLuong, string Type)
        {
            _vnpt_pn = vnptPn;
            _so_don_hang = soDonHang;
            _so_phieu_nhap_kho = soPhieuNhapKho;
            _ngay_nhap_kho = ngayNhapKho;
            _id_cuon = idCuon;
            _so_luong = soLuong;
            _type = Type;
        }

        public void ParseString(string labelContent)
        {
            try
            {
                char[] delimiterChars = { ':' };
                char[] StarChars = { '*' };

                string[] tem_data = labelContent.Split(StarChars);

                //using (StringReader reader = new StringReader(labelContent))
                //{
                //string line;
                //while ((line = reader.ReadLine()) != null)
                foreach (string line in tem_data)
                {
                    // Do something with the line
                    string[] datas = line.Split(delimiterChars);

                    string key = datas[0].ToLower();
                    switch (key)
                    {
                        case "pn":
                            {
                                _vnpt_pn = datas[1];
                            } break;
                        case "spnk":
                            {
                                _so_phieu_nhap_kho = datas[1];
                            } break;
                        case "sdh":
                            {
                                _so_don_hang = datas[1];
                            } break;
                        case "nnk":
                            {
                                _ngay_nhap_kho = datas[1];
                            } break;
                        case "id":
                            {
                                _id_cuon = datas[1];
                            } break;
                        case "sl":
                            {
                                _so_luong = datas[1];
                            } break;
                        case "type":
                            {
                                _type = datas[1];
                            } break;
                    }

                }
                //}
            }
            catch (Exception e)
            {

            }


        }

        public string TemToString()
        {
            string qrString = "";
            qrString += "pn:" + _vnpt_pn + "*";
            qrString += "spnk:" + _so_phieu_nhap_kho + "*";
            qrString += "sdh:" + _so_don_hang + "*";
            qrString += "nnk:" + _ngay_nhap_kho + "*";
            qrString += "id:" + _id_cuon + "*";
            qrString += "sl:" + _so_luong + "*";
            qrString += "type:" + _type + "*";

            return qrString;
        }

        public List<KeyValuePair<string, string>> ToList()
        {
            var lstData = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("pn", _vnpt_pn),
                    new KeyValuePair<string, string>("spnk", _so_phieu_nhap_kho),
                    new KeyValuePair<string, string>("sdh", _so_don_hang),
                    new KeyValuePair<string, string>("nnk", _ngay_nhap_kho),
                    new KeyValuePair<string, string>("id", _id_cuon),
                    new KeyValuePair<string, string>("sl", _so_luong),
                    new KeyValuePair<string, string>("type", _type),
                };

            return lstData;
        }
    }

    public class TemCuonException : Exception
    {
        public TemCuonException() : base() { }
        public TemCuonException(string message) : base(message) { }
    }

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
