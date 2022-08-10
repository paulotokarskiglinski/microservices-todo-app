namespace dotnet_usuario_api.Models
{
    public class UsuarioDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string UsuarioCollectionName { get; set; } = null!;
    }
}
