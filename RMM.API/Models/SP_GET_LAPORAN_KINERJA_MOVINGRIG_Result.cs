//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RMM.API.Models
{
    using System;
    
    public partial class SP_GET_LAPORAN_KINERJA_MOVINGRIG_Result
    {
        public int id { get; set; }
        public string area { get; set; }
        public string rig { get; set; }
        public string rute_dari { get; set; }
        public string rute_ke { get; set; }
        public Nullable<double> jarak { get; set; }
        public Nullable<int> transporter_id { get; set; }
        public Nullable<System.DateTime> tanggal_mulai { get; set; }
        public Nullable<int> target_hari { get; set; }
        public Nullable<int> target_trip { get; set; }
        public Nullable<long> biaya { get; set; }
        public string last_modified_by { get; set; }
        public Nullable<System.DateTime> last_modified_date { get; set; }
    }
}