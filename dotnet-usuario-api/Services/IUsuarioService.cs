using dotnet_usuario_api.Models;

namespace dotnet_usuario_api.Services
{
    public interface IUsuarioService
    {
        Task CreateAsync(Usuario item);

        Task<List<Usuario>> GetAsync();

        Task<Usuario?> GetAsync(string id);

        Task RemoveAsync(string id);

        Task UpdateAsync(string id, Usuario item);
    }
}