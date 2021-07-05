using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    public class t_sn_work : EntityBase
    {

        public string sn_id { get; set; }

        public string work_id { get; set; }

        /// <summary>
        ///类型  0-默认 1-返工单
        /// </summary>
        public int sn_work_type { get; set; }

        /// <summary>
        /// 0-未开始 1-生产中  2-完成 3-异常  4-返工中  5-闲置  6-报废 7-冻结
        /// </summary>
        public int status { get; set; }

        public string product_id { get; set; }


        public string warehouse_id { get; set; }

        public string location_id { get; set; }

    }
}
