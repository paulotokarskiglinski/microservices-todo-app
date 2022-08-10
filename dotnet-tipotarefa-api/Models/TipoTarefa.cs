using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace dotnet_tipotarefa_api.Models
{
    public class TipoTarefa
    {
        public TipoTarefa()
        {
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("descricao")]
        public string Descricao { get; set; }
    }
}
