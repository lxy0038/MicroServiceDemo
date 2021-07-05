using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    /// <summary>
    /// sn动态数据
    /// </summary>
    public class t_sn_d : EntityBase
    {

        public string work_id { get; set; }


        public string product_id { get; set; }


        public string sn_work_id { get; set; }
    }
}
