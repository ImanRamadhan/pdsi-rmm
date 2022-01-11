using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Configuration;
//using Kairos.Library.UserManagement;
//using Kairos.Library.DataAccessHelper;
using System.Data;

namespace RigMaterialMovementWeb.Helper
{
    //rahmat
    public class Authentication
    {
        public string displayName = "";
        public bool IsAuthenticated(string domainName1, string domainName2, string userName, string password)
        {
            //ReturnVariable retVal = new ReturnVariable { ReturnType = "S" };
            bool isValid = false;
            try
            {
                string path1 = string.Format("LDAP://{0}", domainName1);
                string path2 = string.Format("LDAP://{0}", domainName2);

                string domainAndUsername = "Kairos" + @"\" + userName;
                using (DirectoryEntry adsEntry = new DirectoryEntry(path1, domainAndUsername, password))
                {
                    //Bind to the native AdsObject to force authentication.
                    object obj = adsEntry.NativeObject;

                    using (DirectorySearcher adsSearcher = new DirectorySearcher(adsEntry)
                    {
                        Filter = "(samaccountname= " + userName + ")",
                        PropertiesToLoad = { "cn", "memberOf" }
                    })
                    {
                        try
                        {
                            string name = "";
                            SearchResult result = adsSearcher.FindOne();
                            using (DirectoryEntry userResult = result.GetDirectoryEntry())
                            {
                                displayName = userResult.Properties["displayName"].Value.ToString();
                            }
                            if (result != null)
                                isValid = true;
                           
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error authenticating user. " + ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Error Message: {0}\r\n", ex.Message);
                sb.AppendFormat("Error Source: {0}\r\n", ex.Source);
                sb.AppendFormat("Error Stack Trace: {0}\r\n", ex.StackTrace);
                isValid = false;
                return isValid;
            }
            finally
            {
            }
            return isValid;
        }

        public bool IsUserExists(string domainName, string userName)
        {
            string path1 = string.Format("LDAP://{0}", domainName);
            //bool isExist = false;
            // create your domain context
            using (PrincipalContext domain = new PrincipalContext(ContextType.Domain, path1))
            {
                // find the user
                UserPrincipal foundUser = UserPrincipal.FindByIdentity(domain, IdentityType.SamAccountName, userName);


                return foundUser != null;

            }
        }

        public bool Exists(string objectPath)
        {
            bool found = false;
            if (DirectoryEntry.Exists("LDAP://" + objectPath))
            {
                found = true;
            }
            return found;
        }
        public bool AutomaticLogin(string domainName1, string userName1)
        {
            bool isValid = false;
            try
            {
                string path1 = string.Format("LDAP://{0}", domainName1);
                string userName = "";
                string password = "";

                string domainAndUsername = "Kairos" + @"\" + userName;
                using (DirectoryEntry adsEntry = new DirectoryEntry(path1, domainAndUsername, password))
                {
                    object obj = adsEntry.NativeObject;

                    using (DirectorySearcher adsSearcher = new DirectorySearcher(adsEntry)
                    {
                        Filter = "(samaccountname= " + userName1 + ")",
                        PropertiesToLoad = { "cn", "memberOf" }
                    })
                    {
                        try
                        {
                            string name = "";
                            SearchResult result = adsSearcher.FindOne();
                            using (DirectoryEntry userResult = result.GetDirectoryEntry())
                            {
                                displayName = userResult.Properties["displayName"].Value.ToString();
                            }
                            if (result != null)
                                isValid = true;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error authenticating user. " + ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Error Message: {0}\r\n", ex.Message);
                sb.AppendFormat("Error Source: {0}\r\n", ex.Source);
                sb.AppendFormat("Error Stack Trace: {0}\r\n", ex.StackTrace);
                isValid = false;
                return isValid;
            }
            finally
            {
            }
            return isValid;
        }
    }

    //umkonline
    /*
    public class LoginAuthentication
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public static object UserProfile { get; private set; }

        public bool IsAuthentication(string path, string username, string password)
        {
            string bypass = ConfigurationManager.AppSettings["ByPassLogin"].ToString();
            bool value = bool.Parse(bypass);
            if (value == true) return value;
            ActiveDirectory ADHelper = new ActiveDirectory(path, username, password, string.Empty);
            string l = ADHelper.Login(username, password).ToString();
            if (l == "LOGIN_OK")
            {
                value = true;
            }
            return value;
        }

        public LoginResult Login(string userName, string password)
        {
            var result = new LoginResult();

            if (userName != "" && password != "")
            {
                string ADpath = "LDAP://" + "PT. PERTAMINA (PERSERO)";
                bool MatchwithAD = IsAuthentication(ADpath, userName, password);

                if (MatchwithAD)
                {
                    var token = new Token();
                    token.UserName = userName;
                    token.IssuedDate = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss");
                    result.prof = new UserProfile(token);
                    if (result.prof.UserName == null)
                    {
                        result.Status = "F";
                        result.Message = "Username doesn't exists";
                    }
                    else
                    {
                        result.Status = "S";
                        result.Message = "Login Success";
                        result.Token = token;
                    }

                }
                else
                {
                    result.Status = "F";
                    result.Message = "Username and password is not correct";
                }

            }
            else
            {
                result.Status = "F";
                result.Message = "Username or password cannot be empty";
            }

            return result;
        }
    }

    public class LoginResult
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Token Token { get; set; }
        public UserProfile prof { get; set; }
    }

    public class Token
    {
        public string UserName { get; set; }
        public string IssuedDate { get; set; }
        public string ExpireDate { get; set; }
        public string LoginID { get; set; }
    }

    public UserProfile(Token token)
    {
        var param = Kairos.Library.DataAccessHelper.SQLAccessHelper.NewParam;
        param.Add("@UserName", token.UserName);

        var up = new DataTable();
        var roleGroups = new DataTable();
        var roles = new DataTable();
        var authparams = new DataTable();

        
    }
    */
}