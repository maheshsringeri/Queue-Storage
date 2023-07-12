using Azure.Storage.Queues;
using System.Text.Json;

namespace Queue_Storage
{
    public class WeatherDataService : BackgroundService
    {
        private readonly ILogger _logger;
        public WeatherDataService(ILogger<WeatherDataService> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=mtest0storageaccount;AccountKey=EdT8u4m9PFzEgUdrzrIDnJXMcBpW+LqTkM4nCyriZ+LTrzMavJvN4fsOqSr6m8T1KQXc6EjtIY2t+AStH6NM4Q==;EndpointSuffix=core.windows.net";
            var queueName = "add-weatherdata";

            var queueClient = new QueueClient(connectionString, queueName);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.Log(LogLevel.Information, "Read message from queue");

                var queueMessage = await queueClient.ReceiveMessageAsync();

                if (queueMessage.Value != null)
                {
                    var weatherData = JsonSerializer.Deserialize<WeatherForecast>(queueMessage.Value.MessageText);

                    _logger.LogInformation($"New message read:{weatherData}");

                    await queueClient.DeleteMessageAsync(queueMessage.Value.MessageId, queueMessage.Value.PopReceipt);
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}
