using RabbitMQ.Common;

var receiver = new RabbitMQConnection();

// Receiver uygulaması için mesajları işleyecek olan handler fonksiyonunu tanımlıyoruz.
Action<string> messageHandler = (message) =>
{
	Thread.Sleep(5000);
	Console.WriteLine($"Received Message: {message}");
};

// Receiver uygulaması için SubscribeMessages metodunu çağırıyoruz.
receiver.SubscribeMessages("sampleQueue", messageHandler);

Console.ReadKey();

