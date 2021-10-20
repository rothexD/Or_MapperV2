using System;
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
    }
}