using dotnet_todo_api.Models;

namespace dotnet_todo_api.Services
{
    public interface ITarefaService
    {
        public Task CreateAsync(Tarefa item);

        public Task<List<Tarefa>> GetAsync();

        public Task<Tarefa?> GetAsync(string id);

        public Task RemoveAsync(string id);

        public Task UpdateAsync(string id, Tarefa item);
    }
}
