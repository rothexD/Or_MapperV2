using System;
using System.Diagnostics.CodeAnalysis;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public static class ShowDeleteFromDb
    {
        public static void Show()
        {
            ShowHelper.BeginNewShowcase("Delete a teacher");
            
            var teacherDelete = new Teacher();
            teacherDelete.ID = "t.Delete";
            teacherDelete.Name = "t.Delete Name";
            teacherDelete.FirstName = "t.Delete firstname";
            teacherDelete.Gender = Gender.female;
            teacherDelete.BirthDate = DateTime.Parse("11.01.1998");
            teacherDelete.HireDate = DateTime.Parse("12.01.1998");
            teacherDelete.Salary = 50000;
            
            Orm.Save(teacherDelete);
            var x = Orm.Get<Teacher>("t.Delete");
            ShowHelper.printNewtonsoftJson(x);
            
            Orm.Delete<Teacher>(x.ID);
            
            var y = Orm.Get<Teacher>("t.Delete");
            ShowHelper.printNewtonsoftJson(y);
            
            ShowHelper.EndNewShowcase();
        }
    }
}