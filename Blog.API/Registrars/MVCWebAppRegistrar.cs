using Blog.API.Middleware;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Blog.API.Registrars;

public class MVCWebAppRegistrar: IWebApplicationRegistrar
{
    public void RegisterServices(WebApplication app)
    {
        // Registered first so it wraps every middleware/endpoint after it —
        // previously registered last, after all Map*() calls, which meant it
        // never ran for requests that matched an endpoint (see learning-notes/notes/14-middlewares-and-filters.md).
        app.UseMiddleware<ErrorHandlingMiddleware>();

        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.ApiVersion.ToString());
                }
            });
        }

        // Only redirect to HTTPS when an HTTPS endpoint is actually configured.
        // Docker Compose runs ASPNETCORE_URLS=http://+:8080 only (no cert, no
        // HTTPS port at all) — redirecting there sent every request to a port
        // nothing was listening on, which looked like the container itself was
        // unreachable (see learning-notes/notes/39-docker-containerization.md).
        var urls = app.Configuration["ASPNETCORE_URLS"];

        if (string.IsNullOrEmpty(urls) || urls.Contains("https", StringComparison.OrdinalIgnoreCase))
        {
            app.UseHttpsRedirection();
        }

        app.UseAuthorization();

        app.UseCors();
        
        app.UseStaticFiles();

        app.MapControllers();
        
        app.MapHealthChecks("/health");
    }
}