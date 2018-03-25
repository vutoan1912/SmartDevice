using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ERP.Base
{
    class Config
    {
        //use
        //public const string API_URL = "http://10.2.9.130:9090/api/";   
        public const string API_URL = "http://10.2.9.131:9090/api/";
        //
        public const string API_URL_PRO = "http://10.84.8.75:8084/api/external/sd/";
        public const string API_URL_STAGING = "http://10.84.8.51:8084/api/external/sd/";
        public const string API_URL_DEV = "http://10.84.11.6:8084/api/external/sd/";

        public const string LogPath = "Logs/";

        private static string _token;
        public static string Token
        {
            get { return _token; }
            set { _token = value; }
        }
    }
}