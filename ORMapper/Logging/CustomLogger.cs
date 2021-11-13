using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace ORMapper.Logging
{
    public static class CustomLogger
    {
        private static ILoggerFactory myFactory = LoggerFactory.Create(x => x.AddConsole() /*.AddFilter(x => x == LogLevel.Debug )*/);

        public static ILogger GetLogger<T>()
        {
            return myFactory.CreateLogger<T>();
        }
        public static ILogger GetLogger(string className)
        {
            return myFactory.CreateLogger(className);
        }
    }
}