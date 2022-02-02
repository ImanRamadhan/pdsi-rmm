using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RigMaterialMovementWeb.Models
{
    public class RigMaterial
    {
        public List<RigMaterialMovementList> DataTable { get; set; }
    }
    public class DDLTransporter
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class DDLArea
    {
        public string
            id
        { get; set; }
        public string name { get; set; }
    }

    public class DDLFaktorKeterlambatan
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class RigMaterialMovementList
    {
        public List<DDLTransporter> DDLTransporter { get; set; }
        public string penilaian2 { get; set; }
        public string penilaian1 { get; set; }
        public int tripMoveIn { get; set; }
        public decimal? DailyTargetMoveOut { get; set; }
        public decimal? persentaseMoveIn2 { get; set; }
        public decimal persentaseMoveIn1 { get; set; }
        public decimal? persentase2 { get; set; }
        public decimal persentase { get; set; }
        public int? trip { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public string test_tanggal_move { get; set; }
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
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? tanggal_mulai_dari { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? tanggal_mulai_sampai { get; set; }
        public string tanggal_mulai_string { get; set; }
        public int? target_hari { get; set; }
        public int? target_trip { get; set; }
        public long? biaya { get; set; }
        public string last_modified_by { get; set; }
        public DateTime? last_modified_date { get; set; }
        public DateTime? jam { get; set; }
        public string faktorketerlambatan { get; set; }

        public int harike { get; set; }



        public int rig_material_movement_id { get; set; }
        public int? trip_move_out { get; set; }
        public int? trip_move_in { get; set; }
        public string kendala { get; set; }
        public string tindaklanjut { get; set; }
    }

    public class RigMaterialMovementDetail
    {
        public int id { get; set; }
        public string area { get; set; }
        public string rig { get; set; }
        public string rute_dari { get; set; }
        public string rute_ke { get; set; }
        public double? jarak { get; set; }
        public int? transporter_id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? tanggal_mulai { get; set; }
        public int? target_hari { get; set; }
        public int? target_trip { get; set; }
        public long? biaya { get; set; }
        public string last_modified_by { get; set; }
        public DateTime? last_modified_date { get; set; }
        public DateTime? jam { get; set; }
        public string faktorketerlambatan { get; set; }

        public int rig_material_movement_id { get; set; }
        public int? trip_move_out { get; set; }
        public int? trip_move_in { get; set; }
        public string kendala { get; set; }
        public string tindaklanjut { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? tanggal_move { get; set; }

    }

    public class GetEditListRigMaterial
    {

        public int id { get; set; }
        public string area { get; set; }
        public string rig { get; set; }
        public string rute_dari { get; set; }
        public string rute_ke { get; set; }
        public double? jarak { get; set; }
        public int? transporter_id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? tanggal_mulai { get; set; }
        public string datestring { get; set; }
        public int? target_hari { get; set; }
        public int? target_trip { get; set; }
        public long? biaya { get; set; }
        public string last_modified_by { get; set; }
        public DateTime? last_modified_date { get; set; }
        public string transporter { get; set; }
    }
    public class GetEditListRigMaterialDetail
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? tanggalmove { get; set; }
        public int id { get; set; }
        public int rig_material_movement_id { get; set; }
        public int? trip_move_out { get; set; }
        public int? trip_move_in { get; set; }
        public string kendala { get; set; }
        public string tindaklanjut { get; set; }
        public string last_modified_by { get; set; }
        public DateTime? last_modified_date { get; set; }
        public string faktorketerlambatan { get; set; }
    }
}