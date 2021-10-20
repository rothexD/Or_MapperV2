using System;
using System.Data;
using ORMapper.Models;

namespace ORMapper.FluentQuery.IFluentSqlInterfaces
{
    public interface ITable
    {
        public IWhere Table(Type table);
        public IDbCommand getCommand { get; }
    }
}