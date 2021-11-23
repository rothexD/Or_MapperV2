using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace OrMapper.Logging
{
    public static class CustomLoggerDependencyContainer
    {
        private static ILoggerFactory _myFactory = LoggerFactory.Create(x => x.AddConsole() /*.AddFilter(x => x == LogLevel.Debug )*/);
        private static Dictionary<string,ILogger> _storage = new ();
        public static ILogger GetLogger<T>()
        {
            if (_storage.ContainsKey(typeof(T).Name))
            {
                return _storage[typeof(T).Name];
            }
            return _storage[typeof(T).Name] = _myFactory.CreateLogger<T>();
        }
        public static ILogger GetLogger(string className)
        {
            if (_storage.ContainsKey(className))
            {
                return _storage[className];
            }
            return _storage[className] = _myFactory.CreateLogger(className);
        }
    }
}