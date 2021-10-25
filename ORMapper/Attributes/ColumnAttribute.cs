using System;

namespace ORMapper.Attributes
{
    /// <summary>
    /// Defines a property is a coloumn
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string ColumnName = null;
        public Type ColumnType = null;
        public bool Nullable = false;
    }
}