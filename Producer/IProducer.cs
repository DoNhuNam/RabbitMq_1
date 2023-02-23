using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Producer
{
    public interface IProducer
    {
        void Publish<T>(IModel channel, T mes);
    }
    public class DirectExchangePublisher : IProducer
    {
        public void Publish<T>(IModel channel, T mes)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000 }
            };
            channel.ExchangeDeclare("demo-direct-exchange", ExchangeType.Direct, arguments: ttl);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mes));
            channel.BasicPublish("demo-direct-exchange", "account.init", null, body);
        }
    }
    public class FanoutExchangePublisher : IProducer
    {
        public void Publish<T>(IModel channel, T mes)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000 }
            };
            channel.ExchangeDeclare("demo-fanout-exchange", ExchangeType.Fanout, arguments: ttl);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mes));

            var properties = channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object> { { "account", "update" } };

            channel.BasicPublish("demo-fanout-exchange", "account.new", properties, body);
        }
    }
    public class HeaderExchangePublisher : IProducer
    {
        public void Publish<T>(IModel channel, T mes)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000 }
            };
            channel.ExchangeDeclare("demo-header-exchange", ExchangeType.Headers, arguments: ttl);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mes));

            var properties = channel.CreateBasicProperties();
            properties.Headers = new Dictionary<string, object> { { "account", "update" } };

            channel.BasicPublish("demo-header-exchange", string.Empty, properties, body);
        }
    }
    public class TopicExchangePublisher : IProducer
    {
        public void Publish<T>(IModel channel, T mes)
        {
            var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000 }
            };
            channel.ExchangeDeclare("demo-topic-exchange", ExchangeType.Topic, arguments: ttl);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mes));

            channel.BasicPublish("demo-topic-exchange", "user.update", null, body);
        }
    }
    public class QueueProducer : IProducer
    {
        public void Publish<T>(IModel channel, T mes)
        {
            channel.QueueDeclare("demo-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(mes));

            channel.BasicPublish("", "demo-queue", null, body);
        }
    }
}
