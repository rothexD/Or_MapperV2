using System.Diagnostics.CodeAnalysis;
using OrMapper.Attributes;

namespace OrMapper.Tests.TestClasses
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