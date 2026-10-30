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

// Ocelot is terminal middleware and handles every request that reaches it,
// so the root path must be answered before it (an endpoint mapped with MapGet would never run).
app.MapWhen(
    context => context.Request.Path == "/",
    root => root.Run(context => context.Response.WriteAsync("API Gateway")));

await app.UseOcelot();

app.Run();
