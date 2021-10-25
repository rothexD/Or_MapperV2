﻿using ORMapper.Attributes;

namespace ShowcaseOrm.Models
{
    /// <summary>This is a student implementation (from School example).</summary>
    [Table(TableName = "STUDENTS")]
    public class Student: Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the student's grade.</summary>
        public int Grade { get; set; }
    }
}