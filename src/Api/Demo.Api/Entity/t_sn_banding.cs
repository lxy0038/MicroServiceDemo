using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    public class t_sn_banding : EntityBase
    {

        public string sn_id { get; set; }


        public string com_sn_id { get; set; }

        public string feed_summary { get; set; }

        public string workstage_id { get; set; }

        public string line_id { get; set; }

        public string work_id { get; set; }


        public string user_id { get; set; }

    }
}
