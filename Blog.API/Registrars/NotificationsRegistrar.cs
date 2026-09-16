using Blog.API.Notifications;
using Blog.Application.Notifications;

namespace Blog.API.Registrars;

public class NotificationsRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        // Scoped, not Singleton — MassTransit's IPublishEndpoint is
        // registered Scoped, and a Singleton can't consume a Scoped
        // dependency (caught by ASP.NET Core's DI scope validation, which
        // only runs in Development — the Docker/Release build never
        // exercised this path).
        builder.Services.AddScoped<INotificationService, MessageBusNotificationService>();
    }
}
