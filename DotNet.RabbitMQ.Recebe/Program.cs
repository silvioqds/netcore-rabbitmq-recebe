
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

//Crio a fila, somente caso o sistema "recebe" comeca a rodar primeiro, se ja tiver criada, irá ignorar.
channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine("Aguardando mensagens...");

var consumer = new EventingBasicConsumer(channel);

//Quando a fila receber uma mensage, o evento received é ativado e chama a funcão delegada
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    var aluno = JsonSerializer.Deserialize<Aluno>(message);

    Console.WriteLine(message);
};

channel.BasicConsume(queue: "hello", autoAck: true, consumer);

Console.WriteLine("Aperte [ENTER] para sair");
Console.ReadLine();


class Aluno
{
    public int Id { get; set; }
    public string? Name { get; set; }
}