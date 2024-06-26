using Blog.DAL;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Registrars;

public class DbRegistrar: IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgre"));
        });
    }
}