using System;
using System.Collections.Generic;
using System.Text;

namespace Common.AspNetCore
{
    public class ConsulConfig
    {
        public string Address { get; set; }

        public string Name { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }
        public string Protocol { get; set; }

        public int Interval { get; set; }
        public int Timeout { get; set; }

        public string[] Tags { get; set; }

        public bool IsUseTls { get; set; }

        public int DeregisterCriticalServiceAfter { get; set; }
    }
}
