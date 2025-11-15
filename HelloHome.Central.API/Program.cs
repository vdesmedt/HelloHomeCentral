using HelloHome.Central.Common.Mqtt;
using HelloHome.Central.Repository;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLamar(registry =>
{
    registry.IncludeRegistry(new HelloHome.Central.Common.IoC.Registries.MqttRegistry(builder.Configuration));
    registry.IncludeRegistry<HelloHome.Central.Common.IoC.Registries.DbContextRegistry>();
    registry.IncludeRegistry<HelloHome.Central.Common.IoC.Registries.CommandAndQueriesRegistry>();
    registry.IncludeRegistry<HelloHome.Central.Common.IoC.Registries.BusinessLogicRegistry>();
    registry.IncludeRegistry<HelloHome.Central.Common.IoC.Registries.HandlerRegistry>();
});
builder.Services.AddHostedService<MqttHostedService>();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<HhDbContext>(bld =>
{
    bld.UseMySql(builder.Configuration.GetConnectionString("local"), 
        new MariaDbServerVersion(new Version(10, 4, 11)));
});
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });


var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/wf", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");
app.MapGet("/api/hello", () => new { message = "Hello from ASP.NET Core 👋" });

var todos = new List<Todo>
{
    new(1, "Learn DI", false),
    new(2, "Call Minimal API", true)
};

app.MapGet("/api/todos", () => todos);
app.MapPost("/api/todos", (Todo t) =>
{
    var nextId = todos.Count == 0 ? 1 : todos.Max(x => x.Id) + 1;
    var nt = t with { Id = nextId };
    todos.Add(nt);
    return Results.Created($"/api/todos/{nt.Id}", nt);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
record Todo(int Id, string Title, bool Done);