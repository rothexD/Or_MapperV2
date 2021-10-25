using System;
using System.Collections.Generic;
using ORMapper.Attributes;

namespace ShowcaseOrm.Models
{
    /// <summary>This is a teacher implementation (from School example).</summary>
    [Table(TableName = "TEACHERS")]
    public class Teacher: Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the teacher's salary.</summary>
        public int Salary { get; set; }


        [Column(ColumnName = "HDATE")]
        /// <summary>Gets or sets the teacher's hire date.</summary>
        public DateTime HireDate { get; set; }


        [ForeignKey(ColumnName = "KTEACHER")] 
        public List<Class> Classes { get; private set; } = new();
        
        [ForeignKey(ColumnName = "KTEACHER")] 
        public List<Course> Courses { get; private set; } = new();
    }
}