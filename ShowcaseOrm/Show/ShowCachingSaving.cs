using System;
using System.Diagnostics.CodeAnalysis;
using ORMapper;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public class ShowCachingSaving
    {
        public static void Show()
        {
            ShowHelper.BeginNewShowcase("Showing Caching saving");

            
            var a = Orm.Get<Teacher>("t.0");
            Console.WriteLine("should not print a insert");
            Orm.Save(a);

            var b = Orm.Get<Teacher>("t.0");
            b.Name = "zappelfisch";
            b.Salary = 40000;
            Console.WriteLine("should print a insert");
            Orm.Save(b);
            
            
            var c = Orm.Get<Teacher>("t.0");
            c.Courses[0].Name = "Banane";
            Console.WriteLine("should print a insert modified in depth");
            Orm.Save(c);
            
            
            ShowHelper.EndNewShowcase();
        }
    }
}