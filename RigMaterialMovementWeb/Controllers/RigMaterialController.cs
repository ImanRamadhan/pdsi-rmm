using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
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
using Newtonsoft.Json.Linq;
using RigMaterialMovementWeb.Models;

namespace RigMaterialMovementWeb.Controllers
{
    public class RigMaterialController : Controller
    {
        // GET: RigMaterial

        [Authorize]
        public ActionResult Create(RigMaterialMovementList Model)
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
            var UserRole = DB.M_RoleManagement.Where(n => n.username.ToUpper() == username.ToUpper()).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;
            ResponseMessage Response = new ResponseMessage();

           


            var Rig = DB.M_Rig.ToList();
            var Rig2 = DB.VW_RIG.ToList();

            //get token
            HttpClient clients = new HttpClient();
            string APIUrls = ConfigurationManager.AppSettings["GetTokenUrl"].ToString();
            string usernameToken = ConfigurationManager.AppSettings["usernameToken"].ToString();
            string passwordToken = ConfigurationManager.AppSettings["passwordToken"].ToString();

            var param = new FormUrlEncodedContent(new[]
                {
                             new KeyValuePair<string, string>("UserName", usernameToken),
                             new KeyValuePair<string, string>("Password", passwordToken),
                             new KeyValuePair<string, string>("RememberMe", "0"),
                             new KeyValuePair<string, string>("method", "LOGIN")
                    });
            HttpResponseMessage Responses1 = clients.PostAsync(APIUrls, param).Result;
            string token = "";
            if (Responses1.IsSuccessStatusCode)
            {
                var response = Responses1.Content.ReadAsStringAsync().Result;
                JObject j = JObject.Parse(response);
                token = j["Token"].ToString();
            }
            //get area & rig
            HttpClient client = new HttpClient();
            string APIUrl = ConfigurationManager.AppSettings["GetRigUrl"].ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Token",token);
           // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            HttpResponseMessage Responses = client.GetAsync(APIUrl).Result;
            List<DDL> DDL = new List<DDL>();
           
            if (Responses.IsSuccessStatusCode)
            {
                var response = Responses.Content.ReadAsStringAsync().Result;
               JObject j = JObject.Parse(response);
                JArray jArr = (JArray)j["responseData"];
                foreach (JObject jobj in jArr)
                {
                   DDL dataDDL = JsonConvert.DeserializeObject<DDL>(jobj.ToString());
                   DDL.Add(dataDDL);
                }
                  
            }


            if (Model.area == "JAMBI - NAD")
            {
                var datavmjambi = (from Transporter in Rig2
                                   where Transporter.area_id == 1
                                 select new DDLTransporter
                                 {
                                     id = Transporter.id,
                                     name = Transporter.name
                                 }).ToList();
                ViewBag.datavm666 = datavmjambi;
            }
            if (Model.area == "SBS")
            {
                var datavmsbs = (from Transporter in Rig2
                                 where Transporter.area_id == 2
                                 select new DDLTransporter
                                 
                                 {
                                     id = Transporter.id,
                                     name = Transporter.name
                                 }).ToList();
                ViewBag.datavm666 = datavmsbs;
            }
            if (Model.area == "JAWA")
            {
                var datavmjawa = (from Transporter in Rig2
                                  where Transporter.area_id == 3
                                  select new DDLTransporter
                                 {
                                     id = Transporter.id,
                                     name = Transporter.name
                                 }).ToList();
                ViewBag.datavm666 = datavmjawa;
            }
            if (Model.area == "KTI")
            {
                var datavmkti = (from Transporter in Rig2
                                 where Transporter.area_id == 4
                                 select new DDLTransporter
                                 {
                                     id = Transporter.id,
                                     name = Transporter.name
                                 }).ToList();
                ViewBag.datavm666 = datavmkti;
            }
            else if(Model.area == null)
            {
                var datavm666 = (from Transporter in Rig
                                 select new DDLTransporter
                                 {
                                     id = Transporter.id,
                                     name = Transporter.name
                                 }).ToList();
                ViewBag.datavm666 = datavm666;
            }
            var TransporterDB = DB.M_Transporter.ToList();
           
            var datavm2 = (from Transporter in TransporterDB
                           select new DDLTransporter
                           {
                               id = Transporter.id,
                               name = Transporter.name
                           }).ToList();
            ViewBag.datavm2 = datavm2;

            var AreaDB = DB.M_Area.ToList();
            var AreaDB2 = DB.VW_AREA.ToList();

            var datavm66 = (from Transporter in DDL
                           select new DDLTransporter
                           {
                               name = Transporter.WBSArea
                           }).ToList();
            ViewBag.datavm66 = datavm66;
          


            //Define error -> true and return Data
            Response.HasAnError = false;
            return View();

        }

