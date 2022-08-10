namespace dotnet_todo_signalr.Models
{
    public class Tarefa
    {
        public string? Id { get; set; }

        public string Descricao { get; set; }

        public DateTime? DataTarefa { get; set; }

        public bool Feito { get; set; }

        public string IdTipoTarefa { get; set; }

        public string IdUsuario { get; set; }
    }
}
