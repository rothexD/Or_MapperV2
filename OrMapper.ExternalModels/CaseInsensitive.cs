using System;

namespace OrMapper.ExternalModels
{
    /// <summary>
    /// simple Secure paramter class that takes a parameter and returns the value or the DBNUll.Value,goal is marking an object as secure for CustomQuery
    /// </summary>
    public class CaseInsensitive
    {
        public object Parameter { get; private set; }

        public CaseInsensitive(object obj)
        {
            if (obj is null)
            {
                throw new Exception("obj para was null");
            }
            Parameter = obj;
        }
        public static CaseInsensitive Create(object obj)
        {
            return new CaseInsensitive(obj);
        }
    }
}