using ITentikaTest.Entities;
using ITentikaTest.IClients;

namespace ITentikaTest.BackgroundServices
{
    public class GeneratorHostedService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IProcessorClient _processorClient;
        private readonly Timer _timer;
        private readonly Random _random;
        private int _upperPeriod;
        private int _enumSize = Enum.GetNames(typeof(EventType)).Length;

        public GeneratorHostedService(IProcessorClient processorClient, IConfiguration configuration)
        {
            _configuration = configuration;
            _processorClient = processorClient;
            _random = new Random();
            _timer = new Timer(GenerateEvent);
            _upperPeriod = int.Parse(_configuration["UpperPeriod"]);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer.Change(0, _random.Next(0, _upperPeriod));
            return Task.CompletedTask;
        }

        public async void GenerateEvent(object? state)
        {
            var newEvent = new Event((EventType) _random.Next(1, _enumSize + 1));
            await _processorClient.Send(newEvent);
            var period = _random.Next(0, _upperPeriod);
            _timer.Change(period, period);
        }

        public async Task GenerateEventByUser(EventType type)
        {
            //возможно будут проблемы рейскондишена
            var newEvent = new Event(type);
            await _processorClient.Send(newEvent);
            var period = _random.Next(0, _upperPeriod);
            _timer.Change(period, period);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
