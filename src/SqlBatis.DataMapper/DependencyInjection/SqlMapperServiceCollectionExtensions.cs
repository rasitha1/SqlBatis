using System;
using SqlBatis.DataMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SqlBatis.DataMapper.TypeHandlers;
using SqlBatis.DataMapper.Utilities;

namespace SqlBatis.DataMapper.DependencyInjection
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
        public static ISqlMapperBuilder AddSqlMapper(this IServiceCollection services, Action<SqlMapperOptions> configureOptions)
        {
            var builder = services.AddSqlMapper(Options.DefaultName, configureOptions);
            // register default instance
            builder.Services.AddSingleton(sp => sp.GetRequiredService<ISqlMapperFactory>().GetMapper());
            return builder;
        }

        /// <summary>
        /// Registers a named instance of <see cref="ISqlMapper"/> in the DI pipeline. Use <see cref="ISqlMapperFactory"/>
        /// to retrieve a named instance
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name">Name of the mapper.</param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static ISqlMapperBuilder AddSqlMapper(this IServiceCollection services, string name, Action<SqlMapperOptions> configureOptions)
        {
            services.AddOptions();
            services.AddTransient<IDomSqlMapBuilder, DomSqlMapBuilder>();
            services.AddSingleton<ISqlMapperFactory, DefaultSqlMapperFactory>();
            services.AddTransient<TypeHandlerFactory>();
            services.AddTransient<DBHelperParameterCache>();

            var builder = new DefaultSqlMapperBuilder(services, name);
            builder.Services.AddOptions<SqlMapperOptions>(name)
                .Configure(configureOptions)
                .PostConfigure(options => options.ConfigurationComplete = true)
                .ValidateDataAnnotations();

            return builder;
        }
    }
}
