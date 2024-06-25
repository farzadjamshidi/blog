using Blog.API.Extensions;
using Blog.API.Hubs;
using Blog.API.Middleware;
using Blog.API.Services;
using Blog.API.Setup;
using Blog.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Blog.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddSignalR();
 
        builder.Services.AddControllers();

        builder.Services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1,0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
            config.ApiVersionReader = new UrlSegmentApiVersionReader();
        });

        builder.Services.AddVersionedApiExplorer(config =>
        {
            config.GroupNameFormat = "'v'VVV";
            config.SubstituteApiVersionInUrl = true;
        });
        
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
                    policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true);
                });
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = "localhost:6379";
        });
        
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
        
        app.MapHub<MessageHub>("/notification");

        app.MapControllers();
        
        
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        app.Run();
    }
}