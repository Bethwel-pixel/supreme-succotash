﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JozyKQL.PG.Attributes;

namespace succotashBE.data.Model
{
    public partial class ClientPROGRAMS
    {
        [PrimaryKey, OrderBy(rank: 1, orderByDesc: true)]
        public int? Id { get; set; }
        public int? Programid { get; set; }
        public int? Clientid { get; set; }
        public int? Createdby { get; set; }
        public int? Updatedby { get; set; }
        public DateTime? Createdat { get; set; }
        public DateTime? Updatedat { get; set; }
    }
}
