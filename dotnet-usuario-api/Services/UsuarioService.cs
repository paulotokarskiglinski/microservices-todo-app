using dotnet_usuario_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace dotnet_usuario_api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMongoCollection<Usuario> _collection;

        public UsuarioService(IOptions<UsuarioDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<Usuario>(databaseSettings.Value.UsuarioCollectionName);
        }

        public async Task<List<Usuario>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Usuario?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Usuario item) =>
            await _collection.InsertOneAsync(item);

        public async Task UpdateAsync(string id, Usuario item) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, item);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
