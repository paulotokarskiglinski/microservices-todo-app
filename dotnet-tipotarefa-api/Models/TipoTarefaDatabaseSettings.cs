namespace dotnet_tipotarefa_api.Models
{
    public class TarefaDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string TipoTarefaCollectionName { get; set; } = null!;
    }
}
