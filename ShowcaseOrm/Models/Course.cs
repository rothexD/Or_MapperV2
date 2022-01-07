using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using OrMapper.Attributes;

namespace ShowcaseOrm.Models
{
    /// <summary>This class represents a course in the school model.</summary>
    [Table(TableName = "COURSES")]
    [ExcludeFromCodeCoverage]
    public class Course
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the course ID.</summary>
        [PrimaryKey]
        public string ID { get; set; }


        /// <summary>Gets or sets the course name.</summary>
        public string Name { get; set; }


        /// <summary>Gets or sets the course teacher.</summary>
        [ForeignKeyOneToOne(ColumnName = "KTEACHER")]
        public Teacher Teacher { get; set; }
        
        [ForeignKeyManyToMany(RemoteTableName = typeof(STUDENT_COURSES), ColumnName = "KCOURSE", TheirReferenceToThisColumnName = "kstudent")]
        public List<Student> Students { get; set; } = new();
    }
}