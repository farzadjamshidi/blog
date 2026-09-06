using Blog.Application.Caching;

namespace Blog.API.Registrars;

public class CachingRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        // In-process IDistributedCache — no external service needed, fully
        // testable here. Swap to real Redis later with ONE line, no other
        // code changes anywhere:
        //   builder.Services.AddStackExchangeRedisCache(options =>
        //       options.Configuration = "localhost:6379");
        // (Microsoft.Extensions.Caching.StackExchangeRedis is already
        // referenced in this project.
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSingleton<ICacheService, DistributedCacheService>();
    }
}
