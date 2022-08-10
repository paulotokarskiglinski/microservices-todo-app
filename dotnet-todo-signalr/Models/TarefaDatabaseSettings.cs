namespace dotnet_todo_signalr.Models
{
    public class TarefaDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string TarefaCollectionName { get; set; } = null!;
    }
}