        public ActionResult CreateDDL(RigMaterialMovementList Model)
        {

            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();
            var Rig = DB.M_Rig.ToList();
           
                var datavmjambi = (from Transporter in Rig
                                   where Transporter.area_id == 1
                                   select new DDLTransporter
                                   {
                                       id = Transporter.id,
                                       name = Transporter.name
                                   }).ToList();
           
          
                var datavmsbs = (from Transporter in Rig
                                 where Transporter.area_id == 2
                                 select new DDLTransporter

                                 {
                                     id = Transporter.id,
                                     name = Transporter.name
                                 }).ToList();
             
            
                var datavmjawa = (from Transporter in Rig
                                  where Transporter.area_id == 3
                                  select new DDLTransporter
                                  {
                                      id = Transporter.id,
                                      name = Transporter.name
                                  }).ToList();
            
          
                var datavmkti = (from Transporter in Rig
                                 where Transporter.area_id == 4
                                 select new DDLTransporter
                                 {
                                     id = Transporter.id,
                                     name = Transporter.name
                                 }).ToList();
            var datavmno = (from Transporter in Rig
                             where Transporter.area_id == 20
                             select new DDLTransporter
                             {
                                 id = Transporter.id,
                                 name = Transporter.name
                             }).ToList();


            var datavm666 = (from Transporter in Rig
                                 select new DDLTransporter
                                 {
                                     id = Transporter.id,
                                     name = Transporter.name
                                 }).ToList();
          
           

          



            //Define error -> true and return Data
            Response.HasAnError = false;
            if (Model.area == "JAMBI - NAD")
            {
                return Json(new { data = datavmjambi }, JsonRequestBehavior.AllowGet);
            }
            if (Model.area == "SBS")
            {
                return Json(new { data = datavmsbs}, JsonRequestBehavior.AllowGet);
            }
            if (Model.area == "JAWA")
            {
                return Json(new { data = datavmjawa}, JsonRequestBehavior.AllowGet);
            }
            if (Model.area == "KTI")
            {
                return Json(new { data = datavmkti}, JsonRequestBehavior.AllowGet);
            }
            if (Model.area == "no")
            {
                return Json(new { data = datavmno }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = datavmkti }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(RigMaterialMovementList Model)
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
            var UserRole = DB.M_RoleManagement.Where(n => n.username.ToUpper() == username.ToUpper()).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;


            if (Model.commandline == "CreateNew")
            {
                T_RigMaterialMovement NewMC = new T_RigMaterialMovement
                {

                    tanggal_mulai = Model.tanggal_mulai,
                    area = Model.area,
                    rig = Model.rig,
                    rute_dari = Model.rute_dari,
                    rute_ke = Model.rute_ke,
                    jarak = Model.jarak,
                    transporter_id = Model.transporter_id,
                    biaya = Model.biaya,
                    target_hari = Model.target_hari,
                    target_trip = Model.target_trip,
                    last_modified_by = username,
                    last_modified_date = DateTime.Now


                };
                //Insert to Database

                DB.T_RigMaterialMovement.Add(NewMC);
                DB.SaveChanges();

                var GetLatest2 = DB.T_RigMaterialMovement
                               .Where(x => x.id != null)
                               .OrderByDescending(x => x.id)
                               .Take(1)
                               .Select(x => x.id)
                               .ToList()
                               .FirstOrDefault();

                var TransporterDB = DB.M_Transporter.ToList();
                var datavm2 = (from Transporter in TransporterDB
                               select new DDLTransporter
                               {
                                   id = Transporter.id,
                                   name = Transporter.name
                               }).ToList();
                ViewBag.datavm2 = datavm2;
                var AreaDB = DB.M_Area.ToList();

                var datavm66 = (from Transporter in AreaDB
                                select new DDLArea
                                {
                                    name = Transporter.name
                                }).ToList();
                ViewBag.datavm66 = datavm66;

                var datavm77 = (from Transporter in DB.M_Rig
                                select new DDLArea
                                {
                                    name = Transporter.name
                                }).ToList();
                ViewBag.datavm77 = datavm77;


                var KeterlambatanDB = DB.M_FaktorKeterlambatan.ToList();
                var datavm3 = (from Faktor in KeterlambatanDB
                               select new DDLFaktorKeterlambatan
                               {
                                   id = Faktor.id,
                                   name = Faktor.name
                               }).ToList();
                ViewBag.datavm3 = datavm3;

                var EditRigMaterialData = (from RigMovement in DB.T_RigMaterialMovement
                                           where RigMovement.id == GetLatest2

                                           select new GetEditListRigMaterial
                                           {
                                               tanggal_mulai = RigMovement.tanggal_mulai,
                                               area = RigMovement.area,
                                               rig = RigMovement.rig,
                                               rute_dari = RigMovement.rute_dari,
                                               rute_ke = RigMovement.rute_ke,
                                               jarak = RigMovement.jarak,
                                               transporter_id = RigMovement.transporter_id,
                                               biaya = RigMovement.biaya,
                                               target_hari = RigMovement.target_hari,
                                               target_trip = RigMovement.target_trip

                                           }).ToList();

                ViewBag.Datanya = EditRigMaterialData;

                var DataTable2 = (from MaterialMovement in DB.T_RigMaterialMovementDetail
                                  where MaterialMovement.rig_material_movement_id == GetLatest2
                                  select new RigMaterialMovementList
                                  {
                                      id = MaterialMovement.id,
                                      trip_move_out = MaterialMovement.trip_move_out,
                                      trip_move_in = MaterialMovement.trip_move_in,
                                      kendala = MaterialMovement.kendala,
                                      tindaklanjut = MaterialMovement.tindak_lanjut,
                                      faktorketerlambatan = MaterialMovement.faktor_keterlambatan,
                                      tanggal_move = MaterialMovement.tanggal_move
                                  }).ToList();

                ViewBag.test = GetLatest2;
             
                var datavm666 = (from Transporter in AreaDB
                                select new DDLArea
                                {
                                    name = Transporter.name
                                }).ToList();
                ViewBag.datavm66 = datavm666;

                var datavm777 = (from Transporter in DB.M_Rig
                                select new DDLArea
                                {
                                    name = Transporter.name
                                }).ToList();
                ViewBag.datavm77 = datavm777;

                List<RigMaterialMovementList> Table2 = new List<RigMaterialMovementList>();
                Table2 = DataTable2;

                return View(Table2);
            }
            else {
                var GetLatest2 = DB.T_RigMaterialMovement
                             .Where(x => x.id != null)
                             .OrderByDescending(x => x.id)
                             .Take(1)
                             .Select(x => x.id)
                             .ToList()
                             .FirstOrDefault();

                var TransporterDB = DB.M_Transporter.ToList();
                var datavm2 = (from Transporter in TransporterDB
                               select new DDLTransporter
                               {
                                   id = Transporter.id,
                                   name = Transporter.name
                               }).ToList();
                ViewBag.datavm2 = datavm2;


                var KeterlambatanDB = DB.M_FaktorKeterlambatan.ToList();
                var datavm3 = (from Faktor in KeterlambatanDB
                               select new DDLFaktorKeterlambatan
                               {
                                   id = Faktor.id,
                                   name = Faktor.name
                               }).ToList();
                ViewBag.datavm3 = datavm3;

                var EditRigMaterialData = (from RigMovement in DB.T_RigMaterialMovement
                                           join transporter in DB.M_Transporter
                                           on RigMovement.transporter_id equals transporter.id

                                           where RigMovement.id == GetLatest2

                                           select new GetEditListRigMaterial
                                           {
                                               tanggal_mulai = RigMovement.tanggal_mulai,
                                               area = RigMovement.area,
                                               rig = RigMovement.rig,
                                               rute_dari = RigMovement.rute_dari,
                                               rute_ke = RigMovement.rute_ke,
                                               jarak = RigMovement.jarak,
                                               transporter_id = RigMovement.transporter_id,
                                               biaya = RigMovement.biaya,
                                               target_hari = RigMovement.target_hari,
                                               target_trip = RigMovement.target_trip,
                                               transporter = transporter.name

                                           }).ToList();

                ViewBag.Datanya = EditRigMaterialData;

                var DataTable2 = (from MaterialMovement in DB.T_RigMaterialMovementDetail
                                  where MaterialMovement.rig_material_movement_id == GetLatest2
                                  select new RigMaterialMovementList
                                  {
                                      id = MaterialMovement.id,
                                      trip_move_out = MaterialMovement.trip_move_out,
                                      trip_move_in = MaterialMovement.trip_move_in,
                                      kendala = MaterialMovement.kendala,
                                      tindaklanjut = MaterialMovement.tindak_lanjut,
                                      faktorketerlambatan = MaterialMovement.faktor_keterlambatan,
                                      tanggal_move = MaterialMovement.tanggal_move
                                  }).ToList();

                ViewBag.test = GetLatest2;
                var datavm666 = (from Transporter in DB.M_Area
                                 select new DDLArea
                                 {
                                     name = Transporter.name
                                 }).ToList();
                ViewBag.datavm66 = datavm666;

                var datavm777 = (from Transporter in DB.M_Rig
                                 select new DDLArea
                                 {
                                     name = Transporter.name
                                 }).ToList();
                ViewBag.datavm77 = datavm777;
                List<RigMaterialMovementList> Table2 = new List<RigMaterialMovementList>();
                Table2 = DataTable2;

                return View(Table2);
            }
        }
        [Authorize]
        public ActionResult EditExisting(int param)
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
            var UserRole = DB.M_RoleManagement.Where(n => n.username.ToUpper() == username.ToUpper()).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;

            ResponseMessage Response = new ResponseMessage();
            ViewBag.test = param;
            var EditRigMaterialData = (from RigMovement in DB.T_RigMaterialMovement
                                       join transporter in DB.M_Transporter
                                       on RigMovement.transporter_id equals transporter.id
                                      where RigMovement.id == param

                                      select new GetEditListRigMaterial
                                      {
                                          tanggal_mulai = RigMovement.tanggal_mulai,
                                          area = RigMovement.area,
                                          rig = RigMovement.rig,
                                          rute_dari = RigMovement.rute_dari,
                                          rute_ke = RigMovement.rute_ke,
                                          jarak = RigMovement.jarak,
                                          transporter_id = RigMovement.transporter_id,
                                          biaya = RigMovement.biaya,
                                          target_hari = RigMovement.target_hari,
                                          target_trip = RigMovement.target_trip,
                                          transporter = transporter.name

                                      }).ToList();

            var AreaDB = DB.M_Area.ToList();
            var Rig = DB.M_Rig.ToList();
            var AreaDB2 = DB.VW_AREA.ToList();
            var Rig2 = DB.VW_AREA.ToList();
            //var datavm66 = (from Transporter in AreaDB2
            //                select new DDLTransporter
            //                {
            //                    id = Transporter.id,
            //                    name = Transporter.name
            //                }).ToList();
            //ViewBag.datavm66 = datavm66;
            //get token
            HttpClient clients = new HttpClient();
            string APIUrls = ConfigurationManager.AppSettings["GetTokenUrl"].ToString();
            string usernameToken = ConfigurationManager.AppSettings["usernameToken"].ToString();
            string passwordToken = ConfigurationManager.AppSettings["passwordToken"].ToString();

            var param11 = new FormUrlEncodedContent(new[]
                {
                             new KeyValuePair<string, string>("UserName", usernameToken),
                             new KeyValuePair<string, string>("Password", passwordToken),
                             new KeyValuePair<string, string>("RememberMe", "0"),
                             new KeyValuePair<string, string>("method", "LOGIN")
                    });
            HttpResponseMessage Responses1 = clients.PostAsync(APIUrls, param11).Result;
            string token = "";
            if (Responses1.IsSuccessStatusCode)
            {
                var response = Responses1.Content.ReadAsStringAsync().Result;
                JObject j = JObject.Parse(response);
                token = j["Token"].ToString();
            }
            //get area & rig
            HttpClient client = new HttpClient();
            string APIUrl = ConfigurationManager.AppSettings["GetRigUrl"].ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Token", token);
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            HttpResponseMessage Responses = client.GetAsync(APIUrl).Result;
            List<DDL> DDL = new List<DDL>();

            if (Responses.IsSuccessStatusCode)
            {
                var response = Responses.Content.ReadAsStringAsync().Result;
                JObject j = JObject.Parse(response);
                JArray jArr = (JArray)j["responseData"];
                foreach (JObject jobj in jArr)
                {
                    DDL dataDDL = JsonConvert.DeserializeObject<DDL>(jobj.ToString());
                    DDL.Add(dataDDL);
                }

            }

            var area = (from Transporter in DDL
                        select new DDLTransporter
                        {
                            name = Transporter.WBSArea
                        }).ToList();

            ViewBag.area = area.GroupBy(x => x.name).Select(x => x.First()).ToList();
            var datavm666 = (from Transporter in Rig2
                             select new DDLTransporter
                             {
                                 id = Transporter.id,
                                 name = Transporter.name
                             }).ToList();
            ViewBag.datavm666 = datavm666;


            var TransporterDB = DB.M_Transporter.ToList();
            var datavm2 = (from Transporter in TransporterDB
                           select new DDLTransporter
                           {
                               id = Transporter.id,
                               name = Transporter.name
                           }).ToList();
            ViewBag.datavm2 = datavm2;

            var KeterlambatanDB = DB.M_FaktorKeterlambatan.ToList();
            var KeterlambatanDB2 = DB.VW_FAKTORKETERLAMBATAN.ToList();
            var datavm7 = (from Faktor in KeterlambatanDB
                           select new DDLFaktorKeterlambatan
                           {
                               id = Faktor.id,
                               name = Faktor.name
                           }).ToList();
            ViewBag.datavm7 = datavm7;

            //Define error -> true and return Data
            // List<GetEditListRigMaterial> newmodel = new List<GetEditListRigMaterial>();
            Response.HasAnError = false;
            var json = new JavaScriptSerializer().Serialize(EditRigMaterialData);
            ViewBag.Datanya = EditRigMaterialData;

            var DataTable2 = (from MaterialMovement in DB.T_RigMaterialMovementDetail
                              where MaterialMovement.rig_material_movement_id == param
                              select new RigMaterialMovementList
                              {
                                  id = MaterialMovement.id,
                                  trip_move_out = MaterialMovement.trip_move_out,
                                  trip_move_in = MaterialMovement.trip_move_in,
                                  kendala = MaterialMovement.kendala,
                                  tindaklanjut = MaterialMovement.tindak_lanjut,
                                  faktorketerlambatan = MaterialMovement.faktor_keterlambatan,
                                  tanggal_move = MaterialMovement.tanggal_move
                              }).ToList();

            List<RigMaterialMovementList> Table2 = new List<RigMaterialMovementList>();
            Table2 = DataTable2;

            return View(Table2);

        }
        public string EditExisting2(int id)
        {

            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();

            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(n => n.username.ToUpper() == username.ToUpper()).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;



            var EditRigMaterialData = from RigMovement in DB.T_RigMaterialMovement
                                      where RigMovement.id == id

                                      select new GetEditListRigMaterial
                                      {
                                          area = RigMovement.area,
                                          rig = RigMovement.rig,
                                          rute_dari = RigMovement.rute_dari,
                                          rute_ke = RigMovement.rute_ke

                                      };
            Response.HasAnError = false;
            var json = new JavaScriptSerializer().Serialize(EditRigMaterialData);
            return json;

        }
        [Authorize]
        public ActionResult RigMaterialList()
        {
            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();
            var RoleDB = DB.M_RoleManagement.ToList();
            if (Session["Newusername"] == null)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(x => x.username.ToUpper() == username.ToUpper()).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
             ViewBag.Role = ddl;

            var appr = (from ROLE in RoleDB
                        where ROLE.username.ToUpper() == username.ToUpper()
                        select new Approval
                        {
                            role_id = ROLE.role_id,
                            username = ROLE.username


                        }).ToList();
            ViewBag.approval = appr;

            var TransporterDB = DB.M_Transporter.ToList();

            var datavm2 = (from Transporter in TransporterDB
                           select new DDLTransporter
                           {
                               id = Transporter.id,
                               name = Transporter.name
                           }).ToList();
            ViewBag.transporter = datavm2;

            var AreaDB = DB.M_Area.ToList();
            var Rig = DB.M_Rig.ToList();
            //get token
            HttpClient clients = new HttpClient();
            string APIUrls = ConfigurationManager.AppSettings["GetTokenUrl"].ToString();
            string usernameToken = ConfigurationManager.AppSettings["usernameToken"].ToString();
            string passwordToken = ConfigurationManager.AppSettings["passwordToken"].ToString();

            var param = new FormUrlEncodedContent(new[]
                {
                             new KeyValuePair<string, string>("UserName", usernameToken),
                             new KeyValuePair<string, string>("Password", passwordToken),
                             new KeyValuePair<string, string>("RememberMe", "0"),
                             new KeyValuePair<string, string>("method", "LOGIN")
                    });
            HttpResponseMessage Responses1 = clients.PostAsync(APIUrls, param).Result;
            string token = "";
            if (Responses1.IsSuccessStatusCode)
            {
                var response = Responses1.Content.ReadAsStringAsync().Result;
                JObject j = JObject.Parse(response);
                token = j["Token"].ToString();
            }
            //get area & rig
            HttpClient client = new HttpClient();
            string APIUrl = ConfigurationManager.AppSettings["GetRigUrl"].ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Token", token);
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            HttpResponseMessage Responses = client.GetAsync(APIUrl).Result;
            List<DDL> DDL = new List<DDL>();

            if (Responses.IsSuccessStatusCode)
            {
                var response = Responses.Content.ReadAsStringAsync().Result;
                JObject j = JObject.Parse(response);
                JArray jArr = (JArray)j["responseData"];
                foreach (JObject jobj in jArr)
                {
                    DDL dataDDL = JsonConvert.DeserializeObject<DDL>(jobj.ToString());
                    DDL.Add(dataDDL);
                }

            }

            var area = (from Transporter in DDL
                        select new DDLTransporter
                        {
                            name = Transporter.WBSArea
                        }).ToList();

            ViewBag.area = area;
            var datavm666 = (from Transporter in Rig
                             select new DDLTransporter
                             {
                                 id = Transporter.id,
                                 name = Transporter.name
                             }).ToList();
            ViewBag.rig = datavm666;

            return View(); 
        }



