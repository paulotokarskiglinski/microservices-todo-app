using Microsoft.OpenApi;
using dotnet_todo_api.Models;
using dotnet_todo_api.Services;
using RabbitMQ.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<TarefaDatabaseSettings>(builder.Configuration.GetSection("TodoDatabase"));
builder.Services.AddSingleton<TarefaService>();
builder.Services.AddSingleton<UsuarioService>();
builder.Services.AddSingleton<TipoTarefaService>();

builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITipoTarefaService, TipoTarefaService>();

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

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET TODO API v1");
});

#region RabbitMQ Producer
ConnectionFactory factory = new ConnectionFactory()
{
    Uri = new Uri(app.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>()!.ConnectionString)
};
IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

foreach (var queue in new[] { "post_tarefa", "put_tarefa", "delete_tarefa" })
{
    await channel.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
}

async Task Publish<T>(string queue, T message)
{
    var body = JsonSerializer.SerializeToUtf8Bytes(message);
    await channel.BasicPublishAsync(exchange: "", routingKey: queue, body: body);
}
#endregion

app.MapGet("/tarefa", async (ITarefaService tarefaService, IUsuarioService usuarioService, ITipoTarefaService tipoTarefaService) =>
{
    var items = await tarefaService.GetAsync();

    var usuarios = await usuarioService.GetAsync();

    var tipoTarefas = await tipoTarefaService.GetAsync();

    foreach (var item in items)
    {
        item.Usuario = usuarios.FirstOrDefault(x => x.Id == item.IdUsuario);

        item.TipoTarefa = tipoTarefas.FirstOrDefault(x => x.Id == item.IdTipoTarefa);
    }

    return items;
});

app.MapGet("/tarefa/{id}", async (ITarefaService tarefaService, IUsuarioService usuarioService, string id) =>
{
    var item = await tarefaService.GetAsync(id);

    if (item == null)
        return Results.NotFound();

    var usuario = await usuarioService.GetAsync(item.IdUsuario);

    if (usuario == null)
        return Results.NotFound();

    item.Usuario = usuario;

    return Results.Ok(item);
});

app.MapPost("/tarefa", async (ITarefaService tarefaService, Tarefa item) =>
{
    await tarefaService.CreateAsync(item);

    await Publish("post_tarefa", item);

    return Results.Created($"/tarefa/{item.Id}", item);
});

app.MapPut("/tarefa", async (ITarefaService tarefaService, Tarefa tarefa) =>
{
    var item = await tarefaService.GetAsync(tarefa.Id);

    if (item == null)
        return Results.NotFound();

    item.Descricao = tarefa.Descricao;
    item.Feito = tarefa.Feito;

    if (item.Feito)
        item.DataTarefa = DateTime.Now;

    await tarefaService.UpdateAsync(tarefa.Id, item);

    await Publish("put_tarefa", item);

    return Results.Created($"/tarefa/{item.Id}", item);
});

app.MapDelete("/tarefa/{id}", async (ITarefaService tarefaService, string id) =>
{
    var item = await tarefaService.GetAsync(id);

    if (item == null)
        return Results.NotFound();

    await tarefaService.RemoveAsync(id);

    await Publish("delete_tarefa", id);

    return Results.Ok(id);
});

app.Run();
