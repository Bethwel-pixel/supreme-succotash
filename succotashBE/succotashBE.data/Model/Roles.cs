using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JozyKQL.PG.Attributes;

namespace succotashBE.data.Model
{
    internal class Roles
    {
        [PrimaryKey, OrderBy(rank: 1, orderByDesc: true)]
        public int? Id { get; set; }
        public string? Rolename { get; set; }
        public int? Createdby { get; set; }
        public int? Updatedby { get; set; }
        public DateTime? Createdat { get; set; }
        public DateTime? Updatedat { get; set; }
    }
}
