using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public static class ShowInsertStudent
    {
        public static void Show(bool PrintToConsole = true)
        {
            ShowHelper.BeginNewShowcase("Showing insert Student");

            var student = new Student();
            student.ID = "s.0";
            student.Name = "s.0 Name";
            student.FirstName = "s.0 firstname";
            student.Grade = 1;
            student.Gender = Gender.female;
            student.BirthDate = DateTime.Parse("11.01.1998");
            
            Orm.Save(student);

            var i = Orm.Get<Student>("s.0");
            Console.WriteLine(i.ID);
            Console.WriteLine(i.Name);
            Console.WriteLine(i.FirstName);
            Console.WriteLine(i.BirthDate);
            Console.WriteLine(i.Gender);
            Console.WriteLine(i.Grade);
            
            ShowHelper.EndNewShowcase();
        }
    }
}