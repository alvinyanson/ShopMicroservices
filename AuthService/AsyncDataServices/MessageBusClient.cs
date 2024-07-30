using AuthService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AuthService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {

        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };


            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("Connected to Message Bus... 🔥🔥🔥");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not connect to the Message Bus: {ex.Message}");

            }
        }

        public void UserSignUp(RegisterUserDto userSignUpDto)
        {
            var message = JsonSerializer.Serialize(userSignUpDto);

            Console.WriteLine($"User sign up: {message}");

            if (_connection.IsOpen)
            {
                Console.WriteLine("RabbitMQ connection is open, sending message now...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("RabbitMQ connection closed, not sending...");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);

            Console.WriteLine($"We have sent {message} to ProductCatalogService 🔥🔥🔥");
        }

        public void Dispose()
        {
            Console.WriteLine("Message bus disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ Connection Shutdown");
        }
    }
}
