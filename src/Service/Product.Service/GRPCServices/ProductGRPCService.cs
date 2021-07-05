using Grpc.Core;
using Common.AspNetCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mes.GrpcService;
using Demo.Models;

namespace Product.Service
{
    public class ProductGRPCService : ProductDataService.ProductDataServiceBase
    {
        private readonly IServiceProvider _provider;
        public ProductGRPCService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public override Task<GrpcReply> GetProduct(GrpcRequest request, ServerCallContext context)
        {
            GrpcReply reply = new GrpcReply();
            try
            {
                var s = request.Data?.ToObjectFromJson<ProductParam>();

                var key = $"demo_{s?.Id}";
                var m = LibDistributedCacheUtils.Get<ProductModel>(_provider, key);
                if (m == null)
                {
                   m = new ProductModel()
                    {
                        Code = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                        Id = Guid.NewGuid().ToString(),
                        Name = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                        OrgId = Guid.NewGuid().ToString(),
                        Remark = LibSysUtils.GetRandomStrByLength(300, 600)
                    };
                    LibDistributedCacheUtils.Set(_provider, key, m, new TimeSpan(10, 0, 0));
                }  

                reply.Data = m.ToJson();
            }
            catch (Exception ex)
            {
                reply.Messages.Add(new GrpcMessageReply() { Code = string.Empty, MessageType = (int)EFMessageType.Error, Message = LibSysUtils.GrpcSerial(ex.Message) });
            }
            return Task.FromResult(reply); 
        }

        public override Task<GetProductByRequestReply> GetProductByRequest(GetProductByRequestRequest request, ServerCallContext context)
        {
            return base.GetProductByRequest(request, context);
        }

    }
}
