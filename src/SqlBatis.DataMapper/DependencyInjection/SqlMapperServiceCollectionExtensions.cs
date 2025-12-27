using System;
using SqlBatis.DataMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SqlBatis.DataMapper.Commands;
using SqlBatis.DataMapper.Configuration.ParameterMapping;
using SqlBatis.DataMapper.MappedStatements.ResultStrategy;
using SqlBatis.DataMapper.Scope;
using SqlBatis.DataMapper.TypeHandlers;
using SqlBatis.DataMapper.Utilities;

namespace SqlBatis.DataMapper.DependencyInjection
{
    /// <summary>
    /// Extension methods for setting up an <see cref="ISqlMapper"/> in the DI pipeline
    /// </summary>
    public static class SqlMapperServiceCollectionExtensions
    {
        /// <param name="services"></param>
        extension(IServiceCollection services)
        {
            /// <summary>
            /// Registers the default <see cref="ISqlMapper"/> in the DI pipeline
            /// </summary>
            /// <param name="configureOptions"></param>
            /// <returns></returns>
            public ISqlMapperBuilder AddSqlMapper(Action<SqlMapperOptions> configureOptions)
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
            /// <param name="name">Name of the mapper.</param>
            /// <param name="configureOptions"></param>
            /// <returns></returns>
            public ISqlMapperBuilder AddSqlMapper(string name, Action<SqlMapperOptions> configureOptions)
            {
                services.AddOptions();
                services.AddLogging();
                services.AddTransient<IDomSqlMapBuilder, DomSqlMapBuilder>();
                services.AddSingleton<ISqlMapperFactory, DefaultSqlMapperFactory>();

                services.AddTransient<ConfigurationScope>();
                services.AddTransient<PreparedCommandFactory>();
                services.AddTransient<ResultStrategyFactory>();
                services.AddTransient<TypeHandlerFactory>();
                services.AddTransient<DBHelperParameterCache>();
                services.AddTransient<ResultClassStrategy>();
                services.AddTransient<InlineParameterMapParser>();
            


                var builder = new DefaultSqlMapperBuilder(services, name);
                builder.Services.AddOptions<SqlMapperOptions>(name)
                    .Configure(configureOptions)
                    .PostConfigure(options => options.ConfigurationComplete = true)
                    .ValidateDataAnnotations();

                return builder;
            }

            /// <summary>
            /// Registers a type as a singleton in the DI pipeline that depends on the named <see cref="ISqlMapper"/>
            /// instance.
            /// </summary>
            /// <typeparam name="TService"></typeparam>
            /// <typeparam name="TImplementation"></typeparam>
            /// <param name="mapperName"></param>
            /// <returns></returns>
            public IServiceCollection AddSingletonWithNamedMapper<TService, TImplementation>(string mapperName)
                where TService : class
                where TImplementation : class, TService
            {
                services.AddSingleton<INamedMapperDependencyResolver, NamedMapperDependencyResolver>();
                return services.AddSingleton<TService>(provider => provider
                    .GetRequiredService<INamedMapperDependencyResolver>()
                    .GetInstance<TImplementation>(mapperName));
            }

            /// <summary>
            /// Registers a type as a transient in the DI pipeline that depends on the named <see cref="ISqlMapper"/>
            /// instance.
            /// </summary>        /// <typeparam name="TService"></typeparam>
            /// <typeparam name="TImplementation"></typeparam>
            /// <param name="mapperName"></param>
            /// <returns></returns>
            public IServiceCollection AddTransientWithNamedMapper<TService, TImplementation>(string mapperName)
                where TService : class
                where TImplementation : class, TService
            {
                services.AddSingleton<INamedMapperDependencyResolver, NamedMapperDependencyResolver>();
                return services.AddTransient<TService>(provider => provider
                    .GetRequiredService<INamedMapperDependencyResolver>()
                    .GetInstance<TImplementation>(mapperName));
            }

            /// <summary>
            /// Registers a type as a scoped in the DI pipeline that depends on the named <see cref="ISqlMapper"/>
            /// instance.
            /// </summary>        /// <typeparam name="TService"></typeparam>
            /// <typeparam name="TImplementation"></typeparam>
            /// <param name="mapperName"></param>
            /// <returns></returns>
            public IServiceCollection AddScopedWithNamedMapper<TService, TImplementation>(string mapperName)
                where TService : class
                where TImplementation : class, TService
            {
                services.AddSingleton<INamedMapperDependencyResolver, NamedMapperDependencyResolver>();
                return services.AddScoped<TService>(provider => provider
                    .GetRequiredService<INamedMapperDependencyResolver>()
                    .GetInstance<TImplementation>(mapperName));
            }
        }
    }
}
