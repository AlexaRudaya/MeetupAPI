var factory = new ConnectionFactory
{
    HostName = "localhost"
};

//var connection = factory.CreateConnection();  // In order RabbitMQ to work it is necessary to run Rabbit server

//using var channel = connection.CreateModel();

//channel.QueueDeclare("event", exclusive: false);
//channel.QueueDeclare("speaker", exclusive: false);
//channel.QueueDeclare("sponsor", exclusive: false);

//var consumer = new EventingBasicConsumer(channel);

//consumer.Received += (model, eventArgs) =>
//{
//    var body = eventArgs.Body.ToArray();
//    var message = Encoding.UTF8.GetString(body);
//    Console.WriteLine($"Current message is sent: {message}");
//};

//channel.BasicConsume(queue: "event", autoAck: true, consumer: consumer);
//channel.BasicConsume(queue: "speaker", autoAck: true, consumer: consumer);
//channel.BasicConsume(queue: "sponsor", autoAck: true, consumer: consumer);

//Console.ReadKey();