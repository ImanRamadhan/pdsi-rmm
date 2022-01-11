using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kairos.Library.CommonHelper.Spelling
{
    public class CurrencyInWordGeneral
    {
        string oneWords = ",Satu,Dua,Tiga,Empat,Lima,Enam,Tujuh,Delapan,Sembilan,Sepuluh,Sebelas,Dua Belas,Tiga Belas,Empat Belas,Lima Belas,Enam Belas,Tujuh Belas,Delapan Belas,Sembilan Belas";
        string tenWords = ",Sepuluh,Dua Puluh,Tiga Puluh,Empat Puluh,Lima Puluh,Enam Puluh,Tujuh Puluh,Delapan Puluh,Sembilan Puluh";
        string[] ones = null;
        string[] tens = null;
        //private string CurrencyDescription;

        public CurrencyInWordGeneral()
        {
            ones = oneWords.Split(',');
            tens = tenWords.Split(',');
            //this.CurrencyDescription = currencyDescription;
        }

        private string LoadCurrencySpelling(string Currency)
        {
            string Result = Currency;
            //try
            //{
            //    using (SqlConnection conn = SQLAccessHelper.GetDBConnection(Constants.DB_CONNECTIONS_SAP_MASTER_DATA))
            //    {
            //        using (SqlDataReader dr = SQLAccessHelper.DoSqlQuery(CommandType.Text, "SELECT Description_Currency FROM TblM_Currency WHERE ID_Currency_PK='" + Currency + "'", null, conn))
            //        {
            //            if (dr.HasRows)
            //            {
            //                dr.Read();
            //                Result = dr["Description_Currency"].ToString();
            //            }
            //        }
            //    }
            //}
            //catch
            //{
            //    Result = Currency;
            //}
            return Result;
        }
        
        private string ConvertTens(Double input)
        {
            string output = null;
            if (input < 20)
            {
                output = ones[Convert.ToInt32(input)];
                input = 0;
            }
            else
            {
                output = tens[Convert.ToInt32(Math.Floor(input / 10))];
                input -= Math.Floor(input / 10) * 10;
            }
            output = output + (string.IsNullOrEmpty(ones[Convert.ToInt32(input)].Trim()) ? "" : "-" + ones[Convert.ToInt32(input)]);
            return output;
        }
        private string ConvertHundreds(Double input)
        {
            string output = null;
            if (input <= 99)
            {
                output = (ConvertTens(input));
            }
            else
            {
                output = ones[Convert.ToInt32(Math.Floor(input / 100))];
                output += " Ratus ";
                if (input - Math.Floor(input / 100) * 100 == 0)
                {
                    output += "";
                }
                else
                {
                    output += "" + ConvertTens(input - Math.Floor(input / 100) * 100);
                }
            }
            return output;
        }
        public string ConvertToWord(string input, string curr)
        {
            string mataUang = null;
            string sen = "SEN";
            mataUang = LoadCurrencySpelling(curr);  // CurrencyDescription;
            //if (curr.ToUpper() == "IDR")
            //{
            //    mataUang = "RUPIAH";
            //    sen = "SEN";
            //}
            //else if (curr.ToUpper().StartsWith("USD"))
            //{
            //    mataUang = "US-DOLLAR";
            //    sen = "CENTS";
            //}
            //else
            //{
            //    mataUang = curr.ToUpper();
            //    sen = "SEN";
            //}

            input = input.Replace("$", "").Replace(",", "");
            if (input.Length > 16)
                return " - ";
            //"Cannot convert up to billion<BR>Maximum limits has reached (Max. Convert limits = 999,999,999.999)"

            string dollars = null;
            string cents = null;
            if (input.IndexOf(".") > 0)
            {
                dollars = input.Substring(0, input.IndexOf('.')).PadLeft(12, '0');
                cents = input.Substring(input.IndexOf('.') + 1).PadRight(2, '0');
                if (cents == "00")
                    cents = "0";
            }
            else
            {
                dollars = input.PadLeft(12, '0');
                cents = "0";
            }
            string output = null;
            int bill = 0;
            int mill = 0;
            int thou = 0;
            int hund = 0;
            int cent = 0;
            string bills = null;
            string mills = null;
            string thous = null;
            string hunds = null;
            bill = Convert.ToInt32(dollars.Substring(0, 3));
            mill = Convert.ToInt32(dollars.Substring(3, 3));
            thou = Convert.ToInt32(dollars.Substring(6, 3));
            hund = Convert.ToInt32(dollars.Substring(9, 3));
            cent = Convert.ToInt32(cents);
            bills = ConvertHundreds(bill);
            mills = ConvertHundreds(mill);
            thous = ConvertHundreds(thou);
            hunds = ConvertHundreds(hund);
            cents = ConvertHundreds(cent);
            output = (string.IsNullOrEmpty(bills.Trim()) ? "" : bills + " Milyar ");
            output += (string.IsNullOrEmpty(mills.Trim()) ? "" : mills + " Juta ");
            output += (string.IsNullOrEmpty(thous.Trim()) ? "" : thous + " Ribu ");
            output += (string.IsNullOrEmpty(hunds.Trim()) ? "" : hunds);
            output = (output.Length == 0 ? " " : output + " " + mataUang);
            output = (output == "One " + mataUang ? "One " + mataUang : output);
            output += (string.IsNullOrEmpty(cents) ? "" : " Dan " + cents + " " + sen);
            return output.ToUpper();
        }
    }
}
