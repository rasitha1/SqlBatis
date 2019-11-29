using System;
using IBatisNet.DataMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IBatisNet.DataMapper.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up an <see cref="ISqlMapper"/> in the DI pipeline
    /// </summary>
    public static class SqlMapperServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the default <see cref="ISqlMapper"/> in the DI pipeline
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlMapper(this IServiceCollection services, Action<SqlMapperOptions> configureOptions)
        {
            services.AddCommon();
            services.AddOptions<SqlMapperOptions>()
                .Configure(configureOptions)
                .PostConfigure(options => options.ConfigurationComplete=true)
                .ValidateDataAnnotations();
            // register default instance
            services.AddSingleton(sp => sp.GetRequiredService<ISqlMapperFactory>().GetMapper());
            return services;
        }

        /// <summary>
        /// Registers a named instance of <see cref="ISqlMapper"/> in the DI pipeline. Use <see cref="ISqlMapperFactory"/>
        /// to retrieve a named instance
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name">Name of the mapper.</param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlMapper(this IServiceCollection services, string name, Action<SqlMapperOptions> configureOptions)
        {
            services.AddCommon();
            services.AddOptions<SqlMapperOptions>(name)
                .Configure(configureOptions)
                .PostConfigure(options => options.ConfigurationComplete = true)
                .ValidateDataAnnotations();
            return services;
        }

        private static void AddCommon(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddTransient<IDomSqlMapBuilder, DomSqlMapBuilder>();
            services.AddSingleton<ISqlMapperFactory, DefaultSqlMapperFactory>();
        }
    }
}
