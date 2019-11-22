using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using IBatisNet.DataMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            services.AddOptions();
            services.Configure(configureOptions);
            services.AddTransient<IDomSqlMapBuilder, DomSqlMapBuilder>();
            services.AddSingleton(sp =>
            {
                var builder = sp.GetRequiredService<IDomSqlMapBuilder>();
                var options = sp.GetRequiredService<IOptions<SqlMapperOptions>>();
                builder.Properties = Convert(options.Value.Parameters);
                return builder.Configure(options.Value.Resource);
            });

            return services;
        }

        private static NameValueCollection Convert(IDictionary<string, string> map)
        {
            var nvc = new NameValueCollection();
            foreach (var kvp in map)
            {
                nvc[kvp.Key] = kvp.Value;
            }

            return nvc;
        }
    }
}
