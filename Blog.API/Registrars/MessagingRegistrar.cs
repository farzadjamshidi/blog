using MassTransit;

namespace Blog.API.Registrars;

public class MessagingRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        // Real RabbitMQ when a host is configured (the Docker Compose
        // "rabbitmq" service sets RabbitMQ__Host) — MassTransit's
        // in-memory transport otherwise, so plain local dev without
        // Docker still starts without an external dependency. The
        // published message just has nowhere real to go in that case —
        // no local consumer exists outside Docker Compose.
        var rabbitMqHost = builder.Configuration["RabbitMQ:Host"];

        builder.Services.AddMassTransit(x =>
        {
            if (!string.IsNullOrEmpty(rabbitMqHost))
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqHost);
                    cfg.ConfigureEndpoints(context);
                });
            }
            else
            {
                x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
            }
        });
    }
}
