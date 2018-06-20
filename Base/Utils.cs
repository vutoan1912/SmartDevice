using System;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

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

        //public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        //{
        //    try
        //    {
        //        List<T> list = new List<T>();

        //        foreach (var row in table.AsEnumerable())
        //        {
        //            T obj = new T();

        //            foreach (var prop in obj.GetType().GetProperties())
        //            {
        //                try
        //                {
        //                    PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
        //                    propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
        //                }
        //                catch
        //                {
        //                    continue;
        //                }
        //            }

        //            list.Add(obj);
        //        }

        //        return list;
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public static List<T> DataTableToList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
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

        private static string PREFIX_PACKAGE = "PACK";
        private static string PREFIX_UID = "UID";
        private static string PREFIX_OLD = "PN:";

        public static int getTypePackage(string packageID, string type)
        {
            if (packageID.StartsWith(PREFIX_PACKAGE)) return 1; //new package
            else if (packageID.StartsWith(PREFIX_UID)) return 2; //new uid
            else if (packageID.StartsWith(PREFIX_OLD) && type == "1") return 3; //old package
            else if (packageID.StartsWith(PREFIX_OLD) && type == "0") return 4; //old uid
            else if (Util.OnlyHexInString(packageID) && packageID.Length != 15) return 5; //final product package
            else if (Util.OnlyHexInString(packageID)) return 6; //serial number
            else return 0;

        }
    }

    public class DelegateControl
    {
        public DelegateControl()
        {
        }

        public delegate void ControlUpdater(Control uiControl, string value);
        public static void UpdateControlWithValue(Control uiControl, string value)
        {
            uiControl.Text = value;
        }

        public static string readControlText(Control varControl)
        {
            if (varControl.InvokeRequired)
            {
                return (string)varControl.Invoke(
                  new Func<String>(() => readControlText(varControl))
                );
            }
            else
            {
                string varText = varControl.Text;
                return varText;
            }
        }
    }
}