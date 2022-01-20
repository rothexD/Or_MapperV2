using System.Diagnostics.CodeAnalysis;
using OrMapper.Attributes;

namespace OrMapper.Tests.TestClasses
{
    /// <summary>This class represents a class in the school model.</summary>
    [Table(TableName = "CLASSES")]
    [ExcludeFromCodeCoverage]
    internal class Class
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the class ID.</summary>
        [PrimaryKey]
        public string ID { get; set; }


        /// <summary>Gets or sets the class name.</summary>
        public string Name { get; set; }


        /// <summary>Gets or sets the class teacher.</summary>
        [ForeignKey(ColumnName = "KTEACHER")]
        public Teacher Teacher { get; set; }
        
        [ForeignKeyManyToMany(RemoteTableName = typeof(STUDENT_COURSES), ColumnName = "kstudent", TheirReferenceToThisColumnName = "kcourse")]
        public Teacher Teacher2 { get; set; }
    }
}