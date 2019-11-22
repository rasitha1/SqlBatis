using System;
using System.Collections.Generic;
using System.Text;
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
        /// <returns></returns>
        public static IServiceCollection AddSqlMapper(this IServiceCollection services, Action<SqlMapperOptions> configureOptions)
        {
            return services;
        }
    }

    public class SqlMapperOptions
    {
        public string Resource { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
