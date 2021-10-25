using System;

namespace ORMapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : ColumnAttribute
    {
        public string MyReferenceToThisColumnName = null;
        public string TheirReferenceToThisColumnName = null;
        public Type RemoteTableName = null;
        public bool isManyToMany = false;
    }
}