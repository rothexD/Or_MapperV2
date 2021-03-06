using System;

namespace OrMapper.Attributes
{
    /// <summary>
    /// Defines a table is a foreignkey
    ///
    /// For 1:1 usage:
    /// [ForeignKey(ColumnName = "KTEACHER")]
    /// property
    /// - Defines A ForeignkeyColoumn in this table
    ///
    /// For 1:n usage:
    /// [ForeignKey(ColumnName = "KTEACHER")]
    /// public List<T> Classes { get; set; } = new();
    ///
    /// For n:m usage:
    /// [ForeignKey(RemoteTableName = typeof(STUDENT_COURSES), ColumnName = "course", TheirReferenceToThisColumnName = "kcourse")]
    /// public List<T> Course { get; set; } = new();
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : ColumnAttribute
    {
    }
}