using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Service
{
    public class MqConnection
    {
        private ConnectionFactory factory = null;

        public MqConnection()
        { 
        }

        public ConnectionFactory Factory
        {
            get
            {
                if (factory == null)
                {
                    factory = new ConnectionFactory()
                    {
                        HostName = "192.168.8.140",
                        Port = 5672,
                        UserName = "guest",
                        Password = "guest",
                        ContinuationTimeout = new TimeSpan(0, 0, 3)
                    };
                }

                return factory;
            }
        }
    }
}
