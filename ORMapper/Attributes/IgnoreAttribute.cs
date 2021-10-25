using System;

namespace ORMapper.Attributes
{
    /// <summary>
    /// Defines a property should be ignored in Orm class
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
}