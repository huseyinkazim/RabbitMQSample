using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQ.Common
{


	public class RabbitMQConnection
	{
		private readonly ConnectionFactory _connectionFactory;

		public RabbitMQConnection()
		{
			_connectionFactory = new ConnectionFactory { Uri = new Uri(ConnectionInformation.connectionUrl) };
		}

		public void PublishMessage(string message, string queueName)
		{
			using (var connection = _connectionFactory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);

				var body = Encoding.UTF8.GetBytes(message);
				channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
			}
		}

		public void SubscribeMessages(string queueName, Action<string> messageHandler)
		{
			using (var connection = _connectionFactory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
				var consumer = new EventingBasicConsumer(channel);

				consumer.Received += (model, ea) =>
				{
					var message = Encoding.UTF8.GetString(ea.Body.ToArray());
					messageHandler(message);
				};

				channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

				Console.WriteLine($" [*] Waiting for messages. To exit press CTRL+C");
				Console.ReadLine();
			}
		}
	}
}