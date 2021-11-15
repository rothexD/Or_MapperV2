using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace OrMapper.Logging
{
    public static class CustomLoggerDependencyContainer
    {
        private static ILoggerFactory myFactory = LoggerFactory.Create(x => x.AddConsole() /*.AddFilter(x => x == LogLevel.Debug )*/);
        private static Dictionary<string,ILogger> storage = new ();
        public static ILogger GetLogger<T>()
        {
            if (storage.ContainsKey(typeof(T).Name))
            {
                return storage[typeof(T).Name];
            }
            return storage[typeof(T).Name] = myFactory.CreateLogger<T>();
        }
        public static ILogger GetLogger(string className)
        {
            if (storage.ContainsKey(className))
            {
                return storage[className];
            }
            return storage[className] = myFactory.CreateLogger(className);
        }
    }
}