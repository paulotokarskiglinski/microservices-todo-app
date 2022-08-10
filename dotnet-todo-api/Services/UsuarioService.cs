using System.Net.Http.Headers;
using System.Text.Json;
using dotnet_todo_api.Models;

namespace dotnet_todo_api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private static readonly HttpClient client = new HttpClient();

        public UsuarioService()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/javascript"));
        }

        public async Task<List<Usuario>?> GetAsync()
        {
            HttpResponseMessage response = await client.GetAsync("http://dotnet-gateway-ocelot:80/api/usuario");
            return JsonSerializer.Deserialize<List<Usuario>>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<Usuario?> GetAsync(string id)
        {
            HttpResponseMessage response = await client.GetAsync("http://dotnet-gateway-ocelot:80/api/usuario/" + id);
            return JsonSerializer.Deserialize<Usuario>(response.Content.ReadAsStringAsync().Result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
