// Sender uygulaması için RabbitMQConnection sınıfından bir nesne oluşturuyoruz.
using RabbitMQ.Common;

RabbitMQConnection sender = new();


// Sender uygulaması için kullanıcıdan mesajları okuyup göndermek için bir döngü başlatıyoruz.
while (true)
{
	Console.WriteLine("Enter a message to send:");
	var message = Console.ReadLine();
	sender.PublishMessage(message, "sampleQueue");
}

