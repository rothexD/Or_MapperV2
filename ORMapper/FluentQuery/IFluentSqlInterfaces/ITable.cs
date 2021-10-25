using System;
using System.Data;

namespace ORMapper.FluentQuery.IFluentSqlInterfaces
{
    public interface ITable
    {
        public IDbCommand getCommand { get; }
        public IWhere Table(Type table);
    }
}