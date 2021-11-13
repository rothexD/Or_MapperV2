using System;
using System.Data.Common;

namespace ORMapper.FluentSqlQueryApi
{
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