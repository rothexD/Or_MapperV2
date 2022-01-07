using System.Diagnostics.CodeAnalysis;
using OrMapper.Attributes;

namespace ShowcaseOrm.Models
{
    [ManyToManyTable]
    [ExcludeFromCodeCoverage]
    public class STUDENT_COURSES
    {
        [PrimaryKey]
        public string KStudent { get; set; }
        public string KCourse { get; set; }
    }
}