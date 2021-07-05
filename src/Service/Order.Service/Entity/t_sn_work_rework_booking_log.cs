using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    public class t_sn_work_rework_booking_log : EntityBase
    {
        public string sn_id { get; set; }

        public string sn_work_id { get; set; }


        public string sn_work_rework_id { get; set; }

        public string sn_work_rework_detail_id { get; set; }

        public string procedure_id { get; set; }

        public string workstage_id { get; set; }

        public string line_id { get; set; }


        /// <summary>
        /// 制造资源ID 设备
        /// </summary>
        public string equipment_id { get; set; }

        /// <summary>
        /// 制造资源ID 产品工装
        /// </summary>
        public string fixture_id { get; set; }



        public string user_id { get; set; }


        public bool is_bad { get; set; }
        public string remark { get; set; }


        /// <summary>
        /// 测试次数记录 使用btye进行记录
        /// 0-默认
        /// 001  测试三次  1次OK
        /// 100  测试三次  2次NG  1次OK
        /// </summary>
        public int test_count_r { get; set; } 

        /// <summary>
        /// 测试类型记录    进行额外的标识
        /// </summary>
        public int test_type_r { get; set; }
    }
}
