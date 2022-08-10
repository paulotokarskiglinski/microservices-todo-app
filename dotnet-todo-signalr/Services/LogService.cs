using dotnet_todo_signalr.HubConfig;
using dotnet_todo_signalr.Models;
using Microsoft.AspNetCore.SignalR;

namespace dotnet_todo_signalr.Services
{
    public class LogService : ILogService
    {
        private readonly IHubContext<LogHub> _hub;

        public LogService(IHubContext<LogHub> hub)
        {
            _hub = hub;
        }

        public async Task CreateAsync(Log item)
        {
            await _hub.Clients.All.SendAsync("LogMessage", item);
        }
    }
}

