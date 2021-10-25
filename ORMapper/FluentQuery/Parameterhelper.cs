using System;
using System.Data;

namespace ORMapper.FluentQuery
{
    public static class Parameterhelper
    {
        /// <summary>
        /// Helps with filling a Command with parameters
        /// </summary>
        /// <param name="key">Key for IDataParameter</param>
        /// <param name="value">Value for IDataParameter</param>
        /// <param name="command">IDbCommand</param>
        public static void ParaHelp(object key, object value, IDbCommand command)
        {
            IDataParameter p = command.CreateParameter();
            p.ParameterName = key.ToString();
            p.Value = value ?? DBNull.Value;
            command.Parameters.Add(p);
        }
    }
}