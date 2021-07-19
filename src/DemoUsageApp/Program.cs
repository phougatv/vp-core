namespace DemoUsageApp
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using System;

    public class Program
    {
        /// <summary> Main method, the entry-point of the application. </summary>
        /// <param name="args">The args <see cref="string[]"/></param>
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        /// <summary> Creates the host builder. </summary>
        /// <param name="args">The args <see cref="string[]"/></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseConfiguration(GetConfiguration());
                });

        #region Private Methods
        /// <summary> Gets the configuration. </summary>
        /// <returns></returns>
        private static IConfiguration GetConfiguration()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true);

            return builder.Build();
        }
        #endregion Private Methods
    }
}
