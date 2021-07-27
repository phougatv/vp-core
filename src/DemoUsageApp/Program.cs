namespace DemoUsageApp
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;

    public class Program
    {
        /// <summary> Main method, the entry-point of the application. </summary>
        /// <param name="args">The args <see cref="string[]"/></param>
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            var loggerFactory = GetLoggerFactory();
            var logger = loggerFactory.CreateLogger<Program>();
            try
            {
                logger.LogInformation("Starting Host...");
                CreateHostBuilder(args, configuration).Build().Run();
            }
            catch (Exception ex)
            {
                logger.LogCritical("Host terminated unexpectedly! See following error message.");
#if DEBUG
                logger.LogError($"Error message: {ex.Message}");
#endif
            }
            finally
            {
                logger.LogInformation("Disposing LoggerFactory...");
                loggerFactory.Dispose();
            }
        }

        /// <summary> Creates the host builder. </summary>
        /// <param name="args">The args <see cref="string[]"/></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.AddConsole();
                    //loggingBuilder.AddDebug();
                    //loggingBuilder.AddEventLog();
                    //loggingBuilder.AddEventSourceLogger();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(configuration);
                    webBuilder.UseStartup(context =>
                    {
                        return new Startup(
                            configuration,
                            context.HostingEnvironment,
                            GetLoggerFactory().CreateLogger<Startup>());
                    });
                });

        #region Private Methods
        /// <summary> Gets the configuration. </summary>
        /// <returns></returns>
        private static IConfiguration GetConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .Build();
        }

        /// <summary>
        /// Gets the logger factory.
        /// </summary>
        /// <returns></returns>
        private static ILoggerFactory GetLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                //builder.AddDebug();
                //builder.AddEventLog();
                //builder.AddEventSourceLogger();
            });
        }
        #endregion Private Methods
    }
}
