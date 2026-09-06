using Blog.API.Hubs;
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

        // if (app.Environment.IsDevelopment())
        // {
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
        // }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors();
        
        app.UseStaticFiles();
        
        app.MapHub<MessageHub>("/notification");

        app.MapControllers();
        
        app.MapHealthChecks("/health");
    }
}