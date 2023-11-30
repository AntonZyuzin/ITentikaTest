using ITentikaTest.Entities;
using ITentikaTest.IClients;
using Newtonsoft.Json;
using System.Text;

namespace ITentikaTest.Clients
{
    public class ProcessorClient : IProcessorClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public ProcessorClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task Send(Event newEvent)
        {
            await _httpClient.PostAsync(_configuration["Url"] + "SendEvent", new StringContent(JsonConvert.SerializeObject(newEvent), Encoding.UTF8, "application/json"));
        }



        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
