//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RigMaterialMovementWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_RigMaterialMovementDetail
    {
        public int id { get; set; }
        public Nullable<int> rig_material_movement_id { get; set; }
        public Nullable<int> trip_move_out { get; set; }
        public Nullable<int> trip_move_in { get; set; }
        public string kendala { get; set; }
        public string tindak_lanjut { get; set; }
        public string test_tanggal_move { get; set; }
        public Nullable<System.DateTime> tanggal_move { get; set; }
        public string faktor_keterlambatan { get; set; }
        public string last_modified_by { get; set; }
        public Nullable<System.DateTime> last_modified_date { get; set; }
    
        public virtual T_RigMaterialMovement T_RigMaterialMovement { get; set; }
    }
}
