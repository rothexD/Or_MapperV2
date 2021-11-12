using ORMapper.Attributes;

namespace ShowcaseOrm.Models
{
    [ManyToManyTable]
    public class STUDENT_COURSES
    {
        [PrimaryKey]
        public string KStudent { get; set; }
        public string KCourse { get; set; }
    }
}