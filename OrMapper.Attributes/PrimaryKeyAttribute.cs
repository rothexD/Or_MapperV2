using System;

namespace OrMapper.Attributes
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