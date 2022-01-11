using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kairos.Library.DataTypeHelper
{
    public static class StringHelper
    {
        public static string SetEmptyStringIfNull(object obj)
        {
            try
            {
                return obj.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string FileName(string fullPath)
        {
            if (fullPath != null)
            {
                int pos = fullPath.LastIndexOf(@"\");

                if (pos > 0)
                {
                    return fullPath.Substring(pos + 1);
                }
            }
            return fullPath;
        }

        public static string FileExtension(string fullPath)
        {
            if (fullPath != null && fullPath.Length > 0)
            {
                string fileName = FileName(fullPath);

                int pos = fileName.LastIndexOf(".");

                if (pos > 0)
                {
                    return fileName.Substring(pos + 1);
                }
            }
            return string.Empty;
        }

        public static bool IsNumeric(String strVal)
        {
            Regex reg = new Regex("[^0-9-]");
            Regex reg2 = new Regex("^-[0-9]+$|^[0-9]+$");
            return (!reg.IsMatch(strVal) && reg2.IsMatch(strVal));
        }

        public static string[] SplitWord(string input, params char[] CharSeparator)
        {
            return input.Split(CharSeparator, StringSplitOptions.RemoveEmptyEntries);
        }

        public static int WordCount(string input, params char[] CharSeparator)
        {
            return input.Split(CharSeparator,
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static String NulltoEmptyString(String input)
        {
            String Output = input;
            if (Output == null)
            {
                Output = "";
            }
            else
            {
                Output = Output.Trim();
            }
            return Output;
        }
    }
}
