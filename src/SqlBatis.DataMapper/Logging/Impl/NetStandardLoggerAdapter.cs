using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace SqlBatis.DataMapper.Logging.Impl
{
    /// <summary>
    ///     Factory for creating <see cref="ILog" /> instances that write data using <see cref="ILoggerFactory" />.
    /// </summary>
    public class NetStandardLoggerAdapter : ILoggerFactoryAdapter
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IDictionary<string,NetStandardLogger> _cache = new ConcurrentDictionary<string, NetStandardLogger>();

        /// <summary>
        /// Instantiates a new instance of <see cref="NetStandardLoggerAdapter"/>
        /// </summary>
        /// <param name="loggerFactory"></param>
        public NetStandardLoggerAdapter(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        #region ILoggerFactoryAdapter Members

        /// <summary>
        ///     Get a ILog instance by <see cref="Type" />.
        /// </summary>
        /// <param name="type">Usually the <see cref="Type" /> of the current class.</param>
        /// <returns>An ILog instance that will write data to <see cref="Console.Out" />.</returns>
        public ILog GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }

        /// <summary>
        ///     Get a ILog instance by name.
        /// </summary>
        /// <param name="name">Usually a <see cref="Type" />'s Name or FullName property.</param>
        /// <returns>An ILog instance that will write data to <see cref="Console.Out" />.</returns>
        public ILog GetLogger(string name)
        {
            if (!_cache.TryGetValue(name, out var logger))
            {
                logger = new NetStandardLogger(_loggerFactory.CreateLogger(name));
                _cache[name] = logger;
            }

            return logger;
        }

        #endregion
    }
}