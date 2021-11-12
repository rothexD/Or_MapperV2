using System;
using System.Data;

namespace ORMapper.extentions
{
    public static class Parameterhelper
    {
        /// <summary>
        /// Helps with filling a Command with parameters
        /// </summary>
        /// <param name="key">Key for IDataParameter</param>
        /// <param name="value">Value for IDataParameter</param>
        /// <param name="command">IDbCommand</param>
        public static void Help(this IDbCommand command,object key, object value)
        {
            IDataParameter p = command.CreateParameter();
            p.ParameterName = key.ToString()?? throw new ArgumentException("key was null when trying to add a parameter");
            p.Value = value ?? DBNull.Value;
            command.Parameters.Add(p);
        }
    }
}