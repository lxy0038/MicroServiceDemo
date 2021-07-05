using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Common.AspNetCore
{
    public  static class LibServiceAddressUtils
    {
        public static ChannelBase GetServiceAddress(IServiceProvider provider,string moduleCode)
        {
            var s = GetLibServiceAddressManager(provider);
            return s.GetServiceAddress(moduleCode);
        }


        public static void ClearServiceAddress(IServiceProvider provider)
        {
            var s = GetLibServiceAddressManager(provider);
            s.ClearServiceAddress();
        }
        private static ILibServiceAddressManager GetLibServiceAddressManager(IServiceProvider provider)
        {
            ILibServiceAddressManager clientFactory = null;
            try
            {
                if (provider != null)
                    clientFactory = provider.GetService<ILibServiceAddressManager>(); 
            }
            catch(Exception ex) 
            {

            }
            return clientFactory;
        }
    }
}
