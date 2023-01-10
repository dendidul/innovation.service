using Core.Application.Interfaces.Innovation;
using Infrastructure.ExternalService.Workflow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExternalService
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterExternalServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IInnovationWorkflow>(provider => provider.GetRequiredService<InnovationWorkflow>());
            return services;
        }
    }
}
