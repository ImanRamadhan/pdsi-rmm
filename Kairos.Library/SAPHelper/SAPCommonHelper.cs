using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kairos.Library.SAPHelper
{
    public static class SAPCommonHelper
    {

        public static string GenerateINCond(string ColumnName, List<string> CondItemList)
        {
            int MaxPartialOptionsLength = 72;
            string FieldDelimiter = ";";
            List<string> CondItemListDistinct = new List<string>();
            List<string> INCondList = new List<string>();
            string AllStr = string.Empty;
            string FinalResult = string.Empty;
            if (CondItemList != null && (CondItemList.Count > 0))
            {
                foreach (string CondItem in CondItemList)
                {
                    if ((!CondItemListDistinct.Contains(CondItem)) && (CondItem != null) && (CondItem.Trim().Length > 0))
                    {
                        CondItemListDistinct.Add(CondItem.Trim());
                        AllStr += string.Format("'{0}',", CondItem.Trim());
                    }
                }
                AllStr = AllStr.TrimEnd(',');
                if (AllStr.Length > 0)
                {
                    string TmpStr = string.Empty;
                    StringBuilder sb = new StringBuilder();
                    string CondTemplate = ColumnName + " IN (;{0})";
                    while (AllStr.Length > MaxPartialOptionsLength)
                    {
                        TmpStr = AllStr.Substring(0, MaxPartialOptionsLength);
                        AllStr = AllStr.Substring(MaxPartialOptionsLength);
                        sb.Append(TmpStr);
                        sb.Append(FieldDelimiter);
                    }
                    if (AllStr.Trim().Length > 0)
                    {
                        sb.Append(AllStr);
                        sb.Append(FieldDelimiter);
                    }
                    FinalResult = string.Format(CondTemplate, sb.ToString());
                }
                else
                {
                    // INCondList.Add(string.Empty);
                }
            }
            else
            {
                // INCondList.Add(string.Empty);
            }
            return FinalResult;
        }

        public static List<string> GenerateINCondList(string ColumnName, List<string> CondItemList, int CountPerCond)
        {
            List<string> Result = new List<string>();
            List<string> TmpCondItemList = new List<string>();
            int CondCount = 0;
            foreach (string CondItem in CondItemList)
            {
                TmpCondItemList.Add(CondItem);
                CondCount++;
                if (CondCount >= CountPerCond)
                {
                    Result.Add(GenerateINCond(ColumnName, TmpCondItemList));
                    TmpCondItemList = new List<string>();
                    CondCount = 0;
                }
            }
            if (TmpCondItemList.Count > 0)
            {
                Result.Add(GenerateINCond(ColumnName, TmpCondItemList));
                TmpCondItemList = new List<string>();
                CondCount = 0;
            }
            return Result;
        }
        
    }
}
