using System;

namespace ORMapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName = null;
        public Type ColumnType = null;
        public bool Nullable = false;
    }
}