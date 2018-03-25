using System;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace ERP
{
    public class Util
    {
        public static string ToQueryString(object paras)
        {
            StringBuilder sb = new StringBuilder();
            
            object myO = (object)paras;

            foreach (var prop in myO.GetType().GetProperties())
            {
                sb.AppendFormat("{0}={1}", prop.Name, prop.GetValue(myO, null));
                sb.Append("&");
                //Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(myO, null));
            }
            return sb.ToString();
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static int ConvertInt(string ipData) {
            try {
                return int.Parse(ipData);
            }
            catch (Exception ex) {
                return 0;
            }
        }

        public static int ConvertInt(object ipData) {
            string data = ipData.ToString();
            return ConvertInt(data);
        }

        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

        public static string CleanStr(string myStr) {
            if(myStr != null)
                return Regex.Replace(myStr, @"[^0-9a-zA-Z]+", " ");

            return "";
        }

        public static object ReflectPropertyValue(object source, string property)
        {
            return source.GetType().GetProperty(property).GetValue(source, null);
        }

        public static void Logs(string text)
        {
            try
            {
                string Path = "Logs.txt";
                StreamWriter sw = new StreamWriter(Path);

                sw.WriteLine(text);
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }

        public static bool OnlyHexInString(string text)
        {
            // For C-style hex notation (0xFF) you can use @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"
            return System.Text.RegularExpressions.Regex.IsMatch(text, @"\A\b[0-9a-fA-F]+\b\Z");
        }
    }
}