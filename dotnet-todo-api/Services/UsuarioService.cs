using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using dotnet_todo_api.Models;

namespace dotnet_todo_api.Services
{
    public class UsuarioService : IUsuarioService
    {
        // Shared across all instances, so it is configured once here and never mutated per request.
        private static readonly HttpClient client = CreateClient();

        private static HttpClient CreateClient()
        {
            var httpClient = new HttpClient { BaseAddress = new Uri("http://dotnet-gateway-ocelot:8080/api/") };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        public async Task<List<Usuario>?> GetAsync() =>
            await client.GetFromJsonAsync<List<Usuario>>("usuario");

        public async Task<Usuario?> GetAsync(string id)
        {
            using var response = await client.GetAsync("usuario/" + id);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Usuario>();
        }
    }
}
