using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using ERP.Base;
using System.Xml;
using Newtonsoft.Json;

namespace ERP
{
    class HTTP
    {

        public static ApiResponse Get(string url)
        {
            string requestURL = Config.API_URL + url;
            ApiResponse api_res = new ApiResponse();

            StringBuilder sb = new StringBuilder();

            try {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);
                request.Referer = Config.API_URL;
                request.Headers["Authorization"] = ERP.Base.Config.Token;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                Char[] readBuffer = new Char[256];
                int count = reader.Read(readBuffer, 0, 256);

                while (count > 0)
                {
                    String output = new String(readBuffer, 0, count);
                    sb.Append(output);
                    count = reader.Read(readBuffer, 0, 256);
                }
                stream.Dispose();
                reader.Dispose();
                response.Close();
            }
            catch(Exception e){
                api_res.Status = false;
                throw new System.Exception("No network connection !");
            }
            
            //string xml = sb.ToString();

            return api_res;
        }

        public static ApiResponse GetJson(string url)
        {
            string requestURL = Config.API_URL + url;
            ApiResponse api_res = new ApiResponse();

            StringBuilder sb = new StringBuilder();

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestURL);
                request.Referer = Config.API_URL;
                request.Headers["Authorization"] = ERP.Base.Config.Token;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                Char[] readBuffer = new Char[256];
                int count = reader.Read(readBuffer, 0, 256);

                while (count > 0)
                {
                    String output = new String(readBuffer, 0, count);
                    sb.Append(output);
                    count = reader.Read(readBuffer, 0, 256);
                    api_res.RawText = sb.ToString();
                }

