using Microsoft.Extensions.DependencyInjection;
using ReleaseRetention.Data.Services;
using ReleaseRetention.Services;

namespace ReleaseRetention.Extensions
{
    public static class StartupExtensions
    {
        /// <summary>
        /// Add the data entity services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<ReleaseDataService>();
            services.AddScoped<EnvironmentDataService>();
            services.AddScoped<ProjectDataService>();
            services.AddScoped<DeploymentDataService>();
            return services;
        }

        /// <summary>
        /// Add the release retention services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddReleaseRetention(this IServiceCollection services)
        {
            services.AddScoped<ReleaseRetentionService>();
            return services;
        }
    }
}