        [HttpPost]
        public ActionResult Filter(RigMaterialMovementList Model)
        {
            if(Model.area == null)
            {
                Model.area = string.Empty;
            }
            if (Model.rig == null)
            {
                Model.rig = string.Empty;
            }
            if (Model.transporter == null)
            {
                Model.transporter = string.Empty;
            }
            
            DB_RMMEntities DB = new DB_RMMEntities();
            if (Session["Newusername"] == null)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(n => n.username.ToUpper() == username.ToUpper()).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;
            var RoleDB = DB.M_RoleManagement.ToList();
            var appr = (from ROLE in RoleDB
                        where ROLE.username.ToUpper() == username.ToUpper()
                        select new Approval
                        {
                            role_id = ROLE.role_id,
                            username = ROLE.username


                        }).ToList();
            ViewBag.approval = appr;
            ResponseMessage Response = new ResponseMessage();
            var PermintaanDB = DB.T_RigMaterialMovement.ToList();
            var StatusDB = DB.M_Status.ToList();
            var Hari = DB.T_RigMaterialMovement.ToList();
            var detail = DB.T_RigMaterialMovementDetail.ToList();
            try

            {
                if (appr[0].role_id == 1)
                {
                    var DataTableSrc = (from Kerja in PermintaanDB
                                        join transporter in DB.M_Transporter
                                        on Kerja.transporter_id equals transporter.id
                                        where
                                        transporter.name.ToString().Contains(Model.transporter.ToString() ?? "") &&
                                        Kerja.rig.Contains(Model.rig ?? "") &&
                                        Kerja.area.Contains(Model.area ?? "") &&
                                        (Model.tanggal_mulai_dari == null && Model.tanggal_mulai_sampai == null || Kerja.tanggal_mulai >= Model.tanggal_mulai_dari && Kerja.tanggal_mulai <= Model.tanggal_mulai_sampai)
                                        select new RigMaterialMovementList
                                        {
                                            area = Kerja.area,
                                            rig = Kerja.rig,
                                            biaya = Kerja.biaya,
                                            id = Kerja.id,
                                            jarak = Kerja.jarak,
                                            rute_dari = Kerja.rute_dari,
                                            rute_ke = Kerja.rute_ke,
                                            target_hari = Kerja.target_hari,
                                            target_trip = Kerja.target_trip,
                                            tanggal_mulai = Kerja.tanggal_mulai,
                                            transporter = transporter.name

                                        }).ToList();
                    var list = DataTableSrc.ToList();
                    List<string> HariKeList = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int HariKe = (from y in DB.T_RigMaterialMovementDetail
                                      where y.rig_material_movement_id == unique
                                      select y.rig_material_movement_id).Count();

                        if (HariKe == 0)
                        {
                            string EachHarike = "0" + "|" + unique;
                            HariKeList.Add(EachHarike);
                        }
                        else
                        {
                            string EachHarike = HariKe.ToString() + "|" + unique;
                            HariKeList.Add(EachHarike);
                        }

                    };
                    foreach (var item in list)
                    {
                        foreach (var HList in HariKeList.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.harike = item.harike + Convert.ToInt32(ArrHlist[0]);
                        }
                    }

                    List<string> ActualTargt = new List<string>();
                    List<string> DailyTargetMoveOut = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int? anak = (from y in DB.T_RigMaterialMovementDetail
                                     where y.rig_material_movement_id == unique
                                     select y.rig_material_movement_id).Count();

                        int? target = (from t in DB.T_RigMaterialMovement
                                       where t.id == unique
                                       select t.target_trip).FirstOrDefault();

                        if (anak == null)
                        {
                            string EachHarike = "0" + "|" + unique;
                            DailyTargetMoveOut.Add(EachHarike);
                        }
                        else
                        {
                            string testesan = target.ToString() + "|" + unique;
                            string EachHarike = anak.ToString() + "|" + unique;
                            DailyTargetMoveOut.Add(EachHarike);
                            ActualTargt.Add(testesan);
                        }


                    };
                    foreach (var item in list)
                    {
                        foreach (var HList in DailyTargetMoveOut.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.DailyTargetMoveOut = Convert.ToInt32(ArrHlist[0]) * 100 / item.target_hari;
                        }
                    }

                    List<string> TripAnak = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                        if (anak == null)
                        {
                            string EachHarike = "0" + "|" + unique;
                            TripAnak.Add(EachHarike);
                        }
                        else
                        {
                            string EachHarike = anak.ToString() + "|" + unique;
                            TripAnak.Add(EachHarike);
                        }


                    };
                    foreach (var item in list)
                    {
                        foreach (var HList in TripAnak.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.trip = Convert.ToInt32(ArrHlist[0]);
                        }
                    }

