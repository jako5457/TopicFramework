using System.Reflection;
using TopicFramework;
using TopicFramework.Events;
using TopicFramework.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTfRabbitConnectionFactory(cf =>
{
    //Default Creddentials for rabbitmq
    cf.HostName = "localhost";
    cf.UserName = "guest";
    cf.Password = "guest";
});

builder.Services.AddTopicFrameWork(Assembly.GetEntryAssembly(), te =>
{
    te.Add(new TopicEvent("hi", msg => Console.WriteLine(msg.Payload)));
});

TfRabbitmqService.MqttMode = true;

if (builder.Environment.IsDevelopment())
{
    TfRabbitmqService.MessageDebug = true;
}

builder.Services.AddTfRabbit();

var app = builder.Build();

app.UseTopicFramework();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();
