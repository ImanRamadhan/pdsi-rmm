using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RigMaterialMovementWeb.Helper
{
    public class Security
    {
        #region "Fields"
        private static string _keyword = "abcdefghijklmnopqrstuvwxyz";
        #endregion

        #region "Methods"

        public static string Decrypt(string stringToDecrypt)
        {
            //ReturnVariable retVal = new ReturnVariable { ReturnType = "S" };
            string retVal;
            string decryptedString = "";

            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] key = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(_keyword));
            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();

            tripleDESCryptoServiceProvider.Key = key;
            tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
            tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
            byte[] array = Convert.FromBase64String(stringToDecrypt);
            byte[] bytes = null;
            try
            {
                ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateDecryptor();
                bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                decryptedString = uTF8Encoding.GetString(bytes);
                //retVal.SetValue("S", "", decryptedString);
                retVal = decryptedString;
            }
            catch (Exception ex)
            {
                //retVal.SetValue("E", ex.ToString, ex);
                //EventLogHelper.WriteEventLog("CryptoHelper", retVal, stringToDecrypt);
                throw ex;
            }
            finally
            {
                tripleDESCryptoServiceProvider.Clear();
                mD5CryptoServiceProvider.Clear();
            }
            return retVal;
        }

        public static string Encrypt(string stringToEncrypt)
        {
            //ReturnVariable retVal = new ReturnVariable { ReturnType = "S" };
            string retVal;
            string encryptedString = "";

            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] key = mD5CryptoServiceProvider.ComputeHash(uTF8Encoding.GetBytes(_keyword));
            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
            tripleDESCryptoServiceProvider.Key = key;
            tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
            tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
            byte[] bytes = uTF8Encoding.GetBytes(stringToEncrypt);
            byte[] inArray = null;

            try
            {
                ICryptoTransform cryptoTransform = tripleDESCryptoServiceProvider.CreateEncryptor();
                inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
                encryptedString = Convert.ToBase64String(inArray);
                //retVal.SetValue("S", "", encryptedString);
                retVal = encryptedString;
            }
            catch (Exception ex)
            {
                //retVal.SetValue("E", ex.ToString, ex);
                //EventLogHelper.WriteEventLog("CryptoHelper", retVal, stringToEncrypt);
                throw ex;
            }
            finally
            {
                tripleDESCryptoServiceProvider.Clear();
                mD5CryptoServiceProvider.Clear();
            }

            return retVal;
        }

        #endregion
        private const string ENCRYPTION_KEY = "key";

        private readonly static byte[] SALT = Encoding.ASCII.GetBytes(ENCRYPTION_KEY.Length.ToString());
        
        public static string DecryptQueryString(string inputText)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            byte[] encryptedData = Convert.FromBase64String(inputText);
            PasswordDeriveBytes secretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);

            using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
            {
                using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] plainText = new byte[encryptedData.Length];
                        int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                        return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                    }
                }
            }
        }
    }
}