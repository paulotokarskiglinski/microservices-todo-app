using RabbitMQ.Client.Events;

namespace dotnet_todo_signalr.Services
{
    public interface IUsuarioLogService
    {
        public void PostAsync(BasicDeliverEventArgs e, LogService logService);

        public void PutAsync(BasicDeliverEventArgs e, LogService logService);

        public void DeleteAsync(BasicDeliverEventArgs e, LogService logService);
    }
}
