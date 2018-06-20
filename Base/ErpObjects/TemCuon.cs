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
}
