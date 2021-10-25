﻿using System;

namespace ORMapper.Attributes
{
    /// <summary>
    /// defines a class is a table
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public string TableName = "";
    }
}