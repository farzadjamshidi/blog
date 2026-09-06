using Blog.API.HostedServices;

namespace Blog.API.Registrars;

public class HostedServicesRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<PostStatsLoggerService>();
    }
}
