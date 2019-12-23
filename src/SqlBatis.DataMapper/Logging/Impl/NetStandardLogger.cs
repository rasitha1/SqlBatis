using System;
using Microsoft.Extensions.Logging;

namespace SqlBatis.DataMapper.Logging.Impl
{
    internal class NetStandardLogger : AbstractLogger
    {
        private readonly ILogger _logger;

        public NetStandardLogger(ILogger logger)
        {
            _logger = logger;
        }
        protected override void Write(LogLevel logLevel, object message, Exception e)
        {
            _logger.Log(MapLevel(logLevel), e, message?.ToString());
        }


        private static Microsoft.Extensions.Logging.LogLevel MapLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.All:
                    return Microsoft.Extensions.Logging.LogLevel.Trace;
                case LogLevel.Debug:
                    return Microsoft.Extensions.Logging.LogLevel.Debug;
                case LogLevel.Info:
                    return Microsoft.Extensions.Logging.LogLevel.Information;
                case LogLevel.Warn:
                    return Microsoft.Extensions.Logging.LogLevel.Warning;
                case LogLevel.Error:
                    return Microsoft.Extensions.Logging.LogLevel.Error;
                case LogLevel.Fatal:
                    return Microsoft.Extensions.Logging.LogLevel.Critical;
                default:
                    return Microsoft.Extensions.Logging.LogLevel.None;
            }
        }


        protected override bool IsLevelEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(MapLevel(logLevel));
        }
    }
}