using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AspNetCore
{
    public interface ILibServiceAddressManager
    {
        ChannelBase GetServiceAddress(string moduleCode);
        void ClearServiceAddress();
    }
}
