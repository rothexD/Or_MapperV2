using ORMapper.Attributes;

namespace ShowcaseOrm.Models
{
    /// <summary>This class represents a course in the school model.</summary>
    [Table(TableName = "COURSES")]
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
        [ForeignKey(ColumnName = "KTEACHER")]
        public Teacher Teacher { get; set; }
    }
}