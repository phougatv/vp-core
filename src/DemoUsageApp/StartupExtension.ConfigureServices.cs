namespace DemoUsageApp
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary> StartupExntension class, adds services to the container. </summary>
    public static partial class StartupExtension
    {
        #region Internal Method
        /// <summary> Adds all the services required by the DemoUsageApp. </summary>
        /// <param name="services">The services, <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The configuration, <see cref="IConfiguration"/>.</param>
        /// <param name="logger">The logger, <see cref="ILogger{Startup}"/>.</param>
        /// <returns></returns>
        internal static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
        {
            services.AddDotNetCoreServices(configuration, logger);
            services.AddCrossCuttingServices(configuration, logger);

            return services;
        }
        #endregion Internal Method

        #region Private Methods
        /// <summary> Adds .NET Core services. </summary>
        /// <param name="services">The services, <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The configuration, <see cref="IConfiguration"/>.</param>
        /// <param name="logger">The logger, <see cref="ILogger{Startup}"/>.</param>
        /// <returns></returns>
        private static IServiceCollection AddDotNetCoreServices(this IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
        {
            logger.LogInformation("----  Adding .NET Core components  ----");

            services.AddMvc();
            services.AddControllers();

            logger.LogInformation("----  Successfully added .NET Core components  ----");

            return services;
        }

        /// <summary> Adds Cross-Cutting services. </summary>
        /// <param name="services">The services, <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The configuration, <see cref="IConfiguration"/>.</param>
        /// <param name="logger">The logger, <see cref="ILogger{Startup}"/>.</param>
        /// <returns></returns>
        private static IServiceCollection AddCrossCuttingServices(this IServiceCollection services, IConfiguration configuration, ILogger<Startup> logger)
        {
            logger.LogInformation("----  Adding Cross-Cutting components  ----");



            logger.LogInformation("----  Successfully added Cross-Cutting components  ----");

            return services;
        }
        #endregion Private Methods
    }
}
