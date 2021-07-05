using Consul;
using System; 
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Threading;
using Grpc.Core;
using Grpc.Net.Client;

namespace Common.AspNetCore
{
    public class LibServiceAddressManager: ILibServiceAddressManager
    {
        private readonly IConfiguration configuration;
        private readonly IServiceProvider provider = null;
        private static LibServiceAddressByConsul consul = new LibServiceAddressByConsul(); 
        private Dictionary<string, string> address = new Dictionary<string, string>();
        private static Dictionary<string, ChannelBase> _channel = new Dictionary<string, ChannelBase>();

        public  LibServiceAddressManager(IServiceProvider provider, IConfiguration configuration)
        {
            this.provider = provider;
            this.configuration = configuration;
        }

        public void ClearServiceAddress()
        {
            this.address.Clear();
        }


        public ChannelBase GetServiceAddress(string moduleCode)
        {
            if (string.IsNullOrEmpty(moduleCode)) return null;


            if(!this.address.TryGetValue(moduleCode,out string v))
            {
                if (!consul.Items.TryGetValue(moduleCode, out LibServiceAddressByConsulItem item))
                {
                    item = new LibServiceAddressByConsulItem();
                    consul.Items[moduleCode]= item;
                }
                if (item.LastGetTime == null ||
                    item.LastGetTime.Value.AddMinutes(10) < DateTime.Now ||
                    !item.Addresss.Any())
                {
                    var address = this.configuration["Consul:Address"];
                    if (string.IsNullOrEmpty(address)) throw new ArgumentNullException("Consul:Address");
                    GetConsulItem(address, this.configuration["Consul:Http"],
                            moduleCode, item);
                    item.LastGetTime = DateTime.Now;
                }
                if (item != null && item.Addresss.Any())
                {
                    try
                    {
                        if (item.Index > item.Addresss.Count - 1) item.Index = 0;
                        var itemAddress = item.Addresss[item.Index];
                        v = $"{itemAddress.Http}://{itemAddress.Ip}:{itemAddress.Port}";
                        this.address.Add(moduleCode, v);
                        item.Index++;
                    }
                    catch { }
                }
            }
            if (string.IsNullOrEmpty(v)) throw new Exception("The " + moduleCode + " isn't exists");

            return GetChannel(v);
        }

        private ChannelBase GetChannel(string url)
        {
            if (!_channel.TryGetValue(url, out ChannelBase channel))
            {
                channel = GrpcChannel.ForAddress(url);
                lock (_channel)
                {
                    _channel[url] = channel;
                }
            }
            return channel;
        }


        public static void Start(string url, string http)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        foreach (var s in consul.Items)
                        {
                            try
                            {
                                GetConsulItem(url, http, s.Key, s.Value);
                            }
                            catch { }
                        }
                    }
                    catch { }
                    Thread.Sleep(10 * 1000);
                }
            });
        }

        private static void GetConsulItem(string url, string http, string moduleCode, LibServiceAddressByConsulItem item)
        {
            var consulClient = new ConsulClient(options =>
            {
                options.Address = new Uri(url);
            });
            var result = consulClient.Health.Service(moduleCode, null, true, item.Options).Result;

            item.Options.WaitIndex = result.LastIndex;
            var httpModel = !string.IsNullOrEmpty(http) ?
                http : "http";
            item.Addresss.Clear();
            foreach(var t in result.Response) 
            {
                item.Addresss.Add(new LibServiceAddressByConsulItemAddress()
                {
                    Http = httpModel,
                    Ip = t.Service.Address,
                    Port = t.Service.Port
                });
            }

        }

         
    }

    public class LibServiceAddressByConsul
    {
       public LibServiceAddressByConsul()
        {
            this.Items = new SortedDictionary<string, LibServiceAddressByConsulItem>();
        }

        public SortedDictionary<string,LibServiceAddressByConsulItem> Items { get; set; }

    }

    public class LibServiceAddressByConsulItem
    {
        public LibServiceAddressByConsulItem()
        {
            this.Addresss = new List<LibServiceAddressByConsulItemAddress>();
            this.Options = new QueryOptions
            {
                WaitTime = TimeSpan.FromSeconds(1)
            };
        }

        public string ModuleCode { get; set; }

        public QueryOptions Options { get; set; }

        public int Index { get; set; }

        public DateTime? LastGetTime { get; set; }

        public List<LibServiceAddressByConsulItemAddress> Addresss { get; set; }
    }
    public class LibServiceAddressByConsulItemAddress
    {
        public string Http { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }
    }


}
