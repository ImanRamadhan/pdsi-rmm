using RigMaterialMovementWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace RigMaterialMovementWeb.Controllers
{
    public class HomeController : Controller
    {
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
            var UserRole = DB.M_RoleManagement.Where(c => c.username == username).FirstOrDefault();
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
                        Item[i].status = "Tepat Waktu";
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
                    Item[i].selisih = Convert.ToInt32(Item2.tanggal_move.Value.Day - (Item[i].tanggal_mulai.Value.Day + (Item[i].target_hari - 1)));
                    Item[i].selisih_string = Item[i].selisih.ToString() + " " + "hari";
                    if (Item[i].selisih == 0)
                    {
                        Item[i].status = "Tepat Waktu";
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



            List<LaporanRekapData> dataSearch = new List<LaporanRekapData>();

            var datas = (from d in Item
                        
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
            for (int p = 0; p < dataSearch.Count(); p++)
            {

                dataSearch[p].cepat = dataSearch.Where(o => o.status == "Lebih Cepat" && o.rig == dataSearch[p].rig).Count();
                dataSearch[p].lambat = dataSearch.Where(o => o.status == "Lebih Lambat" && o.rig == dataSearch[p].rig).Count();
                dataSearch[p].tepat = dataSearch.Where(o => o.status == "Tepat Waktu" && o.rig == dataSearch[p].rig).Count();
                dataSearch[p].jumlah = dataSearch[p].cepat + dataSearch[p].lambat + dataSearch[p].tepat;
            }



            ViewBag.data = dataSearch;
            ViewBag.Role = ddl;
            return View();
        }


        public JsonResult GetData()
        {
            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();
            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(b => b.username == username).FirstOrDefault();
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
                        Item[i].status = "Tepat Waktu";
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
                    Item[i].selisih = Convert.ToInt32(Item2.tanggal_move.Value.Day - (Item[i].tanggal_mulai.Value.Day + (Item[i].target_hari - 1)));
                    Item[i].selisih_string = Item[i].selisih.ToString() + " " + "hari";
                    if (Item[i].selisih == 0)
                    {
                        Item[i].status = "Tepat Waktu";
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



            List<LaporanRekapData> dataSearch = new List<LaporanRekapData>();

            var datas = (from d in Item
                       
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
                             selisih_string =d.selisih_string,
                             total_hari_hour = d.total_hari_hour,
                             target_hari_hour = d.target_hari_hour
                         }
                          ).ToList();


            dataSearch = datas.ToList();
            for (int p = 0; p < dataSearch.Count(); p++)
            {

                dataSearch[p].cepat = dataSearch.Where(o => o.status == "Lebih Cepat" && o.rig == dataSearch[p].rig).Count();
                dataSearch[p].lambat = dataSearch.Where(o => o.status == "Lebih Lambat" && o.rig == dataSearch[p].rig).Count();
                dataSearch[p].tepat = dataSearch.Where(o => o.status == "Tepat Waktu" && o.rig == dataSearch[p].rig).Count();
                dataSearch[p].jumlah = dataSearch[p].cepat + dataSearch[p].lambat + dataSearch[p].tepat;
            }



            dataSearch = dataSearch.GroupBy(v => v.rig).Select(b => b.First()).ToList();
            var sumCepat = dataSearch.Select(q => q.cepat).Sum();
            var sumLambat = dataSearch.Select(q => q.lambat).Sum();
            var sumTepat = dataSearch.Select(q => q.tepat).Sum();

            KinerjaRigChart kinerja = new KinerjaRigChart();
            kinerja.sumCepat = sumCepat;
            kinerja.sumLambat = sumLambat;
            kinerja.sumTepat = sumTepat;

            return Json(kinerja, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataBar()
        {
            DB_RMMEntities DB = new DB_RMMEntities();
            ResponseMessage Response = new ResponseMessage();
            string username = Session["Newusername"].ToString();
            var UserRole = DB.M_RoleManagement.Where(b => b.username == username).FirstOrDefault();
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
                        Item[i].status = "Tepat Waktu";
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
                    Item[i].selisih = Convert.ToInt32(Item2.tanggal_move.Value.Day - (Item[i].tanggal_mulai.Value.Day + (Item[i].target_hari - 1)));
                    Item[i].selisih_string = Item[i].selisih.ToString() + " " + "hari";
                    if (Item[i].selisih == 0)
                    {
                        Item[i].status = "Tepat Waktu";
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



            List<LaporanRekapData> dataSearch = new List<LaporanRekapData>();

            var datas = (from d in Item
                       
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
            for (int p = 0; p < dataSearch.Count(); p++)
            {

                dataSearch[p].cepat = dataSearch.Where(o => o.status == "Lebih Cepat" && o.rig == dataSearch[p].rig).Count();
                dataSearch[p].lambat = dataSearch.Where(o => o.status == "Lebih Lambat" && o.rig == dataSearch[p].rig).Count();
                dataSearch[p].tepat = dataSearch.Where(o => o.status == "Tepat Waktu" && o.rig == dataSearch[p].rig).Count();
                dataSearch[p].jumlah = dataSearch[p].cepat + dataSearch[p].lambat + dataSearch[p].tepat;
            }



            dataSearch = dataSearch.GroupBy(v => v.rig).Select(b => b.First()).ToList();
            for (int y = 0; y < dataSearch.Count; y++)
            {
                dataSearch[y].cepat = dataSearch.Where(m => m.area == dataSearch[y].area).Select(n => n.cepat).Sum();
                dataSearch[y].lambat = dataSearch.Where(m => m.area == dataSearch[y].area).Select(n => n.lambat).Sum();
                dataSearch[y].tepat = dataSearch.Where(m => m.area == dataSearch[y].area).Select(n => n.tepat).Sum();
            }
            dataSearch = dataSearch.GroupBy(n => n.area).Select(n => n.First()).ToList();
            //var sumCepat = dataSearch.Select(q => q.cepat).Sum();
            //var sumLambat = dataSearch.Select(q => q.lambat).Sum();
            //var sumTepat = dataSearch.Select(q => q.tepat).Sum();

            //KinerjaRigChart kinerja = new KinerjaRigChart();
            //kinerja.sumCepat = sumCepat;
            //kinerja.sumLambat = sumLambat;
            //kinerja.sumTepat = sumTepat;

            return Json(dataSearch, JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}