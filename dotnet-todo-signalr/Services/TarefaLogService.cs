using System.Text.Json;
using dotnet_todo_signalr.Models;
using RabbitMQ.Client.Events;

namespace dotnet_todo_signalr.Services
{
    public class TarefaLogService : ITarefaLogService
    {
        public void PostAsync(BasicDeliverEventArgs e, LogService logService)
        {
            var body = e.Body.ToArray();
            var message = JsonSerializer.Deserialize<Tarefa>(body);

            if (message == null)
                return;

            var log = new Log
            {
                Metodo = "POST",
                Info = JsonSerializer.Serialize(message)
            };

            if (log != null)
                _ = logService.CreateAsync(log);
        }

        public void PutAsync(BasicDeliverEventArgs e, LogService logService)
        {
            var body = e.Body.ToArray();
            var message = JsonSerializer.Deserialize<Tarefa>(body);

            if (message != null)
            {
                var log = new Log
                {
                    Metodo = "PUT",
                    Info = JsonSerializer.Serialize(message)
                };

                if (log != null)
                    _ = logService.CreateAsync(log);
            }
        }

        public void DeleteAsync(BasicDeliverEventArgs e, LogService logService)
        {
            var body = e.Body.ToArray();
            var message = JsonSerializer.Deserialize<string>(body);

            if (message == null)
                return;

            var log = new Log
            {
                Metodo = "DELETE",
                Info = JsonSerializer.Serialize(message)
            };

            if (log != null)
                _ = logService.CreateAsync(log);
        }
    }
}
