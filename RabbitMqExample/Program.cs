using System.Reflection;
using TopicFramework;
using TopicFramework.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTopicFrameWork(Assembly.GetEntryAssembly());
builder.Services.AddConnectionFactory(cf =>
{
    //Default Creddentials for rabbitmq
    cf.HostName = "localhost";
    cf.UserName = "guest";
    cf.Password = "guest";
});

TfRabbitmqService.HasMqtt = true;
builder.Services.AddTfRabbit();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.StartTfRabbit();

app.Run();
