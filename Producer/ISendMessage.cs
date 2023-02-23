using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Producer
{
    public interface ISendMessage
    {
        void Send<T>(T obj);
    }
    public class SendMessage : ISendMessage
    {
        private readonly IProducer _producer;
        private readonly IModel _model;
        public SendMessage(IProducer producer, IModel model)
        {
            _producer = producer;
            _model = model;
        }
        public void Send<T>(T obj)
        {
            _producer.Publish(_model, obj);
        }
    }
}
