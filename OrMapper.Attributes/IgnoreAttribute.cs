using System;

namespace OrMapper.Attributes
{
    /// <summary>
    /// Defines a property should be ignored in Orm class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
}