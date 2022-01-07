using System;
using System.Diagnostics.CodeAnalysis;
using OrMapper.Attributes;

namespace ShowcaseOrm.Models
{
    /// <summary>This is a person implementation (from School example).</summary>
    [ExcludeFromCodeCoverage]
    public abstract class Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected static members                                                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Instance number counter.</summary>
        protected static int _N = 1;


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the person ID.</summary>
        [PrimaryKey]
        public string ID { get; set; }


        /// <summary>Gets or sets the person's name.</summary>
        public string Name { get; set; }


        /// <summary>Gets or sets the person's first name.</summary>
        public string FirstName { get; set; }


        /// <summary>Gets or sets the person's birth date.</summary>
        [Column(ColumnName = "BDATE")]
        public DateTime BirthDate { get; set; }


        /// <summary>Gets or sets the person gender.</summary>
        public Gender Gender { get; set; }


        /// <summary>Gets the instance number.</summary>
        [Ignore]
        public int InstanceNumber { get; protected set; } = _N++;
    }


    /// <summary>This enumeration defines genders.</summary>
    public enum Gender
    {
        female = 0,
        male = 1,
        other = 2
    }
}