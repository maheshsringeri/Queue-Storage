using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Queue_Storage.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task Post([FromBody] WeatherForecast data)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=mtest0storageaccount;AccountKey=EdT8u4m9PFzEgUdrzrIDnJXMcBpW+LqTkM4nCyriZ+LTrzMavJvN4fsOqSr6m8T1KQXc6EjtIY2t+AStH6NM4Q==;EndpointSuffix=core.windows.net";
            var queueName = "add-weatherdata";

            var queueClient = new QueueClient(connectionString, queueName);
            var message = JsonSerializer.Serialize(data);

            //await queueClient.SendMessageAsync(message, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(20));
            await queueClient.SendMessageAsync(message, null, TimeSpan.FromSeconds(-1));

        }




    }
}