using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RigMaterialMovementWeb.Models
{
    public class Master
    {
        public List<TransporterList> DataTable { get; set; }
    }

    public class TransporterList
    {
        public int id { get; set; }
        public string name { get; set; }
        public string last_modified_by { get; set; }
        public DateTime? last_modified_date { get; set; }
    }
}