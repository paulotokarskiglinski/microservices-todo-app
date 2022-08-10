using dotnet_todo_api.Models;

namespace dotnet_todo_api.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>?> GetAsync();

        Task<Usuario?> GetAsync(string id);
    }
}