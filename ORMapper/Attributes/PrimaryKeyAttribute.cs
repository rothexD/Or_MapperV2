using System;

namespace ORMapper.Attributes
{
    /// <summary>
    /// Defines a property is a primary key for table
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : ColumnAttribute
    {
        public PrimaryKeyAttribute()
        {
            Nullable = false;
        }
    }
}