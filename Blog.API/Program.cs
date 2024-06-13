using System.Reflection;
using Blog.API.Extensions;
using Blog.API.Hub;
using Blog.API.Middleware;
using Blog.API.Services;
using Blog.API.Setup;
using Blog.Domain;
using Blog.Domain.Repositories.Interfaces;
using Blog.Domain.Repositories.Postgre;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Blog.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger();
        builder.RegisterAuthentication();

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgre"));
        });

        builder.Services.AddDomainDIRegistration();

        builder.Services.AddSingleton<IdentityService>();
        
        builder.Services.AddAutoMapper(typeof(Program));
        
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
        });
        
        builder.Services.AddSignalR();

        builder.Services.AddSerilog();
        SerilogSetup.AddSerilog(builder.Configuration.GetSection("Logs").Get<LogSetupConfig>());
        builder.Host.UseSerilog();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors();
        
        app.UseStaticFiles();

        app.MapControllers();
        
        app.MapHub<MessageHub>("/notification");
        
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        app.Run();
    }
}