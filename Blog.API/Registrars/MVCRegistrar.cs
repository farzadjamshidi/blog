using Blog.API.Extensions;
using Blog.API.Services;
using Blog.API.Setup;
using Blog.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Blog.API.Registrars;

public class MVCRegistrar: IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
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

        //builder.RegisterAuthentication();
        
        //builder.Services.AddDomainDIRegistration();

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
    }
}