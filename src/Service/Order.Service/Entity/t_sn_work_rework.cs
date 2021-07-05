using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    public class t_sn_work_rework : EntityBase
    {
        public string sn_id { get; set; }
        public string sn_work_id { get; set; }

        public string parent_id { get; set; }

        public string top_parent_id { get; set; }


        /// <summary>
        /// 来料检验 = 0,
		///制程检验 = 1,
		///制程巡检 = 2,
		///成品检验 = 3,
		///出货检验 = 4,
		///QA检验 = 5,
		///首检 = 6,
		///人工检验=7,
        ///OBA开箱检验 = 8,
        ///库存检验 = 9,
        ///生产=100
        /// </summary>
        public int send_repair_source_type { get; set; }

        public int top_send_repair_source_type { get; set; }

         
        public int order_status { get; set; }
          
        public string repair_detail_id { get; set; }

    }
}
