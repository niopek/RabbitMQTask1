using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQProducerLib
{
    public class Producer
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;
        private IConnection? _connection;
        private IModel? _channel;
        public Producer(Uri uri, string queueName)
        {
            _queueName = queueName;
            _factory = new ConnectionFactory()
            {
                Uri = uri,
                ClientProvidedName = "Rabbit Producer"
            };

        }

        public void SendToQueue(ISend sendingObject)
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            string exchangeName = "ProducerExchange";
            string routingKey = "producer-key";
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(_queueName, false, false, false, null);
            _channel.QueueBind(_queueName, exchangeName, routingKey, null);

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };

            byte[] sendingBodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sendingObject, settings));

            _channel.BasicPublish(exchangeName, routingKey, null, sendingBodyBytes);

            _channel.Close();
            _connection.Close();
        }

    }

    public interface ISend
    {
        void Send();
    }
}
