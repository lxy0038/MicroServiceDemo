using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    /// <summary>
    /// 组件投料
    /// </summary>
    public class t_sn_work_com_feed : EntityBase
    {

        public string sn_id { get; set; }

        public string sn_work_id { get; set; }


        public string work_pro_dt_id { get; set; }

        public string product_id { get; set; }

        /// <summary>
        /// 投入备注
        /// </summary>
        public string feed_summary { get; set; }

        public string work_bom_id { get; set; }



        public decimal qty { get; set; }
         

        public decimal feed_qty { get; set; }

        /// <summary>
        /// 0-未开始  1-已开始  2-已完成
        /// </summary>
        public int order_status { get; set; }

        /// <summary>
        /// 分组编号
        /// </summary>
        public string replace_group_code { get; set; }
    }
}
