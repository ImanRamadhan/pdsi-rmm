using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RMM.API.Models
{
    public class PermintaanPekerjaan
    {
        public int permintaan_pekerjaan_id { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? tanggalstat { get; set; }
        public DateTime? tanggal { get; set; }
        public string notes { get; set; }
        public string status { get; set; }
        public string commandline { get; set; }
        public int id { get; set; }
        public string no_wo { get; set; }
        public string no_activity { get; set; }
        public string judul_pekerjaan { get; set; }
        public DateTime? tanggal_pekerjaan { get; set; }
        public DateTime? tanggal_pekerjaandari { get; set; }
        public DateTime? tanggal_pekerjaansampai { get; set; }
        public string lokasi_asal { get; set; }
        public string cp_lokasi_asal { get; set; }
        public string lokasi_tujuan { get; set; }
        public string cp_lokasi_tujuan { get; set; }
        public string detail_barang { get; set; }
        public string keterangan { get; set; }
        public string last_modified_by { get; set; }
        public DateTime? last_modified_date { get; set; }
        public int? status_id { get; set; }
        public string approved_by { get; set; }
        public DateTime? approved_date { get; set; }
    }
    public class DDLStatusPermintaanPekerjaan
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class Approval
    {
        public int? role_id { get; set; }
        public string username { get; set; }
    }
}
