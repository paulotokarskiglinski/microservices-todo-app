using Ocelot.Middleware;
using Ocelot.DependencyInjection;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("ocelot.json")
                            .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
    builder => builder
        .WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

builder.Services.AddOcelot(configuration);

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseOcelot().Wait();

app.MapGet("/", () => "API Gateway");

app.Run();
