using System;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    public static class ShowNtoMSave
    {
        public static void Show()
        {
            ShowHelper.Begin("Showing saving a n to m relation");
            
            
            var teacherExample = new Teacher();
            teacherExample.ID = "t.2";
            teacherExample.Name = "t.2 Name";
            teacherExample.FirstName = "t.2 firstname";
            teacherExample.Gender = Gender.female;
            teacherExample.BirthDate = DateTime.Parse("11.01.1998");
            teacherExample.HireDate = DateTime.Parse("12.01.1998");
            teacherExample.Salary = 50000;

            
            var course = new Course();
            course.ID = "example many to many";
            course.Name = "example many to many";
            course.Teacher = teacherExample;
            
            var course2 = new Course();
            course2.ID = "example many to many2";
            course2.Name = "example many to many2";
            course2.Teacher = teacherExample;
            
            var student1 = new Student();
            student1.ID = "s.1";
            student1.Name = "s.1 Name";
            student1.FirstName = "s.1 firstname";
            student1.Grade = 1;
            student1.Gender = Gender.female;
            student1.BirthDate = DateTime.Parse("11.01.1998");
            
            var student2 = new Student();
            student2.ID = "s.2";
            student2.Name = "s.2 Name";
            student2.FirstName = "s.2 firstname";
            student2.Grade = 1;
            student2.Gender = Gender.female;
            student2.BirthDate = DateTime.Parse("11.01.1998");
            
            course.Students.Add(student1);
            course.Students.Add(student2);
            
            student1.Course.Add(course2);
            Orm.Save(course);
            
            var i = Orm.Get<Student>("s.1");
            Console.WriteLine("printing student 1");
            Console.WriteLine(i.ID);
            Console.WriteLine(i.Name);
            Console.WriteLine(i.FirstName);
            Console.WriteLine(i.BirthDate);
            Console.WriteLine(i.Gender);
            Console.WriteLine(i.Grade);
            Console.WriteLine("printing all courses for student 1");
            foreach (var iCourse in i.Course)
            {
                Console.WriteLine(iCourse.Name);
            }
            
            var z = Orm.Get<Student>("s.2");
            Console.WriteLine("printing student 2");
            Console.WriteLine(z.ID);
            Console.WriteLine(z.Name);
            Console.WriteLine(z.FirstName);
            Console.WriteLine(z.BirthDate);
            Console.WriteLine(z.Gender);
            Console.WriteLine(z.Grade);
            Console.WriteLine("printing all courses for student 2");
            foreach (var iCourse in i.Course)
            {
                Console.WriteLine(iCourse.Name);
            }
            
            var y = Orm.Get<Course>("example many to many");
            Console.WriteLine("printing all students for course \"example many to many\"");
            foreach (var yStudent in y.Students)
            {
                Console.WriteLine(yStudent.ID);
            }
            var y2 = Orm.Get<Course>("example many to many2");
            Console.WriteLine("printing all students for course \"example many to many2\"");
            foreach (var yStudent in y2.Students)
            {
                Console.WriteLine(yStudent.ID);
            }
            ShowHelper.End();
        }
    }
}