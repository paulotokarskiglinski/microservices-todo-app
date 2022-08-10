using dotnet_todo_signalr.HubConfig;
using dotnet_todo_signalr.Models;
using dotnet_todo_signalr.Services;
using Microsoft.OpenApi.Models;
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
        Description = "TODO desenvolvido em .NET 6 com minimal API e MongoDB",
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
        Uri = new Uri(app.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>().ConnectionString)
    };
    IConnection connection = factory.CreateConnection();
    IModel channel = connection.CreateModel();

    #region Tarefa
    channel.QueueDeclare(queue: "put_tarefa", false, false, false, null);
    channel.QueueDeclare(queue: "post_tarefa", false, false, false, null);
    channel.QueueDeclare(queue: "delete_tarefa", false, false, false, null);

    EventingBasicConsumer putTarefaConsumer = new EventingBasicConsumer(channel);
    EventingBasicConsumer postTarefaConsumer = new EventingBasicConsumer(channel);
    EventingBasicConsumer deleteTarefaConsumer = new EventingBasicConsumer(channel);

    putTarefaConsumer.Received += (sender, e) => tarefaLogService.PutAsync(e, logService);
    postTarefaConsumer.Received += (sender, e) => tarefaLogService.PostAsync(e, logService);
    deleteTarefaConsumer.Received += (sender, e) => tarefaLogService.DeleteAsync(e, logService);

    channel.BasicConsume(queue: "put_tarefa", autoAck: true, consumer: putTarefaConsumer);
    channel.BasicConsume(queue: "post_tarefa", autoAck: true, consumer: postTarefaConsumer);
    channel.BasicConsume(queue: "delete_tarefa", autoAck: true, consumer: deleteTarefaConsumer);
    #endregion

    #region TipoTarefa
    channel.QueueDeclare(queue: "put_tipotarefa", false, false, false, null);
    channel.QueueDeclare(queue: "post_tipotarefa", false, false, false, null);
    channel.QueueDeclare(queue: "delete_tipotarefa", false, false, false, null);

    EventingBasicConsumer putTipoTarefaConsumer = new EventingBasicConsumer(channel);
    EventingBasicConsumer postTipoTarefaConsumer = new EventingBasicConsumer(channel);
    EventingBasicConsumer deleteTipoTarefaConsumer = new EventingBasicConsumer(channel);

    putTipoTarefaConsumer.Received += (sender, e) => tipoTarefaLogService.PutAsync(e, logService);
    postTipoTarefaConsumer.Received += (sender, e) => tipoTarefaLogService.PostAsync(e, logService);
    deleteTipoTarefaConsumer.Received += (sender, e) => tipoTarefaLogService.DeleteAsync(e, logService);

    channel.BasicConsume(queue: "put_tipotarefa", autoAck: true, consumer: putTipoTarefaConsumer);
    channel.BasicConsume(queue: "post_tipotarefa", autoAck: true, consumer: postTipoTarefaConsumer);
    channel.BasicConsume(queue: "delete_tipotarefa", autoAck: true, consumer: deleteTipoTarefaConsumer);
    #endregion

    #region Usuario
    channel.QueueDeclare(queue: "put_usuario", false, false, false, null);
    channel.QueueDeclare(queue: "post_usuario", false, false, false, null);
    channel.QueueDeclare(queue: "delete_usuario", false, false, false, null);

    EventingBasicConsumer putUsuarioConsumer = new EventingBasicConsumer(channel);
    EventingBasicConsumer postUsuarioConsumer = new EventingBasicConsumer(channel);
    EventingBasicConsumer deleteUsuarioConsumer = new EventingBasicConsumer(channel);

    putUsuarioConsumer.Received += (sender, e) => usuarioLogService.PutAsync(e, logService);
    postUsuarioConsumer.Received += (sender, e) => usuarioLogService.PostAsync(e, logService);
    deleteUsuarioConsumer.Received += (sender, e) => usuarioLogService.DeleteAsync(e, logService);

    channel.BasicConsume(queue: "put_usuario", autoAck: true, consumer: putUsuarioConsumer);
    channel.BasicConsume(queue: "post_usuario", autoAck: true, consumer: postUsuarioConsumer);
    channel.BasicConsume(queue: "delete_usuario", autoAck: true, consumer: deleteUsuarioConsumer);
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
