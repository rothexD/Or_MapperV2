using System;

namespace OrMapper.ExternalModels
{
    /// <summary>
    /// Secure paramter class that takes a parameter and returns the value or the DBNUll.Value,goal is marking an object as secure for CustomQuery
    /// </summary>
    public class CaseInsensitive
    {
        private object _Parameter { get; set; }
        public object Parameter
        {
            get
            {
                if (_Parameter is null)
                {
                    return DBNull.Value;
                }

                return _Parameter;
            }
        }
        public CaseInsensitive(object obj)
        {
            _Parameter = obj;
        }
        public static CaseInsensitive Create(object obj)
        {
            if (obj is CaseInsensitive)
            {
                throw new ArgumentException("already case insensitive");
            }
            return new CaseInsensitive(obj);
        }
    }
}