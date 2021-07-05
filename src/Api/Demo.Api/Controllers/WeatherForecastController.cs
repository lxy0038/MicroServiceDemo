using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Mes.GrpcService;
using System.Threading.Tasks;
using Common.AspNetCore;
using Grpc.Net.Client;
using Demo.Models;
using Mes.SN.Service.Models;
using Mes.SN.Service.Entity;

namespace Demo.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : DemoControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IServiceProvider _provider;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IServiceProvider provider) :base()
        {
            _logger = logger;
            _provider = provider;
        }

        [HttpGet]
        public async Task<EFApiActionResult<OrderModel>> Get()
        {
            return await SafeExecuteAsync<OrderModel>(async (messages) =>
            {
                var client = new OrderDataService.OrderDataServiceClient(
                        LibServiceAddressUtils.GetServiceAddress(this._provider, "Order"));

                Random ran = new Random();
                int RandKey = ran.Next(2000, 10000);
                var orderp = new OrderParam() { Id = RandKey.ToString() };
                var s = await client.GetOrderAsync(new GrpcRequest() { Data = orderp.ToJson() });
                if (!string.IsNullOrEmpty(s.Data))
                    return s.Data.ToObjectFromJson<OrderModel>();
                if (s.Messages != null)
                    foreach (var m in s.Messages)
                        messages.Add(new EFApiActionMessage()
                        {
                            Code = LibSysUtils.GrpcDeSerial(m.Code),
                            Message = m.Message
                        });
                return null;
            });
        }



        //[HttpGet]
        //public EFApiActionResult<OrderModel> Get()
        //{
        //    return  SafeExecute<OrderModel>((messages) =>
        //    { 
        //        Random ran = new Random();
        //        int RandKey = ran.Next(2000, 10000);
        //        var s = new OrderParam() { Id = RandKey.ToString(), Remark = LibSysUtils.GetRandomStrByLength(1000, 4000) };
        //        var key = $"demo_{s?.Id}";

        //        var m = LibDistributedCacheUtils.Get<SNDBCacheInfoModel>(_provider, key);
        //        if (m == null)
        //        {
        //            m = GetSNDBCacheInfoModel();
        //            LibDistributedCacheUtils.Set(_provider, key, m, new TimeSpan(10, 0, 0));
        //        }
        //        var m1 = LibDistributedCacheUtils.Get<SNDBCacheInfoModel>(_provider, key);
        //        m1.SNState.status = 2;// DateTime.Now.ToString("yyyyMMddhhmmss");
        //        LibDistributedCacheUtils.Set(_provider, key, m1, new TimeSpan(10, 0, 0));
        //        var m2 = LibDistributedCacheUtils.Get<SNDBCacheInfoModel>(_provider, key);
        //        m2.SNState.status = 2;// DateTime.Now.ToString("yyyyMMddhhmmss");
        //        var om = new OrderModel() { Remark = m2.ToJson() };
        //        return om;
        //    });
        //}



        private string GetId()
        {
            return "0001001_" + Guid.NewGuid().ToString();
        }
        private SNDBCacheInfoModel GetSNDBCacheInfoModel()
        {
            SNDBCacheInfoModel info = new SNDBCacheInfoModel()
            {
                SN = new t_sn()
                {
                    code = "L10379B6R006954",
                    sn_type = 0,
                    create_time = DateTime.Now,
                    id = GetId(),
                    is_deleted = false,
                    modify_time = DateTime.Now,
                    org_entity_type = 0,
                    org_id = GetId(),
                    parent_id = GetId(),
                    row_version = 0
                },
                SNDync = new t_sn_d()
                {
                    create_time = DateTime.Now,
                    sn_work_id = GetId(),
                    id = GetId(),
                    modify_time = DateTime.Now,
                    org_id = GetId(),
                    product_id = GetId(),
                    work_id = GetId(),
                },
                SNId = GetId(),
                SNPdt = new t_sn_work()
                {
                    work_id = GetId(),
                    sn_id = GetId(),
                    create_time = DateTime.Now,
                    modify_time = DateTime.Now,
                    location_id = GetId(),
                    org_id = GetId(),
                    id = GetId(),
                    product_id = GetId(),
                    warehouse_id = GetId(),
                },
                SNPdtRework = new t_sn_work_rework()
                {
                    id = GetId(),
                    sn_id = GetId(),
                    sn_work_id = GetId(),
                    create_time = DateTime.Now,
                    modify_time = DateTime.Now,
                    org_id = GetId(),
                    parent_id = GetId(),
                    repair_detail_id = GetId(),
                    top_parent_id = GetId()
                },
                SNState = new t_sn_s() { id = GetId(), cur_wk_pro_dt_id = GetId(), next_wk_pro_dt_id = GetId(), org_id = GetId() },
                SNBandings = new List<t_sn_banding>(),
                SNCodes = new List<t_sn_code>(),
                SNPdtComFeeds = new List<t_sn_work_com_feed>(),
                SNPdtReworkDetails = new List<t_sn_work_rework_detail>(),
            };
            for (var i = 0; i < 6; i++)
                info.SNBandings.Add(new t_sn_banding() { id = GetId(), com_sn_id = GetId(), line_id = GetId(), org_id = GetId(), sn_id = GetId(), user_id = GetId(), workstage_id = GetId(), work_id = GetId() });
            for (var i = 0; i < 5; i++)
                info.SNCodes.Add(new t_sn_code() { code = "L10379B6R006954", id = GetId(), org_id = GetId(), sn_id = GetId() });
            for (var i = 0; i < 6; i++)
                info.SNPdtComFeeds.Add(new t_sn_work_com_feed() { id = GetId(), org_id = GetId(), product_id = GetId(), sn_id = GetId(), sn_work_id = GetId(), work_bom_id = GetId(), work_pro_dt_id = GetId() });
            for (var i = 0; i < 8; i++)
                info.SNPdtReworkDetails.Add(new t_sn_work_rework_detail() { id = GetId(), booking_work_id = GetId(), org_id = GetId(), procedure_id = GetId(), sn_id = GetId(), sn_work_rework_booking_log_id = GetId(), sn_work_rework_id = GetId(), work_pro_dt_id = GetId() });
            return info;
        }
    }
}
