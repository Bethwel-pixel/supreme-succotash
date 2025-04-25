using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JozyKQL.PG.Attributes;

namespace succotashBE.data.Views
{
    public partial class Clientsview
    {
        [PrimaryKey, OrderBy(rank:1,orderByDesc:true)]
        public int? Id { get; set; }
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public string? Middlename { get; set; }
        public int? Countryid { get; set; }
        public string? Countryname { get; set; }
        public int? Countyid { get; set; }
        public string? Countyname { get; set; }
        public int? Subcountyid { get; set; }
        public string? Subcountyname { get; set; }
        public int? Genderid { get; set; }
        public string? Gender { get; set; }
        public int? Createdby { get; set; }
        public int? Updatedby { get; set; }
        public DateTime? Createdat { get; set; }
        public DateTime? Updatedat { get; set; }
    }
}
