using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQProducerLib;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace RabbitMQConsumer
{
    public class Consumer 
    {
        private readonly ConnectionFactory _factory;
        private readonly string _queueName;
        private IConnection? _connection;
        private IModel? _channel;
        public Consumer(Uri uri, string queueName)
        {
            _queueName = queueName;
            _factory = new()
            {
                Uri = uri,
                ClientProvidedName = "Rabbit Producer"
            };

        }

        public void StartConsuming()
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            string exchangeName = "ProducerExchange";
            string routingKey = "producer-key";
            _channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            _channel.QueueDeclare(_queueName, false, false, false, null);
            _channel.QueueBind(_queueName, exchangeName, routingKey, null);
            _channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                };

                var senderObject = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(body), settings) as ISend;

                if(senderObject is Email)
                {
                    var email = senderObject as Email;
                    if(email != null)
                    {
                        email.Send();
                    }
                }

                if (senderObject is Car)
                {
                    var email = senderObject as Car;
                    if (email != null)
                    {
                        email.Send();
                    }
                }



                _channel.BasicAck(args.DeliveryTag, false);
            };

            string consumerTag = _channel.BasicConsume(_queueName, false, consumer);

            Console.ReadLine();

            _channel.BasicCancel(consumerTag);
            _channel.Close();
            _connection.Close();
        }
    }
}
