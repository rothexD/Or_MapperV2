using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using OrMapper.Attributes;

namespace ShowcaseOrm.Models
{
    /// <summary>This is a student implementation (from School example).</summary>
    [Table(TableName = "STUDENTS")]
    [ExcludeFromCodeCoverage]
    public class Student : Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the student's grade.</summary>
        public int Grade { get; set; }
        
        [ForeignKeyManyToMany(RemoteTableName = typeof(STUDENT_COURSES), ColumnName = "kstudent", TheirReferenceToThisColumnName = "kcourse")]
        public List<Course> Course { get; set; } = new();
    }
}