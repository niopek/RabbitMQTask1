using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQProducer
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
            _factory = new()
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

            byte[] sendingBodyBytes = JsonSerializer.SerializeToUtf8Bytes(sendingObject);

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
