using Blog.DAL;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.HostedServices;

public class PostStatsLoggerService(
    IServiceScopeFactory scopeFactory,
    ILogger<PostStatsLoggerService> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(10);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<DataContext>();

                var postCount = await ctx.Posts.CountAsync(stoppingToken);
                logger.LogInformation("Post stats: {PostCount} posts currently stored", postCount);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // A failed stats query shouldn't take down the whole API —
                // log and try again next interval instead of letting the
                // exception propagate out of ExecuteAsync (which would stop the host).
                logger.LogError(ex, "Failed to log post stats");
            }

            await Task.Delay(Interval, stoppingToken);
        }
    }
}
