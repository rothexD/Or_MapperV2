using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace ORMapper.Logging
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