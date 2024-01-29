using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HouseRentingSystem.Web.Infrastructure.Extensions
{
	public static class WebApplicationBuilderExtension
	{
		/// <summary>
		/// This method register all services with their interfaces and implementation of given assembly.
		/// The assembly is taken from the type of random service implementation provided.
		/// </summary>
		/// <param name="serviceType">Type of random service implementation.</param>
		/// <exception cref="InvalidOperationException"></exception>
		public static void AddApplicationServices(this IServiceCollection services, Type serviceType)
		{
			Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);

			if (serviceAssembly == null)
			{
				throw new InvalidOperationException("Invalid service type provider!");
			}

			Type[] serviceTypes = serviceAssembly.GetTypes().Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
				.ToArray();
			foreach (var type in serviceTypes)
			{
				Type? interfaceType = type.GetInterface($"I{type.Name}");

				if (interfaceType == null)
				{
					throw new InvalidOperationException($"No interface is provided for the service with name {type.Name}");
				}

				services.AddScoped(interfaceType, type);
			}
		}
	}
}
