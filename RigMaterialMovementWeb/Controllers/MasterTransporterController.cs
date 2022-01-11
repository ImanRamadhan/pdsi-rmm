using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
using RigMaterialMovementWeb.Models;

namespace RigMaterialMovementWeb.Controllers
{
    public class MasterTransporterController : Controller
    {
        [Authorize]
        public ActionResult MasterTransporterList()
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
            var UserRole = DB.M_RoleManagement.Where(x => x.username == username).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            if (Role.name != "Administrator")
            {
                return RedirectToAction("ErrorValidate", "Error");
            }

            var MasterTransporterDB = DB.M_Transporter.ToList();


            var DataTable = (from Transporter in MasterTransporterDB
                             select new TransporterList
                             {
                                 id = Transporter.id,

                                name = Transporter.name

                             }).ToList();

            List<TransporterList> Table = new List<TransporterList>();
            Table = DataTable;
            Response.HasAnError = false;
            return View(Table);

        }

        [HttpPost]
        public HttpResponseMessage CreateMasterTransporter(TransporterList Model)
        {
            try
            {

               
               
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();

                var exist = DB.M_Transporter.Where(x => x.name == Model.name).Count();

                if(exist == 0)
                {
                    M_Transporter NewMC = new M_Transporter
                    {
                        name = Model.name,
                        last_modified_date = DateTime.Now
                    };
                    //Insert to Database

                    DB.M_Transporter.Add(NewMC);
                    DB.SaveChanges();
                    //Define error -> true and return Data
                    Response.HasAnError = false;
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                else
                {
                    Response.HasAnError = true;
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

              
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public HttpResponseMessage DeleteMasterTransporter(TransporterList Model)
        {
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                var TransporterDB = DB.M_Transporter.ToList();
                var x = (from y in DB.M_Transporter
                         where y.id == Model.id
                         select y).FirstOrDefault();
                //Insert to Database

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

        [System.Web.Http.HttpPost]
        public object GetDetailMasterTransporter(TransporterList Model)

        {
            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();
            try
            {
                var Transporter = from TransporterDB in DB.M_Transporter
                                          where TransporterDB.id == Model.id

                                          select new TransporterList
                                          {
                                              name = TransporterDB.name
                                              
                                          };

                //Define error -> true and return Data
                // List<GetEditListRigMaterial> newmodel = new List<GetEditListRigMaterial>();
                Response.HasAnError = false;
                var json = new JavaScriptSerializer().Serialize(Transporter);
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
        public HttpResponseMessage EditMasterTransporter(TransporterList Model)
        {
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                // T_RigMaterialMovement NewMC = new T_RigMaterialMovement
                M_Transporter NewMC = DB.M_Transporter.Where(x => x.id == Model.id).FirstOrDefault();
                {
                    NewMC.name = Model.name;
                    NewMC.last_modified_date = DateTime.Now;
                    NewMC.last_modified_by = User.Identity.Name;
                    
                };
                //Insert to Database

                //DB.T_RigMaterialMovement.Add(NewMC);
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


    }

}