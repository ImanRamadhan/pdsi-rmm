using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RigMaterialMovementWeb.Models
{
    public class LaporanRekapData
    {
        public int nomor { get; set; }
        public int? tripMoveIn { get; set; }
        public int tripmvinSum { get; set; }
        public decimal? persentaseMoveIn2 { get; set; }
        public decimal persentaseMoveIn1 { get; set; }
        public decimal? persentase2 { get; set; }
        public decimal persentase { get; set; }
        public int trip { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? tanggal_move { get; set; }
        public string commandline { get; set; }
        public int id { get; set; }
        public string area { get; set; }
        public string rig { get; set; }
        public string rute_dari { get; set; }
        public string transporter { get; set; }
        public string rute_ke { get; set; }
        public double? jarak { get; set; }
        public int? transporter_id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? tanggal_sekarang { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? tanggal_mulai { get; set; }
        public string tanggal_mulai_string { get; set; }
        public int? target_hari { get; set; }
        public int target_hari_hour { get; set; }
        public int? target_trip { get; set; }
        public long? biaya { get; set; }
        public string tanggal_selesai { get; set; }
        public DateTime? tanggal_selesai_date { get; set; }
        public string last_modified_by { get; set; }
        public string last_modified_date { get; set; }
        public string jamMulai { get; set; }
        public string jamSelesai { get; set; }
        public int total_hari { get; set; }
        public int total_hari_hour { get; set; }
        public int selisih { get; set; }
        public string selisih_string { get; set; }
        public string status { get; set; }
        public string faktorketerlambatan { get; set; }
        public int harike { get; set; }
        public int rig_material_movement_id { get; set; }
        public int? trip_move_out { get; set; }
        public int? trip_move_in { get; set; }
        public string kendala { get; set; }
        public string tindaklanjut { get; set; }
        public string faktor_keterlambatan { get; set; }
      
        public int lambat { get; set; }
        public int cepat { get; set; }
        public int tepat { get; set; }
        public int jumlah { get; set; }

      
    }

    public class KinerjaRigChart
    {
        public int sumCepat { get; set; }
        public int sumLambat { get; set; }
        public int sumTepat { get; set; }
    }
}