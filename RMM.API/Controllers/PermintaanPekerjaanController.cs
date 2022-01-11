using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RMM.API.Models;

namespace RMM.API.Controllers
{
    public class PermintaanPekerjaanController : ApiController
    {
        [Route("Api/PermintaanPekerjaan/PostPermintaanPekerjaan")]
        [HttpGet]
        public List<PermintaanPekerjaan> PostPermintaanPekerjaan(string no_wo,string no_activity,string judul_pekerjaan,DateTime? tanggal_pekerjaan,string lokasi_asal, string cp_lokasi_asal,string lokasi_tujuan,string cp_lokasi_tujuan,string detail_barang,string keterangan)
        {
            try
            {
                var Result = new List<PermintaanPekerjaan>();
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                T_PermintaanPekerjaan NewMC = new T_PermintaanPekerjaan
                {

                    no_wo = no_wo,
                    no_activity = no_activity,
                    judul_pekerjaan = judul_pekerjaan,
                    tanggal_pekerjaan = DateTime.Now,
                    lokasi_asal = lokasi_asal,
                    cp_lokasi_asal = cp_lokasi_asal,
                    lokasi_tujuan = lokasi_tujuan,
                    cp_lokasi_tujuan = cp_lokasi_tujuan,
                    detail_barang = detail_barang,
                    keterangan = keterangan,
                    status_id = 1

                };

                Result.Add(new PermintaanPekerjaan
                {
                    no_wo = no_wo,
                    no_activity = no_activity,
                    judul_pekerjaan = judul_pekerjaan,
                    tanggal_pekerjaan = DateTime.Now,
                    lokasi_asal = lokasi_asal,
                    cp_lokasi_asal = cp_lokasi_asal,
                    lokasi_tujuan = lokasi_tujuan,
                    cp_lokasi_tujuan = cp_lokasi_tujuan,
                    detail_barang = detail_barang,
                    keterangan = keterangan,
                    status_id = 1
                });
                //Insert to Database
               
                DB.T_PermintaanPekerjaan.Add(NewMC);
                DB.SaveChanges();
                //Define error -> true and return Data
                Response.HasAnError = false;
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [Route("Api/PermintaanPekerjaan/PostPermintaanPekerjaanDetail")]
        [HttpGet]
        public List<PermintaanPekerjaan> PostPermintaanPekerjaanDetail(int permintaan_pekerjaan_id,string notes,DateTime? tanggal, int? status_id)
        {
            try
            {
                var Result = new List<PermintaanPekerjaan>();
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
                T_PermintaanPekerjaanDetail NewMC = new T_PermintaanPekerjaanDetail
                {

                    permintaan_pekerjaan_id = permintaan_pekerjaan_id,
                    notes = notes,
                    tanggal = DateTime.Now
                   // status_id = status_id


                };

                Result.Add(new PermintaanPekerjaan
                {

                    permintaan_pekerjaan_id = permintaan_pekerjaan_id,
                    notes = notes,
                    tanggal = DateTime.Now
                    //status_id = status_id

                });

                //Insert to Database

                DB.T_PermintaanPekerjaanDetail.Add(NewMC);
                DB.SaveChanges();
                //Define error -> true and return Data
                Response.HasAnError = false;
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [Route("Api/PermintaanPekerjaan/Approval")]
        [HttpGet]
        public List<PermintaanPekerjaan> Approval(int id)
        {
            try
            {
                var Result = new List<PermintaanPekerjaan>();
                DB_RMMEntities DB = new DB_RMMEntities();
                ResponseMessage Response = new ResponseMessage();
               
                //Insert to Database

                T_PermintaanPekerjaan update = DB.T_PermintaanPekerjaan.Where(x => x.id == id).FirstOrDefault();
                {
                    update.status_id = 2;
                }
                DB.SaveChanges();
                var PermintaanDB = DB.T_PermintaanPekerjaan.ToList();
                var StatusDB = DB.M_Status.ToList();
                var DataTable = (from Kerja in PermintaanDB
                                 join Status in StatusDB
                                 on Kerja.status_id equals Status.id
                                 where Kerja.id == id
                                 select new PermintaanPekerjaan
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
                                     approved_date = Kerja.approved_date,
                                     status_id = Kerja.status_id

                                 }).ToList();



                Result.Add(new PermintaanPekerjaan
                {
                    no_wo = DataTable[0].no_wo,
                    no_activity = DataTable[0].no_activity,
                    judul_pekerjaan = DataTable[0].judul_pekerjaan,
                    tanggal_pekerjaan = DataTable[0].tanggal_pekerjaan,
                    lokasi_asal = DataTable[0].lokasi_asal,
                    cp_lokasi_asal = DataTable[0].cp_lokasi_asal,
                    lokasi_tujuan = DataTable[0].lokasi_tujuan,
                    cp_lokasi_tujuan = DataTable[0].cp_lokasi_tujuan,
                    detail_barang = DataTable[0].detail_barang,
                    keterangan = DataTable[0].keterangan,
                    status = DataTable[0].status
                });
                //Define error -> true and return Data
                Response.HasAnError = false;
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
