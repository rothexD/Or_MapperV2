using System;
using System.Data;

namespace ORMapper.FluentQuery
{
    public static class Parameterhelper
    {
        public static void ParaHelp(object key,object value,IDbCommand command)
        {
            IDataParameter p = command.CreateParameter();
            p.ParameterName = key.ToString();
            p.Value = value ?? DBNull.Value;
            command.Parameters.Add(p);
        }
    }
}