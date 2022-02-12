# TopicFramework.RabbitMq

This is a extension to Topicframework that implements the AMQ protocol using the official RabbitMq Library.

## Contents

- Setup in Program.cs
- Configurations


## Setup in Program.cs

you have the control to setup the rabbitMq ConnectionFactory as you like.

```C#
builder.Services.AddConnectionFactory(cf =>
{
    //Default Creddentials for rabbitmq
    cf.HostName = "localhost";
    cf.UserName = "guest";
    cf.Password = "guest";
});
```

For Setting it up with TopicFramework you use

```C#
builder.Services.AddTfRabbit();
```

## Configurations 

If your RabbitMq server uses the Mqtt extension you can activate the client to join the topic exchange by using:

```C#
TfRabbitmqService.HasMqtt = true;
```