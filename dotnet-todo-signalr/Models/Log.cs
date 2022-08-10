namespace dotnet_todo_signalr.Models
{
    public class Log
    {
        public string Metodo { get; set; }

        public string Info { get; set; }

        public DateTime Data => DateTime.Now;
    }
}
