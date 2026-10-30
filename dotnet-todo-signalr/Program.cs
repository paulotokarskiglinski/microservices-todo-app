using dotnet_todo_signalr.HubConfig;
using dotnet_todo_signalr.Models;
using dotnet_todo_signalr.Services;
using Microsoft.OpenApi;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddSingleton<LogService>();
builder.Services.AddSingleton<TarefaLogService>();
builder.Services.AddSingleton<UsuarioLogService>();
builder.Services.AddSingleton<TipoTarefaLogService>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<ITarefaLogService, TarefaLogService>();
builder.Services.AddScoped<IUsuarioLogService, UsuarioLogService>();
builder.Services.AddScoped<ITipoTarefaLogService, TipoTarefaLogService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
    builder => builder
        .WithOrigins("http://localhost:8080", "http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = ".NET TODO API",
        Description = "TODO desenvolvido em .NET 10 com minimal API e MongoDB",
        Version = "v1"
    });
});

var app = builder.Build();

#region RabbitMQ Consumer
using (var scope = app.Services.CreateScope())
{
    var logService = scope.ServiceProvider.GetRequiredService<LogService>();
    var tarefaLogService = scope.ServiceProvider.GetRequiredService<TarefaLogService>();
    var usuarioLogService = scope.ServiceProvider.GetRequiredService<UsuarioLogService>();
    var tipoTarefaLogService = scope.ServiceProvider.GetRequiredService<TipoTarefaLogService>();

    ConnectionFactory factory = new ConnectionFactory()
    {
        Uri = new Uri(app.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>()!.ConnectionString)
    };
    IConnection connection = await factory.CreateConnectionAsync();
    IChannel channel = await connection.CreateChannelAsync();

    // Queues must be declared the same way (durable) as in the producer APIs.
    async Task Consume(string queue, Func<BasicDeliverEventArgs, Task> handler)
    {
        await channel.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (sender, e) => handler(e);

        await channel.BasicConsumeAsync(queue: queue, autoAck: true, consumer: consumer);
    }

    #region Tarefa
    await Consume("put_tarefa", e => tarefaLogService.PutAsync(e, logService));
    await Consume("post_tarefa", e => tarefaLogService.PostAsync(e, logService));
    await Consume("delete_tarefa", e => tarefaLogService.DeleteAsync(e, logService));
    #endregion

    #region TipoTarefa
    await Consume("put_tipotarefa", e => tipoTarefaLogService.PutAsync(e, logService));
    await Consume("post_tipotarefa", e => tipoTarefaLogService.PostAsync(e, logService));
    await Consume("delete_tipotarefa", e => tipoTarefaLogService.DeleteAsync(e, logService));
    #endregion

    #region Usuario
    await Consume("put_usuario", e => usuarioLogService.PutAsync(e, logService));
    await Consume("post_usuario", e => usuarioLogService.PostAsync(e, logService));
    await Consume("delete_usuario", e => usuarioLogService.DeleteAsync(e, logService));
    #endregion
}
#endregion

app.UseCors("CorsPolicy");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET TODO API v1");
});

app.MapHub<LogHub>("/LogHub");

app.Run();
