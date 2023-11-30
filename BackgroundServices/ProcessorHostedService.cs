
using ITentikaTest.Domain.IRepositories;
using ITentikaTest.Entities;

namespace ITentikaTest.BackgroundServices
{
    public class ProcessorHostedService : IHostedService
    {
        private readonly List<Event> _events = new List<Event>();
        private readonly IServiceProvider _serviceProvider;

        public ProcessorHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void AcceptEvent(Event newEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var incidentRepository = scope.ServiceProvider.GetRequiredService<IIncidentRepository>();

                if (newEvent.EventType == EventType.First)
                {
                    if (_events.Count == 0)
                    {
                        var incident = new Incident(IncidentType.First, newEvent);
                        incidentRepository.CreateIncident(incident);
                    }
                    else
                    {
                        _events.RemoveAll(e => (DateTime.Now - e.Time) > TimeSpan.FromSeconds(20));

                        //тут желательно сделать тред сейв
                        foreach (var e in _events)
                        {
                            var incident = new Incident(IncidentType.Second, new[] { e, newEvent });
                            incidentRepository.CreateIncident(incident);
                        }
                        _events.Clear();
                    }
                }
                else if (newEvent.EventType == EventType.Second)
                {
                    //В ходе обсуждения выяснилось, что при получении нового е2, мы его игнорируем
                    if (!_events.Any(e => e.EventType == EventType.Second))
                    {
                        _events.Add(newEvent);
                    }
                }

                //события 3 и 4 игнорируем
            }
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}


//[E2                   |   E1    ] → Incident 1 (E1)
//0sec                 20sec

//[E2              E2   |       ] → Incident ?
// 0sec                 20sec

//[E2              E2 E1|      ] → Incident 2 (E2_1, E1), Incident 2 (E2_2, E1)
// 0sec                 20sec