using Mes.SN.Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mes.SN.Service.Models
{
    public class SNDBCacheInfoModel
    { 

        public string SNId { get; set; }

        public t_sn SN { get; set; }

        public t_sn_d SNDync { get; set; }

        public t_sn_work SNPdt { get; set; }

        public t_sn_s SNState { get; set; }

        public List<t_sn_banding> SNBandings { get; set; }

        public List<t_sn_code> SNCodes { get; set; }


        public List<t_sn_work_com_feed> SNPdtComFeeds { get; set; }
         


        public t_sn_work_rework SNPdtRework { get; set; }

        public List<t_sn_work_rework_detail> SNPdtReworkDetails { get; set; }

    }


    public class SNDBCachePdtInfoModel
    {
        public t_sn_work SNPdt { get; set; } 
    }

    public class SNDBCachePdtReworkInfoModel
    {
        public t_sn_work_rework SNPdtRework { get; set; }
        public List<t_sn_work_rework_detail> SNPdtReworkDetails { get; set; }
    }


    public class SNDBCachePdtModel
    {

        public SNDBCachePdtModel()
        {
            this.Infos = new List<SNDBCachePdtInfoModel>();
        }

        public string SNId { get; set; }


        public List<SNDBCachePdtInfoModel> Infos { get; set; }
    }


    public class SNDBCachePdtReworkModel
    {
        public SNDBCachePdtReworkModel()
        {
            this.Infos = new List<SNDBCachePdtReworkInfoModel>();
        }
        public string SNId { get; set; }


        public List<SNDBCachePdtReworkInfoModel> Infos { get; set; }
    }

    public class SNDBCacheLogModel
    {
        public string SNId { get; set; }

        public List<t_sn_log> SNLogs { get; set; } 
    }
}
