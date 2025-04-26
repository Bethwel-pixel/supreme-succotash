using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JozyKQL.PG.Attributes;

namespace succotashBE.data.Model
{
    public partial class Users
    {
        [PrimaryKey, OrderBy ( rank:1, orderByDesc:true)]
        public int? Id { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public int? Genderid { get; set; }
        public int? Countyid { get; set; }
        public int? Countryid { get; set; }
        public int? Roleid { get; set; }
        public int? Subcountyid { get; set; }
        [IgnoreOnUpdate]
        public String? Password { get; set; }
        [IgnoreOnUpdate]
        public int? Createdby { get; set; }
        [IgnoreOnInsert]
        public int? Updatedby { get; set; }
        [DateNow, IgnoreOnUpdate]
        public DateTime? Createdat { get; set; }
        [DateNow, IgnoreOnInsert]
        public DateTime? Updatedat { get; set; }
        public string? Username { get; set; }
    }
}
