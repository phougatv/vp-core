namespace VP.Core.DataAccess.Sql.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data;
    using VP.Core.DataAccess.Sql.Accessors;
    using VP.Core.DataAccess.Sql.Executioner;
    using VP.Core.DataAccess.Sql.Persistence;

    /// <summary>
    /// SqlExtension class.
    /// Provides an extension method (AddSql) to be used in ConfigureServices method of you application's Startup class.
    /// </summary>
    public static class SqlExtension
    {
        public static IServiceCollection AddSql(this IServiceCollection services, IConfiguration configuration, ILogger logger)
        {
            logger.LogInformation("----  Adding SQL data-access...  ----");

            var sqlConfiguration = configuration.GetSection("Shared:DataAccess:Sql").Get<SqlConfiguration>(binderOptions => { binderOptions.BindNonPublicProperties = true; });
            services.AddSingleton(sqlConfiguration);
            services.AddSingleton<IConnectionStringAccessor, ConnectionStringAccessor>();

            services.AddScoped<Queue<SqlCommandDetail>>();
            services.AddScoped<IExecutioner, SqlExecutioner>();
            services.AddScoped<IPersistent, SqlPersistent>();
            services.AddScoped<IDbConnection, SqlConnection>();

            logger.LogInformation("----  Successfully added SQL data-access.  ----");

            return services;
        }
    }
}
