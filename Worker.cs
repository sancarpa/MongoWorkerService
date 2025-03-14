using MongoDB.Driver;

namespace MongoWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMongoCollection<DataPoint> _collection;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            var connectionString = "mongodb://mongo:27017";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("test");
            _collection = database.GetCollection<DataPoint>("datapoints");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var dataPoint = new DataPoint
                {
                    Timestamp = DateTime.UtcNow,
                    Value = new Random().Next(100)
                };

                await _collection.InsertOneAsync(dataPoint, cancellationToken: stoppingToken);
                _logger.LogInformation($"Data point inserted: {dataPoint.Timestamp}, {dataPoint.Value}");

                await Task.Delay(10000, stoppingToken); // Intervallo di 10 secondi
            }
        }
    }

    public class DataPoint
    {
        public DateTime Timestamp { get; set; }
        public int Value { get; set; }
    }
}