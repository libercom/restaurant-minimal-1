using Newtonsoft.Json;
using ProjectA.Models;
using System.Collections.Concurrent;
using System.Text;

namespace ProjectA.Services
{
    public class BaseService : BackgroundService
    {
        private readonly ILogger<BaseService> _logger;
        private readonly ConcurrentQueue<Client> queue;
        private readonly string _apiEndpoint;

        public BaseService(IConfiguration configuration, ILogger<BaseService> logger)
        {
            queue = new ConcurrentQueue<Client>();
            _apiEndpoint = configuration.GetSection("ApiEndpoint").Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000);

            for (int i = 0; i < 5; i++)
            {
                Task.Run(Generate);
            }

            for (int i = 0; i < 3; i++)
            {
                Task.Run(Send);
            }
        }

        private async Task Generate()
        {
            while (true)
            {
                var value = new Client { Id = Guid.NewGuid() };
                queue.Enqueue(value);

                await Task.Delay(3000);
            }
        }

        private async Task Send()
        {
            while (true)
            {
                if (!queue.IsEmpty)
                {
                    await Task.Delay(3000);

                    queue.TryDequeue(out var value);

                    var httpClient = new HttpClient();
                    var serializedValue = JsonConvert.SerializeObject(value);

                    await httpClient.PostAsync(_apiEndpoint, new StringContent(serializedValue, Encoding.UTF8, "application/json"));
                }
            }
        }
    }
}
