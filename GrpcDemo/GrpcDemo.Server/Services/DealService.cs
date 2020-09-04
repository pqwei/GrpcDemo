using Grpc.Core;
using GrpcDeal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static GrpcDeal.dealService;

namespace GrpcDemo.Server.Services
{
    public class DealService : dealServiceBase
    {
        public override Task<DealReply> GetDeal(DealIdRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DealReply { Name = "魏朋强", Remark = "测试" });
        }
    }
}
