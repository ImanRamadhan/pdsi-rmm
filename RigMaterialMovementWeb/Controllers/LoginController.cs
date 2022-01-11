using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RigMaterialMovementWeb.Models;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Data;
using RigMaterialMovementWeb.ViewModel;
using RigMaterialMovementWeb.Helper;
using System.Web.Security;
using System.Configuration;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace RigMaterialMovementWeb.Controllers
{
    public class LoginController : Controller
    {
        Authentication auth = new Authentication();
        Security sec = new Security();
        private string DomainName = ConfigurationManager.AppSettings["LDAPDomainName"];
        private string DomainServerIPAddress1 = ConfigurationManager.AppSettings["LDAPDomainIPAddress1"];
        private string DomainServerIPAddress2 = ConfigurationManager.AppSettings["LDAPDomainIPAddress2"];


        //// GET: Login
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            string name = User.Identity.Name;
            bool isLogged = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            if (isLogged && !String.IsNullOrEmpty(name))
            {
               
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                if (Session["Newusername"] == null)
                {
                    FormsAuthentication.SignOut();
                    Session.RemoveAll();
                    //Response.Redirect("~/Page/Login/Login2.aspx");
                    return RedirectToAction("Login", "Login");
                }
                string username = Session["Newusername"].ToString();
                var UserRole = DB.M_RoleManagement.Where(b => b.username == username).FirstOrDefault();
                if(UserRole == null)
                {
                    return RedirectToAction("ErrorValidate", "Error");
                }
                else
                {
                    return RedirectToLocal("~/Home/Index");
                }
                
                //}
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var userAccount = model.Username;
                var getBackSlash = userAccount.Contains("\\");
                string userName;
                if (getBackSlash)
                {
                    string[] temp = Convert.ToString(userAccount).Split('\\');
                    userName = temp[1];
                }
                else
                    userName = userAccount;

                // Password
                var password = model.Password;

                if (userName != null && password != null)
                {
                    //HttpClient clients = new HttpClient();
                    //string APIUrls = "https://apps.pertamina.com/pdsidas/ldap/api/Auth/DoAction";
                    //var param1 = new FormUrlEncodedContent(new[]
                    //    {
                    //         new KeyValuePair<string, string>("UserName", "test.test1"),
                    //         new KeyValuePair<string, string>("Password", "password.1"),
                    //         new KeyValuePair<string, string>("RememberMe", "0"),
                    //         new KeyValuePair<string, string>("method", "LOGIN")
                    //});
                    //HttpResponseMessage Responses1 = clients.PostAsync(APIUrls, param1).Result;
                    //string token = "";
                    //if (Responses1.IsSuccessStatusCode)
                    //{
                    //    var response = Responses1.Content.ReadAsStringAsync().Result;
                    //    JObject j = JObject.Parse(response);
                    //    token = j["Token"].ToString();
                    //}

                    string token = ConfigurationManager.AppSettings["TokenLogin"].ToString();

                    string methodLDAP = "validate";
                    string bypass = ConfigurationManager.AppSettings["ByPassLogin"].ToString();
                    bool value = bool.Parse(bypass);
                    if (value == false)
                    {
                        methodLDAP = "login";
                    }

                        HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Token", token);
                    string status = null;
                    JObject resultData = null;
                    if (userName != null && password != null)
                    {
                        string APIUrl = ConfigurationManager.AppSettings["LDAPUrl"].ToString();
                        var param = new FormUrlEncodedContent(new[]
                        {
                        new KeyValuePair<string, string>("Password", password),
                        new KeyValuePair<string, string>("Method", methodLDAP),
                        new KeyValuePair<string, string>("UserName", userName)
                    });
                        HttpResponseMessage Responses = client.PostAsync(APIUrl, param).Result;
                        if (Responses.IsSuccessStatusCode)
                        {
                            var Data = Responses.Content.ReadAsStringAsync().Result;
                            resultData = JObject.Parse(Data);
                            status = resultData["Status"].ToString();
                        }
                        if (status == "00")
                        {
                            string userEmail = resultData["Data"]["Email"].ToString();
                            Session["Username"] = Security.Encrypt(userName);
                            Session["Newusername"] = userName;
                            string user = Security.Encrypt(userName);
                            FormsAuthentication.SetAuthCookie(Session["Username"].ToString(), false);
                            string asd = Security.Decrypt(Session["Username"].ToString());
                            Session["EmployeeName"] = resultData["Data"]["NamaLengkap"].ToString();
                            Session["UserAccount"] = asd;

                            DB_RMMEntities DB = new DB_RMMEntities();
                            ResponseMessage Response = new ResponseMessage();
                            string username = Session["Newusername"].ToString();
                            var UserRole = DB.M_RoleManagement.Where(b => b.username == username).FirstOrDefault();
                            if (UserRole == null)
                            {
                                return RedirectToAction("ErrorAccess", "Error");
                            }
                            else
                            {
                                return RedirectToLocal("~/Home/Index");
                            }
                        }
                        else
                        {
                            Session["Warning"] = "User Not Authenticated!";
                            //Response.Redirect("~/Page/Login/Login2.aspx", false);
                            return View(model);
                        }
                    }
                    else
                    {
                        Session["Warning"] = "User Not Authenticated!";
                        //Response.Redirect("~/Page/Login/Login2.aspx", false);
                        return View(model);
                    }


                    //if (auth.IsAuthenticated(DomainServerIPAddress1, DomainServerIPAddress2, userName, password))
                    //{
                    //    Session["Username"] = Security.Encrypt(userName);
                    //    Session["Newusername"] = userName;
                    //    string user = Security.Encrypt(userName);
                    //    FormsAuthentication.SetAuthCookie(Session["Username"].ToString(), false);
                    //    string asd = Security.Decrypt(Session["Username"].ToString());
                    //    Session["EmployeeName"] = auth.displayName;
                    //    Session["UserAccount"] = asd;



                    //    DB_RMMEntities DB = new DB_RMMEntities();
                    //    ResponseMessage Response = new ResponseMessage();
                    //    string username = Session["Newusername"].ToString();
                    //    var UserRole = DB.M_RoleManagement.Where(b => b.username == username).FirstOrDefault();
                    //    if (UserRole == null)
                    //    {
                    //        return RedirectToAction("ErrorAccess", "Error");
                    //    }
                    //    else
                    //    {
                    //        return RedirectToLocal("/Home/Index");
                    //    }
                    //    //return RedirectToAction("Index", "Home");
                    //}
                    //else
                    //{
                    //    Session["Warning"] = "User Not Authenticated!";
                    //    //Response.Redirect("~/Page/Login/Login2.aspx", false);
                    //    return View(model);
                    //}
                }
            }
            catch(Exception e)
            {
                return View(model);
            }
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Session.RemoveAll();
            Response.Cookies.Add(new HttpCookie("UP", ""));
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOut()
        {
            try
            {
                //IAuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            catch
            {

            }
            return RedirectToAction("Login", "Login");
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}