using Services;
using Services.Abstraction.Contracts;
using Services.Implementations;

namespace E_Commerce.API.Extensions
{
    public static class CoreServiceExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();

            services.AddAutoMapper(o => { }, typeof(AssemblyReference).Assembly);

            return services;
        }
    }
}
