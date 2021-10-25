using System.Collections.Generic;
using ORMapper.Attributes;

namespace ShowcaseOrm.Models
{
    /// <summary>This class represents a class in the school model.</summary>
    [Table( TableName= "CLASSES")]
    public class Class
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
        public Teacher Teacher
        {
            get; set;
        }

    }
}