using System;

namespace ORMapper.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : ColumnAttribute
    {
        public PrimaryKeyAttribute()
        {
            Nullable = false;
        }
    }
}