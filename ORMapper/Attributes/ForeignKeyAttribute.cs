using System;

namespace ORMapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : ColumnAttribute
    {
        public string RemoteTableName = null;
        public string RemoteColumnName = null;

    }
}
