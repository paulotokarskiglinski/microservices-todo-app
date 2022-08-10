using System.Net.Http.Headers;
using System.Text.Json;
using dotnet_todo_api.Models;

namespace dotnet_todo_api.Services
{
    public class TipoTarefaService : ITipoTarefaService
    {
        private static readonly HttpClient client = new HttpClient();

        public TipoTarefaService()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/javascript"));
        }

        public async Task<List<TipoTarefa>?> GetAsync()
        {
            HttpResponseMessage response = await client.GetAsync("http://dotnet-gateway-ocelot:80/api/tipotarefa");
            return JsonSerializer.Deserialize<List<TipoTarefa>>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<TipoTarefa?> GetAsync(string id)
        {
            HttpResponseMessage response = await client.GetAsync("http://dotnet-gateway-ocelot:80/api/tipotarefa/" + id);
            return JsonSerializer.Deserialize<TipoTarefa>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
