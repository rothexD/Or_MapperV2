using System;
using System.Collections.Generic;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    public static class Show1ToNSave
    {
        public static void Show()
        {
            ShowHelper.Begin("Showing saving a 1 to n relation");
            
            var teacher1ton = new Teacher();
            teacher1ton.ID = "t.1";
            teacher1ton.Name = "t.1 Name";
            teacher1ton.FirstName = "t.1 firstname";
            teacher1ton.Gender = Gender.female;
            teacher1ton.BirthDate = DateTime.Parse("11.01.1998");
            teacher1ton.HireDate = DateTime.Parse("12.01.1998");
            teacher1ton.Salary = 50000;

            teacher1ton.Courses.Add(new Course()
            {
                Teacher = teacher1ton,
                ID = "Course example1",
                Name = "course example name",
                Students = new()
            });
            teacher1ton.Courses.Add(new Course()
            {
                Teacher = teacher1ton,
                ID = "Course example2",
                Name = "course example name",
                Students = new()
            });
        
            Orm.Save(teacher1ton);
            var i = Orm.Get<Teacher>("t.1");
            
            Console.WriteLine("getting teacher only");
            Console.WriteLine(i.ID);
            Console.WriteLine(i.Name);
            Console.WriteLine(i.FirstName);
            Console.WriteLine(i.BirthDate);
            Console.WriteLine(i.Gender);
            Console.WriteLine(i.HireDate);
            Console.WriteLine(i.Salary);
            Console.WriteLine(i.Courses[0].Name);
            Console.WriteLine(i.Courses[0].ID);
            Console.WriteLine(i.Courses[1].Name);
            Console.WriteLine(i.Courses[1].ID);
            ShowHelper.End();
        }
    }
}