                    List<string> TripAnakMI = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                        if (anak == null)
                        {
                            string EachHarike = "0" + "|" + unique;
                            TripAnakMI.Add(EachHarike);
                        }
                        else
                        {
                            string EachHarike = anak.ToString() + "|" + unique;
                            TripAnakMI.Add(EachHarike);
                        }



                    };
                    foreach (var item in list)
                    {
                        foreach (var HList in TripAnakMI.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.tripMoveIn = Convert.ToInt32(ArrHlist[0]);
                        }
                    }



                    foreach (var item in list)
                    {
                        foreach (var HList in DailyTargetMoveOut.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.DailyTargetMoveOut = Convert.ToInt32(ArrHlist[0]) / item.target_trip * 100;
                        }
                    }

                    List<string> Persentase = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                        if (anak == null)
                        {
                            string EachHarike = "0" + "|" + unique;
                            Persentase.Add(EachHarike);
                        }
                        else
                        {
                            string EachHarike = anak.ToString() + "|" + unique;
                            Persentase.Add(EachHarike);
                        }


                    };


                    foreach (var item in list)
                    {
                        foreach (var HList in Persentase.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.persentase = item.persentase + Convert.ToInt32(ArrHlist[0]);
                            item.persentase2 = item.persentase / item.target_trip * 100;
                            if (item.persentase2 >= item.DailyTargetMoveOut)
                            {
                                item.penilaian1 = "Lebih Cepat";
                            }
                            else if (item.persentase2 >= item.DailyTargetMoveOut)
                            {
                                item.penilaian1 = "Sesuai";
                            }
                            else
                            {
                                item.penilaian1 = "Lebih Lambat";
                            }
                        }
                    }


                    List<string> PersentaseMoveIn = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_in).Sum();

                        if (anak == null)
                        {
                            string EachHarike = "0" + "|" + unique;
                            PersentaseMoveIn.Add(EachHarike);
                        }
                        else
                        {
                            string EachHarike = anak.ToString() + "|" + unique;
                            PersentaseMoveIn.Add(EachHarike);
                        }

                    };
                    foreach (var item in list)
                    {
                        foreach (var HList in PersentaseMoveIn.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.persentaseMoveIn1 = item.persentaseMoveIn1 + Convert.ToInt32(ArrHlist[0]);
                            item.persentaseMoveIn2 = item.persentaseMoveIn1 / item.target_trip * 100;
                            if (item.persentaseMoveIn2 >= item.DailyTargetMoveOut)
                            {
                                item.penilaian2 = "Lebih Cepat";
                            }
                            if (item.persentaseMoveIn2 == item.DailyTargetMoveOut)
                            {
                                item.penilaian2 = "Sesuai";
                            }
                            else
                            {
                                item.penilaian2 = "Lebih Lambat";
                            }
                        }
                    }


                    List<RigMaterialMovementList> Table = new List<RigMaterialMovementList>();
                    Table = list;
                    Response.HasAnError = false;
                    return Json(new { data = Table }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var DataTableSrc = (from Kerja in PermintaanDB
                                        join transporter in DB.M_Transporter
                                        on Kerja.transporter_id equals transporter.id
                                        where
                                        transporter.name.ToString().Contains(Model.transporter.ToString() ?? "") &&
                                        Kerja.rig.Contains(Model.rig ?? "") &&
                                        Kerja.area.Contains(Model.area ?? "") &&
                                        (Model.tanggal_mulai_dari == null && Model.tanggal_mulai_sampai == null || Kerja.tanggal_mulai >= Model.tanggal_mulai_dari && Kerja.tanggal_mulai <= Model.tanggal_mulai_sampai) &&
                                        Kerja.last_modified_by.ToUpper() == username.ToUpper()
                                        select new RigMaterialMovementList
                                        {
                                            area = Kerja.area,
                                            rig = Kerja.rig,
                                            biaya = Kerja.biaya,
                                            id = Kerja.id,
                                            jarak = Kerja.jarak,
                                            rute_dari = Kerja.rute_dari,
                                            rute_ke = Kerja.rute_ke,
                                            target_hari = Kerja.target_hari,
                                            target_trip = Kerja.target_trip,
                                            tanggal_mulai = Kerja.tanggal_mulai,
                                            transporter = transporter.name

                                        }).ToList();
                    var list = DataTableSrc.ToList();
                    List<string> HariKeList = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int HariKe = (from y in DB.T_RigMaterialMovementDetail
                                      where y.rig_material_movement_id == unique
                                      select y.rig_material_movement_id).Count();

                        if (HariKe == 0)
                        {
                            string EachHarike = "0" + "|" + unique;
                            HariKeList.Add(EachHarike);
                        }
                        else
                        {
                            string EachHarike = HariKe.ToString() + "|" + unique;
                            HariKeList.Add(EachHarike);
                        }

                    };
                    foreach (var item in list)
                    {
                        foreach (var HList in HariKeList.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.harike = item.harike + Convert.ToInt32(ArrHlist[0]);
                        }
                    }

                    List<string> ActualTargt = new List<string>();
                    List<string> DailyTargetMoveOut = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int? anak = (from y in DB.T_RigMaterialMovementDetail
                                     where y.rig_material_movement_id == unique
                                     select y.rig_material_movement_id).Count();

                        int? target = (from t in DB.T_RigMaterialMovement
                                       where t.id == unique
                                       select t.target_trip).FirstOrDefault();

                        if (anak == null)
                        {
                            string EachHarike = "0" + "|" + unique;
                            DailyTargetMoveOut.Add(EachHarike);
                        }
                        else
                        {
                            string testesan = target.ToString() + "|" + unique;
                            string EachHarike = anak.ToString() + "|" + unique;
                            DailyTargetMoveOut.Add(EachHarike);
                            ActualTargt.Add(testesan);
                        }


                    };
                    foreach (var item in list)
                    {
                        foreach (var HList in DailyTargetMoveOut.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.DailyTargetMoveOut = Convert.ToInt32(ArrHlist[0]) * 100 / item.target_hari;
                        }
                    }

                    List<string> TripAnak = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                        if (anak == null)
                        {
                            string EachHarike = "0" + "|" + unique;
                            TripAnak.Add(EachHarike);
                        }
                        else
                        {
                            string EachHarike = anak.ToString() + "|" + unique;
                            TripAnak.Add(EachHarike);
                        }


                    };
                    foreach (var item in list)
                    {
                        foreach (var HList in TripAnak.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.trip = Convert.ToInt32(ArrHlist[0]);
                        }
                    }

                    List<string> TripAnakMI = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                        if (anak == null)
                        {
                            string EachHarike = "0" + "|" + unique;
                            TripAnakMI.Add(EachHarike);
                        }
                        else
                        {
                            string EachHarike = anak.ToString() + "|" + unique;
                            TripAnakMI.Add(EachHarike);
                        }



                    };
                    foreach (var item in list)
                    {
                        foreach (var HList in TripAnakMI.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.tripMoveIn = Convert.ToInt32(ArrHlist[0]);
                        }
                    }



                    foreach (var item in list)
                    {
                        foreach (var HList in DailyTargetMoveOut.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.DailyTargetMoveOut = Convert.ToInt32(ArrHlist[0]) / item.target_trip * 100;
                        }
                    }

                    List<string> Persentase = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                        if (anak == null)
                        {
                            string EachHarike = "0" + "|" + unique;
                            Persentase.Add(EachHarike);
                        }
                        else
                        {
                            string EachHarike = anak.ToString() + "|" + unique;
                            Persentase.Add(EachHarike);
                        }


                    };


                    foreach (var item in list)
                    {
                        foreach (var HList in Persentase.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.persentase = item.persentase + Convert.ToInt32(ArrHlist[0]);
                            item.persentase2 = item.persentase / item.target_trip * 100;
                            if (item.persentase2 >= item.DailyTargetMoveOut)
                            {
                                item.penilaian1 = "Lebih Cepat";
                            }
                            else if (item.persentase2 >= item.DailyTargetMoveOut)
                            {
                                item.penilaian1 = "Sesuai";
                            }
                            else
                            {
                                item.penilaian1 = "Lebih Lambat";
                            }
                        }
                    }


                    List<string> PersentaseMoveIn = new List<string>();
                    foreach (var ID in Hari)
                    {
                        var unique = ID.id;

                        int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_in).Sum();

                        if (anak == null)
                        {
                            string EachHarike = "0" + "|" + unique;
                            PersentaseMoveIn.Add(EachHarike);
                        }
                        else
                        {
                            string EachHarike = anak.ToString() + "|" + unique;
                            PersentaseMoveIn.Add(EachHarike);
                        }

                    };
                    foreach (var item in list)
                    {
                        foreach (var HList in PersentaseMoveIn.Where(x => x.Contains($"|{item.id}")))
                        {
                            var ArrHlist = HList.Split('|');
                            item.persentaseMoveIn1 = item.persentaseMoveIn1 + Convert.ToInt32(ArrHlist[0]);
                            item.persentaseMoveIn2 = item.persentaseMoveIn1 / item.target_trip * 100;
                            if (item.persentaseMoveIn2 >= item.DailyTargetMoveOut)
                            {
                                item.penilaian2 = "Lebih Cepat";
                            }
                            if (item.persentaseMoveIn2 == item.DailyTargetMoveOut)
                            {
                                item.penilaian2 = "Sesuai";
                            }
                            else
                            {
                                item.penilaian2 = "Lebih Lambat";
                            }
                        }
                    }


                    List<RigMaterialMovementList> Table = new List<RigMaterialMovementList>();
                    Table = list;
                    Response.HasAnError = false;
                    return Json(new { data = Table }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                Response.HasAnError = true;
                throw ex;
              
            }

          
        }
        [Authorize]
        public ActionResult RigMaterial()
        {
            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();
            var RoleDB = DB.M_RoleManagement.ToList();
            if (Session["Newusername"] == null)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            var user = Session["Newusername"].ToString();
            var appr = (from ROLE in RoleDB
                        where ROLE.username.ToUpper() == user.ToUpper()
                        select new Approval
                        {
                            role_id = ROLE.role_id,
                            username = ROLE.username


                        }).ToList();
            ViewBag.approval = appr;

            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(x => x.username.ToUpper() == username.ToUpper()).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;

            var MaterialMovementDB = DB.T_RigMaterialMovement.ToList();
            var TransporterDB = DB.M_Transporter.ToList();
            var DetailDB = DB.T_RigMaterialMovementDetail.ToList();

         

            var Hari =  DB.T_RigMaterialMovement.ToList();
            var detail = DB.T_RigMaterialMovementDetail.ToList();

            if (appr[0].role_id == 1)
            {


                var DataTable = (from MaterialMovement in MaterialMovementDB
                                 join Transporter in TransporterDB
                                 on MaterialMovement.transporter_id equals Transporter.id
                                 select new RigMaterialMovementList
                                 {

                                     area = MaterialMovement.area,
                                     rig = MaterialMovement.rig,
                                     biaya = MaterialMovement.biaya,
                                     id = MaterialMovement.id,
                                     jarak = MaterialMovement.jarak,
                                     rute_dari = MaterialMovement.rute_dari,
                                     rute_ke = MaterialMovement.rute_ke,
                                     target_hari = MaterialMovement.target_hari,
                                     target_trip = MaterialMovement.target_trip,
                                     tanggal_mulai = MaterialMovement.tanggal_mulai,
                                     transporter = Transporter.name

                                 }).ToList();
                var list = DataTable.ToList();
                List<string> HariKeList = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int HariKe = (from y in DB.T_RigMaterialMovementDetail
                                  where y.rig_material_movement_id == unique
                                  select y.rig_material_movement_id).Count();

                    if (HariKe == 0)
                    {
                        string EachHarike = "0" + "|" + unique;
                        HariKeList.Add(EachHarike);
                    }
                    else
                    {
                        string EachHarike = HariKe.ToString() + "|" + unique;
                        HariKeList.Add(EachHarike);
                    }

                };
                foreach (var item in list)
                {
                    foreach (var HList in HariKeList.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.harike = item.harike + Convert.ToInt32(ArrHlist[0]);
                    }
                }

                List<string> ActualTargt = new List<string>();
                List<string> DailyTargetMoveOut = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int? anak = (from y in DB.T_RigMaterialMovementDetail
                                 where y.rig_material_movement_id == unique
                                 select y.rig_material_movement_id).Count();

                    int? target = (from t in DB.T_RigMaterialMovement
                                   where t.id == unique
                                   select t.target_trip).FirstOrDefault();

                    if (anak == null)
                    {
                        string EachHarike = "0" + "|" + unique;
                        DailyTargetMoveOut.Add(EachHarike);
                    }
                    else
                    {
                        string testesan = target.ToString() + "|" + unique;
                        string EachHarike = anak.ToString() + "|" + unique;
                        DailyTargetMoveOut.Add(EachHarike);
                        ActualTargt.Add(testesan);
                    }


                };
                foreach (var item in list)
                {
                    foreach (var HList in DailyTargetMoveOut.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.DailyTargetMoveOut = Convert.ToInt32(ArrHlist[0]) * 100 / item.target_hari;
                    }
                }

                List<string> TripAnak = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                    if (anak == null)
                    {
                        string EachHarike = "0" + "|" + unique;
                        TripAnak.Add(EachHarike);
                    }
                    else
                    {
                        string EachHarike = anak.ToString() + "|" + unique;
                        TripAnak.Add(EachHarike);
                    }


                };
                foreach (var item in list)
                {
                    foreach (var HList in TripAnak.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.trip = Convert.ToInt32(ArrHlist[0]);
                    }
                }

                List<string> TripAnakMI = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                    if (anak == null)
                    {
                        string EachHarike = "0" + "|" + unique;
                        TripAnakMI.Add(EachHarike);
                    }
                    else
                    {
                        string EachHarike = anak.ToString() + "|" + unique;
                        TripAnakMI.Add(EachHarike);
                    }



                };
                foreach (var item in list)
                {
                    foreach (var HList in TripAnakMI.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.tripMoveIn = Convert.ToInt32(ArrHlist[0]);
                    }
                }



                List<string> Persentase = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                    if (anak == null)
                    {
                        string EachHarike = "0" + "|" + unique;
                        Persentase.Add(EachHarike);
                    }
                    else
                    {
                        string EachHarike = anak.ToString() + "|" + unique;
                        Persentase.Add(EachHarike);
                    }


                };
                foreach (var item in list)
                {
                    foreach (var HList in Persentase.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.persentase = item.persentase + Convert.ToInt32(ArrHlist[0]);
                        item.persentase2 = item.persentase / item.target_trip * 100;
                        if (item.persentase2 >= item.DailyTargetMoveOut)
                        {
                            item.penilaian1 = "Lebih Cepat";
                        }
                        if (item.persentase2 == item.DailyTargetMoveOut)
                        {
                            item.penilaian1 = "Sesuai";
                        }
                        else
                        {
                            item.penilaian1 = "Lebih Lambat";
                        }
                    }
                }


                List<string> PersentaseMoveIn = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_in).Sum();

                    if (anak == null)
                    {
                        string EachHarike = "0" + "|" + unique;
                        PersentaseMoveIn.Add(EachHarike);
                    }
                    else
                    {
                        string EachHarike = anak.ToString() + "|" + unique;
                        PersentaseMoveIn.Add(EachHarike);
                    }

                };
                foreach (var item in list)
                {
                    foreach (var HList in PersentaseMoveIn.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.persentaseMoveIn1 = item.persentaseMoveIn1 + Convert.ToInt32(ArrHlist[0]);
                        item.persentaseMoveIn2 = item.persentaseMoveIn1 / item.target_trip * 100;
                        if (item.persentaseMoveIn2 >= item.DailyTargetMoveOut)
                        {
                            item.penilaian2 = "Lebih Cepat";
                        }
                        else if (item.persentaseMoveIn2 == item.DailyTargetMoveOut)
                        {
                            item.penilaian2 = "Sesuai";
                        }
                        else
                        {
                            item.penilaian2 = "Lebih Lambat";
                        }
                    }
                }


                List<RigMaterialMovementList> Table = new List<RigMaterialMovementList>();
                Table = list;
                Response.HasAnError = false;
                return Json(new { data = Table }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var DataTable = (from MaterialMovement in MaterialMovementDB
                                 join Transporter in TransporterDB
                                 on MaterialMovement.transporter_id equals Transporter.id
                                 where MaterialMovement.last_modified_by.ToUpper() == user.ToUpper()
                                 select new RigMaterialMovementList
                                 {

                                     area = MaterialMovement.area,
                                     rig = MaterialMovement.rig,
                                     biaya = MaterialMovement.biaya,
                                     id = MaterialMovement.id,
                                     jarak = MaterialMovement.jarak,
                                     rute_dari = MaterialMovement.rute_dari,
                                     rute_ke = MaterialMovement.rute_ke,
                                     target_hari = MaterialMovement.target_hari,
                                     target_trip = MaterialMovement.target_trip,
                                     tanggal_mulai = MaterialMovement.tanggal_mulai,
                                     transporter = Transporter.name

                                 }).ToList();
                var list = DataTable.ToList();
                List<string> HariKeList = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int HariKe = (from y in DB.T_RigMaterialMovementDetail
                                  where y.rig_material_movement_id == unique
                                  select y.rig_material_movement_id).Count();

                    if (HariKe == 0)
                    {
                        string EachHarike = "0" + "|" + unique;
                        HariKeList.Add(EachHarike);
                    }
                    else
                    {
                        string EachHarike = HariKe.ToString() + "|" + unique;
                        HariKeList.Add(EachHarike);
                    }

                };
                foreach (var item in list)
                {
                    foreach (var HList in HariKeList.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.harike = item.harike + Convert.ToInt32(ArrHlist[0]);
                    }
                }

                List<string> ActualTargt = new List<string>();
                List<string> DailyTargetMoveOut = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int? anak = (from y in DB.T_RigMaterialMovementDetail
                                 where y.rig_material_movement_id == unique
                                 select y.rig_material_movement_id).Count();

                    int? target = (from t in DB.T_RigMaterialMovement
                                   where t.id == unique
                                   select t.target_trip).FirstOrDefault();

                    if (anak == null)
                    {
                        string EachHarike = "0" + "|" + unique;
                        DailyTargetMoveOut.Add(EachHarike);
                    }
                    else
                    {
                        string testesan = target.ToString() + "|" + unique;
                        string EachHarike = anak.ToString() + "|" + unique;
                        DailyTargetMoveOut.Add(EachHarike);
                        ActualTargt.Add(testesan);
                    }


                };
                foreach (var item in list)
                {
                    foreach (var HList in DailyTargetMoveOut.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.DailyTargetMoveOut = Convert.ToInt32(ArrHlist[0]) * 100 / item.target_hari;
                    }
                }

                List<string> TripAnak = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                    if (anak == null)
                    {
                        string EachHarike = "0" + "|" + unique;
                        TripAnak.Add(EachHarike);
                    }
                    else
                    {
                        string EachHarike = anak.ToString() + "|" + unique;
                        TripAnak.Add(EachHarike);
                    }


                };
                foreach (var item in list)
                {
                    foreach (var HList in TripAnak.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.trip = Convert.ToInt32(ArrHlist[0]);
                    }
                }

                List<string> TripAnakMI = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                    if (anak == null)
                    {
                        string EachHarike = "0" + "|" + unique;
                        TripAnakMI.Add(EachHarike);
                    }
                    else
                    {
                        string EachHarike = anak.ToString() + "|" + unique;
                        TripAnakMI.Add(EachHarike);
                    }



                };
                foreach (var item in list)
                {
                    foreach (var HList in TripAnakMI.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.tripMoveIn = Convert.ToInt32(ArrHlist[0]);
                    }
                }



                List<string> Persentase = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_out).Sum();

                    if (anak == null)
                    {
                        string EachHarike = "0" + "|" + unique;
                        Persentase.Add(EachHarike);
                    }
                    else
                    {
                        string EachHarike = anak.ToString() + "|" + unique;
                        Persentase.Add(EachHarike);
                    }


                };
                foreach (var item in list)
                {
                    foreach (var HList in Persentase.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.persentase = item.persentase + Convert.ToInt32(ArrHlist[0]);
                        item.persentase2 = item.persentase / item.target_trip * 100;
                        if (item.persentase2 >= item.DailyTargetMoveOut)
                        {
                            item.penilaian1 = "Lebih Cepat";
                        }
                        if (item.persentase2 == item.DailyTargetMoveOut)
                        {
                            item.penilaian1 = "Sesuai";
                        }
                        else
                        {
                            item.penilaian1 = "Lebih Lambat";
                        }
                    }
                }


                List<string> PersentaseMoveIn = new List<string>();
                foreach (var ID in Hari)
                {
                    var unique = ID.id;

                    int? anak = (from tes in DB.T_RigMaterialMovementDetail where tes.rig_material_movement_id == unique select tes.trip_move_in).Sum();

                    if (anak == null)
                    {
                        string EachHarike = "0" + "|" + unique;
                        PersentaseMoveIn.Add(EachHarike);
                    }
                    else
                    {
                        string EachHarike = anak.ToString() + "|" + unique;
                        PersentaseMoveIn.Add(EachHarike);
                    }

                };
                foreach (var item in list)
                {
                    foreach (var HList in PersentaseMoveIn.Where(x => x.Contains($"|{item.id}")))
                    {
                        var ArrHlist = HList.Split('|');
                        item.persentaseMoveIn1 = item.persentaseMoveIn1 + Convert.ToInt32(ArrHlist[0]);
                        item.persentaseMoveIn2 = item.persentaseMoveIn1 / item.target_trip * 100;
                        if (item.persentaseMoveIn2 >= item.DailyTargetMoveOut)
                        {
                            item.penilaian2 = "Lebih Cepat";
                        }
                        else if (item.persentaseMoveIn2 == item.DailyTargetMoveOut)
                        {
                            item.penilaian2 = "Sesuai";
                        }
                        else
                        {
                            item.penilaian2 = "Lebih Lambat";
                        }
                    }
                }


                List<RigMaterialMovementList> Table = new List<RigMaterialMovementList>();
                Table = list;
                Response.HasAnError = false;
                return Json(new { data = Table }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RigMaterialForm()
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
            var UserRole = DB.M_RoleManagement.Where(n => n.username.ToUpper() == username.ToUpper()).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;
            ResponseMessage Response = new ResponseMessage();



           
            var TransporterDB = DB.M_Transporter.ToList();
            var MaterialMovementDB = DB.T_RigMaterialMovementDetail.ToList();
            var NotDetail = DB.T_RigMaterialMovement.ToList();
            var DataTable = (from MaterialMovement in MaterialMovementDB
                             //join Test in NotDetail
                             //on MaterialMovement.rig_material_movement_id equals Test.id
                             select new RigMaterialMovementDetail
                             {
                                 id = MaterialMovement.id,
                                 trip_move_out = MaterialMovement.trip_move_out,
                                 trip_move_in = MaterialMovement.trip_move_in,
                                 kendala = MaterialMovement.kendala,
                                 tindaklanjut = MaterialMovement.tindak_lanjut

                             }).ToList();


            var datavm2 = (from Transporter in TransporterDB
                           select new DDLTransporter
                           {
                               id = Transporter.id,
                               name = Transporter.name
                           }).ToList();
            List<RigMaterialMovementDetail> Table = new List<RigMaterialMovementDetail>();
            Table = DataTable;
            Response.HasAnError = false;
            ViewBag.datavm2 = datavm2;
            return View(Table);
        }
        [HttpPost]
        public HttpResponseMessage CreateRigMaterial(RigMaterialMovementList Model) {
            try
            {

                DB_RMMEntities DB = new DB_RMMEntities();

                string username = Session["Newusername"].ToString();
                var UserRole = DB.M_RoleManagement.Where(n => n.username.ToUpper() == username.ToUpper()).FirstOrDefault();
                var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
                DDLRole ddl = new DDLRole
                {
                    name = Role.name
                };
                ViewBag.Role = ddl;

                ResponseMessage Response = new ResponseMessage();
                T_RigMaterialMovement NewMC = new T_RigMaterialMovement
                {
                    tanggal_mulai = DateTime.Now,
                    area = Model.area,
                    rig = Model.rig,
                    rute_dari = Model.rute_dari,
                    rute_ke = Model.rute_ke,
                    jarak = Model.jarak,
                    transporter_id = Model.transporter_id,
                    biaya = Model.biaya,
                    target_hari= Model.target_hari,
                    target_trip = Model.target_trip,
                    last_modified_by = username,
                    last_modified_date = DateTime.Now

                };
                //Insert to Database
                
                DB.T_RigMaterialMovement.Add(NewMC);
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

        [HttpPost]
        public HttpResponseMessage CreateRigMaterialDetails(RigMaterialMovementList Model)
        {
            try
            {
                //DateTime dateTime11; // 1/1/0001 12:00:00 AM  
                //bool isSuccess2 = DateTime.TryParseExact(Model.test_tanggal_move, "dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime11);

                //yyyy-MM-dd HH:mm:ss
                string date = Model.test_tanggal_move.Substring(8, 2);
                string month = Model.test_tanggal_move.Substring(5, 2);
                string year = Model.test_tanggal_move.Substring(0, 4);
                string hour = Model.test_tanggal_move.Substring(11, 2);
                string minute = Model.test_tanggal_move.Substring(14, 2);
                string ActualDate = year + "-" + month + "-" + date + " " + hour + ":" + minute;
                //DateTime test = Convert.ToDateTime(ActualDate);

                

                DateTime myDate = DateTime.ParseExact(ActualDate, "yyyy-MM-dd HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);



                DB_RMMEntities DB = new DB_RMMEntities();
               // DateTime dt = DateTime.ParseExact(dateTime11, "dd/MM/yyyy hh:ii", CultureInfo.InvariantCulture);

                ResponseMessage Response = new ResponseMessage();
                T_RigMaterialMovementDetail NewMC = new T_RigMaterialMovementDetail
                {
                    trip_move_out = Model.trip_move_out,
                    trip_move_in = Model.trip_move_in,
                    kendala = Model.kendala,
                    tindak_lanjut = Model.tindaklanjut,
                    rig_material_movement_id = Model.rig_material_movement_id,
                    faktor_keterlambatan = Model.faktorketerlambatan,
                    tanggal_move = myDate,
                    test_tanggal_move = Model.test_tanggal_move



                };
                //Insert to Database

                DB.T_RigMaterialMovementDetail.Add(NewMC);
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


        public HttpResponseMessage CreateRigMaterialDetail(RigMaterialMovementDetail Model)
        {
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();

                var GetLatest = DB.T_RigMaterialMovement
                             .Where(x => x.id != null)
                             .OrderByDescending(x => x.id)
                             .Take(1)
                             .Select(x => x.id)
                             .ToList()
                             .FirstOrDefault();


                T_RigMaterialMovementDetail NewMC = new T_RigMaterialMovementDetail
                {
                    
                    rig_material_movement_id = GetLatest,
                    trip_move_out = Model.trip_move_out,
                    trip_move_in = Model.trip_move_in,
                    kendala = Model.kendala,
                    tindak_lanjut = Model.tindaklanjut,
                    faktor_keterlambatan = Model.faktorketerlambatan, 
                    tanggal_move = Model.tanggal_move
                    //jarak = Model.jarak,
                    //transporter_id = Model.transporter_id,
                    //target_hari = Model.target_hari,
                    //target_trip = Model.target_trip

                };
                //Insert to Database

                DB.T_RigMaterialMovementDetail.Add(NewMC);
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

        [HttpPost]
        public HttpResponseMessage EditRigMaterial(RigMaterialMovementList Model)
        {
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();

                string username = Session["Newusername"].ToString();
                var UserRole = DB.M_RoleManagement.Where(n => n.username.ToUpper() == username.ToUpper()).FirstOrDefault();
                var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
                DDLRole ddl = new DDLRole
                {
                    name = Role.name
                };
                ViewBag.Role = ddl;

                ResponseMessage Response = new ResponseMessage();
               // T_RigMaterialMovement NewMC = new T_RigMaterialMovement
                T_RigMaterialMovement NewMC = DB.T_RigMaterialMovement.Where(x => x.id == Model.id).FirstOrDefault();
                {
                    NewMC.tanggal_mulai = Model.tanggal_mulai;
                    NewMC.area = Model.area;
                    NewMC.rig = Model.rig;
                    NewMC.rute_dari = Model.rute_dari;
                    NewMC.rute_ke = Model.rute_ke;
                    NewMC.jarak = Model.jarak;
                    NewMC.transporter_id = Model.transporter_id;
                    NewMC.target_hari = Model.target_hari;
                    NewMC.target_trip = Model.target_trip;
                    NewMC.last_modified_by = username;
                    NewMC.last_modified_date = DateTime.Now;

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

        [HttpPost]
        public HttpResponseMessage EditRigMaterialParent(RigMaterialMovementList Model)
        {
            try
            {
                string username = Session["Newusername"].ToString();
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();


                string date = Model.test_tanggal_move.Substring(8, 2);
                string month = Model.test_tanggal_move.Substring(5, 2);
                string year = Model.test_tanggal_move.Substring(0, 4);
                string hour = Model.test_tanggal_move.Substring(11, 2);
                string minute = Model.test_tanggal_move.Substring(14, 2);
                string ActualDate = year + "-" + month + "-" + date + " " + hour + ":" + minute;
                //DateTime test = Convert.ToDateTime(ActualDate);



                DateTime myDate = DateTime.ParseExact(ActualDate, "yyyy-MM-dd HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);


                // T_RigMaterialMovement NewMC = new T_RigMaterialMovement
                T_RigMaterialMovement NewMC = DB.T_RigMaterialMovement.Where(x => x.id == Model.id).FirstOrDefault();
                {
                    NewMC.tanggal_mulai = myDate;
                    NewMC.area = Model.area;
                    NewMC.rig = Model.rig;
                    NewMC.rute_dari = Model.rute_dari;
                    NewMC.rute_ke = Model.rute_ke;
                    NewMC.jarak = Model.jarak;
                    NewMC.transporter_id = Model.transporter_id;
                    NewMC.target_hari = Model.target_hari;
                    NewMC.target_trip = Model.target_trip;
                    NewMC.biaya = Model.biaya;
                    NewMC.last_modified_by = username;
                    NewMC.last_modified_date = DateTime.Now;

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


        [HttpPost]
        public HttpResponseMessage EditRigMaterialDetail(RigMaterialMovementList Model)
        {
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                string date = Model.test_tanggal_move.Substring(8, 2);
                string month = Model.test_tanggal_move.Substring(5, 2);
                string year = Model.test_tanggal_move.Substring(0, 4);
                string hour = Model.test_tanggal_move.Substring(11, 2);
                string minute = Model.test_tanggal_move.Substring(14, 2);
                string ActualDate = year + "-" + month + "-" + date + " " + hour + ":" + minute;
                //DateTime test = Convert.ToDateTime(ActualDate);



                DateTime myDate = DateTime.ParseExact(ActualDate, "yyyy-MM-dd HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);

                // T_RigMaterialMovement NewMC = new T_RigMaterialMovement
                T_RigMaterialMovementDetail NewMC = DB.T_RigMaterialMovementDetail.Where(x => x.id == Model.id).FirstOrDefault();
                {
                    NewMC.trip_move_out = Model.trip_move_out;
                    NewMC.trip_move_in = Model.trip_move_in;
                    NewMC.kendala = Model.kendala;
                    NewMC.tindak_lanjut = Model.tindaklanjut;
                    NewMC.faktor_keterlambatan = Model.faktorketerlambatan;
                    NewMC.tanggal_move = myDate;
                    
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
        public ActionResult DeleteRigMaterial(int id)
        {
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                var MaterialMovementDB = DB.T_RigMaterialMovement.ToList();
                var MaterialMovementDetailDB = DB.T_RigMaterialMovementDetail.Where(xd => xd.rig_material_movement_id == id).Count();
                               
                               
                var x = (from y in DB.T_RigMaterialMovement
                         where y.id == id
                         select y).FirstOrDefault();
                for(int d = 0; d < MaterialMovementDetailDB; d++)
                {
                    var yx = (from a in DB.T_RigMaterialMovementDetail
                              where a.rig_material_movement_id == id
                              select a).FirstOrDefault();
                    //Insert to Database
                    DB.T_RigMaterialMovementDetail.Remove(yx);
                    DB.SaveChanges();
                }
                DB.T_RigMaterialMovement.Remove(x);
                DB.SaveChanges();
                //Define error -> true and return Data
                Response.HasAnError = false;
                return RedirectToAction("RigMaterialList");
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public HttpResponseMessage DeleteRigMaterialDetail(RigMaterialMovementDetail Model)
        {
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                var MaterialMovementDB = DB.T_RigMaterialMovementDetail.ToList();
                var x = (from y in DB.T_RigMaterialMovementDetail
                         where y.id == Model.id
                         select y).FirstOrDefault();
                //Insert to Database

                DB.T_RigMaterialMovementDetail.Remove(x);
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
        public object GetEditList(GetEditListRigMaterialDetail Model)

        {
            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();
            try
            {

                var testest = (from RigMovement in DB.T_RigMaterialMovement
                              where RigMovement.id == Model.id

                              select new GetEditListRigMaterial
                              {
                                  tanggal_mulai = RigMovement.tanggal_mulai
                                
                              }).ToList(); 
                    
                    
                    
                    var EditRigMaterialData = from RigMovement in DB.T_RigMaterialMovement
                                          where RigMovement.id == Model.id

                                          select new GetEditListRigMaterial
                                          {   tanggal_mulai = RigMovement.tanggal_mulai,
                                              area = RigMovement.area,
                                              rig = RigMovement.rig,
                                              rute_dari= RigMovement.rute_dari,
                                              rute_ke = RigMovement.rute_ke,
                                              jarak = RigMovement.jarak,
                                              transporter_id = RigMovement.transporter_id,
                                              biaya = RigMovement.biaya,
                                              target_hari = RigMovement.target_hari,
                                              target_trip = RigMovement.target_trip

                                          };

                //Define error -> true and return Data
                // List<GetEditListRigMaterial> newmodel = new List<GetEditListRigMaterial>();
                Response.HasAnError = false;
                var json = new JavaScriptSerializer().Serialize(EditRigMaterialData);
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
        [System.Web.Http.HttpPost]
        public object GetEditListDetail(GetEditListRigMaterialDetail Model)

        {
            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();
            try
            {
                var EditRigMaterialData = from RigMovement in DB.T_RigMaterialMovementDetail
                                          where RigMovement.id == Model.id

                                          select new GetEditListRigMaterialDetail
                                          {
                                              tanggalmove = RigMovement.tanggal_move,
                                              trip_move_out = RigMovement.trip_move_out,
                                              trip_move_in= RigMovement.trip_move_in,
                                              kendala = RigMovement.kendala,
                                              tindaklanjut = RigMovement.tindak_lanjut,
                                              faktorketerlambatan = RigMovement.faktor_keterlambatan

                                          };
                var TransporterDB = DB.M_Transporter.ToList();
                var datavm2 = (from Transporter in TransporterDB
                               select new DDLTransporter
                               {
                                   id = Transporter.id,
                                   name = Transporter.name
                               }).ToList();
                ViewBag.datavm2 = datavm2;
                //Define error -> true and return Data
                // List<GetEditListRigMaterial> newmodel = new List<GetEditListRigMaterial>();
                Response.HasAnError = false;
                var json = new JavaScriptSerializer().Serialize(EditRigMaterialData);
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
    }

   

}

    
