using Consumer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Producer;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IModel _model;
        private readonly ISendMessage _sendMessage;
        private readonly IConsumer _consumer;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IModel model, ISendMessage sendMessage, IConsumer consumer)
        {
            _logger = logger;
            _model = model;
            _sendMessage = sendMessage;
            _consumer = consumer;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _sendMessage.Send<Student>(new Student() { Code = "Do Nam" });

            _consumer.Consume(_model);
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        public class Student
        {
            public string Code { get; set; }
        }
    }
}
