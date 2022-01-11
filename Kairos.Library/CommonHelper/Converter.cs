using Kairos.Library.CommonHelper.Spelling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kairos.Library.CommonHelper
{
    public static class Converter
    {
        public const string c_DefaultCulture = "id-ID";
        public static System.Globalization.NumberFormatInfo StandardNFI = null;
        public static System.Globalization.CultureInfo IDCultureInfo = null;

        static Converter()
        {
            StandardNFI = new System.Globalization.NumberFormatInfo();
            StandardNFI.CurrencyDecimalSeparator = ",";
            StandardNFI.NumberDecimalSeparator = ",";
            StandardNFI.CurrencyGroupSeparator = ".";
            StandardNFI.NumberGroupSeparator = ".";
            StandardNFI.CurrencyDecimalDigits = 2;
            StandardNFI.NumberDecimalDigits = 2;
            StandardNFI.CurrencySymbol = string.Empty;

            IDCultureInfo = new System.Globalization.CultureInfo(c_DefaultCulture);
        }

        /// <summary>
        /// Converts a numeric value to words suitable for the portion of
        /// a check that writes out the amount.
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <returns></returns>
        public static string ConvertDecimalToSpellingString(decimal GeneralValue, string currency, string CurrencyDescription = "")
        {
            decimal value = Math.Abs(GeneralValue);
            switch (currency)
            {
                case "IDR":
                    CurrencyInWordIDR SpellIDR = new CurrencyInWordIDR();
                    string TmpStr = SpellIDR.ConvertToWord(value.ToString("0.00"), currency);
                    return TmpStr.Replace("  ", " ").Replace("SATU PULUH", "SEPULUH").Replace("SATU RATUS", "SERATUS").Replace("SATU RIBU", "SERIBU").Replace('-', ' ');
                case "USD":
                    return CurrencyInWordsUSD.ConvertDecimalToSpellingString(value);
                default:
                    CurrencyInWordGeneral SpellOthers = new CurrencyInWordGeneral();
                    string TmpStrOthers = SpellOthers.ConvertToWord(value.ToString("0.00"), currency);
                    return TmpStrOthers.Replace("  ", " ").Replace("SATU PULUH", "SEPULUH").Replace("SATU RATUS", "SERATUS").Replace("SATU RIBU", "SERIBU").Replace('-', ' ');
            }
        }

        public static string ConvertDecimalToFormattedString(decimal value)
        {
            return value.ToString("C", StandardNFI);
        }
        public static decimal ConvertFormattedStringToDecimal(string value)
        {
            return Convert.ToDecimal(value, StandardNFI);
        }

        /// <summary>
        /// Converting an integer to it's hex string representation
        /// </summary>
        /// <param name="number">an integer</param>
        /// <returns></returns>
        public static string ConvertIntegerToHexString(int number)
        {
            return String.Format("{0:x}", number);
        }
        /// <summary>
        /// Converting a hex string into it's integer value
        /// </summary>
        /// <param name="hexString">Hexadecimal string</param>
        /// <returns></returns>
        public static int ConvertHexStringToInteger(string hexString)
        {
            return int.Parse(hexString,
                System.Globalization.NumberStyles.HexNumber, null);
        }

        /// <summary>
        /// Convert From dd.MM.yyyy to MM/dd/yyyy datetime format
        /// </summary>
        /// <param name="s">a datetime string with dd.MM.yyyy format</param>
        /// <returns></returns>
        public static string ConvertWebDateToSQLDate(string s)
        {
            //sDate in format : dd.mm.yyyy --> mm/dd/yyyy
            string ret = s;
            try
            {
                string[] tmp = null;
                tmp = s.Trim().Split('.');
                ret = tmp[1] + "/" + tmp[0] + "/" + tmp[2];
            }
            catch //(Exception ex)
            {
                ret = s;
            }

            return ret;
        }
        /// <summary>
        /// Convert From dd.MM.yyyy to yyyymmdd datetime format
        /// </summary>
        /// <param name="s">a datetime string with dd.MM.yyyy format</param>
        /// <returns></returns>
        public static string ConvertWebDateToSAPDate(string s)
        {
            //s in format : dd.mm.yyyy --> yyyymmdd
            string ret = s;
            try
            {
                string[] tmp = null;
                tmp = s.Trim().Split('.');
                ret = tmp[2] + tmp[1] + tmp[0];
            }
            catch //(Exception ex)
            {
                ret = s;
            }

            return ret;
        }
        /// <summary>
        /// Convert webdate (dd.MM.yyyy) to Datetime variable which is common
        /// </summary>
        /// <param name="dt">a date</param>
        /// <returns></returns>
        public static DateTime ConvertWebDateToDateTime(string s)
        {
            DateTime ret = DateTime.MinValue;
            //sDate in format : dd.mm.yyyy --> datetime
            try
            {
                string[] tmp = null;
                tmp = s.Trim().Split('.');
                ret = new DateTime(Convert.ToInt32(tmp[2]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[0]));
            }
            catch //(Exception ex)
            {
                ret = DateTime.MinValue;
            }
            return ret;
        }

        /// <summary>
        /// Convert Datetime variable to dd.MM.yyyy format which is common
        /// </summary>
        /// <param name="dt">a date</param>
        /// <returns></returns>
        public static string ConvertDateTimeToWebDate(System.DateTime dt)
        {
            //dt in format : mm/dd/yyyy --> dd.mm.yyyy
            string ret = dt.ToShortDateString();
            try
            {
                ret = dt.ToString("dd.MM.yyyy");
            }
            catch //(Exception ex)
            {
                ret = dt.ToShortDateString();
            }

            return ret;
        }
        /// <summary>
        /// Convert Datetime variable to dd.MM.yyyy format which is common
        /// </summary>
        /// <param name="dt">a date</param>
        /// <returns></returns>
        public static string ConvertDateTimeToLongWebDate(System.DateTime dt)
        {
            //dt in format : mm/dd/yyyy --> dd.mm.yyyy
            string ret = dt.ToShortDateString();
            try
            {
                ret = dt.ToString("dd MMMM yyyy", IDCultureInfo);
            }
            catch //(Exception ex)
            {
                ret = dt.ToShortDateString();
            }

            return ret;
        }
        /// <summary>
        /// Convert Datetime variable to SQL Date format which is common
        /// </summary>
        /// <param name="dt">a date</param>
        /// <returns></returns>
        public static string ConvertDateTimeToSQLDate(System.DateTime dt)
        {
            //dt in format : mm/dd/yyyy --> yyyy-MM-dd HH:mm:ss
            string ret = dt.ToShortDateString();
            try
            {
                ret = dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch //(Exception ex)
            {
                ret = dt.ToShortDateString();
            }

            return ret;
        }
        /// <summary>
        /// Convert Datetime variable to SAP Date format which is common
        /// </summary>
        /// <param name="dt">a date</param>
        /// <returns></returns>
        public static string ConvertDateTimeToSAPDate(System.DateTime dt)
        {
            //dt in format : mm/dd/yyyy --> yyyyMMdd
            string ret = dt.ToShortDateString();
            try
            {
                ret = dt.ToString("yyyyMMdd");
            }
            catch //(Exception ex)
            {
                ret = dt.ToShortDateString();
            }

            return ret;
        }
        public static string ConvertDateTimeToSAPDate(System.DateTime? dt)
        {
            return ConvertDateTimeToSAPDate(dt.Value);
        }
        /// <summary>
        /// Convert a datetime string from yyyyMMdd to dd.MM.yyyy
        /// yyyyMMdd is common for SAP format for Date data type
        /// </summary>
        /// <param name="dt">a datetime string in yyyyMMdd format</param>
        /// <returns></returns>
        public static string ConvertSAPDateToWebDate(string dt)
        {
            //s in format : yyyymmdd --> dd.mm.yyyy
            string ret = string.Empty;
            try
            {
                ret = dt.Substring(6, 2) + "." + dt.Substring(4, 2) + "." + dt.Substring(0, 4);
            }
            catch
            {
            }
            return ret;
        }
        /// <summary>
        /// Convert a datetime string from yyyyMMdd to yyyy-MM-dd
        /// yyyyMMdd is common for SAP format for SQL Date data type
        /// </summary>
        /// <param name="dt">a datetime string in yyyyMMdd format</param>
        /// <returns></returns>
        public static string ConvertSAPDateToSQLDate(string dt)
        {
            //s in format : yyyymmdd --> yyyy-mm-dd
            string ret = string.Empty;
            try
            {
                ret = dt.Substring(0, 4) + "-" + dt.Substring(4, 2) + "-" + dt.Substring(6, 2);
            }
            catch
            {
            }
            return ret;
        }
        /// <summary>
        /// Convert SAP Date (yyyyMMdd) to Datetime variable which is common
        /// </summary>
        /// <param name="dt">a date</param>
        /// <returns></returns>
        public static DateTime ConvertSAPDateToDateTime(string s)
        {
            DateTime ret = DateTime.MinValue;
            //sDate in format : yyyyMMdd --> datetime
            try
            {
                string[] tmp = (string[])Array.CreateInstance(typeof(string), 3);
                tmp[0] = s.Substring(0, 4);
                tmp[1] = s.Substring(4, 2);
                tmp[2] = s.Substring(6, 2);
                ret = new DateTime(Convert.ToInt32(tmp[0]), Convert.ToInt32(tmp[1]), Convert.ToInt32(tmp[2]));
            }
            catch //(Exception ex)
            {
                ret = DateTime.MinValue;
            }
            return ret;
        }

        /// <summary>
        /// Convert a datetime string from HHmmss to HH:mm:ss
        /// HHmmss is common for SAP format for SQL Time data type
        /// </summary>
        /// <param name="dt">a datetime string in HHmmss format</param>
        /// <returns></returns>
        public static string ConvertSAPTimeToSQLTime(string dt)
        {
            //s in format : HHmmss --> HH:mm:ss
            string ret = string.Empty;
            try
            {
                ret = dt.Substring(0, 2) + ":" + dt.Substring(2, 2) + ":" + dt.Substring(4, 2);
            }
            catch
            {
            }
            return ret;
        }

        public static DataTable ConvertXmlStringToDataTable(string xmlString)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            XmlNodeReader xmlreader = new XmlNodeReader(xmlDoc);
            DataSet ds = new DataSet();
            DataTable dtdatascrip = new DataTable();
            ds.ReadXml(xmlreader);
            return ds.Tables[0];
        }

        public static DataTable ConvertJsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)JsonConvert.DeserializeObject(jsonString, (typeof(DataTable)));
            return dt;
        }
    }   
}
