using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure;

public static class ServiceConfig
{
    public static void RegisterService(IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var serviceInterfaces = assembly.GetTypes()
            .Where(type => type.IsInterface &&
                           type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseService<,>)))
            .ToList();

        foreach (var serviceInterface in serviceInterfaces)
        {
            var serviceImplementation = assembly
                .GetTypes()
                .FirstOrDefault(type => type is { IsClass: true, IsAbstract: false } && serviceInterface.IsAssignableFrom(type));

            if (serviceImplementation is not null)
                services.AddScoped(serviceInterface, serviceImplementation);
        }
    }
}