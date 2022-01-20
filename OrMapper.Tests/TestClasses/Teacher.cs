using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using OrMapper.Attributes;
using ShowcaseOrm.Models;

namespace OrMapper.Tests.TestClasses
{
    /// <summary>This is a teacher implementation (from School example).</summary>
    [Table(TableName = "TEACHERS")]
    [ExcludeFromCodeCoverage]
    public class Teacher : Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the teacher's salary.</summary>
        public int Salary { get; set; }


        [Column(ColumnName = "HDATE")]
        /// <summary>Gets or sets the teacher's hire date.</summary>
        public DateTime HireDate { get; set; }


        [ForeignKeyOneToMany(ColumnName = "KTEACHER")] public List<ShowcaseOrm.Models.Class> Classes { get; set; } = new();

        [ForeignKeyOneToMany(ColumnName = "KTEACHER")] public List<Course> Courses { get; set; } = new();
    }
}