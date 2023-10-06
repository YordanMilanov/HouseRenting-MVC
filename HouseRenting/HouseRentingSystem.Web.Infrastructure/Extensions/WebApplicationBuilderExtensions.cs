using System.Reflection;
using HouseRentingSystem.Services;
using HouseRentingSystem.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HouseRentingSystem.Web.Infrastructure.Extensions
{
    /// <summary>
    /// This method registers all services with their interfaces and implementations of given assembly.
    /// the assembly is taken from the type of random service implementation provided.
    /// </summary>
    /// <param name="serviceType">Type of any service should be provided</param>
    /// <exception cref="InvalidOperationException"></exception>
    public class WebApplicationBuilderExtensions
    {
        public WebApplicationBuilderExtensions() { }
        public static void AddApplicationServices(IServiceCollection services, Type serviceType)
        {
            Assembly serviceAssembly = Assembly.GetAssembly(serviceType);


            if (serviceAssembly == null)
            {
                throw new InvalidOperationException("Invalid service type provided!");
            }
            Type[] serviceTypes = serviceAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();
            foreach (Type implementationType in serviceTypes)
            {
                Type? interfaceType = implementationType.GetInterface($"I{implementationType.Name}");
                if (interfaceType == null)
                {
                    throw new InvalidOperationException(
                        $"No interface is provided for the service with name: {implementationType.Name}");
                }

                services.AddScoped(interfaceType, implementationType);
            }
            services.AddScoped<IHouseService, HouseService>();
        }
    }
}
