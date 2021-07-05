using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    public class t_sn_s : EntityBase
    {
        /// <summary>
        /// 0-未开始 1-生产中  2-完成 3-异常  4-返工中  5-闲置  6-报废 7-冻结
        /// </summary>
        public int status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int pre_status { get; set; }

         
        public int box_status { get; set; }

         /// <summary>
         /// 当前制程检验类型
         /// 0-默认
         /// 1-制程检验
         /// 2-OBA
         /// </summary>
        public int pqc_type { get; set; }

        /// <summary>
        /// 0-未检验  1-成功  2-失败
        /// </summary>

        public int pqc_status { get; set; }



        /// <summary>
        /// 0-未开始  1-待入库  2-已入库  3-已出库
        /// </summary>
        public int storage_status { get; set; }

        /// <summary>
        /// 0-未开始   1-成品待送检  2-成品已送检  3-成品已检验
        /// </summary>
        public int fqc_status { get; set; }

        /// <summary>
        /// 当前已报工 工单工艺路线明细ID   当前站
        /// </summary>
        public string cur_wk_pro_dt_id { get; set; }


        /// <summary>
        /// 下一报工 工单工艺路线明细ID   下一站
        /// </summary>
        public string next_wk_pro_dt_id { get; set; }
    }
}
