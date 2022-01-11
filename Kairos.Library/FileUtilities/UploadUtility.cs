using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Share = Microsoft.SharePoint.Client;
using Microsoft.SharePoint;
using System.Configuration;

namespace Kairos.Library.FileUtilities
{
    public class UploadUtility
    {
        static FileStream filestream = null;

        public static string UploadFile(String FullPathDestinationFile,
                                        String FullPathSourceFile,
                                        //bool IsSecureNetwork = false,
                                        string username = "",
                                        string password = "")
        {
            WebClient webClient = new WebClient();

            try
            {
                //if (IsSecureNetwork)
                    webClient.Credentials = new NetworkCredential(username, password);

                //delete file if exists 
                    if (File.Exists(FullPathDestinationFile))
                        File.Delete(FullPathDestinationFile);

                    webClient.UploadFile(FullPathDestinationFile, "PUT", FullPathSourceFile);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                webClient.Dispose();
                ////filestream.Close();
                ////filestream.Dispose();
            }
            return FullPathDestinationFile;
        }

        //public static string UploadByte(byte[] file, string filename, string toSharepoint)
        public static string UploadByte(byte[] file, string PathFolderSourceFile, string SourceFileName, string FullPathDestinationFile)
        {
            try
            {
                //string[] FoldersPath = toSharepoint.Split('/');
                //string FolderCreated = string.Empty;
                //if (FoldersPath.Length == 4)
                //    FolderCreated = FoldersPath[2];
                //else if (FoldersPath.Length == 5)
                //    FolderCreated = FoldersPath[2] + "/" + FoldersPath[3];
                //else if (FoldersPath.Length == 6)
                //    FolderCreated = FoldersPath[2] + "/" + FoldersPath[3] + "/" + FoldersPath[4];
                //else
                //{
                //    FoldersPath = new string[] { "", "Documents" };
                //    FolderCreated = "UnknownDocument";
                //}

                //CreateFolder(FolderCreated, FoldersPath[1]);

                string path = HostingEnvironment.MapPath(PathFolderSourceFile);
                string FullPathSourceFile = path + SourceFileName;

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                filestream = new FileStream(FullPathSourceFile, FileMode.Create, FileAccess.ReadWrite);
                filestream.Write(file, 0, file.Length);
                //byte[] bytes = new byte[filestream.Length];
                //filestream.Read(file, 0, (int)filestream.Length);

                filestream.Close();
                filestream.Dispose();

                //destination = FullPathDestinationFile;
                UploadFile(FullPathDestinationFile, FullPathSourceFile);

                if (File.Exists(FullPathSourceFile))
                    File.Delete(FullPathSourceFile);

                return FullPathDestinationFile;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /* REMARK BECAUSE SAME WITH METHOD ABOVE */
        //public static string UploadLib(byte[] file, string filename, string toSharepoint, string strLibrary)
        //{
        //    try
        //    {
        //        String[] foldersPath = toSharepoint.Split('/');
        //        int f;
        //        String folderCreated = "";
        //        for (f = 1; f < foldersPath.Length - 1; f++)
        //        {
        //            if (f == 1)
        //                folderCreated = folderCreated + foldersPath[f];
        //            else
        //                folderCreated = folderCreated + "/" + foldersPath[f];
        //        }
        //        CreateFolder(folderCreated, strLibrary);

        //        string pathLocal = HostingEnvironment.MapPath(folder) + filename;
        //        string path = System.Web.Hosting.HostingEnvironment.MapPath(folder);
        //        if (!Directory.Exists(path))
        //            Directory.CreateDirectory(path);

        //        FileStream fileStream = new FileStream(pathLocal, FileMode.Create, FileAccess.ReadWrite);
        //        fileStream.Write(file, 0, file.Length);

        //        string destination = server + "/" + toSharepoint;
        //        UploadFile(destination, pathLocal);
        //        return destination;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        filestream.Close();
        //        filestream.Dispose();
        //    }
        //}

        /* REMARK BECAUSE NOT GENERAL */
        //public static string GetPathByName(int vendorId, String vendorName)
        //{
        //    try
        //    {
        //        String library = "Vendor";
        //        String month = DateTime.Now.ToString("MMM");
        //        String year = DateTime.Now.Year.ToString();
        //        String companyFolder = vendorId + "-" + vendorName;
        //        String path = server + "/" + library + "/" + companyFolder + "/" + year + "/" + month + "/";
        //        return path;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        /* REMARK BECAUSE NOT GENERAL */
        //public string GetPathByLib(string library)
        //{
        //    try
        //    {
        //        String month = DateTime.Now.ToString("MMM");
        //        String year = DateTime.Now.Year.ToString();
        //        String path = server + "/" + library + "/" + year + "/" + month + "/";
        //        return path;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}


        public void UploadDocument(string filepath, string SharepointSite, string DoclibName)
        {
            String fileToUpload = filepath;
            String sharePointSite = SharepointSite;
            String documentLibraryName = DoclibName;

            using (SPSite oSite = new SPSite(sharePointSite))
            {
                using (SPWeb oWeb = oSite.OpenWeb())
                {
                    if (!System.IO.File.Exists(fileToUpload))
                        throw new FileNotFoundException("File not found.", fileToUpload);

                    SPFolder myLibrary = oWeb.Folders[documentLibraryName];

                    // Prepare to upload
                    Boolean replaceExistingFiles = true;
                    String fileName = System.IO.Path.GetFileName(fileToUpload);
                    FileStream fileStream = File.OpenRead(fileToUpload);

                    // Upload document
                    //SPFile spfile = myLibrary.Files.Add(fileName, fileStream, replaceExistingFiles);

                    // Commit 
                    myLibrary.Update();
                }
            }
        }

        public void UploadSharepoint(string urllist, byte[] filepathsource, string filename, string listname)
        {
            using (System.IO.MemoryStream fs = new MemoryStream(filepathsource))
            {
                NetworkCredential credential = new NetworkCredential(ConfigurationManager.AppSettings["UserSharepoint"], ConfigurationManager.AppSettings["PasswordSharepoint"], ConfigurationManager.AppSettings["DomainSharepoint"]);
                Share.ClientContext context = new Share.ClientContext(urllist);
                context.Credentials = credential;
                Share.Web web = context.Web;
                Share.FileCreationInformation newFile = new Share.FileCreationInformation();
                newFile.ContentStream = fs;
                //System.IO.File.ReadAllBytes(filepathsource);
                newFile.Url = filename;
                Share.List docs = web.Lists.GetByTitle(listname);
                Share.File uploadFile = docs.RootFolder.Files.Add(newFile);
                context.Load(uploadFile);
                context.ExecuteQuery();
            }
        }

        public string UploadSharepoint(string urllist, byte[] filepathsource, string filename, string listname, string folder)
        {
            using (System.IO.MemoryStream fs = new MemoryStream(filepathsource))
            {
                NetworkCredential credential = new NetworkCredential(ConfigurationManager.AppSettings["UserSharepoint"], ConfigurationManager.AppSettings["PasswordSharepoint"], ConfigurationManager.AppSettings["DomainSharepoint"]);
                Share.ClientContext context = new Share.ClientContext(urllist);
                context.Credentials = credential;

                try
                {
                    Share.Web web = context.Web;
                    Share.FileCreationInformation newFile = new Share.FileCreationInformation();
                    newFile.ContentStream = fs;
                    newFile.Overwrite = true;
                    //System.IO.File.ReadAllBytes(filepathsource);
                    newFile.Url = filename;
                    Share.List docs = web.Lists.GetByTitle(listname);
                    Share.Folder subFolder;
                    subFolder = docs.RootFolder.Folders.Add(folder);
                    Share.File uploadFile = subFolder.Files.Add(newFile);
                    context.Load(uploadFile);
                    context.ExecuteQuery();

                    return "Successfully Upload File";
                }
                catch (Exception error)
                {
                    return error.Message;
                }
                finally
                {
                    context.Dispose();
                }
            }
        }
    }
}
