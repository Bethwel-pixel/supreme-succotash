using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JozyKQL.PG.Attributes;

namespace succotashBE.data.Model
{
    public partial class COUNTY
    {
        [PrimaryKey, OrderBy(rank:1, orderByDesc:true)]
        public int? Id { get; set; }
        public string? Countyname { get; set; }
        public int? Countryid { get; set; }
        [IgnoreOnUpdate]
        public int? Createdby { get; set; }
        [IgnoreOnInsert]
        public int? Updatedby { get; set; }
        [DateNow, IgnoreOnUpdate]
        public DateTime? Createdat { get; set; }
        [DateNow, IgnoreOnInsert]
        public DateTime? Updatedat { get; set; }
    }
}
