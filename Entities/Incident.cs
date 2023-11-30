using Newtonsoft.Json;

namespace ITentikaTest.Entities
{
    public class Incident
    {
        private Incident() {}
        public Incident(IncidentType incidentType, params Event[] events)
        {
            IncidentType = incidentType;
            Time = DateTime.Now;
            CoreEventsSerialized = JsonConvert.SerializeObject(events);
        }

        public Guid Id { get; private set; }
        public IncidentType IncidentType { get; private set; }
        public DateTime Time { get; private set; }
        public string CoreEventsSerialized { get; private set; }
    }

    public enum IncidentType
    {
        First = 1,
        Second = 2
    }
}
