using System;
using Common.AspNetCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Mes.GrpcService;
using Demo.Models;
using Mes.SN.Service.Models;
using Mes.SN.Service.Entity;
using Grpc.Net.Client;
using DotNetCore.CAP;
using Newtonsoft.Json;
using IVAN.MES.Rabbitmq.Publish;

namespace Order.Service
{
    public class OrderRPCService : OrderDataService.OrderDataServiceBase, ICapSubscribe
    {
        private readonly IServiceProvider _provider;
        private readonly ICapPublisher _capBus;
        //private readonly Pubmq pubmq;

        public OrderRPCService(IServiceProvider provider, ICapPublisher producer)
        {
            _provider = provider;
            _capBus = producer;
            //this.pubmq = pubmq;
        }
        public override  Task<GrpcReply> GetOrder(GrpcRequest request, ServerCallContext context)
        {
            GrpcReply reply = new GrpcReply();
            try
            {
                var s = request.Data?.ToObjectFromJson<OrderParam>();
                var snMqMessageString = JsonConvert.SerializeObject(s);
                //var mq = new Pubmq();
                if (s != null)
                    LibMQUtils.Publish(this._provider, "order.get", s);
                    //await pubmq.BasicPublish("order.get", snMqMessageString);
                if (s != null)
                  LibMQUtils.Publish(this._provider, "order.get", s);
                var key = $"demo_{s?.Id}";

                //var r =   new ProductDataService.ProductDataServiceClient(
                //     GrpcChannel.ForAddress(LibServiceAddressUtils.GetServiceAddress(this._provider, "Product")));
                //Random ran = new Random();
                //int RandKey = ran.Next(2000, 10000); 

                //var pm = new ProductParam() { Id = RandKey.ToString(), Remark = LibSysUtils.GetRandomStrByLength(1000, 4000) };
                //var mm = await r.GetProductAsync(new GrpcRequest() { Data = pm.ToJson() });
                //var mmpp = !string.IsNullOrEmpty(mm.Data) ? mm.Data.ToObjectFromJson<ProductModel>() : null;
                //if (mm.Messages.Any())
                //{
                //    foreach(var ssss in mm.Messages)
                //        reply.Messages.Add(new GrpcMessageReply() { Code = string.Empty, MessageType = (int)EFMessageType.Error, Message = ssss.Message });
                //}

                //var m3 = LibDistributedCacheUtils.Get< SNDBCacheInfoModel>(_provider, key);
                //if (m3==null)
                //{
                //   var m = GetSNDBCacheInfoModel();
                //    LibDistributedCacheUtils.Set(_provider, key, m, new TimeSpan(10, 0, 0));
                //}
                //var m1 = LibDistributedCacheUtils.Get< SNDBCacheInfoModel>(_provider, key);
                //m1.SNState.status = 2;// DateTime.Now.ToString("yyyyMMddhhmmss");
                //LibDistributedCacheUtils.Set(_provider, key, m1, new TimeSpan(10, 0, 0));
                //var m2 = LibDistributedCacheUtils.Get<SNDBCacheInfoModel>(_provider, key);
                //m2.SNState.status = 2;// DateTime.Now.ToString("yyyyMMddhhmmss");
                //    LibDistributedCacheUtils.Set(_provider, key, m2, new TimeSpan(10, 0, 0));
                var om = new OrderModel() { };
                reply.Data = om.ToJson();
            }
            catch (Exception ex)
            {
                reply.Messages.Add(new GrpcMessageReply() { Code = string.Empty, MessageType = (int)EFMessageType.Error, Message = LibSysUtils.GrpcSerial(ex.Message) });
            }

            return Task.FromResult( reply);
        }

        public override Task<GetOrderByRequestReply> GetOrderByRequest(GetOrderByRequestRequest request, ServerCallContext context)
        {
            GetOrderByRequestReply requestReply = new GetOrderByRequestReply();
            return Task.FromResult(requestReply);
        }

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
