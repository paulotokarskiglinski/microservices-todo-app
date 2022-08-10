using dotnet_tipotarefa_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace dotnet_tipotarefa_api.Services
{
    public class TipoTarefaService : ITipoTarefaService
    {
        private readonly IMongoCollection<TipoTarefa> _collection;

        public TipoTarefaService(IOptions<TarefaDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<TipoTarefa>(databaseSettings.Value.TipoTarefaCollectionName);
        }

        public async Task<List<TipoTarefa>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<TipoTarefa?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(TipoTarefa item) =>
            await _collection.InsertOneAsync(item);

        public async Task UpdateAsync(string id, TipoTarefa item) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, item);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
