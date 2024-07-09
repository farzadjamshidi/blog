using Blog.API.Extensions;
using Blog.API.Options;

namespace Blog.API.Registrars;

public class SwaggerRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
    }
}