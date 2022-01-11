using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
using RigMaterialMovementWeb.Models;

namespace RigMaterialMovementWeb.Controllers
{
    public class PermintaanPekerjaanController : Controller
    {
        // GET: PermintaanPekerjaan

        public ActionResult PermintaanPekerjaanApproval()
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
            var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
            var StatusDB = DB.M_Status.ToList();

            var StatusDB2 = DB.VW_STATUS.ToList();
            var status = (from StatusLinq in StatusDB2
                          select new DDLStatusPermintaanPekerjaan
                          {
                              id = StatusLinq.id,
                              name = StatusLinq.name


                          }).ToList();
            var RoleDB = DB.M_RoleManagement.ToList();
            var appr = (from ROLE in RoleDB
                        where ROLE.username.ToUpper() == username.ToUpper()
                        select new Approval
                          {
                              role_id = ROLE.role_id,
                              username = ROLE.username


                          }).ToList();
            ViewBag.approval = appr;
            ViewBag.statussrc = status;
            ViewBag.datavm2 = status;
            return View();

        }

        public HttpResponseMessage DeletePekerjaan(PermintaanPekerjaanList Model)
        {
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                var TransporterDB = DB.T_PermintaanPekerjaan.ToList();
                var MaterialMovementDetailDB = DB.T_PermintaanPekerjaanDetail.Where(xd => xd.permintaan_pekerjaan_id == Model.id).Count();
                var x = (from y in DB.T_PermintaanPekerjaan
                         where y.id == Model.id
                         select y).FirstOrDefault();
                for (int d = 0; d < MaterialMovementDetailDB; d++)
                {
                    var yx = (from a in DB.T_PermintaanPekerjaanDetail
                              where a.permintaan_pekerjaan_id == Model.id
                              select a).FirstOrDefault();
                    //Insert to Database
                    DB.T_PermintaanPekerjaanDetail.Remove(yx);
                    DB.SaveChanges();
                }
                DB.T_PermintaanPekerjaan.Remove(x);
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

        public HttpResponseMessage DeletePermintaanPekerjaanDetail(PermintaanPekerjaanList Model)
        {
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                var TransporterDB = DB.T_PermintaanPekerjaan.ToList();
                var MaterialMovementDetailDB = DB.T_PermintaanPekerjaanDetail.Where(xd => xd.permintaan_pekerjaan_id == Model.id).Count();
                var x = (from y in DB.T_PermintaanPekerjaanDetail
                         where y.id == Model.id
                         select y).FirstOrDefault();
                DB.T_PermintaanPekerjaanDetail.Remove(x);
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
        public ActionResult EditPekerjaan(PermintaanPekerjaanList Model)
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

            T_PermintaanPekerjaan Transporter = DB.T_PermintaanPekerjaan.Where(x => x.id == Model.id).FirstOrDefault();
            {

                Transporter.no_wo = Model.no_wo;
                Transporter.no_activity = Model.no_activity;
                Transporter.judul_pekerjaan = Model.judul_pekerjaan;
                Transporter.tanggal_pekerjaan = DateTime.Now;
                Transporter.lokasi_tujuan = Model.lokasi_tujuan;
                Transporter.lokasi_asal = Model.lokasi_asal;
                Transporter.cp_lokasi_asal = Model.cp_lokasi_asal;
                Transporter.cp_lokasi_tujuan  = Model.cp_lokasi_tujuan;
                Transporter.detail_barang = Model.detail_barang;
                Transporter.keterangan = Model.keterangan;
                Transporter.approver = Model.approver;
            };
            DB.SaveChanges();
            //Define error -> true and return Data
            // List<GetEditListRigMaterial> newmodel = new List<GetEditListRigMaterial>();
            Response.HasAnError = false;
            //var json = new JavaScriptSerializer().Serialize(transporter);

            return View();
        }



        public ActionResult EditPermintaanPekerjaanDetail(PermintaanPekerjaanList Model)
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

            T_PermintaanPekerjaanDetail Transporter = DB.T_PermintaanPekerjaanDetail.Where(x => x.id == Model.id).FirstOrDefault();
            {

                Transporter.notes = Model.notes;

            };
            DB.SaveChanges();
            //Define error -> true and return Data
            // List<GetEditListRigMaterial> newmodel = new List<GetEditListRigMaterial>();
            Response.HasAnError = false;
            //var json = new JavaScriptSerializer().Serialize(transporter);

            return View();
        }

