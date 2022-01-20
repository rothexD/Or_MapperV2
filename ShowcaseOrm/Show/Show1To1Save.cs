using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public static class Show1To1Save
    {
        public static void Show()
        {
            ShowHelper.BeginNewShowcase("Showing saving a 1 to 1 relation");

            var teacher1to1 = new Teacher();
            teacher1to1.ID = "t.0";
            teacher1to1.Name = "t.0 Name";
            teacher1to1.FirstName = "t.0 firstname";
            teacher1to1.Gender = Gender.female;
            teacher1to1.BirthDate = DateTime.Parse("11.01.1998");
            teacher1to1.HireDate = DateTime.Parse("12.01.1998");
            teacher1to1.Salary = 50000;
            

            var course = new Course();
            course.Name = "course.0";
            course.ID = "course.0";
            course.Teacher = teacher1to1;
            course.Students = new List<Student>();
           
            teacher1to1.Courses.Add(course);
            Orm.Save(course);
            
            var i = Orm.Get<Teacher>("t.0");
            Console.WriteLine("getting teacher only");
            Console.WriteLine(i.ID);
            Console.WriteLine(i.Name);
            Console.WriteLine(i.FirstName);
            Console.WriteLine(i.BirthDate);
            Console.WriteLine(i.Gender);
            Console.WriteLine(i.HireDate);
            Console.WriteLine(i.Salary);
            
            
            Console.WriteLine("getting course and teacher");
            var z = Orm.Get<Course>("course.0");
            
            Console.WriteLine(course.Name );
            Console.WriteLine(course.ID );
            Console.WriteLine(course.Teacher.ID);
            Console.WriteLine(course.Teacher.Name);
            Console.WriteLine(course.Teacher.FirstName);
            Console.WriteLine(course.Teacher.BirthDate);
            Console.WriteLine(course.Teacher.Gender);
            Console.WriteLine(course.Teacher.HireDate);
            Console.WriteLine(course.Teacher.Salary);
            
            ShowHelper.EndNewShowcase();
        }
    }
}