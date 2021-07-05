using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    public class t_sn_code : EntityBase
    {
        public string sn_id { get; set; }


        public string code { get; set; }

        /// <summary>
        /// 0-默认
        /// 1-DSN
        /// </summary>
        public string code_type { get; set; }
    }
}
