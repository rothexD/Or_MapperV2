using System;

namespace ORMapper.extentions.IFluentSqlInterfaces
{
    public interface IFrom
    {
        public IJoinAndWhere From(string tableName);
    }
}