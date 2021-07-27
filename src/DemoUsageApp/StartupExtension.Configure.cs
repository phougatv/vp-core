namespace DemoUsageApp
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    /// <summary> StartupExtension class, configures the HTTP request pipeline. </summary>
    public static partial class StartupExtension
    {
        #region Internal Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app">The app, <see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env, <see cref="IWebHostEnvironment"/>.</param>
        /// <param name="configuration">The configuration, <see cref="IConfiguration"/>.</param>
        /// <param name="logger">The logger, <see cref="ILogger{Startup}"/>.</param>
        /// <returns></returns>
        internal static IApplicationBuilder UseComponents(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCustomComponents(env, configuration, logger);
            app.UseDotNetCore(env, configuration, logger);

            return app;
        }
        #endregion Internal Methods

        #region Private Methods
        /// <summary> Configures Custom-Components. </summary>
        /// <param name="app">The app, <see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env, <see cref="IWebHostEnvironment"/>.</param>
        /// <param name="configuration">The configuration, <see cref="IConfiguration"/>.</param>
        /// <param name="logger">The logger, <see cref="ILogger{Startup}"/>.</param>
        /// <returns></returns>
        private static IApplicationBuilder UseCustomComponents(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, ILogger logger)
        {
            logger.LogInformation("CUSTOM-COMPONENTS: Configuring...");
            logger.LogInformation("CUSTOM-COMPONENTS: Successfully configured.");

            return app;
        }

        /// <summary> Configures .NET Core. </summary>
        /// <param name="app">The app, <see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env, <see cref="IWebHostEnvironment"/>.</param>
        /// <param name="configuration">The configuration, <see cref="IConfiguration"/>.</param>
        /// <param name="logger">The logger, <see cref="ILogger{Startup}"/>.</param>
        /// <returns></returns>
        private static IApplicationBuilder UseDotNetCore(this IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, ILogger logger)
        {
            logger.LogInformation(".NET CORE: Configuring...");

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            logger.LogInformation(".NET CORE: Successfully configured.");

            return app;
        }
        #endregion Private Methods
    }
}
