using System;
using System.Diagnostics.CodeAnalysis;
using ORMapper;
using ORMapper.Caches;
using ShowcaseOrm.Models;

namespace ShowcaseOrm.Show
{
    [ExcludeFromCodeCoverage]
    public static class ShowModifyInDepth
    {
        public static void Show()
        {
            ShowHelper.BeginNewShowcase("Showing modify in depth");

            var i = Orm.Get<Teacher>("t.2");

            i.Courses[0].Students[0].Grade = 5;
            var safe = i.Courses[0].Students[0].ID;
            
            Console.WriteLine("saving modified teacher");
            Orm.Save(i);
            
            Console.WriteLine(Orm.Get<Student>(safe).Grade);
            ShowHelper.EndNewShowcase();
        }
    }
}