using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RigMaterialMovementWeb.Models
{
    public class Role
    {
        public List<RoleList> DataTable { get; set; }
    }

    public class RoleList
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public Nullable<int> role_id { get; set; }
        public string last_modified_by { get; set; }
        public DateTime? last_modified_date { get; set; }
        public string role_name { get; set; }
    }

    public class DDLRole
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}