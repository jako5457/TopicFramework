using TopicFramework;
using TopicFramework.Mqtt;
using TopicFramework.Mqtt.Broker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTopicFrameWork();
builder.Services.AddMqttBroker();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.UseMqttBroker();

app.Run();
