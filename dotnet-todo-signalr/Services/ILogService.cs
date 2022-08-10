using dotnet_todo_signalr.Models;

namespace dotnet_todo_signalr.Services
{
    public interface ILogService
    {
        public Task CreateAsync(Log item);
    }
}
