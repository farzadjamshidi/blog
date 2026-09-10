using Blog.API.Notifications;
using Blog.API.SignalR;
using Blog.Application.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Registrars;

public class NotificationsRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IUserIdProvider, UserProfileIdUserIdProvider>();
        builder.Services.AddSingleton<INotificationService, SignalRNotificationService>();
    }
}
