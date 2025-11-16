using HelloHome.Central.Common.Mqtt;
using HelloHome.Central.Repository;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseLamar(registry =>
{
    registry.IncludeRegistry(new HelloHome.Central.Common.IoC.Registries.MqttRegistry(builder.Configuration));
    registry.IncludeRegistry<HelloHome.Central.Common.IoC.Registries.DbContextRegistry>();
    registry.IncludeRegistry<HelloHome.Central.Common.IoC.Registries.CommandAndQueriesRegistry>();
    registry.IncludeRegistry<HelloHome.Central.Common.IoC.Registries.BusinessLogicRegistry>();
    registry.IncludeRegistry<HelloHome.Central.Common.IoC.Registries.HandlerRegistry>();
});
builder.Services
    .AddHostedService<MqttHostedService>()
    .AddOpenApi()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddDbContext<HhDbContext>(bld =>
    {
        bld.UseMySql(builder.Configuration.GetConnectionString("local"), 
            new MariaDbServerVersion(new Version(10, 4, 11)));
    })
    .AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

app
    .UseSwagger()
    .UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HelloHomeCentral.API v1"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();