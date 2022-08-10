using dotnet_todo_api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace dotnet_todo_api.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly IMongoCollection<Tarefa> _collection;

        public TarefaService(IOptions<TarefaDatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<Tarefa>(databaseSettings.Value.TarefaCollectionName);
        }

        public async Task<List<Tarefa>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Tarefa?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Tarefa item) =>
            await _collection.InsertOneAsync(item);

        public async Task UpdateAsync(string id, Tarefa item) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, item);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
