using Microsoft.OpenApi;
using dotnet_tipotarefa_api.Models;
using dotnet_tipotarefa_api.Services;
using RabbitMQ.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<TarefaDatabaseSettings>(builder.Configuration.GetSection("TodoDatabase"));
builder.Services.AddSingleton<TipoTarefaService>();

builder.Services.AddScoped<ITipoTarefaService, TipoTarefaService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = ".NET TODO API",
        Description = "API de Tipo de Tarefa desenvolvido em .NET 10 com minimal API e MongoDB",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET Tipo de Tarefa API v1");
});

#region RabbitMQ Producer
ConnectionFactory factory = new ConnectionFactory()
{
    Uri = new Uri(app.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>()!.ConnectionString)
};
IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

foreach (var queue in new[] { "post_tipotarefa", "put_tipotarefa", "delete_tipotarefa" })
{
    await channel.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
}

async Task Publish<T>(string queue, T message)
{
    var body = JsonSerializer.SerializeToUtf8Bytes(message);
    await channel.BasicPublishAsync(exchange: "", routingKey: queue, body: body);
}
#endregion

app.MapGet("/tipotarefa", async (ITipoTarefaService service) => await service.GetAsync());

app.MapGet("/tipotarefa/{id}", async (ITipoTarefaService service, string id) =>
{
    var item = await service.GetAsync(id);

    if (item == null)
        return Results.NotFound();

    return Results.Ok(item);
});

app.MapPost("/tipotarefa", async (ITipoTarefaService service, TipoTarefa item) =>
{
    await service.CreateAsync(item);

    await Publish("post_tipotarefa", item);

    return Results.Created($"/tipotarefa/{item.Id}", item);
});

app.MapPut("/tipotarefa", async (ITipoTarefaService service, TipoTarefa item) =>
{
    var result = await service.GetAsync(item.Id);

    if (result == null)
        return Results.NotFound();

    await service.UpdateAsync(item.Id, result);

    await Publish("put_tipotarefa", item);

    return Results.Created($"/tipotarefa/{item.Id}", result);
});

app.MapDelete("/tipotarefa/{id}", async (ITipoTarefaService service, string id) =>
{
    var item = await service.GetAsync(id);

    if (item == null)
        return Results.NotFound();

    await service.RemoveAsync(id);

    await Publish("delete_tipotarefa", id);

    return Results.Ok(id);
});

app.Run();
