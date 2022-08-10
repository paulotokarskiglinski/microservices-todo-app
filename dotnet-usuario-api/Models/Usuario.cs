using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace dotnet_usuario_api.Models
{
    public class Usuario
    {
        public Usuario()
        {
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nome")]
        public string Nome { get; set; }

        [BsonElement("nomeusuario")]
        public string NomeUsuario { get; set; }

        [BsonElement("senha")]
        public string Senha { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }
    }
}
