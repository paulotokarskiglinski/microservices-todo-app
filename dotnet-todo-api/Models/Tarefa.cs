using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace dotnet_todo_api.Models
{
    public class Tarefa
    {
        public Tarefa()
        {
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("descricao")]
        public string Descricao { get; set; }

        [BsonElement("datatarefa")]
        public DateTime? DataTarefa { get; set; }

        [BsonElement("feito")]
        public bool Feito { get; set; }

        [BsonElement("idtipotarefa")]
        public string IdTipoTarefa { get; set; }

        [BsonElement("idusuario")]
        public string IdUsuario { get; set; }

        [BsonIgnore]
        public Usuario Usuario { get; set; }

        [BsonIgnore]
        public TipoTarefa TipoTarefa { get; set; }
    }
}
