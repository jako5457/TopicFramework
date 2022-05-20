using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TopicFramework;
using TopicFramework.Events;
using MQTTnet;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TopicFramework.Mqtt;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Starting Mqtt broker.....");

IConfiguration config = new ConfigurationBuilder()
                        .AddEnvironmentVariables()
                        .AddJsonFile("appsettings.json")
                        .AddUserSecrets<Program>()
                        .Build();

MqttServiceSettings.DebugLog = true;

HostBuilder hostBuilder = new HostBuilder();

hostBuilder.ConfigureLogging((context, builder) => builder.AddConsole());

hostBuilder.ConfigureServices(services =>
{
    services.AddSingleton<MqttFactory>();
    services.AddTopicFrameWork(Assembly.GetEntryAssembly(), events =>
    {
        events.Add(new TopicEvent("Hello", msg => Console.WriteLine("Message: " + msg.Payload)));
    });

    services.AddTfMqttClientService(options =>
    {
        options.WithClientId("TopicFrameworkClient");
        options.WithTcpServer("test.mosquitto.org", 1883);
    });
});

var app = hostBuilder.Build();

app.UseTopicFramework();

app.Run();