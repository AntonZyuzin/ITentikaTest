using ITentikaTest.Entities;

namespace ITentikaTest.Domain.IRepositories
{
    public interface IIncidentRepository
    {
        public void CreateIncident(Incident newIncident);
        public List<Incident> GetIncidents(string? sortProperty = null, int? page = null, int? limit = null);
    }
}
