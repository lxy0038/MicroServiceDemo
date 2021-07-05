using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    public class t_sn_work_rework_detail : EntityBase
    {
        public string sn_work_rework_id { get; set; }
        public string sn_id { get; set; }

        public string work_pro_dt_id { get; set; }

        public string procedure_id { get; set; }

        public int num { get; set; }


        public string booking_work_id { get; set; }
         

        /// <summary>
        /// 0-未开始  1-异常 2-完成
        /// </summary>
        public int status { get; set; } 

        public string sn_work_rework_booking_log_id { get; set; }

    }
}
