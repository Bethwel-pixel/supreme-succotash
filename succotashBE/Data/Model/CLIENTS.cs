using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public partial class CLIENTS
    {
        
        public int? Id { get; set; }
        public string? Firstname { get; set; }
        public String? Surname { get; set; }
        public string? Middlename { get; set; }
        public int? Countryid { get; set; }
        public int? Countyid { get; set; }
        public int? Subcountyid { get; set; }
        public int? Genderid { get; set; }
        public int? Createdby { get; set; }
        public int? Updatedby { get; set; }
        public DateTime? Createdat { get; set; }
        public DateTime? Updatedat { get; set; }
    }
}
