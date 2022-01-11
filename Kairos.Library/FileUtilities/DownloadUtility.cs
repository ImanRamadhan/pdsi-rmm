using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Kairos.Library.FileUtilities
{
    public class DownloadUtility
    {
        static byte[] bytes;
        //static string username = ConfigurationManager.AppSettings["UserSp"].ToString();
        //static string password = ConfigurationManager.AppSettings["PasswordSp"].ToString();
        //static string server = ConfigurationManager.AppSettings["ServerSp"].ToString();
        //static string domain = ConfigurationManager.AppSettings["DomainSp"].ToString();
        //static string folder = ConfigurationManager.AppSettings["FolderSp"].ToString();

        //static string sourceFile = string.Empty;
        static string destination = string.Empty;
        static string filename = string.Empty;

        static FileStream filestream = null;
        static BinaryReader binaryReader = null;

        public static byte[] DownloadByte(string AddressSourceFile, 
                                            //string FolderDestination, 
                                            //bool IsSecureNetwork = false, 
                                            string Username = "", 
                                            string Password = "")
        {
            WebClient webClient = new WebClient();
            //if (IsSecureNetwork)
                webClient.Credentials = new NetworkCredential(Username, Password);

            try
            {
                //String[] filePath = AddressSourceFile.Split('/');
                //filename = filePath[filePath.Length - 1];
                ////set destination address
                //destination = HostingEnvironment.MapPath(FolderDestination) + filename;

                //string localPath = new Uri(destination).LocalPath;
                bytes = webClient.DownloadData(AddressSourceFile);
                
                //if (File.Exists(destination))
                //    File.Delete(destination);
                
                return bytes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public struct Document
        {
            public int DocId { get; set; }
            public string DocName_Old { get; set; }
            public string DocName_New { get; set; }
            public byte[] DocContent { get; set; }
            public string DocPath { get; set; }
        }

        public static Document DownloadDocument(string AddressSourceFile,
                                                string FolderDestination, 
                                                //bool IsSecureNetwork = false, 
                                                string username = "", 
                                                string password = "" )
        {
            WebClient webClient = new WebClient();
            //if (IsSecureNetwork)
                webClient.Credentials = new NetworkCredential(username, password);

            try
            {
                byte[] fileContent = null;
                String[] filePath = AddressSourceFile.Split('/');
                filename = filePath[filePath.Length - 1];

                //set destination address
                destination = HostingEnvironment.MapPath(FolderDestination) + filename;
                webClient.DownloadFile(AddressSourceFile, destination);

                //openfile and convert to byte
                filestream = File.Open(destination, FileMode.Open, FileAccess.Read);
                bytes = new byte[filestream.Length];
                filestream.Read(bytes, 0, (int)filestream.Length);

                //convert stream to binary
                binaryReader = new BinaryReader(filestream);
                long byteLength = new FileInfo(destination).Length;
                fileContent = binaryReader.ReadBytes((Int32)byteLength);

                //disposed
                webClient.Dispose();
                filestream.Close();
                filestream.Dispose();
                binaryReader.Close();

                //set document property
                Document Doc = new Document();
                Doc.DocName_Old = filename;
                Doc.DocContent = fileContent;
                return Doc;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DownloadFile(string AddressSourceFile, 
                                            //bool IsSecureNetwork = false,
                                            string Username = "",
                                            string Password = "")
        {
            try
            {
                HttpContext Context = HttpContext.Current;
                HttpResponse Response = HttpContext.Current.Response;
                string FileName = Path.GetFileName(AddressSourceFile);
                string Extension = Path.GetExtension(FileName);
                //byte[] Content = DownloadByte(AddressSourceFile, IsSecureNetwork, Username, Password);
                byte[] Content = DownloadByte(AddressSourceFile, Username, Password);

                Context.Response.ClearContent();    
                Context.Response.Clear();
                Context.Response.ContentType = ReturnExtension(Extension);
                Context.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Context.Response.BufferOutput = true;
                Context.Response.OutputStream.Write(Content, 0, Content.Length);
                Context.ApplicationInstance.CompleteRequest();
                Context.Response.Flush();
                Context.ApplicationInstance.CompleteRequest();
            }
            //Context.Response.End();
            catch (System.Threading.ThreadAbortException abrtEX)
            {
                throw abrtEX;
            }
        }

        public static void DownloadFile(string AddressSourceFile, byte[] ContentByte)
        {
            try
            {
                HttpContext Context = HttpContext.Current;
                HttpResponse Response = HttpContext.Current.Response;
                string FileName = Path.GetFileName(AddressSourceFile);
                string Extension = Path.GetExtension(FileName);

                Context.Response.ClearContent();
                Context.Response.Clear();
                Context.Response.ContentType = ReturnExtension(Extension);
                Context.Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Context.Response.BufferOutput = true;
                Context.Response.OutputStream.Write(ContentByte, 0, ContentByte.Length);
                Context.Response.Flush();
                Context.ApplicationInstance.CompleteRequest();
                //Context.Response.End();
            }
            catch (System.Threading.ThreadAbortException abrtEX)
            {
                throw abrtEX;
            }
        }

        public static string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
