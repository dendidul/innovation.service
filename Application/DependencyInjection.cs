using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
