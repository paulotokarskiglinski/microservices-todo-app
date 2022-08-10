using Microsoft.OpenApi.Models;
using dotnet_usuario_api.Models;
using dotnet_usuario_api.Services;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

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
        Description = "API de Usuário desenvolvido em .NET 6 com minimal API e MongoDB",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET Usuário API v1");
});

ConnectionFactory factory = new ConnectionFactory()
{
    Uri = new Uri(app.Configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>().ConnectionString)
};
IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

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

    #region RabbitMQ Producer
    channel.QueueDeclare(queue: "post_usuario",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(item));

    channel.BasicPublish(exchange: "",
                         routingKey: "post_usuario",
                         basicProperties: null,
                         body: body);
    #endregion

    return Results.Created($"/usuario/{item.Id}", item);
});

app.MapPut("/usuario", async (IUsuarioService service, Usuario item) =>
{
    var result = await service.GetAsync(item.Id);

    if (result == null)
        return Results.NotFound();

    await service.UpdateAsync(item.Id, result);

    #region RabbitMQ Producer
    channel.QueueDeclare(queue: "put_usuario",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(item));

    channel.BasicPublish(exchange: "",
                         routingKey: "put_usuario",
                         basicProperties: null,
                         body: body);
    #endregion

    return Results.Created($"/usuario/{item.Id}", result);
});

app.MapDelete("/usuario/{id}", async (IUsuarioService service, string id) =>
{
    var item = await service.GetAsync(id);

    if (item == null)
        return Results.NotFound();

    await service.RemoveAsync(id);

    #region RabbitMQ Producer
    channel.QueueDeclare(queue: "delete_usuario",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(id));

    channel.BasicPublish(exchange: "",
                         routingKey: "delete_usuario",
                         basicProperties: null,
                         body: body);
    #endregion

    return Results.Ok(id);
});

app.Run();
