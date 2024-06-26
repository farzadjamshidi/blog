namespace Blog.API.Registrars;

public interface IWebApplicationRegistrar: IRegistrar
{
    void RegisterServices(WebApplication app);
}