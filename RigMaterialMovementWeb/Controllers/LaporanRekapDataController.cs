using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RigMaterialMovementWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RigMaterialMovementWeb.Controllers
{
    public class LaporanRekapDataController : Controller
    {
        // GET: LaporanRekapData
        [Authorize]
        public ActionResult Index()
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
            var UserRole = DB.M_RoleManagement.Where(n => n.username == username).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };
            ViewBag.Role = ddl;
            var MaterialMovementDB = DB.T_RigMaterialMovement.ToList();
            var TransporterDB = DB.M_Transporter.ToList();
            var DetailDB = DB.T_RigMaterialMovementDetail.ToList();
            var Data = (from MaterialMovement in MaterialMovementDB
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
            int x = 1;
            var DataTable = (from data in Data
                             join Detail in DetailDB
                             on data.id equals Detail.rig_material_movement_id
                             orderby data.id
                             select new LaporanRekapData
                             {
                                 
                                 area = data.area,
                                 rig = data.rig,
                                 biaya = data.biaya,
                                 id = data.id,
                                 jarak = data.jarak,
                                 rute_dari = data.rute_dari,
                                 rute_ke = data.rute_ke,
                                 target_hari = data.target_hari,
                                 target_trip = data.target_trip,
                                 tanggal_mulai = data.tanggal_mulai,
                                 transporter = data.transporter,
                                 tindaklanjut = Detail.tindak_lanjut,
                                 kendala = Detail.kendala,
                                 faktorketerlambatan = Detail.faktor_keterlambatan,
                                 trip_move_in = Detail.trip_move_in,
                                 trip_move_out = Detail.trip_move_out,
                                 tanggal_move = Detail.tanggal_move,
                                 tripMoveIn = Detail.trip_move_in
                             }).ToList();
           

            List<LaporanRekapData> Item = new List<LaporanRekapData>();
            Item = DataTable.ToList();
            

            for (int i = 0; i < Item.Count; i++)
            {
                LaporanRekapData Item2 = new LaporanRekapData();
                List<LaporanRekapData> Item3 = new List<LaporanRekapData>();
                var val = (from sum in Item
                           where sum.id == Item[i].id
                           select sum.trip_move_in).Sum();
                Item[i].tripmvinSum = Convert.ToInt32(val) ;
                Item2 = Item.Where(z => z.id == Item[i].id).OrderByDescending(y => y.tanggal_move).FirstOrDefault();
                Item[i].tanggal_mulai_string = Item[i].tanggal_mulai.Value.ToShortDateString();
                Item[i].jamMulai = Item[i].tanggal_mulai.Value.ToShortTimeString();
                Item[i].tanggal_selesai = Item2.tanggal_move.Value.ToShortDateString();
                Item[i].jamSelesai = Item2.tanggal_move.Value.ToShortTimeString();
                Item3 = Item.Where(a => a.id == Item[i].id).ToList();
                Item[i].total_hari = Item3.Count();
                if(Item[i].tanggal_mulai_string == Item[i].tanggal_selesai)
                {
                   
                    TimeSpan span = Item2.tanggal_move.Value.Subtract(Item[i].tanggal_mulai.Value);
                    int hour = span.Hours;
                    int minute = span.Minutes;
                    int sec = span.Seconds;
                    Item[i].selisih_string = hour.ToString() +" "+ "jam" +" "+ minute.ToString() +" "+ "menit";
                    if (hour == 24)
                    {
                        Item[i].status = "Sesuai";
                    }
                    else if (hour > 24)
                    {
                        Item[i].status = "Lebih Lambat";
                    }
                    else if (hour < 24)
                    {
                        Item[i].status = "Lebih Cepat";
                    }
                }
                else
                {
                    Item[i].selisih = Convert.ToInt32(Item2.tanggal_move.Value.Day - (Item[i].tanggal_mulai.Value.Day + (Item[i].target_hari)));
                    Item[i].selisih_string = Item[i].selisih.ToString() +" "+ "hari";
                    if (Item[i].selisih == 0)
                    {
                        Item[i].status = "Sesuai";
                    }
                    else if (Item[i].selisih > 0)
                    {
                        Item[i].status = "Lebih Lambat";
                    }
                    else if (Item[i].selisih < 0)
                    {
                        Item[i].status = "Lebih Cepat";
                    }
                }
               
                Item[i].total_hari_hour = (Convert.ToInt32(Item[i].total_hari * 24));
                Item[i].target_hari_hour = Convert.ToInt32(Item[i].target_hari * 24);


              
            }
            
            Item = Item.GroupBy(q=>q.id).Select(q=>q.First()).ToList();
            Item = Item.Where(s => s.tripmvinSum >= s.target_trip).ToList();
            var items = (from d in Item
                         orderby d.area
                         select new LaporanRekapData
                         {
                             nomor = x++,
                             area = d.area,
                             rig = d.rig,
                             biaya = d.biaya,
                             id = d.id,
                             jarak = d.jarak,
                             rute_dari = d.rute_dari,
                             rute_ke = d.rute_ke,
                             target_hari = d.target_hari,
                             target_trip = d.target_trip,
                             tanggal_mulai = d.tanggal_mulai,
                             transporter = d.transporter,
                             tindaklanjut = d.tindaklanjut,
                             kendala = d.kendala,
                             faktorketerlambatan = d.faktorketerlambatan,
                             trip_move_in = d.trip_move_in,
                             trip_move_out = d.trip_move_out,
                             tanggal_move = d.tanggal_move,
                             tripMoveIn = d.trip_move_in,
                             status = d.status,
                             tanggal_mulai_string = d.tanggal_mulai_string,
                             jamMulai = d.jamMulai,
                             tanggal_selesai = d.tanggal_selesai, 
                             jamSelesai = d.jamSelesai,
                             total_hari = d.total_hari,
                             selisih = d.selisih,
                             selisih_string = d.selisih_string,
                             total_hari_hour = d.total_hari_hour,
                             target_hari_hour =d.target_hari_hour
        }).ToList();
            Item = items.ToList();

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

            var area = (from Transporter in DDL
                             select new DDLTransporter
                             {
                                 name = Transporter.WBSArea
                             }).ToList();

            ViewBag.area = area;
            //var datavm666 = (from Transporter in Rig
            //                 select new DDLTransporter
            //                 {
            //                     id = Transporter.id,
            //                     name = Transporter.name
            //                 }).ToList();
            //ViewBag.rig = datavm666;
            var TP = DB.M_Transporter.ToList();
            var dataTrasnporter = (from v in TP
                                   select new DDLTransporter
                                   {
                                       id = v.id,
                                       name = v.name
                                   }).ToList();
            
            ViewBag.ddlTransporter = dataTrasnporter;
            ViewBag.data = Item;
            return View();
        }

        [HttpGet]
        public JsonResult GetData()
        {
            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();
            //string username = Session["Newusername"].ToString();
            //var UserRole = DB.M_RoleManagement.Where(n => n.username == username).FirstOrDefault();
            //var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            //DDLRole ddl = new DDLRole
            //{
            //    name = Role.name
            //};

            var MaterialMovementDB = DB.T_RigMaterialMovement.ToList();
            var TransporterDB = DB.M_Transporter.ToList();
            var DetailDB = DB.T_RigMaterialMovementDetail.ToList();
            var Data = (from MaterialMovement in MaterialMovementDB
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
            int x = 1;
            var DataTable = (from data in Data
                             join Detail in DetailDB
                             on data.id equals Detail.rig_material_movement_id
                             orderby data.id
                             select new LaporanRekapData
                             {

                                 area = data.area,
                                 rig = data.rig,
                                 biaya = data.biaya,
                                 id = data.id,
                                 jarak = data.jarak,
                                 rute_dari = data.rute_dari,
                                 rute_ke = data.rute_ke,
                                 target_hari = data.target_hari,
                                 target_trip = data.target_trip,
                                 tanggal_mulai = data.tanggal_mulai,
                                 transporter = data.transporter,
                                 tindaklanjut = Detail.tindak_lanjut,
                                 kendala = Detail.kendala,
                                 faktorketerlambatan = Detail.faktor_keterlambatan,
                                 trip_move_in = Detail.trip_move_in,
                                 trip_move_out = Detail.trip_move_out,
                                 tanggal_move = Detail.tanggal_move,
                                 tripMoveIn = Detail.trip_move_in
                             }).ToList();


            List<LaporanRekapData> Item = new List<LaporanRekapData>();
            Item = DataTable.ToList();


            for (int i = 0; i < Item.Count; i++)
            {
                LaporanRekapData Item2 = new LaporanRekapData();
                List<LaporanRekapData> Item3 = new List<LaporanRekapData>();
                var val = (from sum in Item
                           where sum.id == Item[i].id
                           select sum.trip_move_in).Sum();
                Item[i].tripmvinSum = Convert.ToInt32(val);
                Item2 = Item.Where(z => z.id == Item[i].id).OrderByDescending(y => y.tanggal_move).FirstOrDefault();
                Item[i].tanggal_mulai_string = Item[i].tanggal_mulai.Value.ToShortDateString();
                Item[i].jamMulai = Item[i].tanggal_mulai.Value.ToShortTimeString();
                Item[i].tanggal_selesai = Item2.tanggal_move.Value.ToShortDateString();
                Item[i].jamSelesai = Item2.tanggal_move.Value.ToShortTimeString();
                Item3 = Item.Where(a => a.id == Item[i].id).ToList();
                Item[i].total_hari = Item3.Count();
                if (Item[i].tanggal_mulai_string == Item[i].tanggal_selesai)
                {

                    TimeSpan span = Item2.tanggal_move.Value.Subtract(Item[i].tanggal_mulai.Value);
                    int hour = span.Hours;
                    int minute = span.Minutes;
                    int sec = span.Seconds;
                    Item[i].selisih_string = hour.ToString() + " " + "jam" + " " + minute.ToString() + " " + "menit";
                    if (hour == 24)
                    {
                        Item[i].status = "Sesuai";
                    }
                    else if (hour > 24)
                    {
                        Item[i].status = "Lebih Lambat";
                    }
                    else if (hour < 24)
                    {
                        Item[i].status = "Lebih Cepat";
                    }
                }
                else
                {
                    Item[i].selisih = Convert.ToInt32(Item2.tanggal_move.Value.Day - (Item[i].tanggal_mulai.Value.Day + (Item[i].target_hari)));
                    Item[i].selisih_string = Item[i].selisih.ToString() + " " + "hari";
                    if (Item[i].selisih == 0)
                    {
                        Item[i].status = "Sesuai";
                    }
                    else if (Item[i].selisih > 0)
                    {
                        Item[i].status = "Lebih Lambat";
                    }
                    else if (Item[i].selisih < 0)
                    {
                        Item[i].status = "Lebih Cepat";
                    }
                }

                Item[i].total_hari_hour = (Convert.ToInt32(Item[i].total_hari * 24));
                Item[i].target_hari_hour = Convert.ToInt32(Item[i].target_hari * 24);


             
            }

            Item = Item.GroupBy(q => q.id).Select(q => q.First()).ToList();
            Item = Item.Where(s => s.tripmvinSum >= s.target_trip).ToList();
            var items = (from d in Item
                         orderby d.area
                         select new LaporanRekapData
                         {
                             nomor = x++,

                             area = d.area,
                             rig = d.rig,
                             biaya = d.biaya,
                             id = d.id,
                             jarak = d.jarak,
                             rute_dari = d.rute_dari,
                             rute_ke = d.rute_ke,
                             target_hari = d.target_hari,
                             target_trip = d.target_trip,
                             tanggal_mulai = d.tanggal_mulai,
                             transporter = d.transporter,
                             tindaklanjut = d.tindaklanjut,
                             kendala = d.kendala,
                             faktorketerlambatan = d.faktorketerlambatan,
                             trip_move_in = d.trip_move_in,
                             trip_move_out = d.trip_move_out,
                             tanggal_move = d.tanggal_move,
                             tripMoveIn = d.trip_move_in,
                             status = d.status,
                             tanggal_mulai_string = d.tanggal_mulai_string,
                             jamMulai = d.jamMulai,
                             tanggal_selesai = d.tanggal_selesai,
                             jamSelesai = d.jamSelesai,
                             total_hari = d.total_hari,
                             selisih = d.selisih,
                             selisih_string = d.selisih_string,
                             total_hari_hour = d.total_hari_hour,
                             target_hari_hour = d.target_hari_hour
                         }).ToList();

            Item = items.ToList();
            ViewBag.data = Item;
            return Json(Item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataS(string area, string rig, string transporter, DateTime? tglmulaiStrt, DateTime? tglmulaiEnd, DateTime? tglselesaiStrt, DateTime? tglselesaiEnd, string status)
        {
            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();


            var MaterialMovementDB = DB.T_RigMaterialMovement.ToList();
            var TransporterDB = DB.M_Transporter.ToList();
            var DetailDB = DB.T_RigMaterialMovementDetail.ToList();
            var Data = (from MaterialMovement in MaterialMovementDB
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
            int x = 1;
            var DataTable = (from data in Data
                             join Detail in DetailDB
                             on data.id equals Detail.rig_material_movement_id
                             orderby data.id
                             select new LaporanRekapData
                             {

                                 area = data.area,
                                 rig = data.rig,
                                 biaya = data.biaya,
                                 id = data.id,
                                 jarak = data.jarak,
                                 rute_dari = data.rute_dari,
                                 rute_ke = data.rute_ke,
                                 target_hari = data.target_hari,
                                 target_trip = data.target_trip,
                                 tanggal_mulai = data.tanggal_mulai,
                                 transporter = data.transporter,
                                 tindaklanjut = Detail.tindak_lanjut,
                                 kendala = Detail.kendala,
                                 faktorketerlambatan = Detail.faktor_keterlambatan,
                                 trip_move_in = Detail.trip_move_in,
                                 trip_move_out = Detail.trip_move_out,
                                 tanggal_move = Detail.tanggal_move,
                                 tripMoveIn = Detail.trip_move_in
                             }).ToList();


            List<LaporanRekapData> Item = new List<LaporanRekapData>();
            Item = DataTable.ToList();


            for (int i = 0; i < Item.Count; i++)
            {
                LaporanRekapData Item2 = new LaporanRekapData();
                List<LaporanRekapData> Item3 = new List<LaporanRekapData>();
                var val = (from sum in Item
                           where sum.id == Item[i].id
                           select sum.trip_move_in).Sum();
                Item[i].tripmvinSum = Convert.ToInt32(val);
                Item2 = Item.Where(z => z.id == Item[i].id).OrderByDescending(y => y.tanggal_move).FirstOrDefault();
                Item[i].tanggal_mulai_string = Item[i].tanggal_mulai.Value.ToShortDateString();
                Item[i].jamMulai = Item[i].tanggal_mulai.Value.ToShortTimeString();
                Item[i].tanggal_selesai = Item2.tanggal_move.Value.ToShortDateString();
                Item[i].jamSelesai = Item2.tanggal_move.Value.ToShortTimeString();
                Item3 = Item.Where(a => a.id == Item[i].id).ToList();
                Item[i].total_hari = Item3.Count();
                if (Item[i].tanggal_mulai_string == Item[i].tanggal_selesai)
                {

                    TimeSpan span = Item2.tanggal_move.Value.Subtract(Item[i].tanggal_mulai.Value);
                    int hour = span.Hours;
                    int minute = span.Minutes;
                    int sec = span.Seconds;
                    Item[i].selisih_string = hour.ToString() + " " + "jam" + " " + minute.ToString() + " " + "menit";
                    if (hour == 24)
                    {
                        Item[i].status = "Sesuai";
                    }
                    else if (hour > 24)
                    {
                        Item[i].status = "Lebih Lambat";
                    }
                    else if (hour < 24)
                    {
                        Item[i].status = "Lebih Cepat";
                    }
                }
                else
                {
                    Item[i].selisih = Convert.ToInt32(Item2.tanggal_move.Value.Day - (Item[i].tanggal_mulai.Value.Day + (Item[i].target_hari)));
                    Item[i].selisih_string = Item[i].selisih.ToString() + " " + "hari";
                    if (Item[i].selisih == 0)
                    {
                        Item[i].status = "Sesuai";
                    }
                    else if (Item[i].selisih > 0)
                    {
                        Item[i].status = "Lebih Lambat";
                    }
                    else if (Item[i].selisih < 0)
                    {
                        Item[i].status = "Lebih Cepat";
                    }
                }

                Item[i].total_hari_hour = (Convert.ToInt32(Item[i].total_hari * 24));
                Item[i].target_hari_hour = Convert.ToInt32(Item[i].target_hari * 24);


              
            }

            Item = Item.GroupBy(q => q.id).Select(q => q.First()).ToList();
            Item = Item.Where(s => s.tripmvinSum >= s.target_trip).ToList();


            List<LaporanRekapData> strTglMulai = Item.Where(a => a.tanggal_mulai >= tglmulaiStrt && a.tanggal_mulai <= tglmulaiEnd).ToList();
            List<LaporanRekapData> strTglSelesai = Item.Where(a => a.tanggal_selesai_date >= tglselesaiStrt && a.tanggal_selesai_date <= tglselesaiEnd).ToList();

            List<LaporanRekapData> dataSearch = new List<LaporanRekapData>();

            var datas = (from d in Item
                         where (area == "" || d.area.Contains(area))
                         && (rig == "" || d.rig.Contains(rig))
                         && (transporter == "" || d.transporter.Contains(transporter))
                         && (status == "" || d.status.Contains(status))
                         && (tglmulaiEnd == null && tglmulaiStrt == null || d.tanggal_mulai >= tglmulaiStrt && d.tanggal_mulai <= tglmulaiEnd)
                         && (tglselesaiStrt == null && tglselesaiEnd == null || d.tanggal_selesai_date >= tglselesaiStrt && d.tanggal_selesai_date <= tglselesaiEnd)
                         orderby d.area
                         select new LaporanRekapData
                         {
                             nomor = x++,
                             area = d.area,
                             rig = d.rig,
                             biaya = d.biaya,
                             id = d.id,
                             jarak = d.jarak,
                             rute_dari = d.rute_dari,
                             rute_ke = d.rute_ke,
                             target_hari = d.target_hari,
                             target_trip = d.target_trip,
                             tanggal_mulai = d.tanggal_mulai,
                             transporter = d.transporter,
                             tindaklanjut = d.tindaklanjut,
                             kendala = d.kendala,
                             faktorketerlambatan = d.faktorketerlambatan,
                             trip_move_in = d.trip_move_in,
                             trip_move_out = d.trip_move_out,
                             tanggal_move = d.tanggal_move,
                             tripMoveIn = d.trip_move_in,
                             status = d.status,
                             tanggal_mulai_string = d.tanggal_mulai_string,
                             jamMulai = d.jamMulai,
                             tanggal_selesai = d.tanggal_selesai,
                             jamSelesai = d.jamSelesai,
                             total_hari = d.total_hari,
                             selisih = d.selisih,
                             selisih_string = d.selisih_string,
                             total_hari_hour = d.total_hari_hour,
                             target_hari_hour = d.target_hari_hour
                         }
                          ).ToList();


            dataSearch = datas.ToList();
           // dataSearch = dataSearch.GroupBy(p => p.id).Select(c => c.First()).ToList();
            ViewBag.data = dataSearch;
            return Json(dataSearch, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string area, string rig, string transporter, DateTime? tglmulaiStrt, DateTime? tglmulaiEnd, DateTime? tglselesaiStrt, DateTime? tglselesaiEnd, string status)
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
            var UserRole = DB.M_RoleManagement.Where(n => n.username == username).FirstOrDefault();
            var Role = DB.M_Role.Where(q => q.id == UserRole.role_id).FirstOrDefault();
            DDLRole ddl = new DDLRole
            {
                name = Role.name
            };

            var MaterialMovementDB = DB.T_RigMaterialMovement.ToList();
            var TransporterDB = DB.M_Transporter.ToList();
            var DetailDB = DB.T_RigMaterialMovementDetail.ToList();
            var Data = (from MaterialMovement in MaterialMovementDB
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
            int x = 1;
            var DataTable = (from data in Data
                             join Detail in DetailDB
                             on data.id equals Detail.rig_material_movement_id
                             orderby data.id
                             select new LaporanRekapData
                             {
                                
                                 area = data.area,
                                 rig = data.rig,
                                 biaya = data.biaya,
                                 id = data.id,
                                 jarak = data.jarak,
                                 rute_dari = data.rute_dari,
                                 rute_ke = data.rute_ke,
                                 target_hari = data.target_hari,
                                 target_trip = data.target_trip,
                                 tanggal_mulai = data.tanggal_mulai,
                                 transporter = data.transporter,
                                 tindaklanjut = Detail.tindak_lanjut,
                                 kendala = Detail.kendala,
                                 faktorketerlambatan = Detail.faktor_keterlambatan,
                                 trip_move_in = Detail.trip_move_in,
                                 trip_move_out = Detail.trip_move_out,
                                 tanggal_move = Detail.tanggal_move,
                                 tripMoveIn = Detail.trip_move_in
                             }).ToList();


            List<LaporanRekapData> Item = new List<LaporanRekapData>();
            Item = DataTable.ToList();


            for (int i = 0; i < Item.Count; i++)
            {
                LaporanRekapData Item2 = new LaporanRekapData();
                List<LaporanRekapData> Item3 = new List<LaporanRekapData>();
                var val = (from sum in Item
                           where sum.id == Item[i].id
                           select sum.trip_move_in).Sum();
                Item[i].tripmvinSum = Convert.ToInt32(val);
                Item2 = Item.Where(z => z.id == Item[i].id).OrderByDescending(y => y.tanggal_move).FirstOrDefault();
                Item[i].tanggal_mulai_string = Item[i].tanggal_mulai.Value.ToShortDateString();
                Item[i].jamMulai = Item[i].tanggal_mulai.Value.ToShortTimeString();
                Item[i].tanggal_selesai = Item2.tanggal_move.Value.ToShortDateString();
                Item[i].jamSelesai = Item2.tanggal_move.Value.ToShortTimeString();
                Item3 = Item.Where(a => a.id == Item[i].id).ToList();
                Item[i].total_hari = Item3.Count();
                if (Item[i].tanggal_mulai_string == Item[i].tanggal_selesai)
                {

                    TimeSpan span = Item2.tanggal_move.Value.Subtract(Item[i].tanggal_mulai.Value);
                    int hour = span.Hours;
                    int minute = span.Minutes;
                    int sec = span.Seconds;
                    Item[i].selisih_string = hour.ToString() + " " + "jam" + " " + minute.ToString() + " " + "menit";
                    if (hour == 24)
                    {
                        Item[i].status = "Sesuai";
                    }
                    else if (hour > 24)
                    {
                        Item[i].status = "Lebih Lambat";
                    }
                    else if (hour < 24)
                    {
                        Item[i].status = "Lebih Cepat";
                    }
                }
                else
                {
                    Item[i].selisih = Convert.ToInt32(Item2.tanggal_move.Value.Day - (Item[i].tanggal_mulai.Value.Day + (Item[i].target_hari)));
                    Item[i].selisih_string = Item[i].selisih.ToString() + " " + "hari";
                    if (Item[i].selisih == 0)
                    {
                        Item[i].status = "Sesuai";
                    }
                    else if (Item[i].selisih > 0)
                    {
                        Item[i].status = "Lebih Lambat";
                    }
                    else if (Item[i].selisih < 0)
                    {
                        Item[i].status = "Lebih Cepat";
                    }
                }

                Item[i].total_hari_hour = (Convert.ToInt32(Item[i].total_hari * 24));
                Item[i].target_hari_hour = Convert.ToInt32(Item[i].target_hari * 24);


                
            }

            Item = Item.GroupBy(q => q.id).Select(q => q.First()).ToList();
            Item = Item.Where(s => s.tripmvinSum >= s.target_trip).ToList();


            List<LaporanRekapData> strTglMulai = Item.Where(a => a.tanggal_mulai >= tglmulaiStrt && a.tanggal_mulai <= tglmulaiEnd).ToList();
            List<LaporanRekapData> strTglSelesai = Item.Where(a => a.tanggal_selesai_date >= tglselesaiStrt && a.tanggal_selesai_date <= tglselesaiEnd).ToList();

            List<LaporanRekapData> dataSearch = new List<LaporanRekapData>();

            var datas = (from d in Item
                         where (area == "" || d.area.Contains(area))
                         && (rig == "" || d.rig.Contains(rig))
                         && (transporter == "" || d.transporter.Contains(transporter))
                         && (status == "" || d.status.Contains(status))
                         && (tglmulaiEnd == null && tglmulaiStrt == null || d.tanggal_mulai >= tglmulaiStrt && d.tanggal_mulai <= tglmulaiEnd)
                         && (tglselesaiStrt == null && tglselesaiEnd == null || d.tanggal_selesai_date >= tglselesaiStrt && d.tanggal_selesai_date <= tglselesaiEnd)
                         orderby d.area
                         select new LaporanRekapData
                         {
                             nomor = x++,
                             area = d.area,
                             rig = d.rig,
                             biaya = d.biaya,
                             id = d.id,
                             jarak = d.jarak,
                             rute_dari = d.rute_dari,
                             rute_ke = d.rute_ke,
                             target_hari = d.target_hari,
                             target_trip = d.target_trip,
                             tanggal_mulai = d.tanggal_mulai,
                             transporter = d.transporter,
                             tindaklanjut = d.tindaklanjut,
                             kendala = d.kendala,
                             faktorketerlambatan = d.faktorketerlambatan,
                             trip_move_in = d.trip_move_in,
                             trip_move_out = d.trip_move_out,
                             tanggal_move = d.tanggal_move,
                             tripMoveIn = d.trip_move_in,
                             status = d.status,
                             tanggal_mulai_string = d.tanggal_mulai_string,
                             jamMulai = d.jamMulai,
                             tanggal_selesai = d.tanggal_selesai,
                             jamSelesai = d.jamSelesai,
                             total_hari = d.total_hari,
                             selisih = d.selisih,
                             selisih_string = d.selisih_string,
                             total_hari_hour = d.total_hari_hour,
                             target_hari_hour = d.target_hari_hour
                         }
                          ).ToList();


            dataSearch = datas.ToList();
            //dataSearch = dataSearch.GroupBy(p => p.id).Select(c => c.First()).ToList();
            ViewBag.data = dataSearch;
            return PartialView("PartialPageData");
        }
    
    
        public JsonResult GetDDLRig(string name)
        {
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

            HttpClient client = new HttpClient();
            string APIUrl = ConfigurationManager.AppSettings["GetRigUrl"].ToString();
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Token", token);
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

            List<DDL> dataRig = DDL.Where(x => x.WBSArea == name).ToList();
            var data = (from x in dataRig
                        select new DDLTransporter
                        {
                            name = x.WBSProfitCenterName
                        }).ToList();
            List<DDLTransporter> dataDDLRig = data.ToList();
            return Json(dataDDLRig, JsonRequestBehavior.AllowGet);

        }
    }
}