                stream.Dispose();
                reader.Dispose();
                response.Close();
            }
            catch (Exception e)
            {
                api_res.Status = false;
                throw new System.Exception("No network connection !");
            }

            //string xml = sb.ToString();

            return api_res;
        }

        public static ApiResponse Post(string url, object paras)
        {

            ApiResponse api_res = new ApiResponse();

            string uri = Util.ToQueryString(paras);

            string URLAuth = Config.API_URL + url;

            string postString = uri;

            const string contentType = "application/x-www-form-urlencoded";
            System.Net.ServicePointManager.Expect100Continue = false;

            HttpWebRequest webRequest = WebRequest.Create(URLAuth) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = contentType;
            webRequest.ContentLength = postString.Length;
            webRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webRequest.Referer = Config.API_URL;
            webRequest.Headers["Authorization"] = ERP.Base.Config.Token;

            WebResponse myWebResponse = null;

            try
            {
                StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
                requestWriter.Write(postString);
                requestWriter.Close();

                myWebResponse = webRequest.GetResponse();

                StreamReader responseReader = new StreamReader(myWebResponse.GetResponseStream());
                string responseData = responseReader.ReadToEnd();

                api_res.RawText = responseData;

                responseReader.Close();
                myWebResponse.Close();
                requestWriter.Dispose();
                responseReader.Dispose();

            }
            catch (Exception e)
            {
                api_res.Status = false;
                throw new System.Exception("No network connection !");
            }


            return api_res;
        }

        public static ApiResponse PostJson(string url, object paras) {

            ApiResponse api_res = new ApiResponse();

            string uri = paras.ToString().Trim();

            string URLAuth = Config.API_URL + url;

            string postString = uri;

            const string contentType = "application/json";
            System.Net.ServicePointManager.Expect100Continue = false;

            HttpWebRequest webRequest = WebRequest.Create(URLAuth) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = contentType;
            webRequest.ContentLength = postString.Length;
            webRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webRequest.Referer = Config.API_URL;
            webRequest.Headers["Authorization"] = ERP.Base.Config.Token;

            webRequest.KeepAlive = true;
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.SendChunked = true;

            WebResponse myWebResponse = null;

            try {
                StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
                requestWriter.Write(postString);
                requestWriter.Close();

                myWebResponse = webRequest.GetResponse();

                StreamReader responseReader = new StreamReader(myWebResponse.GetResponseStream());
                string responseData = responseReader.ReadToEnd();

                api_res.RawText = responseData;

                responseReader.Close();
                myWebResponse.Close();
                requestWriter.Dispose();
                responseReader.Dispose();
            }
            catch (WebException e)
            {
                api_res.Status = false;
                if (e.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            //TODO: use JSON.net to parse this string and look at the error message
                            ErrorEntity ErrorEntity = JsonConvert.DeserializeObject<ErrorEntity>(error, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                            throw new System.Exception(ErrorEntity.title);
                        }
                    }
                }
                else
                {
                    throw new System.Exception("Error !");
                }
            }


            return api_res;
        }

        public static ApiResponse Put(string url, object paras)
        {
            ApiResponse api_res = new ApiResponse();
            string uri = paras.ToString().Trim();
            string URLAuth = Config.API_URL + url;
            string postString = uri;

            const string contentType = "application/json";
            System.Net.ServicePointManager.Expect100Continue = false;

            HttpWebRequest webRequest = WebRequest.Create(URLAuth) as HttpWebRequest;
            webRequest.Method = "PUT";
            webRequest.ContentType = contentType;
            webRequest.ContentLength = postString.Length;
            webRequest.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.0.1) Gecko/2008070208 Firefox/3.0.1";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            webRequest.Referer = Config.API_URL;
            webRequest.Headers["Authorization"] = ERP.Base.Config.Token;
            
            webRequest.KeepAlive = true;
            webRequest.AllowWriteStreamBuffering = true;
            webRequest.SendChunked = true;

            WebResponse myWebResponse = null;

            try
            {
                //StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
                //requestWriter.Write(postString);

                using (StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                {
                    requestWriter.Write(postString);
                }

                myWebResponse = webRequest.GetResponse();

                //StreamReader responseReader = new StreamReader(myWebResponse.GetResponseStream());
                //string responseData = responseReader.ReadToEnd();

                using (StreamReader responseReader = new StreamReader(myWebResponse.GetResponseStream()))
                {
                    api_res.RawText = responseReader.ReadToEnd();
                }

                myWebResponse.Close();
                
                //requestWriter.Close();
                //requestWriter.Dispose();
                //responseReader.Close();
                //responseReader.Dispose();
                
            }
            catch (WebException e)
            {
                api_res.Status = false;

                if (e.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)e.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string error = reader.ReadToEnd();
                            //TODO: use JSON.net to parse this string and look at the error message
                            ErrorEntity ErrorEntity = JsonConvert.DeserializeObject<ErrorEntity>(error, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
                            throw new System.Exception(ErrorEntity.title);
                        }
                    }
                }
                else
                {
                    throw new System.Exception("Error !");
                }
            }
            return api_res;
        }
    
    }

    public class ErrorEntity
    {
        public string entityName { get; set; }
        public string errorKey { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string message { get; set; }
        public string @params { get; set; }
    }

    public class HttpRequest
    {
        private WebRequest request;
        private Stream dataStream;

        private string status;

        public String Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public HttpRequest(string url)
        {
            // Create a request using a URL that can receive a post.

            request = WebRequest.Create(url);
        }

        public HttpRequest(string url, string method)
            : this(url)
        {

            if (method.Equals("GET") || method.Equals("POST"))
            {
                // Set the Method property of the request to POST.
                request.Method = method;
            }
            else
            {
                throw new Exception("Invalid Method Type");
            }
        }

        public HttpRequest(string url, string method, string data)
            : this(url, method)
        {

            // Create POST data and convert it to a byte array.
            string postData = data;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/json";

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Get the request stream.
            dataStream = request.GetRequestStream();

            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);

            // Close the Stream object.
            dataStream.Close();

        }

        public string GetResponse()
        {
            // Get the original response.
            WebResponse response = request.GetResponse();

            this.Status = ((HttpWebResponse)response).StatusDescription;

            // Get the stream containing all content returned by the requested server.
            dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content fully up to the end.
            string responseFromServer = reader.ReadToEnd();

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

    }


    class ApiResponse {
        private bool _status = true;
        private String message = "Good";        
        private string _raw_text;

        public bool Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string RawText
        {
            get { return _raw_text; }
            set { _raw_text = value; }
        }
    }

    class ApiModel {
        int _code;
        string _message;

        public int code
        {
            get { return _code; }
            set { _code = value; }
        }

        public string message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
