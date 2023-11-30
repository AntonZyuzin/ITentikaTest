using ITentikaTest.Entities;

namespace ITentikaTest.IClients
{
    public interface IProcessorClient
    {
        Task Send(Event newEvent);
    }
}