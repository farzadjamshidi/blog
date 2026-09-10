using System.Text;
using Blog.Application.Identity.Services;
using Blog.Application.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
namespace Blog.API.Registrars;

public class IdentityRegistrar: IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IdentityService>();
        var jwtSettings = new JwtSettings();
        builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);

        var jwtSection = builder.Configuration.GetSection(nameof(JwtSettings));
        builder.Services.Configure<JwtSettings>(jwtSection);
        builder.Services
            .AddAuthentication(a
                =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey
                        ?? throw new InvalidOperationException())),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudiences = jwtSettings.Audiences,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
                jwt.Audience = jwtSettings.Audiences?[0];
                jwt.ClaimsIssuer = jwtSettings.Issuer;
                jwt.Events = new JwtBearerEvents
                {
                    // Browsers can't set an Authorization header on a WebSocket
                    // handshake, so the SignalR client sends the token as an
                    // access_token query parameter instead — only for the hub's
                    // own path, every other endpoint still uses the header.
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        if (!string.IsNullOrEmpty(accessToken) &&
                            context.HttpContext.Request.Path.StartsWithSegments("/notification"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    }
}