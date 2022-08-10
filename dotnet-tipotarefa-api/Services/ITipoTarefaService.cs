using dotnet_tipotarefa_api.Models;

namespace dotnet_tipotarefa_api.Services
{
    public interface ITipoTarefaService
    {
        Task CreateAsync(TipoTarefa tarefa);

        Task<List<TipoTarefa>> GetAsync();

        Task<TipoTarefa?> GetAsync(string id);

        Task RemoveAsync(string id);

        Task UpdateAsync(string id, TipoTarefa tarefa);
    }
}