using Blog.DAL;
using Blog.Domain.Aggregates.UserProfileAggregate;
using Microsoft.AspNetCore.Identity;
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
        
        builder.Services.AddIdentityCore<IdentityUser>(options =>
            {
                options. Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options. Password. RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddSignInManager()
            .AddEntityFrameworkStores<DataContext>();
    }
}