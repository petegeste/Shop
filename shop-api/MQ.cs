using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;

namespace shop_api
{
    public class MQ : IDisposable
    {
        private IConnection connection;
        private IModel channel;

        public MQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = Environment.GetEnvironmentVariable("SHOP_MESSAGING_HOST")
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: "resize",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        }

        public async Task RequestImageConvert(string imgName)
        {
            var body = Encoding.UTF8.GetBytes(imgName);
            channel.BasicPublish(exchange: "",
                routingKey: "resize",
                basicProperties: null,
                body: body
                );
#warning this is weird
            await Task.Delay(0);
        }

        public void Dispose()
        {
            channel.Dispose();
            connection.Dispose();
        }
    }
}
