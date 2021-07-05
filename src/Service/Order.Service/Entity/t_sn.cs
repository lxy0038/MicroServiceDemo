using Lib.EF.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Entity
{
    public class t_sn : EntityBase
    {
        public string code { get; set; }

        public string parent_id { get; set; }

       /// <summary>
       /// 0-默认
       /// 1-外部SN，即由供应商提供
       /// 2-历史SN，即本厂在上线MES之前生产的SN
       /// 3-外厂SN, 即本集团其他厂生产的SN
       /// </summary>
        public int sn_type { get; set; }
    }
}
