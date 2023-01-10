
using Core.Application.Interfaces.Innovation;
using AHM.Common.Helper.Config;
using AHM.Common.Helper.Encryption;
using Infrastructure.Persistence.Contexts.Innovation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class DependencyInjection
    {

        public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ItwfsInnovationContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("ITWFSConnectionString")));

            services.AddDbContext<itb2eInnovationContext>(options =>
               options.UseSqlServer(
                   configuration.GetConnectionString("B2EConnectionString")));

            services.AddDbContext<HrirsInnovationContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AHMIRSDVConnectionString")));


            services.AddDbContext<HRTxnInnovationContext>(options =>
               options.UseOracle(
                   configuration.GetConnectionString("HrtxnConnectionString")));

            services.AddScoped<IConfigCreator, ConfigCreator>();
            services.AddScoped<IEncryptionHelper, EncryptionHelper>();
            services.AddScoped<IHRTxnInnovationContext>(provider => provider.GetRequiredService<HRTxnInnovationContext>());
            services.AddScoped<IHrirsInnovationContext>(provider => provider.GetRequiredService<HrirsInnovationContext>());
            services.AddScoped<Iitb2eInnovationContext>(provider => provider.GetRequiredService<itb2eInnovationContext>());
            services.AddScoped<IItwfsInnovationContext>(provider => provider.GetRequiredService<ItwfsInnovationContext>());

            return services;
        }
    }
}
