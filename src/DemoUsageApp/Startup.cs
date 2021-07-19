namespace DemoUsageApp
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class Startup
    {
        /// <summary> The Configuration. </summary>
        public IConfiguration Configuration { get; }

        /// <summary> The Logger. </summary>
        public ILogger<Startup> Logger { get; }

        /// <summary> Startup Ctor. </summary>
        /// <param name="configuration">The configuration <see cref="IConfiguration"/>.</param>
        /// <param name="logger">The logger <see cref="ILogger{Startup}"/></param>
        public Startup(
            IConfiguration configuration,
            ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services">The services <see cref="IServiceCollection"/>.</param>
        public void ConfigureServices(IServiceCollection services) => services.AddServices(Configuration, Logger);

        /// <summary> This method gets called by the runtime. Use this method to configure the HTTP request pipeline. </summary>
        /// <param name="app">The app <see cref="IApplicationBuilder"/>.</param>
        /// <param name="env">The env <see cref="IWebHostEnvironment"/>.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) => app.UseComponents(env, Configuration, Logger);
    }
}
