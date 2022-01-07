using System;
using System.Diagnostics.CodeAnalysis;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public static class ShowGetStudent
    {
        public static void Show()
        {
            ShowHelper.Begin("Showing get Student");

            var i = Orm.Get<Student>("t.0");
            Console.WriteLine(i.ID);
            Console.WriteLine(i.Name);
            Console.WriteLine(i.FirstName);
            Console.WriteLine(i.BirthDate);
            Console.WriteLine(i.Gender);
            Console.WriteLine(i.Grade);
            ShowHelper.End();
        }
    }
}