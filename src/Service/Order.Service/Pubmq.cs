using Order.Service;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IVAN.MES.Rabbitmq.Publish
{
    public class Pubmq
    {
        private readonly MqConnection mqConnection;
        public Pubmq(MqConnection mqConnection)
        {
            this.mqConnection = mqConnection;
        }

        public async Task BasicPublish(string queueName, string message)
        {
            using (var connection = mqConnection.Factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: queueName,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(string.Empty, queueName, null, body);
                }
            }
        }
    }
}
