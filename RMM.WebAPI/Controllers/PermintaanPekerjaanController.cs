using RigMaterialMovementWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace RMM.WebAPI.Controllers
{
    public class PermintaanPekerjaanController : ApiController
    {
        
        public List<PermintaanPekerjaanList> PostPermintaanPekerjaan()
        {
            try
            {
                var Result = "";
                DB_RMMEntities DB = new DB_RMMEntities();
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
                    keterangan = Model.keterangan
                   

                };
                //Insert to Database
                Result = NewMC;
                DB.T_PermintaanPekerjaan.Add(NewMC);
                DB.SaveChanges();
                //Define error -> true and return Data
                Response.HasAnError = false;
                return Result();
            }
            catch (Exception ex)
            {
                throw ex;
                //return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
