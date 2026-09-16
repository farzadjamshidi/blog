using Blog.API.Notifications;
using Blog.Application.Notifications;

namespace Blog.API.Registrars;

public class NotificationsRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<INotificationService, MessageBusNotificationService>();
    }
}