        public object GetEdit(PermintaanPekerjaanList Model)

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
            try
            {
                var getEdit = from Transporter in DB.T_PermintaanPekerjaan
                              where Transporter.id == Model.id
                              select new PermintaanPekerjaanList
                              {
                                  id = Transporter.id,
                                  no_wo = Transporter.no_wo,
                                  no_activity = Transporter.no_activity,
                                  judul_pekerjaan = Transporter.judul_pekerjaan,
                                  tanggal_pekerjaan = Transporter.tanggal_pekerjaan,
                                  lokasi_asal = Transporter.lokasi_asal,
                                  cp_lokasi_asal = Transporter.cp_lokasi_asal,
                                  cp_lokasi_tujuan = Transporter.cp_lokasi_tujuan,
                                  lokasi_tujuan = Transporter.lokasi_tujuan,
                                  detail_barang = Transporter.detail_barang,
                                  keterangan = Transporter.keterangan,
                                  approver = Transporter.approver
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

        public object GetEditPermintaanPekerjaanDetail(PermintaanPekerjaanList Model)

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
            try
            {
                var getEdit = from Transporter in DB.T_PermintaanPekerjaanDetail
                              where Transporter.id == Model.id
                              select new PermintaanPekerjaanList
                              {
                                  id = Transporter.id,
                                  tanggalstat = Transporter.tanggal,
                                  notes = Transporter.notes
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

        public ActionResult PermintaanPekerjaanApprovalTabel(PermintaanPekerjaanList Model)
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
            var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
            var StatusDB = DB.M_Status.ToList();
            try

            {
                var DataTable = (from Kerja in PermintaanDB
                                 join Status in StatusDB
                                 on Kerja.status_id equals Status.id
                                 select new PermintaanPekerjaanList
                                 {
                                     id = Kerja.id,
                                     no_wo = Kerja.no_wo,
                                     no_activity = Kerja.no_activity,
                                     judul_pekerjaan = Kerja.judul_pekerjaan,
                                     tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
                                     lokasi_asal = Kerja.lokasi_asal,
                                     cp_lokasi_asal = Kerja.cp_lokasi_asal,
                                     lokasi_tujuan = Kerja.lokasi_tujuan,
                                     cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
                                     detail_barang = Kerja.detail_barang,
                                     keterangan = Kerja.keterangan,
                                     last_modified_by = Kerja.last_modified_by,
                                     last_modified_date = Kerja.last_modified_date,
                                     status = Status.name,
                                     approved_by = Kerja.approved_by,
                                     approved_date = Kerja.approved_date

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

            }


        }

        [HttpPost]
        public ActionResult FilterPermintaanPekerjaanApproval(PermintaanPekerjaanList Model)
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
            var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
            var StatusDB = DB.M_Status.ToList();
            if(Model.status == null)
            {
                Model.status = string.Empty;
            }
            
            try

            {


                var DataTableSrc = (from Kerja in PermintaanDB
                                    where Kerja.no_wo.Contains(Model.no_wo ?? "") &&
                                    Kerja.no_activity.Contains(Model.no_activity ?? "") &&
                                    Kerja.judul_pekerjaan.Contains(Model.judul_pekerjaan ?? "") &&
                                    Kerja.lokasi_asal.Contains(Model.lokasi_asal ?? "") &&
                                    Kerja.lokasi_tujuan.Contains(Model.cp_lokasi_tujuan ?? "") &&
                                    (Model.tanggal_pekerjaandari == null && Model.tanggal_pekerjaansampai == null || Kerja.tanggal_pekerjaan >= Model.tanggal_pekerjaandari && Kerja.tanggal_pekerjaan <= Model.tanggal_pekerjaansampai) &&
                                    Kerja.status_id.ToString().Contains(Model.status ?? "")
                                    join Status in StatusDB
                                    on Kerja.status_id equals Status.id
                                    select new PermintaanPekerjaanList
                                    {
                                        id = Kerja.id,
                                        no_wo = Kerja.no_wo,
                                        no_activity = Kerja.no_activity,
                                        judul_pekerjaan = Kerja.judul_pekerjaan,
                                        tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
                                        lokasi_asal = Kerja.lokasi_asal,
                                        cp_lokasi_asal = Kerja.cp_lokasi_asal,
                                        lokasi_tujuan = Kerja.lokasi_tujuan,
                                        cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
                                        detail_barang = Kerja.detail_barang,
                                        keterangan = Kerja.keterangan,
                                        last_modified_by = Kerja.last_modified_by,
                                        last_modified_date = Kerja.last_modified_date,
                                        status_id = Kerja.status_id,
                                        approved_by = Kerja.approved_by,
                                        approved_date = Kerja.approved_date,
                                        status = Status.name,

                                    }).ToList();
                List<TransporterList> Table = new List<TransporterList>();
                //Table = DataTable;
                Response.HasAnError = false;
                return Json(new { data = DataTableSrc }, JsonRequestBehavior.AllowGet);
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


        [Authorize]
        public ActionResult PermintaanPekerjaanList()
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
            var UserRole = DB.M_RoleManagement.Where(x => x.username.ToUpper() == username.ToUpper()).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };

            ViewBag.Role = ddl;

            var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
            var StatusDB = DB.M_Status.ToList();
            var StatusDB2 = DB.VW_STATUS.ToList();
            var status = (from StatusLinq in StatusDB2
                          select new DDLStatusPermintaanPekerjaan
                          {
                              id = StatusLinq.id,
                              name = StatusLinq.name


                          }).ToList();
            var RoleDB = DB.M_RoleManagement.ToList();
            var user = Session["Newusername"].ToString();
            var appr = (from ROLE in RoleDB
                        where ROLE.username.ToUpper() == user.ToUpper()
                        select new Approval
                        {
                            role_id = ROLE.role_id,
                            username = ROLE.username


                        }).ToList();
            var ApproverDB = DB.M_RoleManagement.ToList();
            var approver = (from orang in ApproverDB
                            where orang.role_id == 4
                        select new Approver
                        {
                           id = orang.role_id,
                           name = orang.username


                        }).ToList();
            ViewBag.orang = approver;
            ViewBag.approval = appr;
            ViewBag.statussrc = status;
            ViewBag.datavm2 = status;
            return View();

        }

        public ActionResult PermintaanPekerjaan(PermintaanPekerjaanList Model)
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
            var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
            var StatusDB = DB.M_Status.ToList();
            var StatusDB2 = DB.VW_STATUS.ToList();
            try
          
            {

                var RoleDB = DB.M_RoleManagement.ToList();
                var user = Session["Newusername"].ToString();
                var appr = (from ROLE in RoleDB
                            where ROLE.username.ToUpper() == user.ToUpper()
                            select new Approval
                            {
                                role_id = ROLE.role_id,
                                username = ROLE.username


                            }).ToList();
                List<PermintaanPekerjaanList> Table = new List<PermintaanPekerjaanList>();
                if (appr[0].role_id == 4)
                {
                    var DataTable = (from Kerja in PermintaanDB
                                     join status in StatusDB on
                                     Kerja.status_id equals status.id
                                     where Kerja.approver.ToUpper() == appr[0].username.ToUpper()
                                     select new PermintaanPekerjaanList
                                     {
                                         id = Kerja.id,
                                         no_wo = Kerja.no_wo,
                                         no_activity = Kerja.no_activity,
                                         judul_pekerjaan = Kerja.judul_pekerjaan,
                                         tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
                                         lokasi_asal = Kerja.lokasi_asal,
                                         cp_lokasi_asal = Kerja.cp_lokasi_asal,
                                         lokasi_tujuan = Kerja.lokasi_tujuan,
                                         cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
                                         detail_barang = Kerja.detail_barang,
                                         keterangan = Kerja.keterangan,
                                         last_modified_by = Kerja.last_modified_by,
                                         last_modified_date = Kerja.last_modified_date,
                                         status = status.name,
                                         approved_by = Kerja.approved_by,
                                         approved_date = Kerja.approved_date,
                                         finished_by = Kerja.finished_by

                                     }).ToList();
                 
                    Table = DataTable;
                }

               else if (appr[0].role_id == 2)
                {
                    var DataTable = (from Kerja in PermintaanDB
                                     join status in StatusDB on
                                     Kerja.status_id equals status.id
                                     where Kerja.status_id == 3 || Kerja.status_id == 2
                                     select new PermintaanPekerjaanList
                                     {
                                         id = Kerja.id,
                                         no_wo = Kerja.no_wo,
                                         no_activity = Kerja.no_activity,
                                         judul_pekerjaan = Kerja.judul_pekerjaan,
                                         tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
                                         lokasi_asal = Kerja.lokasi_asal,
                                         cp_lokasi_asal = Kerja.cp_lokasi_asal,
                                         lokasi_tujuan = Kerja.lokasi_tujuan,
                                         cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
                                         detail_barang = Kerja.detail_barang,
                                         keterangan = Kerja.keterangan,
                                         last_modified_by = Kerja.last_modified_by,
                                         last_modified_date = Kerja.last_modified_date,
                                         status = status.name,
                                         approved_by = Kerja.approved_by,
                                         approved_date = Kerja.approved_date,
                                         finished_by = Kerja.finished_by

                                     }).ToList();

                    Table = DataTable;
                }

                else
                {
                    var DataTable = (from Kerja in PermintaanDB
                                     join status in StatusDB on
                                     Kerja.status_id equals status.id
                                     select new PermintaanPekerjaanList
                                     {
                                         id = Kerja.id,
                                         no_wo = Kerja.no_wo,
                                         no_activity = Kerja.no_activity,
                                         judul_pekerjaan = Kerja.judul_pekerjaan,
                                         tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
                                         lokasi_asal = Kerja.lokasi_asal,
                                         cp_lokasi_asal = Kerja.cp_lokasi_asal,
                                         lokasi_tujuan = Kerja.lokasi_tujuan,
                                         cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
                                         detail_barang = Kerja.detail_barang,
                                         keterangan = Kerja.keterangan,
                                         last_modified_by = Kerja.last_modified_by,
                                         last_modified_date = Kerja.last_modified_date,
                                         status = status.name,
                                         approved_by = Kerja.approved_by,
                                         approved_date = Kerja.approved_date,
                                         finished_by = Kerja.finished_by

                                     }).ToList();
                   
                    Table = DataTable;
                }

                Response.HasAnError = false;
                return Json(new { data = Table }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                Response.HasAnError = true;
                throw ex;

            }


        }

        public object GetModalData()
        {
            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();
            var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
            var StatusDB = DB.M_Status.ToList();

            var CountPPTable = DB.T_PermintaanPekerjaan.Count();
            
            String Nowo;
            String NoAct;

            var GetLatest = DB.T_PermintaanPekerjaan
                                    .Where(x => x.id != null)
                                    .OrderByDescending(x => x.id)
                                    .Take(1)
                                    .Select(x => x.no_wo)
                                    .ToList()
                                    .FirstOrDefault();
            var GetLatestNoAct = DB.T_PermintaanPekerjaan
                                   .Where(x => x.id != null)
                                   .OrderByDescending(x => x.id)
                                   .Take(1)
                                   .Select(x => x.no_activity)
                                   .ToList()
                                   .FirstOrDefault();

            try

            {


                if (CountPPTable == 0)
                {
                    string AutoGenNo_Wo = "WO-1";
                    Nowo = AutoGenNo_Wo;
                }
                else 
                {
                    string str = Regex.Match(GetLatest, @"\d+").Value;
                    int Conversion = Convert.ToInt32(str);
                    int Add1FromLatest = Conversion + 1;
                    string AutoGenNoWo = "WO-" + Add1FromLatest.ToString();
                    Nowo = AutoGenNoWo;
                }
                if (CountPPTable == 0)
                {
                    string AutoGenNo_Act = "1";
                    NoAct = AutoGenNo_Act;
                }
                else
                {
                    string str = Regex.Match(GetLatestNoAct, @"\d+").Value;
                    int Conversion = Convert.ToInt32(str);
                    int Add1FromLatest = Conversion + 1;
                    string AutoGenNoAct = Add1FromLatest.ToString();
                    NoAct = AutoGenNoAct;
                }

                var DataTable = new List<PermintaanPekerjaanList>();
                if (PermintaanDB.Count() > 0)
                { 
                    DataTable = (from Kerja in PermintaanDB
                                     select new PermintaanPekerjaanList
                                     {
                                   
                                         no_wo = Nowo,
                                         no_activity = NoAct
                                     

                                     }).ToList();
                } 
                else
                {
                    PermintaanPekerjaanList data = new PermintaanPekerjaanList();
                    data.no_wo = Nowo;
                    data.no_activity = NoAct;
                    DataTable.Add(data);
                }
                List<TransporterList> Table = new List<TransporterList>();
                //Table = DataTable;
                Response.HasAnError = false;
                var json = new JavaScriptSerializer().Serialize(DataTable);
                return json;
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                Response.HasAnError = true;
                throw ex;

            }


        }


        [HttpPost]
        public HttpResponseMessage CreatePermintaanPekerjaan(PermintaanPekerjaanList Model)
        {
            var user = Session["Newusername"].ToString();
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
                T_PermintaanPekerjaan NewMC = new T_PermintaanPekerjaan
                {
                 
                    no_wo = Model.no_wo,
                    no_activity = Model.no_activity,
                    judul_pekerjaan = Model.judul_pekerjaan,
                    tanggal_pekerjaan = DateTime.Now,
                    lokasi_asal = Model.lokasi_asal,
                    cp_lokasi_asal = Model.cp_lokasi_asal,
                    lokasi_tujuan = Model.lokasi_tujuan,
                    cp_lokasi_tujuan = Model.cp_lokasi_tujuan,
                    detail_barang = Model.detail_barang,
                    keterangan = Model.keterangan,
                    last_modified_by = user,
                    last_modified_date = DateTime.Now,
                    status_id = 1,
                    approver = Model.approver
                    //approved_by = Kerja.approved_by,
                    //approved_date = Kerja.approved_date

                };
                //Insert to Database

                DB.T_PermintaanPekerjaan.Add(NewMC);
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
        public HttpResponseMessage CreatePermintaanPekerjaanDetail(PermintaanPekerjaanList Model)
        {
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                T_PermintaanPekerjaanDetail NewMC = new T_PermintaanPekerjaanDetail
                {

                    permintaan_pekerjaan_id = Model.permintaan_pekerjaan_id,
                    notes = Model.notes,
                    tanggal = DateTime.Now
                  

                };
                //Insert to Database

                DB.T_PermintaanPekerjaanDetail.Add(NewMC);
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

        public ActionResult PermintaanPekerjaanApprovalForm(int param)
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
            var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
            var PermintaanDetailDB = DB.T_PermintaanPekerjaanDetail.ToList();
            var StatusDB = DB.M_Status.ToList();
            var StatusDB2 = DB.VW_STATUS.ToList();
            try

            {
                var table = (from Detail in PermintaanDetailDB
                             where Detail.permintaan_pekerjaan_id == param

                             select new PermintaanPekerjaanList
                             {  id = Detail.id,
                                 notes = Detail.notes,
                                 tanggalstat = Detail.tanggal


                             }).ToList();

                var status = (from StatusLinq in StatusDB2
                              select new DDLStatusPermintaanPekerjaan
                              {
                                  id = StatusLinq.id,
                                  name = StatusLinq.name


                              }).ToList();

                ViewBag.datavm7 = status;

                var DataTable = (from Kerja in PermintaanDB
                                 where Kerja.id == param
                                 select new PermintaanPekerjaanList
                                 {
                                     id = Kerja.id,
                                     no_wo = Kerja.no_wo,
                                     no_activity = Kerja.no_activity,
                                     judul_pekerjaan = Kerja.judul_pekerjaan,
                                     tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
                                     lokasi_asal = Kerja.lokasi_asal,
                                     cp_lokasi_asal = Kerja.cp_lokasi_asal,
                                     lokasi_tujuan = Kerja.lokasi_tujuan,
                                     cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
                                     detail_barang = Kerja.detail_barang,
                                     keterangan = Kerja.keterangan,
                                     last_modified_by = Kerja.last_modified_by,
                                     last_modified_date = Kerja.last_modified_date,
                                     status_id = Kerja.status_id,
                                     approved_by = Kerja.approved_by,
                                     approved_date = Kerja.approved_date

                                 }).ToList();
                List<TransporterList> Table = new List<TransporterList>();
                ViewBag.datanya = DataTable;
                ViewBag.Test = param;
                //Table = DataTable;
                Response.HasAnError = false;
                return View(table);
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                Response.HasAnError = true;
                throw ex;

            }



        }
        public ActionResult PermintaanPekerjaanDetail(int param)
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
            var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
            var PermintaanDetailDB = DB.T_PermintaanPekerjaanDetail.ToList();
            var StatusDB = DB.M_Status.ToList();
            var StatusDB2 = DB.VW_STATUS.ToList();
            try

            {
                var table = (from Detail in PermintaanDetailDB
                             where Detail.permintaan_pekerjaan_id == param

                             select new PermintaanPekerjaanList
                             {
                                 id = Detail.id,
                                 notes = Detail.notes,
                                 tanggalstat = Detail.tanggal


                             }).ToList();

                var status = (from StatusLinq in StatusDB2
                              select new DDLStatusPermintaanPekerjaan
                              {
                                  id = StatusLinq.id,
                                  name = StatusLinq.name


                              }).ToList();

                ViewBag.datavm7 = status;

                var DataTable = (from Kerja in PermintaanDB
                                 where Kerja.id == param
                                 select new PermintaanPekerjaanList
                                 {
                                     id = Kerja.id,
                                     no_wo = Kerja.no_wo,
                                     no_activity = Kerja.no_activity,
                                     judul_pekerjaan = Kerja.judul_pekerjaan,
                                     tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
                                     lokasi_asal = Kerja.lokasi_asal,
                                     cp_lokasi_asal = Kerja.cp_lokasi_asal,
                                     lokasi_tujuan = Kerja.lokasi_tujuan,
                                     cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
                                     detail_barang = Kerja.detail_barang,
                                     keterangan = Kerja.keterangan,
                                     approver = Kerja.approver,
                                     last_modified_by = Kerja.last_modified_by,
                                     last_modified_date = Kerja.last_modified_date,
                                     status_id = Kerja.status_id,
                                     approved_by = Kerja.approved_by,
                                     approved_date = Kerja.approved_date

                                 }).ToList();
                List<TransporterList> Table = new List<TransporterList>();
                ViewBag.datanya = DataTable;
                ViewBag.Test = param;
                //Table = DataTable;
                Response.HasAnError = false;
                return View(table);
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                Response.HasAnError = true;
                throw ex;

            }



        }
        //public ActionResult PermintaanPekerjaanDetail(string param)
        //{
        //    DB_RMMEntities DB = new DB_RMMEntities();

        //    ResponseMessage Response = new ResponseMessage();

        //    string username = Session["Newusername"].ToString();
        //    var UserRole = DB.M_RoleManagement.Where(x => x.username == username).FirstOrDefault();
        //    var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
        //    DDLRole ddl = new DDLRole
        //    {
        //        name = Role.name
        //    };



        //    var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
        //    var StatusDB = DB.M_Status.ToList();
        //    var StatusDB2 = DB.VW_STATUS.ToList();
        //    try

        //    {
        //        var DataTable = (from Kerja in PermintaanDB
        //                         where Kerja.no_wo == param
        //                         select new PermintaanPekerjaanList
        //                         {
        //                             id = Kerja.id,
        //                             no_wo = Kerja.no_wo,
        //                             no_activity = Kerja.no_activity,
        //                             judul_pekerjaan = Kerja.judul_pekerjaan,
        //                             tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
        //                             lokasi_asal = Kerja.lokasi_asal,
        //                             cp_lokasi_asal = Kerja.cp_lokasi_asal,
        //                             lokasi_tujuan = Kerja.lokasi_tujuan,
        //                             cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
        //                             detail_barang = Kerja.detail_barang,
        //                             keterangan = Kerja.keterangan,
        //                             last_modified_by = Kerja.last_modified_by,
        //                             last_modified_date = Kerja.last_modified_date,
        //                             status_id = Kerja.status_id,
        //                             approved_by = Kerja.approved_by,
        //                             approved_date = Kerja.approved_date

        //                         }).ToList();
        //        List<TransporterList> Table = new List<TransporterList>();
        //        ViewBag.Data = DataTable;
        //        //Table = DataTable;
        //        Response.HasAnError = false;
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Message = ex.Message;
        //        Response.HasAnError = true;
        //        throw ex;

        //    }
        //}

        [HttpPost]
        public ActionResult EditPermintaanPekerjaanStatus(PermintaanPekerjaanList Model)
        {
            if (Session["Newusername"] == null)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            var user = Session["Newusername"].ToString();
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
                T_PermintaanPekerjaan NewMC = DB.T_PermintaanPekerjaan.Where(x => x.id == Model.id).FirstOrDefault();
                {
                    NewMC.status_id = 2;
                    NewMC.finished_by = user;
                   // NewMC.approved_date = DateTime.Now;
                };
                //Insert to Database

                //DB.T_RigMaterialMovement.Add(NewMC);
                DB.SaveChanges();
                //Define error -> true and return Data
                Response.HasAnError = false;
                return RedirectToAction("PermintaanPekerjaanApproval");
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public ActionResult EditPermintaanPekerjaanStatusApprove(PermintaanPekerjaanList Model)
        {
            if (Session["Newusername"] == null)
            {
                FormsAuthentication.SignOut();
                Session.RemoveAll();
                //Response.Redirect("~/Page/Login/Login2.aspx");
                return RedirectToAction("Login", "Login");
            }
            var user = Session["Newusername"].ToString();
            try
            {
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                
                T_PermintaanPekerjaan NewMC = DB.T_PermintaanPekerjaan.Where(x => x.id == Model.id).FirstOrDefault();
                {
                    NewMC.status_id = 3;
                    NewMC.approved_by = user;
                    NewMC.approved_date = DateTime.Now;
                };
               
                DB.SaveChanges();
               
                Response.HasAnError = false;
                return RedirectToAction("PermintaanPekerjaanList");
            }
            catch (Exception ex)
            {
                throw ex;
     
            }
        }

        [HttpPost]
        public ActionResult FilterPermintaanPekerjaan(PermintaanPekerjaanList Model)
        {


            if(Model.status == null)
            {
                Model.status = string.Empty;
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

            ResponseMessage Response = new ResponseMessage();
            List<PermintaanPekerjaanList> Table = new List<PermintaanPekerjaanList>();

            var RoleDB = DB.M_RoleManagement.ToList();
            var user = Session["Newusername"].ToString();
            var appr = (from ROLE in RoleDB
                        where ROLE.username.ToUpper() == user.ToUpper()
                        select new Approval
                        {
                            role_id = ROLE.role_id,
                            username = ROLE.username


                        }).ToList();

            var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
            var StatusDB = DB.M_Status.ToList();
            var StatusDB2 = DB.VW_STATUS.ToList();
            try

            {
                if (appr[0].role_id == 4)
                {
                    var DataTableSrc = (from Kerja in PermintaanDB
                                        where Kerja.no_wo.Contains(Model.no_wo ?? "") &&
                                        Kerja.no_activity.Contains(Model.no_activity ?? "") &&
                                        Kerja.judul_pekerjaan.Contains(Model.judul_pekerjaan ?? "") &&
                                        Kerja.lokasi_asal.Contains(Model.lokasi_asal ?? "") &&
                                        Kerja.lokasi_tujuan.Contains(Model.cp_lokasi_tujuan ?? "") &&
                                        (Model.tanggal_pekerjaandari == null && Model.tanggal_pekerjaansampai == null || Kerja.tanggal_pekerjaan >= Model.tanggal_pekerjaandari && Kerja.tanggal_pekerjaan <= Model.tanggal_pekerjaansampai) &&
                                        Kerja.status_id.ToString().Contains(Model.status ?? "") &&
                                        Kerja.approver.ToUpper() == appr[0].username.ToUpper()
                                        join Status in StatusDB
                                       on Kerja.status_id equals Status.id
                                        select new PermintaanPekerjaanList
                                        {
                                            id = Kerja.id,
                                            no_wo = Kerja.no_wo,
                                            no_activity = Kerja.no_activity,
                                            judul_pekerjaan = Kerja.judul_pekerjaan,
                                            tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
                                            lokasi_asal = Kerja.lokasi_asal,
                                            cp_lokasi_asal = Kerja.cp_lokasi_asal,
                                            lokasi_tujuan = Kerja.lokasi_tujuan,
                                            cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
                                            detail_barang = Kerja.detail_barang,
                                            keterangan = Kerja.keterangan,
                                            last_modified_by = Kerja.last_modified_by,
                                            last_modified_date = Kerja.last_modified_date,
                                            status_id = Kerja.status_id,
                                            approved_by = Kerja.approved_by,
                                            approved_date = Kerja.approved_date,
                                            status = Status.name,
                                            finished_by = Kerja.finished_by

                                        }).ToList();
                    Table = DataTableSrc;
                }

                else if (appr[0].role_id == 2)
                {
                    var DataTableSrc = (from Kerja in PermintaanDB
                                        where Kerja.no_wo.Contains(Model.no_wo ?? "") &&
                                        Kerja.no_activity.Contains(Model.no_activity ?? "") &&
                                        Kerja.judul_pekerjaan.Contains(Model.judul_pekerjaan ?? "") &&
                                        Kerja.lokasi_asal.Contains(Model.lokasi_asal ?? "") &&
                                        Kerja.lokasi_tujuan.Contains(Model.cp_lokasi_tujuan ?? "") &&
                                        (Model.tanggal_pekerjaandari == null && Model.tanggal_pekerjaansampai == null || Kerja.tanggal_pekerjaan >= Model.tanggal_pekerjaandari && Kerja.tanggal_pekerjaan <= Model.tanggal_pekerjaansampai) &&
                                        Kerja.status_id == 3
                                        join Status in StatusDB
                                       on Kerja.status_id equals Status.id
                                        select new PermintaanPekerjaanList
                                        {
                                            id = Kerja.id,
                                            no_wo = Kerja.no_wo,
                                            no_activity = Kerja.no_activity,
                                            judul_pekerjaan = Kerja.judul_pekerjaan,
                                            tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
                                            lokasi_asal = Kerja.lokasi_asal,
                                            cp_lokasi_asal = Kerja.cp_lokasi_asal,
                                            lokasi_tujuan = Kerja.lokasi_tujuan,
                                            cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
                                            detail_barang = Kerja.detail_barang,
                                            keterangan = Kerja.keterangan,
                                            last_modified_by = Kerja.last_modified_by,
                                            last_modified_date = Kerja.last_modified_date,
                                            status_id = Kerja.status_id,
                                            approved_by = Kerja.approved_by,
                                            approved_date = Kerja.approved_date,
                                            status = Status.name,
                                            finished_by = Kerja.finished_by

                                        }).ToList();
                    Table = DataTableSrc;
                }
                else
                {
                    var DataTableSrc = (from Kerja in PermintaanDB
                                        where Kerja.no_wo.Contains(Model.no_wo ?? "") &&
                                        Kerja.no_activity.Contains(Model.no_activity ?? "") &&
                                        Kerja.judul_pekerjaan.Contains((Model.judul_pekerjaan == null ? "" : Model.judul_pekerjaan)) &&
                                        Kerja.lokasi_asal.Contains(Model.lokasi_asal ?? "") &&
                                        Kerja.lokasi_tujuan.Contains(Model.cp_lokasi_tujuan ?? "") &&
                                        (Model.tanggal_pekerjaandari == null && Model.tanggal_pekerjaansampai == null || Kerja.tanggal_pekerjaan >= Model.tanggal_pekerjaandari && Kerja.tanggal_pekerjaan <= Model.tanggal_pekerjaansampai) &&
                                        Kerja.status_id.ToString().Contains(Model.status ?? "")
                                        join Status in StatusDB
                                       on Kerja.status_id equals Status.id
                                        select new PermintaanPekerjaanList
                                        {
                                            id = Kerja.id,
                                            no_wo = Kerja.no_wo,
                                            no_activity = Kerja.no_activity,
                                            judul_pekerjaan = Kerja.judul_pekerjaan,
                                            tanggal_pekerjaan = Kerja.tanggal_pekerjaan,
                                            lokasi_asal = Kerja.lokasi_asal,
                                            cp_lokasi_asal = Kerja.cp_lokasi_asal,
                                            lokasi_tujuan = Kerja.lokasi_tujuan,
                                            cp_lokasi_tujuan = Kerja.cp_lokasi_tujuan,
                                            detail_barang = Kerja.detail_barang,
                                            keterangan = Kerja.keterangan,
                                            last_modified_by = Kerja.last_modified_by,
                                            last_modified_date = Kerja.last_modified_date,
                                            status_id = Kerja.status_id,
                                            approved_by = Kerja.approved_by,
                                            approved_date = Kerja.approved_date,
                                            status = Status.name,
                                            finished_by = Kerja.finished_by

                                        }).ToList();
                    Table = DataTableSrc;
                }

                //Table = DataTable;
                Response.HasAnError = false;
                return Json(new { data = Table }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.Message = ex.Message;
                Response.HasAnError = true;
                throw ex;
            }

           
        }

    }
    }
