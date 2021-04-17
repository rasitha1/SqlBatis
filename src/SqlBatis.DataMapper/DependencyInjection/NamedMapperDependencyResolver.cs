using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace SqlBatis.DataMapper.DependencyInjection
{
    /// <summary>
    /// Default <see cref="INamedMapperDependencyResolver"/>
    /// </summary>
    internal class NamedMapperDependencyResolver : INamedMapperDependencyResolver
    {
        private readonly IServiceProvider _provider;
        private readonly ISqlMapperFactory _mapperFactory;

        private readonly ConcurrentDictionary<Type, ObjectFactory> _factoryMap =
            new ConcurrentDictionary<Type, ObjectFactory>();

        public NamedMapperDependencyResolver(IServiceProvider provider, ISqlMapperFactory mapperFactory)
        {
            _provider = provider;
            _mapperFactory = mapperFactory;
        }
        public TImplementation GetInstance<TImplementation>(string name)
        {
            var mapper = _mapperFactory.GetMapper(name);
            if (!_factoryMap.TryGetValue(typeof(TImplementation), out var objectFactory))
            {
                objectFactory = ActivatorUtilities.CreateFactory(typeof(TImplementation), new[] { typeof(ISqlMapper) });
                _factoryMap.TryAdd(typeof(TImplementation), objectFactory);
            }

            return (TImplementation)objectFactory(_provider, new object[] { mapper });
        }
    }
}