
using Blog.API.Extensions;

namespace Blog.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.RegisterServices(typeof(Program));
        
        var app = builder.Build();
        
        app.RegisterPipelines(typeof(Program));
        
        app.Run();
    }
}