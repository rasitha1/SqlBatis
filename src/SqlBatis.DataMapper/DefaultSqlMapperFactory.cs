using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataMapper.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IBatisNet.DataMapper
{
    internal class DefaultSqlMapperFactory : ISqlMapperFactory
    {
        private readonly IServiceProvider _provider;
        private readonly ConcurrentDictionary<string, ISqlMapper> _mappers =
            new ConcurrentDictionary<string, ISqlMapper>();

        public DefaultSqlMapperFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public ISqlMapper GetMapper()
        {
            return GetMapper(Options.DefaultName);
        }

        public ISqlMapper GetMapper(string name)
        {
            return _mappers.GetOrAdd(name, n =>
            {
                var options = _provider.GetRequiredService<IOptionsMonitor<SqlMapperOptions>>().Get(n);
                if (!options.ConfigurationComplete)
                {
                    throw new OptionsValidationException(n, typeof(SqlMapperOptions), new List<string>
                    {
                        $"Did not find configured options for '{n}'. Register a named instance by calling " +
                        $"services.AddSqlMapper(\"{n}\", configureOptions)"
                    });
                }
                var builder = _provider.GetRequiredService<IDomSqlMapBuilder>();
                builder.Properties = ConvertParameters(options);
                return builder.Configure(options.Resource);
            });
        }

        private static NameValueCollection ConvertParameters(SqlMapperOptions options)
        {
            var nvc = new NameValueCollection();
            if (options.Parameters != null)
            {
                foreach (var kvp in options.Parameters)
                {
                    nvc[kvp.Key] = kvp.Value;
                }
            }

            return nvc;
        }
    }
}
