using DotNetCore.CAP;
using DotNetCore.CAP.NoStorage;
using DotNetCore.CAP.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CapOptionsExtensions
    {
        public static CapOptions UseNoStorage(this CapOptions options)
        {
            var o = options.UseInMemoryStorage();
            options.RegisterExtension(new NostorageCapOptionsExtension());
            return o;
        }
    }

    internal class NostorageCapOptionsExtension : ICapOptionsExtension
    {
        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IDataStorage, NoStorageStorage>();
        }
    }
}
