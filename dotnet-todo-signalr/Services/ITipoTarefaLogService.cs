using RabbitMQ.Client.Events;

namespace dotnet_todo_signalr.Services
{
    public interface ITipoTarefaLogService
    {
        public Task PostAsync(BasicDeliverEventArgs e, LogService logService);

        public Task PutAsync(BasicDeliverEventArgs e, LogService logService);

        public Task DeleteAsync(BasicDeliverEventArgs e, LogService logService);
    }
}
