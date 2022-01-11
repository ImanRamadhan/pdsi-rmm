using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using RigMaterialMovementWeb.Models;
using System.Web.Security;

namespace RigMaterialMovementWeb.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master
        public ActionResult Index()
        {
            DB_RMMEntities DB = new DB_RMMEntities();
            if (Session["Newusername"] == null)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(n => n.username == username).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;

            return View();
        }

        public ActionResult CreateTransporter(TransporterList Model)
        {
            try
            {

                DB_RMMEntities DB = new DB_RMMEntities();
                if (Session["Newusername"] == null)
                {
                    FormsAuthentication.SignOut();
                    Session.RemoveAll();
                    //Response.Redirect("~/Page/Login/Login2.aspx");
                    return RedirectToAction("Login", "Login");
                }
                string username = Session["Newusername"].ToString();
                var UserRole = DB.M_RoleManagement.Where(n => n.username == username).FirstOrDefault();
                var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
                DDLRole ddl = new DDLRole
                {
                    name = Role.name
                };
                ViewBag.Role = ddl;
                ResponseMessage Response = new ResponseMessage();

                var exist = DB.M_Transporter.Where(x => x.name == Model.name).Count();
                if (exist == 0)
                {

                    M_Transporter trans = new M_Transporter
                    {
                        name = Model.name,
                        last_modified_by = User.Identity.Name,
                        last_modified_date = DateTime.Now,
                    };
                    //Insert to Database

                    DB.M_Transporter.Add(trans);
                    DB.SaveChanges();
                    //Define error -> true and return Data
                    Response.HasAnError = false;
                    return Content("success");
                }
                else
                {
                    Response.HasAnError = true;
                    return Content("fail");
                    //return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }
                }
            catch (Exception ex)
            {
                throw ex; ;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        //delete
        public HttpResponseMessage DeleteTransporter(TransporterList Model)
        {
            try
            {

                DB_RMMEntities DB = new DB_RMMEntities();
                string username = Session["Newusername"].ToString();
                var UserRole = DB.M_RoleManagement.Where(n => n.username == username).FirstOrDefault();
                var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
                DDLRole ddl = new DDLRole
                {
                    name = Role.name
                };
                ViewBag.Role = ddl;
                ResponseMessage Response = new ResponseMessage();
                var TransporterDB = DB.M_Transporter.ToList();
                var x = (from y in DB.M_Transporter
                         where y.id == Model.id
                         select y).FirstOrDefault();

                DB.M_Transporter.Remove(x);
                DB.SaveChanges();
                //Define error -> true and return Data
                Response.HasAnError = false;
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        //read
        public ActionResult TransporterForm()
        {

            DB_RMMEntities DB = new DB_RMMEntities();
            if (Session["Newusername"] == null)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(n => n.username == username).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;
            ResponseMessage Response = new ResponseMessage();

            var TransporterDB = DB.M_Transporter.ToList();
            var datavm2 = (from Transporter in TransporterDB
                           select new DDLTransporter
                           {
                               id = Transporter.id,
                               name = Transporter.name
                           }).ToList();
            List<TransporterList> Table = new List<TransporterList>();
            //Table = DataTable;
            Response.HasAnError = false;
            ViewBag.datavm2 = datavm2;
            return View(Table);

            //var DataApi = CallAPI("api/ApiRigMaterialMovement/GetMaterialMovement", Model);
            //RigMaterial Data = DataApi.Content.ReadAsAsync<RigMaterial>().Result;
            //ViewBag.DataTable = Data.DataTable;
        }

        public ActionResult MasterTransporterList()
        {
            DB_RMMEntities DB = new DB_RMMEntities();

            if (Session["Newusername"] == null)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(x => x.username == username).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;
            if (Role.name != "Administrator")
            {
                return RedirectToAction("ErrorValidate", "Error");
            }

           
            return View();

        }
        [Authorize]
        public ActionResult MasterTransporter()
        {
            DB_RMMEntities DB = new DB_RMMEntities();
            if (Session["Newusername"] == null)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(n => n.username == username).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;
            ResponseMessage Response = new ResponseMessage();
            try
            {
                var TransporterDB = DB.M_Transporter.ToList();

                var DataTable = (from Transporter in TransporterDB
                                 select new TransporterList
                                 {
                                     id = Transporter.id,
                                     name = Transporter.name
                                 }).ToList();

                List<TransporterList> Table = new List<TransporterList>();
                //Table = DataTable;
                Response.HasAnError = false;
                return Json(new { data = DataTable }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                Response.HasAnError = true;
                throw ex;
                //return Request.CreateResponse(HttpStatusCode.BadRequest, "Error");
            }

            //return View(Table);

            //var DataApi = CallAPI("api/ApiRigMaterialMovement/GetMaterialMovement", Model);
            //RigMaterial Data = DataApi.Content.ReadAsAsync<RigMaterial>().Result;
            //ViewBag.DataTable = Data.DataTable;

        }

        [HttpPost]
        public object GetEdit(TransporterList Model)

        {
            DB_RMMEntities DB = new DB_RMMEntities();
            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(n => n.username == username).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;
            ResponseMessage Response = new ResponseMessage();
            try
            {
                var getEdit = from Transporter in DB.M_Transporter
                              where Transporter.id == Model.id
                              select new TransporterList
                              {
                                  id = Transporter.id,
                                  name = Transporter.name
                              };

                //Define error -> true and return Data
                // List<GetEditListRigMaterial> newmodel = new List<GetEditListRigMaterial>();
                Response.HasAnError = false;
                var json = new JavaScriptSerializer().Serialize(getEdit);
                return json;

                //return Request.CreateResponse(HttpStatusCode.OK, EditRigMaterialData);
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                Response.HasAnError = true;
                throw ex;
                //return Request.CreateResponse(HttpStatusCode.BadRequest, "Error");
            }
        }

        [HttpPost]
        public ActionResult EditTransporter(TransporterList Model)
        {
            DB_RMMEntities DB = new DB_RMMEntities();
            if (Session["Newusername"] == null)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(n => n.username == username).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;
            ResponseMessage Response = new ResponseMessage();

            M_Transporter transporter = DB.M_Transporter.Where(x => x.id == Model.id).FirstOrDefault();
            {
                transporter.name = Model.name;
                transporter.last_modified_by = User.Identity.Name;
                transporter.last_modified_date = DateTime.Now;

            };
            DB.SaveChanges();
            //Define error -> true and return Data
            // List<GetEditListRigMaterial> newmodel = new List<GetEditListRigMaterial>();
            Response.HasAnError = false;
            //var json = new JavaScriptSerializer().Serialize(transporter);

            return View();
        }
    }
}