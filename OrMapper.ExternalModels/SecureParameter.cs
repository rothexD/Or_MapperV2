using System;

namespace OrMapper.ExternalModels
{
    /// <summary>
    /// Secure paramter class that takes a parameter and returns the value or the DBNUll.Value,goal is marking an object as secure for CustomQuery
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
        }

        public SecureParameter(object obj)
        {
            _Parameter = obj;
        }
        public static SecureParameter Create(object obj)
        {
            if (obj is SecureParameter)
            {
                throw new ArgumentException("obj is already secure");
            }
            if (obj is CaseInsensitive)
            {
                throw new ArgumentException("Caseinsensitive obj cant be secureparameter");
            }
            return new SecureParameter(obj);
        }
    }
}