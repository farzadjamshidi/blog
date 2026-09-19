using Blog.Application.Caching;

namespace Blog.API.Registrars;

public class CachingRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        // Real Redis when a connection string is configured (the Docker
        // Compose "redis" service sets Redis__ConnectionString) — in-process
        // otherwise, so plain local dev without Docker keeps needing no
        // external service. ICacheService/DistributedCacheService never
        // change either way — the whole point of building them behind that
        // abstraction in 4.4.
        var redisConnectionString = builder.Configuration["Redis:ConnectionString"];

        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });
        }
        else
        {
            builder.Services.AddDistributedMemoryCache();
        }

        builder.Services.AddSingleton<ICacheService, DistributedCacheService>();
    }
}
