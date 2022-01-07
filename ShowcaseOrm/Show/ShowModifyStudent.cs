using System;
using System.Diagnostics.CodeAnalysis;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public static class ShowModifyStudent
    {
        public static void Show(bool PrintToConsole = true)
        {
            ShowHelper.Begin("Showing modify Student");

            var i = Orm.Get<Student>("s.0");

            i.FirstName = "s.0 modified";
            Orm.Save(i);
            
            var z = Orm.Get<Student>("s.0");
            Console.WriteLine(z.ID);
            Console.WriteLine(z.Name);
            Console.WriteLine(z.FirstName);
            Console.WriteLine(z.BirthDate);
            Console.WriteLine(z.Gender);
            Console.WriteLine(z.Grade);
            
            ShowHelper.End();
        }
    }
}