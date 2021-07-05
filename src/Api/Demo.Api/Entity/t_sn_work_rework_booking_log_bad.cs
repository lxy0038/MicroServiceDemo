using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    public class t_sn_work_rework_booking_log_bad : EntityBase
    {
        public string sn_id { get; set; }

        public string sn_work_id { get; set; }

        public string sn_work_rework_booking_log_id { get; set; }

        public string bad_code_id { get; set; }


        public string remark { get; set; }
    }
}
