using System;
using System.Data.Common;

namespace ORMapper.FluentSqlQueryApi
{
    /// <summary>
    /// simple Secure paramter class that takes a parameter and returns the value or the DBNUll.Value,goal is marking an object as secure for CustomQuery
    /// </summary>
    public class SecureParameter
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
            set
            {
                _Parameter = value;
            }
        }

        public SecureParameter(object obj)
        {
            _Parameter = obj;
        }
        public static SecureParameter Create(object obj)
        {
            return new SecureParameter(obj);
        }
    }
    
}