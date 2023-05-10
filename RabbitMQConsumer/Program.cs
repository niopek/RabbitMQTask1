

using RabbitMQConsumer;

var uri = new Uri("amqp://guest:guest@localhost:5672");

Consumer consumer = new(uri, "testqueue");


consumer.StartConsuming();