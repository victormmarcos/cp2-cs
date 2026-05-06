using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace ProjetoBanco.Api.Messaging;

public class RabbitMqPublisher
{
    private const string QueueName = "contratacoes";
    private readonly IConfiguration _configuration;

    public RabbitMqPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void PublicarContratacao(long contratacaoId)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMq:Host"] ?? "localhost",
            UserName = _configuration["RabbitMq:User"] ?? "guest",
            Password = _configuration["RabbitMq:Password"] ?? "guest"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var payload = JsonSerializer.Serialize(new { ContratacaoId = contratacaoId });
        var body = Encoding.UTF8.GetBytes(payload);

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: QueueName,
            basicProperties: null,
            body: body);
    }
}
