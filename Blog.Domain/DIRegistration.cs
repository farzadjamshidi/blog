using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Domain;

public static class DIRegistration
{
    public static IServiceCollection AddDomainDIRegistration(this IServiceCollection services)
    {
        Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(a => a.Name.EndsWith("Repository") && !a.IsAbstract && !a.IsInterface)
            .Select(a => new { assignedType = a, serviceTypes = a.GetInterfaces().ToList() })
            .ToList()
            .ForEach(typesToRegister =>
            {
                typesToRegister.serviceTypes.ForEach(typeToRegister =>
                    {
                        services.AddScoped(typeToRegister, typesToRegister.assignedType);
                    });
            });

        return services;
    }   
}