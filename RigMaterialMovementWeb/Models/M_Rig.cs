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
    
    public partial class M_Rig
    {
        public int id { get; set; }
        public Nullable<int> area_id { get; set; }
        public string name { get; set; }
    
        public virtual M_Area M_Area { get; set; }
    }
}