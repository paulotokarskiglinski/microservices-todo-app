using dotnet_todo_api.Models;

namespace dotnet_todo_api.Services
{
    public interface ITipoTarefaService
    {
        Task<List<TipoTarefa>?> GetAsync();

        Task<TipoTarefa?> GetAsync(string id);
    }
}