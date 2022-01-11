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
    public class RoleController : Controller
    {
        // GET: Role
		[Authorize]
        public ActionResult Index()
        {
			
			return View();
        }

		public HttpResponseMessage CreateUserRole(RoleList Model)
		{
			try
			{
				DB_RMMEntities DB = new DB_RMMEntities();
				ResponseMessage Response = new ResponseMessage();
				M_RoleManagement roles = new M_RoleManagement
				{
					username = Model.username,
					email = Model.email + "@pertamina.com",
					role_id = Model.role_id,
					last_modified_by = User.Identity.Name,
					last_modified_date = DateTime.Now,
				};
				//Insert to Database

				DB.M_RoleManagement.Add(roles);
				DB.SaveChanges();
				//Define error -> true and return Data
				Response.HasAnError = false;
				return new HttpResponseMessage(HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				throw ex; ;
				//return new HttpResponseMessage(HttpStatusCode.BadRequest);
			}
		}

		//delete
		public HttpResponseMessage DeleteRole(RoleList Model)
		{
			try
			{
				DB_RMMEntities DB = new DB_RMMEntities();
				ResponseMessage Response = new ResponseMessage();
				var TransporterDB = DB.M_RoleManagement.ToList();
				var x = (from y in DB.M_RoleManagement
						 where y.ID == Model.ID
						 select y).FirstOrDefault();

				DB.M_RoleManagement.Remove(x);
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

		public ActionResult RoleManagementList()
		{
			DB_RMMEntities DB = new DB_RMMEntities();
			var RoleDB = DB.M_Role.ToList();
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
			if (Role.name == "Administrator")
			{
				var masterRole = (from role in RoleDB
								  select new DDLRole
								  {
									  id = role.id,
									  name = role.name
								  }).ToList();
				ViewBag.RoleDDL = masterRole;
			}
			else
			{
				return RedirectToAction("ErrorValidate", "Error");
			}
			
			return View();

		}

		public ActionResult MasterRoleManagement()
		{
			DB_RMMEntities DB = new DB_RMMEntities();
			ResponseMessage Response = new ResponseMessage();
			try
			{
				var RoleMgmtDB = DB.M_RoleManagement.ToList();
				var RoleDB = DB.M_Role.ToList();

				var DataTable = (from rolesm in RoleMgmtDB
								 join roles in RoleDB on rolesm.role_id equals roles.id
								 select new RoleList
								 {
									 email = rolesm.email,
									 username = rolesm.username,
									 role_name = roles.name,
									 ID = rolesm.ID
								 }).ToList();
				List<RoleList> Table = new List<RoleList>();
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
		public object GetEditRole(RoleList Model)

		{
			DB_RMMEntities DB = new DB_RMMEntities();
			ResponseMessage Response = new ResponseMessage();
			try
			{
				var getEditRole = from roleM in DB.M_RoleManagement
							  where roleM.ID == Model.ID
							  select new RoleList
							  {
								  ID = roleM.ID,
								  username = roleM.username,
								  email = roleM.email,
								  role_id = roleM.role_id
							  };

				//Define error -> true and return Data
				// List<GetEditListRigMaterial> newmodel = new List<GetEditListRigMaterial>();
				Response.HasAnError = false;
				var json = new JavaScriptSerializer().Serialize(getEditRole);
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
		public ActionResult EditRole(RoleList Model)
		{
			DB_RMMEntities DB = new DB_RMMEntities();
			ResponseMessage Response = new ResponseMessage();

			M_RoleManagement rolemgmt = DB.M_RoleManagement.Where(x => x.ID == Model.ID).FirstOrDefault();
			{
				rolemgmt.username = Model.username;
				rolemgmt.email = Model.email;
				rolemgmt.role_id = Model.role_id;
				rolemgmt.last_modified_by = User.Identity.Name;
				rolemgmt.last_modified_date = DateTime.Now;

			};
			DB.SaveChanges();
			//Define error -> true and return Data
			// List<GetEditListRigMaterial> newmodel = new List<GetEditListRigMaterial>();
			Response.HasAnError = false;
			//var json = new JavaScriptSerializer().Serialize(transporter);

			//return View();
			return RedirectToAction("RoleManagementList", "Role");
		}
	}
}