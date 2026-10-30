using Microsoft.OpenApi;
using dotnet_usuario_api.Models;
using dotnet_usuario_api.Services;
using RabbitMQ.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<UsuarioDatabaseSettings>(builder.Configuration.GetSection("TodoDatabase"));
builder.Services.AddSingleton<UsuarioService>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = ".NET TODO API",
        Description = "API de Usuário desenvolvido em .NET 10 com minimal API e MongoDB",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET Usuário API v1");
});

#region RabbitMQ Producer
ConnectionFactory factory = new ConnectionFactory()
{
    Uri = new Uri(app.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>()!.ConnectionString)
};
IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

foreach (var queue in new[] { "post_usuario", "put_usuario", "delete_usuario" })
{
    await channel.QueueDeclareAsync(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
}

async Task Publish<T>(string queue, T message)
{
    var body = JsonSerializer.SerializeToUtf8Bytes(message);
    await channel.BasicPublishAsync(exchange: "", routingKey: queue, body: body);
}
#endregion

app.MapGet("/usuario", async (IUsuarioService service) => await service.GetAsync());

app.MapGet("/usuario/{id}", async (IUsuarioService service, string id) =>
{
    var item = await service.GetAsync(id);

    if (item == null)
        return Results.NotFound();

    return Results.Ok(item);
});

app.MapPost("/usuario", async (IUsuarioService service, Usuario item) =>
{
    await service.CreateAsync(item);

    await Publish("post_usuario", item);

    return Results.Created($"/usuario/{item.Id}", item);
});

app.MapPut("/usuario", async (IUsuarioService service, Usuario item) =>
{
    var result = await service.GetAsync(item.Id);

    if (result == null)
        return Results.NotFound();

    await service.UpdateAsync(item.Id, result);

    await Publish("put_usuario", item);

    return Results.Created($"/usuario/{item.Id}", result);
});

app.MapDelete("/usuario/{id}", async (IUsuarioService service, string id) =>
{
    var item = await service.GetAsync(id);

    if (item == null)
        return Results.NotFound();

    await service.RemoveAsync(id);

    await Publish("delete_usuario", id);

    return Results.Ok(id);
});

app.Run();
